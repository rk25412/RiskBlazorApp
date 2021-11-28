using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Clear.Risk.Models;
using Clear.Risk.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication; 
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Clear.Risk.Authentication
{
    public partial class ApplicationPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        private ApplicationIdentityDbContext identityContext;

        public ApplicationPrincipalFactory(ApplicationIdentityDbContext identityContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, roleManager, optionsAccessor)
        {
            this.identityContext = identityContext;
        }
        partial void OnCreatePrincipal(ClaimsPrincipal principal, ApplicationUser user);

        public async override Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);

            List<Claim> claims = new List<Claim> {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.String),
                        new Claim(ClaimTypes.GivenName, user.FIRST_NAME, ClaimValueTypes.String),
                        new Claim(ClaimTypes.Surname, user.LAST_NAME, ClaimValueTypes.String),                         
                        new Claim(ClaimTypes.Name, user.FIRST_NAME + " " + user.LAST_NAME, ClaimValueTypes.String),
                        new Claim("CompanyName", user.COMPANY_NAME != null ? user.COMPANY_NAME: "", ClaimValueTypes.String),
                        new Claim("CompanyId", user.CompanyId != null ? user.CompanyId.ToString() : user.Id, ClaimValueTypes.String),
                        new Claim("CountryId", user.PERSONAL_COUNTRY_ID != null ? user.PERSONAL_COUNTRY_ID.ToString(): "", ClaimValueTypes.String),
            };

            foreach (var role in user.RoleNames)
            {
                claims.Add(new Claim(ClaimTypes.Role, role, ClaimValueTypes.String));
            }

            var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
             var userPrincipal = new ClaimsPrincipal(userIdentity);
                        
            var identity = new ClaimsIdentity(claims, "RiskSecurity");
            principal = new ClaimsPrincipal(new[] { identity });

            this.OnCreatePrincipal(principal, user);

            return principal;
        }
    }
}
