using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Clear.Risk.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text.Encodings.Web;
using Clear.Risk.Models.ClearConnection;
using MimeKit;
using MailKit.Net.Smtp;

namespace Clear.Risk
{
    public partial class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IWebHostEnvironment env;
        private readonly ClearConnectionService clearConnectionService;

        public AccountController(IWebHostEnvironment env, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ClearConnectionService clearConnectionService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.env = env;
            this.clearConnectionService = clearConnectionService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password)
        {
            if (env.EnvironmentName == "Development" && userName == "admin" && password == "admin")
            {
                var claims = new List<Claim>()
                {
                        new Claim(ClaimTypes.Name, "admin"),
                        new Claim(ClaimTypes.Email, "admin")
                };

                roleManager.Roles.ToList().ForEach(r => claims.Add(new Claim(ClaimTypes.Role, r.Name)));
                await signInManager.SignInWithClaimsAsync(new ApplicationUser { UserName = userName, Email = userName }, isPersistent: false, claims);

                return Redirect("~/");
            }

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                try
                {
                    var user = await userManager.FindByNameAsync(userName);

                    var person = await clearConnectionService.GetPersonByPersonId(Convert.ToInt32(user.Id));

                    if (user == null)
                    {
                        return Redirect("~/Login?error=Invalid user or password");
                    }

                    if (!person.ACTIVATED)
                    {
                        return Redirect("~/Login?error=Account is not activated yet.");
                    }

                    if (user.CompanyTypeID == 3)
                        return Redirect("~/Login?error=Invalid Logon, As an employee, you can only logon using the app");

                    var validPassword = await userManager.CheckPasswordAsync(user, password);

                    if (!validPassword)
                    {
                        return Redirect("~/Login?error=Invalid user or password");
                    }
                    //var result = await signInManager.PasswordSignInAsync(userName, password, false, false);


                    //var claims = new List<Claim>()
                    //{
                    //        new Claim(ClaimTypes.Name, userName),
                    //        new Claim(ClaimTypes.Email, userName),
                    //        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                    //};

                    ////Get Roles
                    //user.RoleNames = await userManager.GetRolesAsync(user);

                    //foreach(var role in user.RoleNames)
                    //{
                    //    claims.Add(new Claim(ClaimTypes.Role, role));
                    //}

                    //var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    //var userPrincipal = new ClaimsPrincipal(userIdentity);

                    //var identity = new ClaimsIdentity(claims, "ClearSecurity");
                    //ClaimsPrincipal principal = new ClaimsPrincipal(new[] { identity });
                    //HttpContext.User = principal;

                    var result = await signInManager.PasswordSignInAsync(userName, password, false, false);

                    //await AuthenticationHttpContextExtensions.SignInAsync(HttpContext, principal, new AuthenticationProperties { IsPersistent = true });

                    if (result.Succeeded)
                        return Redirect("~/Dashboard");

                }
                catch (Exception ex)
                {
                    return Redirect("~/Login?error=Invalid user or password");
                }

            }

            return Redirect("~/Login?error=Invalid user or password");
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(ApplicationUser user)
        {
            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
            {
                return Redirect("~/Signup?error=Invalid user or password");
            }

            //var user = new ApplicationUser { UserName = userName, Email = userName };

            var result = await userManager.CreateAsync(user);


            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                return Redirect("~/");
            }

            var message = string.Join(", ", result.Errors.Select(error => error.Description));

            return Redirect($"~/Signup?error={message}");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword)
        {
            if (oldPassword == null || newPassword == null)
            {
                //return Redirect($"~/Profile?error=Invalid old or new password");
                return Redirect($"/view-Company?error=Invalid old or new password");

            }

            var id = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = await userManager.FindByIdAsync(id);

            var result = await userManager.ChangePasswordAsync(user, oldPassword, newPassword);

            if (result.Succeeded)
            {
                //await signInManager.SignInAsync(user, isPersistent: true);
                await signInManager.SignOutAsync();
                await AuthenticationHttpContextExtensions.SignOutAsync(HttpContext);


                return Redirect("~/");
            }

            var message = string.Join(", ", result.Errors.Select(error => error.Description));

            return Redirect($"~/Profile?error={message}");
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {

            if (string.IsNullOrEmpty(email))
            {
                return Redirect($"/login?error=An Email required for password");
            }
            var user = await userManager.FindByNameAsync(email);

            if (user == null)
            {
                return Redirect($"/login?error=Invalid user Record.");
            }
            var code = await userManager.GeneratePasswordResetTokenAsync(user);

            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(code);

            var link = $@"{Request.Scheme}://{Request.Host.ToString()}/ResetPassword?code={Convert.ToBase64String(plainTextBytes)}";
            //var link = $@"{Request.Scheme}://{Request.Host.ToString()}/crm/ResetPassword?code={Convert.ToBase64String(plainTextBytes)}";
            //var link = Url.Action(nameof(ResetPassword), "Account", new { code }, Request.Scheme, Request.Host.ToString());

            Smtpsetup smtpsetup = await clearConnectionService.GetSmtpsetup();
            if (smtpsetup != null)
            {
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(smtpsetup.SMTP_MAIL_FROM));
                mimeMessage.To.Add(new MailboxAddress(email));
                mimeMessage.Subject = "Reset Password";
                //string emailbody = $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>";
                string emailbody = $"<p>Please use the following link to <a href='{link}'>reset your password</a>.</p>" +
                    $"<p>If you did not request this password change please feel free to ignore it.</p>" +
                    $"<p>If you have any comments or questions, please don't hesitste to reach us a <a href='mailto:support@clearsoftware-solutions.com'>support@clearsoftware-solutions.com</a>.</p>" +
                    $"<p>Please feel free to respond to this email. It was send from a monitored email address, and we <b>LOVE</b> to hear from you.</p>";
                mimeMessage.Body = new TextPart("HTML")
                {
                    Text = emailbody

                };
                string SmtpServer = smtpsetup.SMTP_SERVER_STRING;
                int SmtpPortNumber = smtpsetup.SMTP_PORT;

                try
                {
                    using (var client = new SmtpClient())
                    {
                        client.Connect(SmtpServer, SmtpPortNumber, false);
                        client.Authenticate(smtpsetup.SMTP_USER_ACCOUNT, smtpsetup.SMTP_ACCOUNT_PASSWORD);
                        await client.SendAsync(mimeMessage);
                        await client.DisconnectAsync(true);
                        return Redirect($"/ForgotPasswordConfirmation");
                    }
                }
                catch (Exception ex)
                {

                }

            }
            return Redirect($"/login?error=SMTP server not set up.");
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string username, string password, string code)
        {
            var base64encodedBytes = System.Convert.FromBase64String(code);

            var token = System.Text.Encoding.UTF8.GetString(base64encodedBytes);

            var user = await userManager.FindByNameAsync(username);

            if(user == null)
            {
                return Redirect($"/login?error=Invalid user Record.");
            }


            var result = await userManager.ResetPasswordAsync(user, token, password);

            if(!result.Succeeded)
                return Redirect($"/login?error=Some Error Occurred");

            return Redirect($"/login?success=Password Successfully Reset");
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            await AuthenticationHttpContextExtensions.SignOutAsync(HttpContext);

            //return Redirect("~/");
            return Redirect("~/Login");
        }
    }
}
