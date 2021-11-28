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
using Syncfusion.Blazor.Grids;
using System.Text.RegularExpressions;

namespace Clear.Risk.Pages.Employees
{
    public partial class EditEmployee : ComponentBase
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

        [Parameter]
        public dynamic tab { get; set; }

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

        protected IEnumerable<Clear.Risk.Models.ClearConnection.PersonSite> _personSite;
       protected IEnumerable<Clear.Risk.Models.ClearConnection.PersonSite> personSite
        {
            get
            {
                return _personSite;
            }
            set
            {
                if (!object.Equals(_personSite, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "personSite", NewValue = value, OldValue = _personSite };
                    _personSite = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        string emailRegex = @"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$";

        protected IEnumerable<Clear.Risk.Models.ClearConnection.Person> Managers { get; set; }
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
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        protected bool isLoading { get; set; }
        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            if (!Security.IsAuthenticated())
            {
                UriHelper.NavigateTo("Login", true);
            }
            else
            {

                await Load();


            }

        }

        [Inject]
        protected WorkOrderService ClearRisk { get; set; }

        [Inject]
        protected SurveyServices SurveyService { get; set; }
        protected async System.Threading.Tasks.Task Load()
        {
            isLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {

                //var clearConnectionGetPeopleResult = await ClearConnection.GetPeople();
                //getPeopleResult = clearConnectionGetPeopleResult;
                if (Security.IsInRole("System Administrator"))
                {
                    var personSiteresut = await ClearConnection.GetPersonSites(new Query());
                    personSite = personSiteresut;
                }
                else
                {
                    var personSiteresut = await ClearConnection.GetPersonSites(Security.getCompanyId(), new Query());
                    personSite = personSiteresut;
                    
                }
                
                var clearConnectionGetStatesResult = await ClearConnection.GetStates();
                getStatesResult = clearConnectionGetStatesResult;

                var clearConnectionGetCountriesResult = await ClearConnection.GetCountries();
                getCountriesResult = clearConnectionGetCountriesResult;

                var clearConnectionGetPersonTypesResult = await ClearConnection.GetPersonTypes();
                getPersonTypesResult = clearConnectionGetPersonTypesResult;

                var clearConnectionGetApplicencesResult = await ClearConnection.GetApplicences();
                getApplicencesResult = clearConnectionGetApplicencesResult;

                var clearRiskGetWorkOrdersResult = await ClearRisk.GetWorkOrders(new Query() { Filter = $@"i =>  i.WorkerPeople.Any(r=>r.PERSON_ID == {int.Parse(PersonId)}) " });
                getWorkOrdersResult = clearRiskGetWorkOrdersResult.Select(x => new WorkOrder
                {
                    WORK_ORDER_ID = x.WORK_ORDER_ID,
                    WORK_ORDER_NUMBER = x.WORK_ORDER_NUMBER,
                    DATE_RAISED = x.DATE_RAISED,
                    DUE_DATE = x.DUE_DATE,
                    OrderStatus = x.OrderStatus ?? null,
                    PriorityMaster = x.PriorityMaster ?? null,
                    EntityStatus = x.EntityStatus ?? null,
                    WarningLevel = x.WarningLevel ?? null,

                }).ToList();

                var clearConnectionGetAssesmentsResult = await ClearConnection.GetAssesments(new Query() { Filter = $@"i => i.AssesmentEmployees.Any(r=>r.EMPLOYEE_ID == {int.Parse(PersonId)}) " });
                getAssesmentsResult = clearConnectionGetAssesmentsResult.Select(x => new Assesment
                {
                    ASSESMENTID = x.ASSESMENTID,
                    RISKASSESSMENTNO = x.RISKASSESSMENTNO,
                    ASSESMENTDATE = x.ASSESMENTDATE,
                    PROJECTNAME = x.PROJECTNAME,
                    TemplateType = x.TemplateType ?? null,
                    EntityStatus = x.EntityStatus ?? null,
                    WarningLevel = x.WarningLevel ?? null,
                    PersonSite = x.PersonSite

                }).ToList();

                var clearRiskGetSurveyReportsResult = await SurveyService.GetSurveyReports(new Query() { Filter = $@"i => i.SURVEYOR_ID == {int.Parse(PersonId)}" });
                getSurveyReportsResult = clearRiskGetSurveyReportsResult.Select(x => new SurveyReport
                {
                    SURVEY_REPORT_ID = x.SURVEY_REPORT_ID,
                    SURVEY_DATE = x.SURVEY_DATE,
                    Survey = x.Survey,
                    Assesment = x.Assesment ?? null,
                    Order = x.Order ?? null,
                    EntityStatus = x.EntityStatus ?? null,
                    WarningLevel = x.WarningLevel ?? null,
                    COMMENTS = x.COMMENTS
                }).ToList();

                person = await ClearConnection.GetPersonByPersonId(int.Parse(PersonId));

                if (Security.IsInRole("System Administrator"))
                {
                    Managers = await ClearConnection.GetEmployee(new Query() { Filter = "i => i.ISMANAGER == true" });
                }
                else
                {
                    Managers = await ClearConnection.GetEmployee(Security.getCompanyId(), new Query() { Filter = $"i => i.ISMANAGER == true && i.PERSON_ID != {Convert.ToInt32(PersonId)}" });
                }

            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", ex.Message, 180000);
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
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

        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.Person args)
        {
            isLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {

                if (args.ISMANAGER == false && args.ASSIGNED_TO_ID == null)
                {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Please designate the employee as a manager or select a manager for the employee!", 180000);
                    return;
                }


                args.PERSONAL_PHONE = args.BUSINESS_PHONE;
                args.PERSONAL_MOBILE = args.BUSINESS_MOBILE;
                var securityCreateUserResult = await ClearConnection.UpdatePerson(args.PERSON_ID, args);
                NotificationService.Notify(NotificationSeverity.Info, $"Success", $"Employee Information Updated Successfully.", 180000);


            }
            catch (System.Exception clearConnectionCreatePersonException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new Employee !, " + clearConnectionCreatePersonException.Message, 180000);
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
        protected async System.Threading.Tasks.Task ButtonBackToList(MouseEventArgs args)
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




        #region Work Order

        protected IList<WorkOrder> getWorkOrdersResult = new List<WorkOrder>();
        

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {

                UriHelper.NavigateTo($@"edit-work-order/{data.WORK_ORDER_ID.ToString()}/{PersonId.ToString()}");

            }
            catch (System.Exception clearRiskDeleteWorkOrderException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to find WorkOrder", 180000);
            }
        }


        protected IList<Clear.Risk.Models.ClearConnection.Assesment> getAssesmentsResult = new List<Clear.Risk.Models.ClearConnection.Assesment>();
       

        protected async System.Threading.Tasks.Task GridAssementViewButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {

                UriHelper.NavigateTo("view-assesment" + "/" + data.ASSESMENTID.ToString() + $"/{PersonId.ToString()}");

            }
            catch (System.Exception clearRiskDeleteWorkOrderException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to find WorkOrder", 180000);
            }
        }

        protected IList<SurveyReport> getSurveyReportsResult = new List<SurveyReport>();
        

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

        protected async System.Threading.Tasks.Task GridSurveyButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                UriHelper.NavigateTo($@"view-survey-report/{data.SURVEY_REPORT_ID.ToString()}/{PersonId.ToString()}");
            }
            catch (System.Exception clearRiskDeleteWorkOrderException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to find WorkOrder", 180000);
            }
        }
        protected RadzenScheduler<WorkOrder> scheduler0;
        protected async System.Threading.Tasks.Task Scheduler0AppointmentSelect(SchedulerAppointmentSelectEventArgs<WorkOrder> args)
        {
            // UriHelper.NavigateTo($"edit-work-order/{args.Data.WORK_ORDER_ID}");
            UriHelper.NavigateTo("edit-work-order" + "/" + args.Data.WORK_ORDER_ID.ToString());
        }

        protected async Task ChangePassword(MouseEventArgs args)
        {
            await DialogService.OpenAsync<ChangeEmployeePassword>("Change Password", new Dictionary<string, object>() { { "PersonId", PersonId } });
        }


        #endregion       
    }
}
