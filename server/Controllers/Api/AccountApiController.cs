using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Clear.Risk.Models;
using Clear.Risk.Authentication;
using Clear.Risk.ViewModels;
using Clear.Risk.Models.ClearConnection;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authentication;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cors;

namespace Clear.Risk.Controllers.Api
{
    [Route("api/Account")]
    [ApiController]
    public partial class AccountApiController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IWebHostEnvironment env;
        private readonly ClearConnectionService clearConnection;

        public readonly SecurityService Security;

        public AccountApiController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IWebHostEnvironment env, ClearConnectionService clearConnectionService, SecurityService security)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.env = env;
            clearConnection = clearConnectionService;
            Security = security;
        }

        private IActionResult Error(string message)
        {
            return BadRequest(new { error = new { message } });
        }

        private IActionResult Jwt(IEnumerable<Claim> claims)
        {
            var handler = new JwtSecurityTokenHandler();

            var token = handler.CreateToken(new SecurityTokenDescriptor

            {
                Issuer = TokenProviderOptions.Issuer,
                Audience = TokenProviderOptions.Audience,
                SigningCredentials = TokenProviderOptions.SigningCredentials,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(TokenProviderOptions.Expiration)
            });

            return Json(new { access_token = handler.WriteToken(token), expires_in = TokenProviderOptions.Expiration.TotalSeconds });
        }


        private IActionResult Jwt(IEnumerable<Claim> claims, ApplicationUser user)
        {
            var handler = new JwtSecurityTokenHandler();

            var token = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = TokenProviderOptions.Issuer,
                Audience = TokenProviderOptions.Audience,
                SigningCredentials = TokenProviderOptions.SigningCredentials,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(TokenProviderOptions.Expiration)
            });

            return Json(new { Profile = user, access_token = handler.WriteToken(token), expires_in = TokenProviderOptions.Expiration.TotalSeconds });
        }

        partial void OnChangePassword(ApplicationUser user, string newPassword);

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("/account/change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] JObject data)
        {
            var oldPassword = data.GetValue("OldPassword", StringComparison.OrdinalIgnoreCase);
            var newPassword = data.GetValue("NewPassword", StringComparison.OrdinalIgnoreCase);

            if (oldPassword == null || newPassword == null)
            {
                return Error("Invalid old or new password.");
            }

            var id = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = await userManager.FindByIdAsync(id);

            OnChangePassword(user, newPassword.ToObject<string>());

            var result = await userManager.ChangePasswordAsync(user, oldPassword.ToObject<string>(), newPassword.ToObject<string>());

            if (!result.Succeeded)
            {
                var message = string.Join(", ", result.Errors.Select(error => error.Description));

                return Error(message);
            }

            return Ok(new
            {
                status = true,
                msg = "Password changed successfully"
            });
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginViewModel data)
        {
            var username = data.UserName; // data.GetValue("UserName", StringComparison.OrdinalIgnoreCase);
            var password = data.Password; //data.GetValue("Password", StringComparison.OrdinalIgnoreCase);

            if (username == null || password == null)
            {
                return Error("Invalid user name or password.");
            }

            var user = await userManager.FindByNameAsync(username);

            if (user == null)
            {
                return Error("Invalid user name or password.");
            }



            var validPassword = await userManager.CheckPasswordAsync(user, password);

            if (!validPassword)
            {
                return Error("Invalid user name or password.");
            }
            //user.PhoneNumber = user.BUSINESS_MOBILE;

            var person = await clearConnection.GetPersonByPersonId(Convert.ToInt32(user.Id));

            if (!person.ACTIVATED)
            {
                return Error("Account not activated yet.");
            }

            var principal = await signInManager.CreateUserPrincipalAsync(user);

            return Jwt(principal.Claims, user);
        }

        partial void OnUserCreated(ApplicationUser user);

        private IActionResult IdentityError(IdentityResult result)
        {
            var message = string.Join(", ", result.Errors.Select(error => error.Description));

            return BadRequest(new { error = new { message } });
        }


        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> ResetPassword([FromBody] InputModel Input)
        {
            if (string.IsNullOrEmpty(Input.Code))
            {
                return BadRequest("A code must be supplied for password reset.");
            }

            var user = await userManager.FindByNameAsync(Input.Email);

            if (user == null)
            {
                return BadRequest("Invalid user Record.");
            }

            var result = await userManager.ResetPasswordAsync(user, Input.Code, Input.Password);


            if (!result.Succeeded)
            {
                var message = string.Join(", ", result.Errors.Select(error => error.Description));

                return BadRequest(message);
            }

            //return Ok();

            return Json(new { Status = true, Message = "Password Reset Successfully" });
        }


        [AllowAnonymous]
        [Route("ForgotPassword")]
        [HttpGet]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                return BadRequest("An Email reqired for password reset.");
            }

            var user = await userManager.FindByNameAsync(Email);

            if (user == null)
            {
                return BadRequest("Invalid user Record.");
            }

            Random r = new Random();
            int otp = r.Next(100000, 999999);

            var code = await userManager.GeneratePasswordResetTokenAsync(user);


            Smtpsetup smtpsetup = await clearConnection.GetSmtpsetup();
            if (smtpsetup != null)
            {
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(smtpsetup.SMTP_MAIL_FROM));
                mimeMessage.To.Add(new MailboxAddress(Email));

                mimeMessage.Subject = "Reset Password";
                string emailbody = EmailBody(otp);
                mimeMessage.Body = new TextPart("Html")
                {
                    Text = emailbody
                };
                string SmtpServer = smtpsetup.SMTP_SERVER_STRING;
                int SmtpPortNumber = smtpsetup.SMTP_PORT;
                bool useSsl = smtpsetup.USE_SSL;
                try
                {
                    using (var client = new SmtpClient())
                    {
                        client.Connect(SmtpServer, SmtpPortNumber, useSsl);
                        client.Authenticate(smtpsetup.SMTP_USER_ACCOUNT, smtpsetup.SMTP_ACCOUNT_PASSWORD);
                        await client.SendAsync(mimeMessage);
                        await client.DisconnectAsync(true);
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }


            }

            return Ok(new
            {
                code = code,
                otp = otp,
            });
        }

        protected string EmailBody(int otp)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append($@"Your One Time Password for Reset Password is <b>{otp}</b>.");


            return builder.ToString();
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("Logout")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            await AuthenticationHttpContextExtensions.SignOutAsync(HttpContext);

            return Ok("Logged Out");
        }


        

        protected string HashPassword(string password)
        {
            using (var algorithm = SHA256.Create())
            {
                var hash = algorithm.ComputeHash(Encoding.ASCII.GetBytes(password));

                return Encoding.ASCII.GetString(hash);
            }
        }

        protected string EmailBody(Clear.Risk.Models.ClearConnection.Person pers)
        {
            string baseUrl = $"{this.Request.Scheme}://{this.Request.Host}/";
            StringBuilder builder = new StringBuilder();
            builder.Append($@"<p style='font-family: Arial, Helvetica, sans-serif;font-size:16px'>");
            builder.Append($@"We are pleased to welcome you to the Clear Risk Assessment system. Your logon details are given below and you can now logon at ");
            builder.Append($@"<i><a href='www.clear-whs.com' style='color:red;text-decoration: none;'>www.clear-whs.com</a></i>");
            builder.Append($@"</p>");
            builder.Append($@"<p style='font-family: Arial, Helvetica, sans-serif;font-size:16px'>");
            builder.Append($@"Logon Credentials: <br />Userid: <span style='color:red'>{pers.PERSONAL_EMAIL}</span><br />Password: <span style='color:red'>{pers.ConfirmPassword}</span>");
            builder.Append($@"</p>");
            builder.Append($@"<p style='font-family: Arial, Helvetica, sans-serif;font-size:16px'>");
            builder.Append($@"Your account is not activated yet. To activate your account click ");
            builder.Append($@"<b><a href='{baseUrl + "activate/" + pers.PERSON_ID + "/" + pers.ACTCODE }'>here.</a></b>");
            builder.Append($@"</p>");
            builder.Append($@"<p style='font-family: Arial, Helvetica, sans-serif;font-size:16px'>");
            builder.Append($@"Learning to use the system is easy and there are User Guides and training aids in the &#8220;How To&#8221; section of the system. ");
            builder.Append($@"</p>");
            builder.Append($@"<p style='font-family: Arial, Helvetica, sans-serif;font-size:16px'>");
            builder.Append($@"Once you logon you can access the &#8220;How To&#8221; section from the main navigation panel. Here you will find the simple instructions of how to get started quickly and easily.");
            builder.Append($@"</p>");
            builder.Append($@"<p style='font-family: Arial, Helvetica, sans-serif;font-size:16px'>");
            builder.Append($@"There has been an amount credited to your account on registration that will allow you to try out the system for free. This is a limited amount which limits the number of transactions you can do for free.  The system will send you a reminder email when the limit has been reached. At this point, and if you still want to keep achieving the benefits that the system provides, you can add more credit to your account. How to do this is explained in the &#8220;How To&#8220; section of the system. ");
            builder.Append($@"</p>");
            builder.Append($@"<p style='font-family: Arial, Helvetica, sans-serif;font-size:16px'>");
            builder.Append($@"If you have any questions that cannot be answered by looking in the &#8220;How To&#8221; section then please contact our support desk at");
            builder.Append($@"</p><br />");

            builder.Append($@"<a href='mailto:Support@Clear-WHS.com'>Support@Clear-WHS.com</a>");

            return builder.ToString();
        }

        [AllowAnonymous]
        [Route("RegisterAccount")]
        [EnableCors]
        [HttpPost]
        public async Task<IActionResult> RegisterAccount([FromBody] RegistrationViewModel args)
        {
            Person newPerson = new Person();

            string password = string.Empty;

            var user = await clearConnection.GetPersonByPersonEmail(args.Email);
            if (user == null)
            {
                newPerson.COMPANY_NAME = args.CompanyName;
                newPerson.FIRST_NAME = args.FirstName;
                newPerson.LAST_NAME = args.LastName;
                newPerson.BUSINESS_EMAIL = args.Email;
                newPerson.PERSONAL_EMAIL = args.Email;
                newPerson.PERSONALADDRESS1 = args.Address1;
                newPerson.PERSONALADDRESS2 = args.Address2;
                newPerson.BUSINESS_ADDRESS1 = args.Address1;
                newPerson.BUSINESS_ADDRESS2 = args.Address2;
                newPerson.PERSONAL_CITY = args.City;
                newPerson.BUSINESS_CITY = args.City;
                newPerson.PERSONAL_POSTCODE = args.PostCode;
                newPerson.BUSINESS_POSTCODE = args.PostCode;
                newPerson.APPLICENCE_STARTDATE = DateTime.Now;
                newPerson.BUSINESS_COUNTRY_ID = args.Country;
                newPerson.PERSONAL_COUNTRY_ID = args.Country;
                newPerson.BUSINESS_STATE_ID = args.State;
                newPerson.PERSONAL_STATE_ID = args.State;
                newPerson.BUSINESS_PHONE = args.Phone;
                newPerson.PERSONAL_PHONE = args.Phone;
                newPerson.BUSINESS_MOBILE = args.Mobile;
                newPerson.PERSONAL_MOBILE = args.Mobile;
                newPerson.PASSWORDHASH = args.Password;
                newPerson.CREATED_DATE = DateTime.Now;

                Applicence applicence = await clearConnection.GetApplicenceByCountry((int)newPerson.PERSONAL_COUNTRY_ID);
                if (applicence == null)
                    applicence = await clearConnection.GetDefaultApplicence();

                if (applicence != null)
                {
                    newPerson.APPLICENCEID = applicence.APPLICENCEID;
                    newPerson.CURRENCY_ID = applicence.CURRENCY_ID;
                    newPerson.CURRENT_BALANCE = applicence.DEFAULT_CREDIT;
                }

                if (newPerson.Contacts == null)
                    newPerson.Contacts = new List<PersonContact>();

                newPerson.Contacts.Add(new PersonContact
                {
                    CREATED_DATE = DateTime.Now,
                    UPDATED_DATE = DateTime.Now,
                    //CREATOR_ID = ,
                    //UPDATER_ID = ,
                    IS_DELETED = false,
                    CONTACT_STATUS_ID = 1,
                    FIRST_NAME = newPerson.FIRST_NAME,
                    MIDDLE_NAME = newPerson.MIDDLE_NAME,
                    LAST_NAME = newPerson.LAST_NAME,
                    ISPRIMARY = true,
                    DESIGNATION_ID = newPerson.DESIGNATION_ID,
                    GENDER = 1,
                    PERSONALADDRESS1 = newPerson.PERSONALADDRESS1,
                    PERSONALADDRESS2 = newPerson.PERSONALADDRESS2,
                    PERSONAL_CITY = newPerson.PERSONAL_CITY,
                    PERSONAL_COUNTRY_ID = (int)newPerson.PERSONAL_COUNTRY_ID,
                    PERSONAL_STATE_ID = newPerson.PERSONAL_STATE_ID,
                    PERSONAL_EMAIL = newPerson.PERSONAL_EMAIL,
                    PERSONAL_PHONE = newPerson.PERSONAL_PHONE,
                    PERSONAL_MOBILE = newPerson.PERSONAL_MOBILE,
                    PERSONAL_POSTCODE = newPerson.PERSONAL_POSTCODE,
                    BUSINESS_ADDRESS1 = newPerson.BUSINESS_ADDRESS1,
                    BUSINESS_ADDRESS2 = newPerson.BUSINESS_ADDRESS2,
                    BUSINESS_CITY = newPerson.BUSINESS_CITY,
                    BUSINESS_COUNTRY_ID = newPerson.BUSINESS_COUNTRY_ID,
                    BUSINESS_STATE_ID = newPerson.BUSINESS_STATE_ID,
                    BUSINESS_EMAIL = newPerson.BUSINESS_EMAIL,
                    BUSINESS_MOBILE = newPerson.BUSINESS_MOBILE,
                    BUSINESS_PHONE = newPerson.BUSINESS_PHONE,
                    BUSINESS_POSTCODE = newPerson.BUSINESS_POSTCODE,
                });

                newPerson.ACTIVATED = false;
                Random r = new Random();
                newPerson.ACTCODE = r.Next(10000000, 99999999);

                if (newPerson.CompanyAccountTransactions == null)
                    newPerson.CompanyAccountTransactions = new List<CompanyAccountTransaction>();

                newPerson.CompanyAccountTransactions.Add(new CompanyAccountTransaction
                {
                    TRANSACTION_DATE = DateTime.Now,
                    PAYMENT_AMOUNT = 0,
                    DEPOSITE_AMOUNT = 10,
                    DESCRIPTION = "Signup Credit",
                    TRXTYPE = "DTD",
                    CREATED_DATE = DateTime.Now,
                    UPDATED_DATE = DateTime.Now,
                    CURRENCY_ID = newPerson.CURRENCY_ID != null ? (int)newPerson.CURRENCY_ID : 0,
                });

                if (newPerson.PersonRoles == null)
                    newPerson.PersonRoles = new List<PersonRole>();

                newPerson.PersonRoles.Add(new PersonRole
                {
                    ROLE_ID = 2
                });

                newPerson.PASSWORDHASH = HashPassword(newPerson.PASSWORDHASH);

                try
                {
                    var securityCreateUserResult = await clearConnection.CreatePerson(newPerson);

                    Smtpsetup smtpsetup = await clearConnection.GetSmtpsetup();

                    if (smtpsetup != null)
                    {
                        var mimeMessage = new MimeMessage();
                        mimeMessage.From.Add(new MailboxAddress(smtpsetup.SMTP_MAIL_FROM));
                        mimeMessage.To.Add(new MailboxAddress(newPerson.BUSINESS_EMAIL));
                        mimeMessage.Subject = "Welcome to Clear";

                        string emailbody = EmailBody(securityCreateUserResult);
                        mimeMessage.Body = new TextPart("Html")
                        {
                            Text = emailbody
                        };
                        string SmtpServer = smtpsetup.SMTP_SERVER_STRING;
                        int SmtpPortNumber = smtpsetup.SMTP_PORT;
                        bool useSsl = smtpsetup.USE_SSL;
                        try
                        {
                            using (var client = new SmtpClient())
                            {
                                client.Connect(SmtpServer, SmtpPortNumber, useSsl);
                                client.Authenticate(smtpsetup.SMTP_USER_ACCOUNT, smtpsetup.SMTP_ACCOUNT_PASSWORD);
                                await client.SendAsync(mimeMessage);
                                await client.DisconnectAsync(true);
                            }
                        }
                        catch (Exception ex)
                        {
                            var result = new
                            {
                                status = false,
                                msg = ex.Message
                            };
                            return BadRequest(result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    var result = new
                    {
                        status = false,
                        msg = ex.Message
                    };
                    return BadRequest(result);
                }
            }
            else
            {
                var result = new
                {
                    status = false,
                    msg = "Email already exists."
                };

                return BadRequest(result);
            }

            return Ok(new
            {
                status = true,
                msg = "Registration Successful"
            });
        }


        [AllowAnonymous]
        [Route("GetCountryList")]
        [EnableCors]
        [HttpGet]
        public async Task<IActionResult> GetCountryList()
        {
            IEnumerable<Clear.Risk.Models.ClearConnection.Country> getCountriesResult = await clearConnection.GetCountries();

            var countryList = from x in getCountriesResult
                              select new
                              {
                                  CountryId = x.ID,
                                  CountryName = x.COUNTRYNAME
                              };
            return Ok(countryList);
        }

        [AllowAnonymous]
        [Route("GetStatesList")]
        [EnableCors]
        [HttpGet]
        public async Task<IActionResult> GetStatesList(int countryId)
        {
            IEnumerable<Clear.Risk.Models.ClearConnection.State> getStatesResult = await clearConnection.GetStates(new Radzen.Query { Filter = $"a => a.COUNTRYID == {countryId}" });

            var result = from x in getStatesResult
                         select new
                         {
                             statedId = x.ID,
                             stateName = x.STATENAME
                         };

            return Ok(result);
        }


        [AllowAnonymous]
        [Route("GetPriceByLocation")]
        [EnableCors]
        [HttpGet]
        public async Task<IActionResult> GetPriceByLocation(string CountryCode)
        {
            string name = CountryCode;

            var countries = await clearConnection.GetCountries(new Radzen.Query() { Filter = $"i => i.SHORTNAME == \"{name}\"" });
            var country = countries.FirstOrDefault();
            var appLicense = await clearConnection.GetApplicences(new Radzen.Query { Filter = $"i => i.COUNTRY_ID == {country.ID}", Expand = $"Currency" });
            var licesnse = appLicense.FirstOrDefault();

            return Ok(new
            {
                currencySymbol = licesnse?.Currency?.CURSYMBOL ?? "",
                price = licesnse?.NETPRICE ?? 0,
            });
        }
    }
}
