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
    public partial class EditClient : ComponentBase
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

        [Inject]
        protected WorkOrderService ClearRisk { get; set; }

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

        protected IList<WorkOrder> getWorkOrdersResult = new List<WorkOrder>();



        string emailRegex = @"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$";

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
            var clearConnectionGetDesigationsResult = await ClearConnection.GetDesigations();
            getDesigationResult = clearConnectionGetDesigationsResult;


            var clearRiskGetPersonSitesResult = await ClearConnection.GetPersonSites(new Query() { Filter = $@"i => i.PERSON_ID == {PersonId}" });
            getPersonSitesResult = clearRiskGetPersonSitesResult.Select(x => new PersonSite
            {
                PERSON_SITE_ID = x.PERSON_SITE_ID,
                SITE_NAME = x.SITE_NAME,
                BUILDING_NAME = x.BUILDING_NAME,
                FLOOR = x.FLOOR,
                ROOMNO = x.ROOMNO,
                SITE_ADDRESS1 = x.SITE_ADDRESS1,
                SITE_ADDRESS2 = x.SITE_ADDRESS2,
                SITE_ADDRESS3 = x.SITE_ADDRESS3,
                CITY = x.CITY,
                State = x.State,
                POST_CODE = x.POST_CODE

            }).ToList();

            var clearConnectionGetAssesmentsResult = await ClearConnection.GetAssesments(new Query() { Filter = $@"i => i.CLIENTID == {PersonId}" });
            getAssesmentsResult = clearConnectionGetAssesmentsResult.Select(x => new Clear.Risk.Models.ClearConnection.Assesment
            {
                REFERENCENUMBER = x.REFERENCENUMBER,
                RISKASSESSMENTNO = x.RISKASSESSMENTNO,
                ASSESMENTDATE = x.ASSESMENTDATE,
                SCHEDULE_TIME = x.SCHEDULE_TIME,
                StatusMaster = x.StatusMaster
            }).ToList();

            var clearRiskGetPersonContactsResult = await ClearConnection.GetPersonContacts(new Query() { Filter = $@"i => i.PERSON_ID == {PersonId}" });
            getPersonContactsResult = clearRiskGetPersonContactsResult.Select(x => new PersonContact
            {
                PERSON_CONTACT_ID = x.PERSON_CONTACT_ID,
                Company = x.Company,
                FIRST_NAME = x.FIRST_NAME,
                MIDDLE_NAME = x.MIDDLE_NAME,
                LAST_NAME = x.LAST_NAME,
                PERSONAL_CITY = x.PERSONAL_CITY,
                PersonalCountry = x.PersonalCountry,
                PERSONALADDRESS1 = x.PERSONALADDRESS1,
                PERSONAL_MOBILE = x.PERSONAL_MOBILE,
                PERSONAL_EMAIL = x.PERSONAL_EMAIL

            }).ToList();

            person = await ClearConnection.GetPersonByPersonId(int.Parse(PersonId));

            var clearRiskGetWorkOrdersResult = await ClearRisk.GetWorkOrders(new Query() { Filter = $@"i => i.CLIENT_ID == {PersonId}" });
            getWorkOrdersResult = clearRiskGetWorkOrdersResult.Select(x => new WorkOrder
            {
                WORK_ORDER_ID = x.WORK_ORDER_ID,
                WORK_ORDER_NUMBER = x.WORK_ORDER_NUMBER,
                DUE_DATE = x.DUE_DATE,
                OrderStatus = x.OrderStatus ?? null,
                PriorityMaster = x.PriorityMaster ?? null,
                WorkOrderType = x.WorkOrderType ?? null,
                EntityStatus = x.EntityStatus ?? null,
                WarningLevel = x.WarningLevel ?? null,
                DATE_RAISED = x.DATE_RAISED

            }).ToList();

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

        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.Person args)
        {
            isLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {


                var securityCreateUserResult = await ClearConnection.UpdatePerson(args.PERSON_ID, args);
                NotificationService.Notify(NotificationSeverity.Info, $"Success", $"Client Information Updated Successfully.");


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

        #region Client Work Site
        //protected RadzenGrid<PersonSite> grid0;

        protected IList<PersonSite> getPersonSitesResult = new List<PersonSite>();


        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddPersonSite>("Add Work Site", new Dictionary<string, object>() { { "ClientId", person.PERSON_ID } }, new DialogOptions() { Width = $"{800}px" });


            await InvokeAsync(() => { StateHasChanged(); });
            var clearRiskGetPersonSitesResult = await ClearConnection.GetPersonSites(new Query() { Filter = $@"i => i.PERSON_ID == {PersonId}" });
            getPersonSitesResult = clearRiskGetPersonSitesResult.Select(x => new PersonSite
            {
                PERSON_SITE_ID = x.PERSON_SITE_ID,
                SITE_NAME = x.SITE_NAME,
                BUILDING_NAME = x.BUILDING_NAME,
                FLOOR = x.FLOOR,
                ROOMNO = x.ROOMNO,
                SITE_ADDRESS1 = x.SITE_ADDRESS1,
                SITE_ADDRESS2 = x.SITE_ADDRESS2,
                SITE_ADDRESS3 = x.SITE_ADDRESS3,
                CITY = x.CITY,
                State = x.State,
                POST_CODE = x.POST_CODE,
                LATITUDE = x.LATITUDE,
                LONGITUDE = x.LONGITUDE

            }).ToList();
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(PersonSite args)
        {
            var dialogResult = await DialogService.OpenAsync<EditPersonSite>("Edit Person Site", new Dictionary<string, object>() { { "PERSON_SITE_ID", args.PERSON_SITE_ID } }, new DialogOptions() { Width = $"{800}px" });
            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var clearRiskDeletePersonSiteResult = await ClearConnection.DeletePersonSite(data.PERSON_SITE_ID);
                    if (clearRiskDeletePersonSiteResult != null)
                    {
                        getPersonSitesResult.Remove(getPersonSitesResult.FirstOrDefault(x => x.PERSON_SITE_ID == data.PERSON_SITE_ID));
                    }
                }
            }
            catch (System.Exception clearRiskDeletePersonSiteException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete PersonSite");
            }
        }
        protected async System.Threading.Tasks.Task GridEditButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<EditPersonSite>("Edit Work Site", new Dictionary<string, object>() { { "PERSON_SITE_ID", data.PERSON_SITE_ID } }, new DialogOptions() { Width = $"{800}px" });
            await InvokeAsync(() => { StateHasChanged(); });
            var clearRiskGetPersonSitesResult = await ClearConnection.GetPersonSites(new Query() { Filter = $@"i => i.PERSON_ID == {PersonId}" });
            getPersonSitesResult = clearRiskGetPersonSitesResult.Select(x => new PersonSite
            {
                PERSON_SITE_ID = x.PERSON_SITE_ID,
                SITE_NAME = x.SITE_NAME,
                BUILDING_NAME = x.BUILDING_NAME,
                FLOOR = x.FLOOR,
                ROOMNO = x.ROOMNO,
                SITE_ADDRESS1 = x.SITE_ADDRESS1,
                SITE_ADDRESS2 = x.SITE_ADDRESS2,
                SITE_ADDRESS3 = x.SITE_ADDRESS3,
                CITY = x.CITY,
                State = x.State,
                POST_CODE = x.POST_CODE,
                LATITUDE = x.LATITUDE,
                LONGITUDE = x.LONGITUDE

            }).ToList();
        }
        #endregion

        #region Assesment History
        protected RadzenGrid<Clear.Risk.Models.ClearConnection.Assesment> grid1;

        protected IList<Clear.Risk.Models.ClearConnection.Assesment> getAssesmentsResult = new List<Clear.Risk.Models.ClearConnection.Assesment>();

        #endregion

        #region Client Contact
        protected RadzenGrid<PersonContact> gridContact;

        protected IList<PersonContact> getPersonContactsResult = new List<PersonContact>();


        protected async System.Threading.Tasks.Task ButtonclientClick(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<Clear.Risk.Pages.Contacts.AddPersonContact>("Add Client Contact", new Dictionary<string, object>() { { "PersonId", person.PERSON_ID } }, new DialogOptions() { Width = $"{800}px" });

            await InvokeAsync(() => { StateHasChanged(); });
            var clearRiskGetPersonContactsResult = await ClearConnection.GetPersonContacts(new Query() { Filter = $@"i => i.PERSON_ID == {PersonId}" });
            getPersonContactsResult = clearRiskGetPersonContactsResult.Select(x => new PersonContact
            {
                PERSON_CONTACT_ID = x.PERSON_CONTACT_ID,
                Company = x.Company,
                FIRST_NAME = x.FIRST_NAME,
                MIDDLE_NAME = x.MIDDLE_NAME,
                LAST_NAME = x.LAST_NAME,
                PERSONAL_CITY = x.PERSONAL_CITY,
                PersonalCountry = x.PersonalCountry,
                PERSONALADDRESS1 = x.PERSONALADDRESS1,
                PERSONAL_MOBILE = x.PERSONAL_MOBILE,
                PERSONAL_EMAIL = x.PERSONAL_EMAIL

            }).ToList();

        }

        protected async System.Threading.Tasks.Task GridContactRowSelect(PersonContact args)
        {
            var dialogResult = await DialogService.OpenAsync<Clear.Risk.Pages.Contacts.EditPersonContact>("Edit Client Contact", new Dictionary<string, object>() { { "PERSON_CONTACT_ID", args.PERSON_CONTACT_ID } }, new DialogOptions() { Width = $"{800}px" });
            await InvokeAsync(() => { StateHasChanged(); });
            var clearRiskGetPersonContactsResult = await ClearConnection.GetPersonContacts(new Query() { Filter = $@"i => i.PERSON_ID == {PersonId}" });
            getPersonContactsResult = clearRiskGetPersonContactsResult.Select(x => new PersonContact
            {
                PERSON_CONTACT_ID = x.PERSON_CONTACT_ID,
                Company = x.Company,
                FIRST_NAME = x.FIRST_NAME,
                MIDDLE_NAME = x.MIDDLE_NAME,
                LAST_NAME = x.LAST_NAME,
                PERSONAL_CITY = x.PERSONAL_CITY,
                PersonalCountry = x.PersonalCountry,
                PERSONALADDRESS1 = x.PERSONALADDRESS1,
                PERSONAL_MOBILE = x.PERSONAL_MOBILE,
                PERSONAL_EMAIL = x.PERSONAL_EMAIL

            }).ToList();
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


        protected async System.Threading.Tasks.Task GridContactDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var clearRiskDeletePersonContactResult = await ClearConnection.DeletePersonContact(int.Parse($"{data.PERSON_CONTACT_ID}"));
                    if (clearRiskDeletePersonContactResult != null)
                    {
                        getPersonContactsResult.Remove(getPersonContactsResult.FirstOrDefault(x => x.PERSON_CONTACT_ID == data.PERSON_CONTACT_ID));
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
