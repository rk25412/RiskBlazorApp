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
using Syncfusion.Blazor.Spinner;

namespace Clear.Risk.Pages
{
    public partial class Signup : ComponentBase
    {

        protected SfSpinner SpinnerObj;
        private string target { get; set; } = "#content1";
        //protected RadzenTemplateForm<Clear.Risk.Models.ClearConnection.Person> Containner;
        protected RadzenContent content1;



        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager UriHelper { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }

        [Inject]
        protected ClearConnectionService ClearConnection { get; set; }

        Clear.Risk.Models.ClearConnection.Person _person;
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
        protected bool IsLoading { get; set; }
        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            IsLoading = true;
            await InvokeAsync(StateHasChanged);
            await Load();
            IsLoading = false;
            await InvokeAsync(StateHasChanged);
        }
        protected async System.Threading.Tasks.Task Load()
        {
            person = new Clear.Risk.Models.ClearConnection.Person()
            {



                CREATED_DATE = DateTime.Now,

                UPDATED_DATE = DateTime.Now,


            };
            //Signup Credit
            person.CURRENT_BALANCE = 1;
            PersonType personType = await ClearConnection.GetPersonTypeByRegistration();
            if (personType != null)
                person.COMPANYTYPE = personType.PERSON_TYPE_ID;

            person.PERSON_STATUS = 0;




            var clearConnectionGetStatesResult = await ClearConnection.GetStates(new Query() { Expand = "Country" });
            getStatesResult = clearConnectionGetStatesResult;

            var clearConnectionGetCountriesResult = await ClearConnection.GetCountries(new Query());
            getCountriesResult = clearConnectionGetCountriesResult;

        }

        string emailRegex = @"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$";
        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.Person args)
        {
            IsLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {


                string password = string.Empty;
                var user = await ClearConnection.GetPersonByPersonEmail(args.PERSONAL_EMAIL);
                if (user == null)
                {
                    args.BUSINESS_ADDRESS1 = args.PERSONALADDRESS1;
                    args.BUSINESS_ADDRESS2 = args.PERSONALADDRESS2;
                    args.BUSINESS_CITY = args.PERSONAL_CITY;
                    args.BUSINESS_POSTCODE = args.PERSONAL_POSTCODE;
                    args.BUSINESS_EMAIL = args.PERSONAL_EMAIL;
                    args.APPLICENCE_STARTDATE = DateTime.Now;
                    args.BUSINESS_COUNTRY_ID = args.PERSONAL_COUNTRY_ID;
                    args.BUSINESS_STATE_ID = args.PERSONAL_STATE_ID;
                    args.BUSINESS_PHONE = args.PERSONAL_PHONE;
                    args.BUSINESS_MOBILE = args.BUSINESS_MOBILE;
                    password = args.PASSWORDHASH;
                    //Create Transaction

                    Applicence applicence = await ClearConnection.GetApplicenceByCountry((int)args.PERSONAL_COUNTRY_ID);

                    if (applicence == null)
                        applicence = await ClearConnection.GetDefaultApplicence();

                    if (applicence != null)
                    {
                        person.APPLICENCEID = applicence.APPLICENCEID;
                        person.CURRENCY_ID = applicence.CURRENCY_ID;
                        person.CURRENT_BALANCE = applicence.DEFAULT_CREDIT;
                    }

                    if (args.Contacts == null)
                        args.Contacts = new List<PersonContact>();

                    args.Contacts.Add(new PersonContact
                    {
                        CREATED_DATE = DateTime.Now,
                        UPDATED_DATE = DateTime.Now,
                        CREATOR_ID = Security.getUserId(),
                        UPDATER_ID = Security.getUserId(),
                        IS_DELETED = false,
                        CONTACT_STATUS_ID = 1,
                        FIRST_NAME = args.FIRST_NAME,
                        MIDDLE_NAME = args.MIDDLE_NAME,
                        LAST_NAME = args.LAST_NAME,
                        ISPRIMARY = true,
                        DESIGNATION_ID = args.DESIGNATION_ID,
                        GENDER = 1,
                        PERSONALADDRESS1 = args.PERSONALADDRESS1,
                        PERSONALADDRESS2 = args.PERSONALADDRESS2,
                        PERSONAL_CITY = args.PERSONAL_CITY,
                        PERSONAL_COUNTRY_ID = (int)args.PERSONAL_COUNTRY_ID,
                        PERSONAL_STATE_ID = args.PERSONAL_STATE_ID,
                        PERSONAL_EMAIL = args.PERSONAL_EMAIL,
                        PERSONAL_PHONE = args.PERSONAL_PHONE,
                        PERSONAL_MOBILE = args.PERSONAL_MOBILE,
                        PERSONAL_POSTCODE = args.PERSONAL_POSTCODE,
                        BUSINESS_ADDRESS1 = args.BUSINESS_ADDRESS1,
                        BUSINESS_ADDRESS2 = args.BUSINESS_ADDRESS2,
                        BUSINESS_CITY = args.BUSINESS_CITY,
                        BUSINESS_COUNTRY_ID = args.BUSINESS_COUNTRY_ID,
                        BUSINESS_STATE_ID = args.BUSINESS_STATE_ID,
                        BUSINESS_EMAIL = args.BUSINESS_EMAIL,
                        BUSINESS_MOBILE = args.BUSINESS_MOBILE,
                        BUSINESS_PHONE = args.BUSINESS_PHONE,
                        BUSINESS_POSTCODE = args.BUSINESS_POSTCODE,
                    });

                    args.ACTIVATED = false;
                    Random r = new Random();
                    args.ACTCODE = r.Next(10000000, 99999999);


                    if (args.CompanyAccountTransactions == null)
                        args.CompanyAccountTransactions = new List<CompanyAccountTransaction>();

                    args.CompanyAccountTransactions.Add(new CompanyAccountTransaction
                    {
                        TRANSACTION_DATE = DateTime.Now,
                        PAYMENT_AMOUNT = 0,
                        DEPOSITE_AMOUNT = 10,
                        DESCRIPTION = "Signup Credit",
                        TRXTYPE = "DTD",
                        CREATED_DATE = DateTime.Now,
                        UPDATED_DATE = DateTime.Now,
                        CURRENCY_ID = args.CURRENCY_ID != null ? (int)args.CURRENCY_ID : 0,
                    });

                    if (args.PersonRoles == null)
                        args.PersonRoles = new List<PersonRole>();

                    args.PersonRoles.Add(new PersonRole
                    {
                        ROLE_ID = 2
                    });
                    args.PASSWORDHASH = HashPassword(args.PASSWORDHASH);
                    var securityCreateUserResult = await ClearConnection.CreatePerson(args);


                    //UriHelper.NavigateTo("Clients");

                    Smtpsetup smtpsetup = await ClearConnection.GetSmtpsetup();
                    if (smtpsetup != null)
                    {
                        var mimeMessage = new MimeMessage();
                        mimeMessage.From.Add(new MailboxAddress(smtpsetup.SMTP_MAIL_FROM));
                        mimeMessage.To.Add(new MailboxAddress(args.BUSINESS_EMAIL));
                        //string breakTag = "<br/>";
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
                                NotificationService.Notify(NotificationSeverity.Success, $"Success", $"Mail has been sent successfully !!!, ");
                                await client.DisconnectAsync(true);
                            }
                        }
                        catch (Exception ex)
                        {
                            NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Sorry, error occured this time sending your message.!");
                            IsLoading = false;
                            StateHasChanged();
                        }
                    }
                    IsLoading = false;
                    StateHasChanged();
                    UriHelper.NavigateTo("ThankYou");
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Company Email Address Already Exist!, ");
                    IsLoading = false;
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", ex.Message);
                IsLoading = false;
                StateHasChanged();
            }

            //DialogService.Close();
            //await JSRuntime.InvokeAsync<string>("window.history.back");
        }

        protected string HashPassword(string password)
        {
            using (var algorithm = SHA256.Create())
            {
                var hash = algorithm.ComputeHash(Encoding.ASCII.GetBytes(password));

                return Encoding.ASCII.GetString(hash);
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close();
            await JSRuntime.InvokeAsync<string>("window.history.back");
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

        char[] nums = { '+', '-', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };

        public bool isExist(char x, char[] arr)
        {
            foreach (char c in arr)
            {
                if (x == c)
                    return true;
            }

            return false;
        }

        protected void ChangeMobilePhone(string args, string field)
        {
            char[] arr1 = args.ToCharArray();
            foreach (char c in arr1)
            {
                if (!isExist(c, nums))
                {
                    NotificationService.Notify(NotificationSeverity.Error, "Error", "Only Numbers are allowed.", 180000);

                    if (field == "Mobile")
                        person.PERSONAL_MOBILE = "";
                    else
                        person.PERSONAL_PHONE = "";

                    return;
                }
            }
        }

        protected string EmailBody(Clear.Risk.Models.ClearConnection.Person pers)
        {
            string baseUrl = UriHelper.BaseUri;
            StringBuilder builder = new StringBuilder();
            builder.Append($@"<p style='font-family: Arial, Helvetica, sans-serif;font-size:16px'>");
            builder.Append($@"We are pleased to welcome you to the Clear Risk Assessment system. Your logon details are given below and you can now logon at ");
            builder.Append($@"<i><a href='www.clear-whs.com' style='color:red;text-decoration: none;'>www.clear-whs.com</a></i>");
            builder.Append($@"</p>");
            builder.Append($@"<p style='font-family: Arial, Helvetica, sans-serif;font-size:16px'>");
            builder.Append($@"Logon Credentials: <br />Userid: <span style='color:red'>{person.PERSONAL_EMAIL}</span><br />Password: <span style='color:red'>{person.ConfirmPassword}</span>");
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

    }
}
