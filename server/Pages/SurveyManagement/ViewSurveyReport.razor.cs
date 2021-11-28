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
using Clear.Risk.Data;

namespace Clear.Risk.Pages.SurveyManagement
{
    public partial class ViewSurveyReport : ComponentBase
    {
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
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }


        [Inject]
        protected SurveyServices ClearRisk { get; set; }

        SurveyReport _surveyreport;
        protected SurveyReport surveyreport
        {
            get
            {
                return _surveyreport;
            }
            set
            {
                if (!object.Equals(_surveyreport, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "surveyreport", NewValue = value, OldValue = _surveyreport };
                    _surveyreport = value;
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
        }

        [Parameter]
        public dynamic SURVEY_REPORT_ID { get; set; }
        [Parameter]
        public dynamic EmpId { get; set; }
        [Parameter]
        public dynamic TabValue { get; set; }
        public dynamic ASSESMENTID { get; set; }
        public dynamic RETURNURL { get; set; }
        


        protected bool isLoading { get; set; }
        protected async System.Threading.Tasks.Task Load()
        {
            var clearRiskGetSurveyReportBySurveyReportIdResult = await ClearRisk.GetSurveyReportBySurveyReportId(int.Parse($"{SURVEY_REPORT_ID}"));
            surveyreport = clearRiskGetSurveyReportBySurveyReportIdResult;

            var clearRiskGetSurveyAnswerChecklistsResult = await ClearRisk.GetSurveyAnswerChecklists(new Query() { Filter = $@"i => i.SURVEY_REPORT_ID == {int.Parse($"{SURVEY_REPORT_ID}")}" });
            getSurveyAnswerChecklistsResult = clearRiskGetSurveyAnswerChecklistsResult;


        }


        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }


        [Parameter]
        public dynamic backpage { get; set; }

        [Parameter]
        public dynamic backId { get; set; }

        [Parameter]
        public dynamic tabNo { get; set; }

        protected async System.Threading.Tasks.Task ButtonBackToListClick(MouseEventArgs args/*, dynamic data*/)
        {
            //if(ASSESMENTID != null)
            //{
            //    UriHelper.NavigateTo($"edit-employee/{TabValue.ToString()}/4");
            //}
            //if (EmpId != null)
            //{
            //    UriHelper.NavigateTo($"edit-employee/{EmpId.ToString()}/4");
            //}
            //else
            //{
            //    UriHelper.NavigateTo("survey-report");
            //}

            if(ASSESMENTID != null)
                UriHelper.NavigateTo($"edit-employee/{TabValue.ToString()}/4");
            else if (EmpId != null)
                UriHelper.NavigateTo($"edit-employee/{EmpId.ToString()}/4");
            else if (backpage != null)
            {
                if(backpage.ToString() == "WO")
                {
                    UriHelper.NavigateTo($@"edit-work-order/{backId}/{tabNo}");
                }
            }
            else
            {
                UriHelper.NavigateTo("survey-report");
            }


        }


        #region "Survey Answer CheckList"
        IEnumerable<SurveyAnswerChecklist> _getSurveyAnswerChecklistsResult;
        protected IEnumerable<SurveyAnswerChecklist> getSurveyAnswerChecklistsResult
        {
            get
            {
                return _getSurveyAnswerChecklistsResult;
            }
            set
            {
                if (!object.Equals(_getSurveyAnswerChecklistsResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getSurveyAnswerChecklistsResult", NewValue = value, OldValue = _getSurveyAnswerChecklistsResult };
                    _getSurveyAnswerChecklistsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        #endregion
    }
}
