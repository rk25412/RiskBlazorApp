using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Clear.Risk.Data;
using Clear.Risk.Models;
using Clear.Risk.Authentication;
using Radzen;
using Clear.Risk.Infrastructures.Helpers;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft;
using AspNetMonsters.Blazor.Geolocation;
using Geocoding;
using Geocoding.Google;
//using Coravel;
using Stripe;
using Hangfire;
using Hangfire.SqlServer;
using Clear.Risk.Services;
using Clear.Risk.Controllers;
//using Blazor.GoogleMap;

namespace Clear.Risk
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        partial void OnConfigureServices(IServiceCollection services);

        partial void OnConfiguringServices(IServiceCollection services);

        public void ConfigureServices(IServiceCollection services)
        {
            OnConfiguringServices(services);

            services.AddHttpContextAccessor();
            services.AddScoped<HttpClient>(serviceProvider =>
            {
                var uriHelper = serviceProvider.GetRequiredService<NavigationManager>();
                return new HttpClient
                {
                    BaseAddress = new Uri(uriHelper.BaseUri)
                };
            });

            services.AddHttpClient();
            services.AddAuthentication();
            services.AddAuthorization();
            //services.AddGoogleMaps(options =>
            //{
            //    options.ApiKey = Configuration["GoogleAPI"];
            //});

            services.AddDbContext<ApplicationIdentityDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("ClearConnectionConnection"));
            }, ServiceLifetime.Transient);

            services.AddIdentity<ApplicationUser, IdentityRole>()
                  .AddEntityFrameworkStores<ApplicationIdentityDbContext>().AddDefaultTokenProviders();

            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>,
                  ApplicationPrincipalFactory>();
            services.AddScoped<SecurityService>();
            services.AddScoped<ClearConnectionService>();
            services.AddScoped<SurveyServices>();

            services.AddScoped<AssesmentService>();
            services.AddScoped<WorkOrderService>();

            services.AddDbContext<Clear.Risk.Data.ClearConnectionContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("ClearConnectionConnection"));

                options.EnableSensitiveDataLogging();
            }, ServiceLifetime.Transient);

            services.AddHangfire(config =>
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                        .UseSimpleAssemblyNameTypeSerializer()
                        .UseRecommendedSerializerSettings()
                        .UseSqlServerStorage(Configuration.GetConnectionString("ClearConnectionConnection"),
                        new SqlServerStorageOptions
                        {
                            CommandBatchMaxTimeout = TimeSpan.FromMinutes(240),
                            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(240),
                            QueuePollInterval = TimeSpan.Zero,
                            UseRecommendedIsolationLevel = true,
                            DisableGlobalLocks = true
                        }));

            services.AddControllersWithViews();
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddServerSideBlazor().AddHubOptions(o =>
            {
                o.MaximumReceiveMessageSize = 10 * 1024 * 1024;
            });
            services.AddServerSideBlazor()
                .AddCircuitOptions(opt =>
                {
                    opt.DetailedErrors = true;
                });
            services.AddScoped<DialogService>();
            services.AddScoped<NotificationService>();
            services.AddScoped<TooltipService>();
            services.AddScoped<ContextMenuService>();
            services.AddScoped<GlobalsService>();
            services.AddSingleton<LocationService>();

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = TokenProviderOptions.Key,
                ValidateIssuer = true,
                ValidIssuer = TokenProviderOptions.Issuer,
                ValidateAudience = true,
                ValidAudience = TokenProviderOptions.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Audience = TokenProviderOptions.Audience;
                options.ClaimsIssuer = TokenProviderOptions.Issuer;
                options.TokenValidationParameters = tokenValidationParameters;
                options.SaveToken = true;
            });

            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddControllers().AddNewtonsoftJson(options =>
               options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver()
           );
            services.AddScoped<RunScheduleAssesment>();
            services.AddScoped<ChangeTransactionStatus>();
            services.AddScoped<CheckBalanceSchedule>();
            services.AddScoped<ScheduleStopper>();
            services.AddScoped<ScheduleStarter>();

            OnConfigureServices(services);
        }

        private BackgroundJobServer _backgroundJobServer;

        partial void OnConfigure(IApplicationBuilder app, IWebHostEnvironment env);
        partial void OnConfiguring(IApplicationBuilder app, IWebHostEnvironment env);

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationIdentityDbContext identityDbContext, IHostApplicationLifetime lifetime, CheckBalanceSchedule balance)
        {
            OnConfiguring(app, env);

            lifetime.ApplicationStopping.Register(OnAppStopping);

            if (env.IsDevelopment())
            {
                Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.Use((ctx, next) =>
                {
                    return next();
                });
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors(config =>
            {
                config.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            });

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseHangfireDashboard("/scheduledJobs", new DashboardOptions
            {
                Authorization = new[] { new AuthFilterForHangfireDashboard() },
                AppPath = "/Dashboard",
                DashboardTitle = "ClearFire"
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
                endpoints.MapHangfireDashboard();
            });

            /*---------------------------------------------------------------------------------*/

            //var provider = app.ApplicationServices;
            //provider.UseScheduler(scheduler =>
            //{
            //    scheduler.ScheduleWithParams<CheckBalanceSchedule>().Daily();
            //});

            /*---------------------------------------------------------------------------------*/

            _backgroundJobServer = new BackgroundJobServer();

            RecurringJob.AddOrUpdate("CheckBalance",() => balance.Invoke() , Cron.Daily);

            identityDbContext.Database.Migrate();

            

            OnConfigure(app, env);
        }

        public void OnAppStopping()
        {
            _backgroundJobServer.Dispose();
        }
    }


}
