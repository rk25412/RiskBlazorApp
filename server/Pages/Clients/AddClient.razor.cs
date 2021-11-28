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

namespace Clear.Risk.Pages.Clients
{
    public partial class AddClient : ComponentBase
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
                person.BUSINESS_STATE_ID = person.PERSONAL_STATE_ID;
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
        string emailRegex = @"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$";
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

            var clearConnectionGetApplicencesResult = await ClearConnection.GetApplicences();
            getApplicencesResult = clearConnectionGetApplicencesResult;

            var clearConnectionGetDesigationsResult = await ClearConnection.GetDesigations();
            getDesigationResult = clearConnectionGetDesigationsResult;

            var company = await ClearConnection.GetPersonByPersonId(Security.getCompanyId());

            person = new Clear.Risk.Models.ClearConnection.Person()
            {

                COMPANYTYPE = 4,
                PARENT_PERSON_ID = Security.getCompanyId(),
                CURRENCY_ID = company.CURRENCY_ID,
                CREATED_DATE = DateTime.Now,
                CREATOR_ID = Security.getUserId(),
                UPDATED_DATE = DateTime.Now,
                UPDATER_ID = Security.getUserId(),
                PERSONAL_COUNTRY_ID = (int)company.PERSONAL_COUNTRY_ID

            };
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
        IEnumerable<Clear.Risk.Models.ClearConnection.Desigation> _getDesigation;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.Desigation> getDesigationResult
        {
            get
            {
                return _getDesigation;
            }
            set
            {
                if (!object.Equals(_getDesigation, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getDesigationResult", NewValue = value, OldValue = _getDesigation };
                    _getDesigation = value;
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

        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.Person args)
        {
            isLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {

                var user = await ClearConnection.GetPersonByPersonEmail(args.PERSONAL_EMAIL);

                if (user == null)
                {
                    //Create Account

                    if (args.Contacts == null)
                        args.Contacts = new List<PersonContact>();

                    args.BUSINESS_PHONE = args.PERSONAL_PHONE;
                    args.BUSINESS_MOBILE = args.PERSONAL_MOBILE;

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

                    if (args.PersonRoles == null)
                        args.PersonRoles = new List<PersonRole>();

                    args.PersonRoles.Add(new PersonRole
                    {
                        ROLE_ID = 4
                    });
                    args.PASSWORDHASH = HashPassword(args.PASSWORDHASH);
                    var securityCreateUserResult = await ClearConnection.CreatePerson(args);

                    UriHelper.NavigateTo("edit-Client" + "/" + args.PERSON_ID.ToString());
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Client Email Address Already Exist!, ");
                }
            }
            catch (System.Exception clearConnectionCreatePersonException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new Client!, " + clearConnectionCreatePersonException.Message);
            }
            finally
            {
                isLoading = false;
                StateHasChanged();

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
                        mstyle = @"input[name=" + "PERSONAL_MOBILE" + "] { border-color: " + "red; color:" + "red" + " }";
                        disableButton = true;
                    }
                    else
                    {
                        //person.BUSINESS_PHONE = "";
                        pstyle = @"input[name=" + "PERSONAL_PHONE" + "] { border-color: " + "red; color:" + "red" + " }";
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



        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            UriHelper.NavigateTo("Clients");
        }
        protected async System.Threading.Tasks.Task ButtonBackToList(MouseEventArgs args)
        {
            UriHelper.NavigateTo("Clients");
        }

        protected string HashPassword(string password)
        {
            using (var algorithm = SHA256.Create())
            {
                var hash = algorithm.ComputeHash(Encoding.ASCII.GetBytes(password));

                return Encoding.ASCII.GetString(hash);
            }
        }
    }
}
