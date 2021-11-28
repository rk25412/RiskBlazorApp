using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using Clear.Risk.Models.ClearConnection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Clear.Risk.Models;
using System.Security.Cryptography;
using System.Text;
using MimeKit;
using MailKit.Net.Smtp;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
//using System.Net.Mail;

namespace Clear.Risk.Pages.Employees
{
    public partial class NewEmployee : ComponentBase
    {
        protected DateTime? value = DateTime.Now;

        IEnumerable<DateTime> dates = new DateTime[] { DateTime.Today.AddDays(-1), DateTime.Today.AddDays(1) };

        Dictionary<DateTime, string> events = new Dictionary<DateTime, string>();
        void Change(DateTime? value, string name, string format)
        {
            events.Add(DateTime.Now, $"{name} value changed to {value?.ToString(format)}");
            StateHasChanged();
        }

        [Inject]
        protected UserManager<ApplicationUser> userManager { get; set; }
        [Inject]
        protected RoleManager<IdentityRole> roleManager { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager UriHelper { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        protected Clear.Risk.Models.ClearConnection.Person _person;

        [Inject]
        protected ClearConnectionService ClearConnection { get; set; }

        protected Clear.Risk.Models.ClearConnection.Person person
        {
            get
            {
                return _person;
            }
            set
            {
                if (!object.Equals(_person, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "person", NewValue = value, OldValue = _person };
                    _person = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected IList<Clear.Risk.Models.ClearConnection.Person> Managers { get; set; }

        protected void Change(object value, string name)
        {

            StateHasChanged();
        }



        protected void SameAsPersonal(bool value, string name)
        {
            if (value && person != null)
            {
                person.BUSINESS_ADDRESS1 = person.PERSONALADDRESS1;
                person.BUSINESS_ADDRESS2 = person.PERSONALADDRESS2;
                person.BUSINESS_CITY = person.PERSONAL_CITY;
                person.BUSINESS_COUNTRY_ID = person.PERSONAL_COUNTRY_ID;
                person.BUSINESS_STATE_ID = person.BUSINESS_STATE_ID;
                person.BUSINESS_EMAIL = person.PERSONAL_EMAIL;
                person.BUSINESS_MOBILE = person.PERSONAL_MOBILE;
                person.BUSINESS_PHONE = person.PERSONAL_PHONE;
                person.BUSINESS_POSTCODE = person.PERSONAL_POSTCODE;
                person.BUSINESS_FAX = person.PERSONAL_FAX;
                person.BUSINESS_WEB_ADD = person.PERSONAL_WEB_ADD;
            }
            StateHasChanged();
        }
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        protected bool isLoading = true;
        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            if (!Security.IsAuthenticated())
            {
                UriHelper.NavigateTo("Login", true);
            }
            else
            {
                isLoading = true;
                StateHasChanged();
                await Task.Delay(1);
                await Load();
                isLoading = false;
                StateHasChanged();
            }

        }

        protected IEnumerable<Clear.Risk.Models.ClearConnection.PersonSite> personSite { get; set; }

        string emailRegex = @"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$";
        int? selectedSite;

        protected async System.Threading.Tasks.Task Load()
        {
            var clearConnectionGetPeopleResult = await ClearConnection.GetPeople();
            getPeopleResult = clearConnectionGetPeopleResult;

            var clearConnectionGetStatesResult = await ClearConnection.GetStates();
            getStatesResult = clearConnectionGetStatesResult;

            var clearConnectionGetCountriesResult = await ClearConnection.GetCountries();
            getCountriesResult = clearConnectionGetCountriesResult;

            var clearConnectionGetPersonTypesResult = await ClearConnection.GetPersonTypes();
            getPersonTypesResult = clearConnectionGetPersonTypesResult;

            var personSiteresut = await ClearConnection.GetPersonSites(Security.getCompanyId(), new Query());
            personSite = personSiteresut;

            var clearConnectionGetApplicencesResult = await ClearConnection.GetApplicences();
            getApplicencesResult = clearConnectionGetApplicencesResult;

            var company = await ClearConnection.GetPersonByPersonId(Security.getCompanyId());

            selectedSite = personSiteresut.Where(i => i.IS_DEFAULT == true).Count() > 0 ? Convert.ToInt32(personSiteresut.Where(i => i.IS_DEFAULT == true).Select(i => i.PERSON_SITE_ID).Single()) : 0;


            person = new Clear.Risk.Models.ClearConnection.Person()
            {

                COMPANYTYPE = 3,
                PARENT_PERSON_ID = Security.getCompanyId(),
                CURRENCY_ID = company.CURRENCY_ID,
                PERSONAL_COUNTRY_ID = null,
                PERSONAL_STATE_ID = null,
                BUSINESS_COUNTRY_ID = null,
                BUSINESS_STATE_ID = null,
                CREATED_DATE = DateTime.Now,
                CREATOR_ID = Security.getUserId(),
                UPDATED_DATE = DateTime.Now,
                UPDATER_ID = Security.getUserId(),
                ISMANAGER = false,
                WORK_SITE_ID = selectedSite
            };



            if (Security.IsInRole("System Administrator"))
            {
                var man = await ClearConnection.GetEmployee(new Query() { Filter = "i => i.ISMANAGER == true" });
                Managers = man.ToList();
            }
            else
            {
                var man = await ClearConnection.GetEmployee(Security.getCompanyId(), new Query() { Filter = "i => i.ISMANAGER == true" });
                Managers = man.ToList();
                company.FIRST_NAME = "Company Admin (" + company.FIRST_NAME;
                company.LAST_NAME = company.LAST_NAME + ")";
                Managers.Insert(0, company);
            }

        }

        IEnumerable<Clear.Risk.Models.ClearConnection.Person> _getPeopleResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.Person> getPeopleResult
        {
            get
            {
                return _getPeopleResult;
            }
            set
            {
                if (!object.Equals(_getPeopleResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getPeopleResult", NewValue = value, OldValue = _getPeopleResult };
                    _getPeopleResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }


        IEnumerable<Clear.Risk.Models.ClearConnection.State> _getStatesResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.State> getStatesResult
        {
            get
            {
                return _getStatesResult;
            }
            set
            {
                if (!object.Equals(_getStatesResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getStatesResult", NewValue = value, OldValue = _getStatesResult };
                    _getStatesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.Country> _getCountriesResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.Country> getCountriesResult
        {
            get
            {
                return _getCountriesResult;
            }
            set
            {
                if (!object.Equals(_getCountriesResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getCountriesResult", NewValue = value, OldValue = _getCountriesResult };
                    _getCountriesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.PersonType> _getPersonTypesResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.PersonType> getPersonTypesResult
        {
            get
            {
                return _getPersonTypesResult;
            }
            set
            {
                if (!object.Equals(_getPersonTypesResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getPersonTypesResult", NewValue = value, OldValue = _getPersonTypesResult };
                    _getPersonTypesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.Applicence> _getApplicencesResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.Applicence> getApplicencesResult
        {
            get
            {
                return _getApplicencesResult;
            }
            set
            {
                if (!object.Equals(_getApplicencesResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getApplicencesResult", NewValue = value, OldValue = _getApplicencesResult };
                    _getApplicencesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        protected async System.Threading.Tasks.Task ButtonBackToList(MouseEventArgs args)
        {
            UriHelper.NavigateTo("employees");
        }




        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.Person args)
        {
            isLoading = true;
            StateHasChanged();
            await Task.Delay(1);

            try
            {
                if (args.ISMANAGER == false && args.ASSIGNED_TO_ID == null)
                {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Please designate the employee as a manager or select a manager for the employee.", 180000);
                    isLoading = false;
                    return;
                }


                args.BUSINESS_EMAIL = args.PERSONAL_EMAIL;
                args.PERSONAL_MOBILE = args.BUSINESS_MOBILE;
                args.PERSONAL_PHONE = args.BUSINESS_PHONE;

                args.ACTIVATED = false;

                //Random r = new Random();
                //args.ACTCODE = r.Next(10000000, 99999999);

                var user = await ClearConnection.GetPersonByPersonEmail(args.PERSONAL_EMAIL);

                if (user == null)
                {

                    if (args.PersonRoles == null)
                        args.PersonRoles = new List<PersonRole>();

                    args.PersonRoles.Add(new PersonRole
                    {
                        ROLE_ID = 3
                    });
                    args.ACTIVATED = true;
                    args.PASSWORDHASH = HashPassword(args.PASSWORDHASH);
                    var securityCreateUserResult = await ClearConnection.CreatePerson(args);



                    Smtpsetup smtpsetup = await ClearConnection.GetSmtpsetup();
                    if (smtpsetup != null)
                    {
                        var mimeMessage = new MimeMessage();
                        mimeMessage.From.Add(new MailboxAddress(smtpsetup.SMTP_MAIL_FROM));
                        mimeMessage.To.Add(new MailboxAddress(args.BUSINESS_EMAIL));


                        mimeMessage.Subject = "Welcome to Clear";
                        string emailbody = await EmailBody(securityCreateUserResult);
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
                                NotificationService.Notify(NotificationSeverity.Success, $"Success", $"Mail has been sent successfully !!!, ", 180000);
                                await client.DisconnectAsync(true);
                            }
                        }
                        catch (Exception ex)
                        {
                            NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Sorry, error occured this time sending your message.!, ", 180000);
                        }
                    }

                    //DialogService.Close(person);

                    if (securityCreateUserResult.PERSON_ID > 0)
                    {
                        NotificationService.Notify(NotificationSeverity.Success, $"EMPLOYEE SAVED", $"Employee has been saved successfully.", 180000);
                        UriHelper.NavigateTo("employees");
                    }
                    else
                    {
                        NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Sorry, unable to create a new employee.", 180000);
                    }
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Employee Email Address Already Exist, ", 180000);
                }
            }
            catch (System.Exception clearConnectionCreatePersonException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new Employee, " + clearConnectionCreatePersonException.Message, 180000);
            }
            finally
            {
                isLoading = false;
                StateHasChanged();

            }

        }
        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            UriHelper.NavigateTo("employees");
        }

        protected string HashPassword(string password)
        {
            using (var algorithm = SHA256.Create())
            {
                var hash = algorithm.ComputeHash(Encoding.ASCII.GetBytes(password));

                return Encoding.ASCII.GetString(hash);
            }
        }

        string pstyle = "";
        string mstyle = "";
        bool disableButton = false;
        protected void ChangeMobilePhone(string args, string field)
        {
            char[] arr1 = args.ToCharArray();
            foreach (char c in arr1)
            {
                if (!" +-1234567890".Contains(c))
                {
                    NotificationService.Notify(NotificationSeverity.Error, "Error", "Entry is not valid. Please re-enter.", 180000);

                    if (field == "Mobile")
                    {
                        //person.BUSINESS_MOBILE = "";
                        mstyle = @"input[name=" + "BUSINESS_MOBILE" + "] { border-color: " + "red; color:" + "red" + " }";
                        disableButton = true;
                    }
                    else
                    {
                        //person.BUSINESS_PHONE = "";
                        pstyle = @"input[name=" + "BUSINESS_PHONE" + "] { border-color: " + "red; color:" + "red" + " }";
                        disableButton = true;
                    }
                    return;
                }
            }

            if (field == "Mobile")
                mstyle = "";
            else
                pstyle = "";

            if (!mstyle.Contains("red") && !pstyle.Contains("red"))
                disableButton = false;
        }

        protected async Task<string> EmailBody(Clear.Risk.Models.ClearConnection.Person pers)
        {
            string baseUrl = UriHelper.BaseUri;

            StringBuilder builder = new StringBuilder();
            var company = await ClearConnection.GetPersonByPersonId(Security.getCompanyId());
            builder.Append($@"<html>");
            builder.Append($@"<body>");
            //builder.Append($@"<link rel='stylesheet' href='https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css' />");
            builder.Append($@"<p style='font-family: Arial, Helvetica, sans-serif;font-size:16px'>");
            builder.Append($@"We are pleased to welcome you to the Clear Risk Assessment System. You've been registered to use the system by your company system admin <i>{company.FIRST_NAME}&nbsp;{company.LAST_NAME}.</i> Your logon details are given below and you can logon at ");
            builder.Append($@"<i><a href='www.clear-whs.com' style='color:red;text-decoration: none;'>www.clear-whs.com</a></i>");
            builder.Append($@"</p>");
            builder.Append($@"<br />");
            builder.Append($@"<p style='font-family: Arial, Helvetica, sans-serif;font-size:16px'>");
            builder.Append($@"Logon Credentials: <br />Userid: <span style='color:red'>{person.BUSINESS_EMAIL}</span><br />Password: <span style='color:red'>{person.ConfirmPassword}</span>");
            builder.Append($@"</p>");
            //builder.Append($@"<p style='font-family: Arial, Helvetica, sans-serif;font-size:16px'>");
            //builder.Append($@"Your account is not activated yet, to activate your account click ");
            //builder.Append($@"<b><a href='{baseUrl + "activate/" + pers.PERSON_ID + "/" + pers.ACTCODE }'>here.</a></b>");
            //builder.Append($@"</p>");
            builder.Append($@"<p style='font-family: Arial, Helvetica, sans-serif;font-size:16px'>");
            builder.Append($@"You can logon using either the Apple IOS App or the Andriod App and you can download these Apps from the respective store. <br />");
            builder.Append($@"Link to iOS <br />");
            builder.Append($@"Link to Android");
            builder.Append($@"</p>");
            builder.Append($@"<p style='font-family: Arial, Helvetica, sans-serif;font-size:16px'>");
            builder.Append($@"Using the system is very easy. Once you login to the system, the app will be updated with work orders, and Risk Assessments that have been assigned to you. The Work Order is assigned to a work location and it will not become active until you are physically present at the site location.");
            builder.Append($@"</p>");
            builder.Append($@"<p style='font-family: Arial, Helvetica, sans-serif;font-size:16px'>");
            builder.Append($@"Once the app detects that you are within range of a work site location it will ask you whether you wish to be classified as &#8220;On-Site&#8221; and it will then &#8220;Activate&#8221; the Work Order. Once the Work Order is &#8220;Active&#8221;, you will be able to access any Risk Assessment, Instruction Document and/or Survey that has been sent to you. You must read the Risk Assessment, Instruction Document and then indicate that you have read them by signing for each. This signature is date and time stamped. You must also complete the Survey if one has been assigned to you. Only after you have read and signed and completed the survey will the Work Order be marked as completed.");
            builder.Append($@"</p>");
            builder.Append($@"<p style='font-family: Arial, Helvetica, sans-serif;font-size:16px'>");
            builder.Append($@"The App will then automatically transfer these items back to the server for storage and analysis. You will be automatically marked as &#8220;Off - Site&#8221; when you leave the work location or you can choose to mark yourself as &#8220;Off-Site&#8221;. ");
            builder.Append($@"</p>");
            builder.Append($@"<p style='font-family: Arial, Helvetica, sans-serif;font-size:16px'>");
            builder.Append($@"If you have any questions then please contact your Company Admin user who can assist you or who will contact us for support.");
            builder.Append($@"</p><br />");

            builder.Append($@"<a href='mailto:Support@Clear-WHS.com'>Support@Clear-WHS.com</a>");

            builder.Append($@"</body>");
            builder.Append($@"</html>");
            return builder.ToString();
        }


        protected async Task SameAsPersonal1(object value, string name)
        {
            if (value != null)
            {
                var item = personSite.Where(a => a.PERSON_SITE_ID == (int)value).FirstOrDefault();
                if (item != null)
                {
                    person.BUSINESS_ADDRESS1 = item.SITE_ADDRESS1;
                    person.BUSINESS_ADDRESS2 = item.SITE_ADDRESS2;
                    person.BUSINESS_CITY = item.CITY;
                    person.BUSINESS_COUNTRY_ID = item.COUNTRY_ID;
                    person.BUSINESS_STATE_ID = item.STATE_ID;
                    //person.BUSINESS_EMAIL = person.PERSONAL_EMAIL;
                    //person.BUSINESS_MOBILE = person.PERSONAL_MOBILE;
                    //person.BUSINESS_PHONE = person.PERSONAL_PHONE;
                    person.BUSINESS_POSTCODE = item.POST_CODE;
                    //person.BUSINESS_FAX = person.PERSONAL_FAX;
                    //person.BUSINESS_WEB_ADD = person.PERSONAL_WEB_ADD;
                }
            }
            else
            {
                person.BUSINESS_ADDRESS1 = "";
                person.BUSINESS_ADDRESS2 = "";
                person.BUSINESS_CITY = "";
                person.BUSINESS_COUNTRY_ID = null;
                person.BUSINESS_STATE_ID = null;
                person.BUSINESS_POSTCODE = "";
            }
            StateHasChanged();
        }
    }
}
