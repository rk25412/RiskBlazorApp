using Clear.Risk.Models.ClearConnection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clear.Risk.Pages.RiskAssesment
{
    public partial class ViewAssesment : ComponentBase
    {
        [Inject]
        protected SecurityService Security { get; set; }

        [Inject]
        protected ClearConnectionService ClearConnection { get; set; }

        [Inject]
        protected SurveyServices SurveyService { get; set; }

        [Inject]
        protected AssesmentService AssesmentConnection { get; set; }

        Clear.Risk.Models.ClearConnection.Assesment _assesment;

        [Inject]
        protected NavigationManager UriHelper { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Parameter]
        public dynamic ASSESMENTID { get; set; }

        [Parameter]
        public dynamic RETURNURL { get; set; }

        [Parameter]
        public dynamic ServayID { get; set; }

        protected Clear.Risk.Models.ClearConnection.Assesment assesment
        {
            get
            {
                return _assesment;
            }
            set
            {
                if (!object.Equals(_assesment, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "assesment", NewValue = value, OldValue = _assesment };
                    _assesment = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
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
            await Load();

        }

        protected IList<SurveyReport> getSurveyReportsResult = new List<SurveyReport>();



        protected async Task Load()
        {
            try
            {
                var clearConnectionGetConsequenceByConsequenceIdResult = await ClearConnection.GetAssesmentByAssesmentid(int.Parse(ASSESMENTID));
                assesment = clearConnectionGetConsequenceByConsequenceIdResult;

                var clearConnectionGetAssesmentAttachementsResult = await ClearConnection.GetAssesmentAttachements(new Query() { Filter = $@"i => i.ASSESMENTID == {ASSESMENTID}" });

                AssesmentAttachements = clearConnectionGetAssesmentAttachementsResult.Select(x => new Clear.Risk.Models.ClearConnection.AssesmentAttachement
                {
                    ATTACHEMENTID = x.ATTACHEMENTID,
                    ATTACHEMENTDATE = x.ATTACHEMENTDATE,
                    Attachments = x.Attachments,
                    //DOCUMENTPDFURL = x.DOCUMENTPDFURL,
                    DOCUMENTTEMPLATEURL = x.DOCUMENTTEMPLATEURL,
                    SwmsTemplate = x.SwmsTemplate
                }).ToList();

                var clearConnectionGetAssesmentEmployeesResult = await ClearConnection.GetAssesmentEmployees(new Query() { Filter = $@"i => i.ASSESMENT_ID == {ASSESMENTID} && i.IS_ACTIVE == true" });
                AssesmentEmployees = clearConnectionGetAssesmentEmployeesResult.Select(x => new Clear.Risk.Models.ClearConnection.AssesmentEmployee
                {
                    ASSESMENT_EMPLOYEE_ID = x.ASSESMENT_EMPLOYEE_ID,
                    EMPLOYEE_ID = x.EMPLOYEE_ID,
                    Employee = x.Employee ?? null,
                    WarningLevel = x.WarningLevel ?? null,
                    FileName = x.FileName,
                    SignatureImageUrl = x.SignatureImageUrl,
                    Sign_Date = x.Sign_Date,
                    VersionNo = x.VersionNo,
                    SignedStatus = x.SignedStatus
                }).ToList();

                var clearConnectionGetAssesmentEmployeeAttachementsResult = await ClearConnection.GetAssesmentEmployeeAttachements(new Query() { Filter = $@"i => i.AssignedEmployee.ASSESMENT_ID == {ASSESMENTID}", Expand = "AssesmentEmployeeStatus" });
                getAssesmentEmployeeAttachementsResult = clearConnectionGetAssesmentEmployeeAttachementsResult.Select(x => new Clear.Risk.Models.ClearConnection.AssesmentEmployeeAttachement
                {
                    AssignedEmployee = x.AssignedEmployee,
                    DOCUMENTNAME = x.DOCUMENTNAME,
                    ASSIGNED_DATE = x.ASSIGNED_DATE,
                    ACCEPTED_DATE = x.ACCEPTED_DATE,
                    SINGNATURE_DATE = x.SINGNATURE_DATE,
                    EMPLOYEE_STATUS = x.EMPLOYEE_STATUS,
                    DOCUMENT_URL = x.DOCUMENT_URL,
                    AssesmentEmployeeStatus = x.AssesmentEmployeeStatus,
                }).ToList();

                var scheduleResults = await ClearConnection.GetAssesmentSchedules(new Query() { Filter = $@"i => i.ASSESMENTID == {ASSESMENTID}" });
                AssesmentSchedules = scheduleResults.Select(x => new Clear.Risk.Models.ClearConnection.AssesmentSchedule
                {
                    ScheduleType = x.ScheduleType,
                    SCHEDULE_AT = x.SCHEDULE_AT,
                    INTERVAL = x.INTERVAL,
                    SCHEDULE_TIME = x.SCHEDULE_TIME,
                    MON = x.MON,
                    TUE = x.TUE,
                    WED = x.WED,
                    THUS = x.THUS,
                    FRI = x.FRI,
                    SAT = x.SAT,
                    SUN = x.SUN
                }).ToList();

                var clearRiskGetSurveyReportsResult = await SurveyService.GetSurveyReports(new Query() { Filter = $@"i => i.ASSESMENT_ID == {int.Parse(ASSESMENTID)}" });
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

            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }







        }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        //protected async System.Threading.Tasks.Task ButtonBackClick(MouseEventArgs args)
        //{
        //    UriHelper.NavigateTo("manage-assesments");
        //}
        protected async System.Threading.Tasks.Task ButtonBackToListClick(MouseEventArgs args/*, dynamic data*/)
        {
            try
            {
                if (RETURNURL != null)
                {
                    UriHelper.NavigateTo($"edit-employee/{RETURNURL.ToString()}/2");
                }
                else if (RETURNURL != null && ServayID != null)
                {
                    UriHelper.NavigateTo($"edit-employee/{RETURNURL.ToString()}/{ServayID.ToString()}/2");
                }
                else
                {
                    UriHelper.NavigateTo("manage-assesments");
                }

            }
            catch (System.Exception clearRiskDeleteWorkOrderException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to find Employee");
            }
        }

        #region Assesment Template
        //protected RadzenGrid<Clear.Risk.Models.ClearConnection.AssesmentAttachement> grid1;

        protected IList<Clear.Risk.Models.ClearConnection.AssesmentAttachement> AssesmentAttachements = new List<Clear.Risk.Models.ClearConnection.AssesmentAttachement>();

        #endregion

        #region Assigned Employee
        //protected RadzenGrid<Clear.Risk.Models.ClearConnection.AssesmentEmployee> grid2;

        protected IList<Clear.Risk.Models.ClearConnection.AssesmentEmployee> AssesmentEmployees = new List<Clear.Risk.Models.ClearConnection.AssesmentEmployee>();



        protected async System.Threading.Tasks.Task AssesmentEmployeeDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                var clearConnectionDeleteAssesmentEmployeeResult = await ClearConnection.DeleteAssesmentEmployee(data.ASSESMENT_EMPLOYEE_ID);
                if (clearConnectionDeleteAssesmentEmployeeResult != null)
                {
                    //grid2.Reload();
                }
            }
            catch (System.Exception clearConnectionDeleteAssesmentEmployeeException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete Assesment");
            }
        }



        #endregion

        #region Risk Assesment
        //protected RadzenGrid<Clear.Risk.Models.ClearConnection.AssesmentEmployeeAttachement> grid0;

        protected IList<Clear.Risk.Models.ClearConnection.AssesmentEmployeeAttachement> getAssesmentEmployeeAttachementsResult = new List<Clear.Risk.Models.ClearConnection.AssesmentEmployeeAttachement>();

        #endregion

        /*----------------------------------------------------------------------------------------------------------------*/


        protected IList<Clear.Risk.Models.ClearConnection.AssesmentSchedule> AssesmentSchedules = new List<Clear.Risk.Models.ClearConnection.AssesmentSchedule>();


        protected async System.Threading.Tasks.Task GridSurveyButtonClick(MouseEventArgs args, dynamic data)
        {
            UriHelper.NavigateTo($@"view-survey-report/{data.SURVEY_REPORT_ID}");

        }
    }
}
