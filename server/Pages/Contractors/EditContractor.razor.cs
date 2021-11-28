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

namespace Clear.Risk.Pages.Contractors
{
    public partial class EditContractor: ComponentBase
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

        [Parameter]
        public dynamic PersonId { get; set; }
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
                    var args = new PropertyChangedEventArgs() { Name = "getCountriesResult", NewValue = value, OldValue = _getDesigation };
                    _getDesigation = value;
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

            person = await ClearConnection.GetPersonByPersonId(int.Parse(PersonId));

            var statusresult = await ClearConnection.GetStatusMasters();
            getStatusResult = statusresult;

            var warningLevelResult = await ClearConnection.GetWarningLevels();
            getWarningResult = warningLevelResult;

            var entityStatusResult = await ClearConnection.GetEntityStatuses();
            getEntityResult = entityStatusResult;

            var clearConnectionGetDesigationsResult = await ClearConnection.GetDesigations();
            getDesigationResult = clearConnectionGetDesigationsResult;

            var clearRiskGetPersonContactsResult = await ClearConnection.GetPersonContacts(new Query() { Filter = $@"i => i.PERSON_ID == {PersonId}" });
            getPersonContactsResult = clearRiskGetPersonContactsResult;
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

        IEnumerable<Clear.Risk.Models.ClearConnection.StatusMaster> _getStatusResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.StatusMaster> getStatusResult
        {
            get
            {
                return _getStatusResult;
            }
            set
            {
                if (!object.Equals(_getStatusResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getStatusResult", NewValue = value, OldValue = _getStatusResult };
                    _getStatusResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }



        IEnumerable<Clear.Risk.Models.ClearConnection.WarningLevel> _getWarningResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.WarningLevel> getWarningResult
        {
            get
            {
                return _getWarningResult;
            }
            set
            {
                if (!object.Equals(_getWarningResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getWarningResult", NewValue = value, OldValue = _getWarningResult };
                    _getWarningResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.EntityStatus> _getEntityResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.EntityStatus> getEntityResult
        {
            get
            {
                return _getEntityResult;
            }
            set
            {
                if (!object.Equals(_getEntityResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getEntityResult", NewValue = value, OldValue = _getEntityResult };
                    _getEntityResult = value;
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


                var securityCreateUserResult = await ClearConnection.UpdatePerson(args.PERSON_ID, args);
                NotificationService.Notify(NotificationSeverity.Info, $"Success", $"Client Information update successfully!, ");


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

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            UriHelper.NavigateTo("Contractors");
        }

        protected string HashPassword(string password)
        {
            using (var algorithm = SHA256.Create())
            {
                var hash = algorithm.ComputeHash(Encoding.ASCII.GetBytes(password));

                return Encoding.ASCII.GetString(hash);
            }
        }

        #region Client Contact
        protected RadzenGrid<PersonContact> gridContact;

        IEnumerable<PersonContact> _getPersonContactsResult;
        protected IEnumerable<PersonContact> getPersonContactsResult
        {
            get
            {
                return _getPersonContactsResult;
            }
            set
            {
                if (!object.Equals(_getPersonContactsResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getPersonContactsResult", NewValue = value, OldValue = _getPersonContactsResult };
                    _getPersonContactsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected async System.Threading.Tasks.Task ButtonclientClick(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<Clear.Risk.Pages.Contacts.AddPersonContact>("Add Client Contact", new Dictionary<string, object>() { { "PersonId", person.PERSON_ID } }, new DialogOptions() { Width = $"{800}px" });
            gridContact.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridContactRowSelect(PersonContact args)
        {
            var dialogResult = await DialogService.OpenAsync<Clear.Risk.Pages.Contacts.EditPersonContact>("Edit Person Contact", new Dictionary<string, object>() { { "PERSON_CONTACT_ID", args.PERSON_CONTACT_ID } }, new DialogOptions() { Width = $"{800}px" });
            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridContactDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var clearRiskDeletePersonContactResult = await ClearConnection.DeletePersonContact(int.Parse($"{data.PERSON_CONTACT_ID}"));
                    if (clearRiskDeletePersonContactResult != null)
                    {
                        gridContact.Reload();
                    }
                }
            }
            catch (System.Exception clearRiskDeletePersonContactException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete PersonContact");
            }
        }
        #endregion


    }
}
