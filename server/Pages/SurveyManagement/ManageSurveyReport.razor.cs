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
    public partial class ManageSurveyReport : ComponentBase
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
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }

        [Inject]
        protected SurveyServices ClearRisk { get; set; }
        protected RadzenGrid<SurveyReport> grid0;
        protected RadzenGrid<SurveyAnswerChecklist> grid1;
        protected bool IsLoading { get; set; }
        IEnumerable<SurveyReport> _getSurveyReportsResult;
        protected IEnumerable<SurveyReport> getSurveyReportsResult
        {
            get
            {
                return _getSurveyReportsResult;
            }
            set
            {
                if (!object.Equals(_getSurveyReportsResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getSurveyReportsResult", NewValue = value, OldValue = _getSurveyReportsResult };
                    _getSurveyReportsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        dynamic _master;
        protected dynamic master
        {
            get
            {
                return _master;
            }
            set
            {
                if (!object.Equals(_master, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "master", NewValue = value, OldValue = _master };
                    _master = value;
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
                IsLoading = true;
                StateHasChanged();
                await Task.Delay(1);
                await Load();
                IsLoading = false;
                StateHasChanged();
            }
        }

        protected async System.Threading.Tasks.Task Load()
        {
            if (Security.IsInRole("System Administrator"))
            {
                var clearRiskGetSurveyReportsResult = await ClearRisk.GetSurveyReports();

                getSurveyReportsResult = (from x in clearRiskGetSurveyReportsResult
                                          select new SurveyReport
                                          {
                                              SURVEY_REPORT_ID = x.SURVEY_REPORT_ID,
                                              SURVEY_DATE = x.SURVEY_DATE,
                                              SURVEY_ID = x.SURVEY_ID,
                                              SURVEYOR_ID = x.SURVEYOR_ID,
                                              Surveyor = x.Surveyor,
                                              Survey = x.Survey,
                                              Assesment = x.Assesment,
                                              Order = x.Order,
                                              WarningLevel = x.WarningLevel,
                                              EntityStatus = x.EntityStatus,
                                              Company = x.Company,
                                              COMMENTS = x.COMMENTS,
                                              ASSESMENT_ID = x.ASSESMENT_ID,
                                              WARNING_LEVEL_ID = x.WARNING_LEVEL_ID,
                                              WORK_ORDER_ID = x.WORK_ORDER_ID,
                                              ENTITY_STATUS_ID = x.ENTITY_STATUS_ID,
                                              COMPANY_ID = x.COMPANY_ID,
                                          }).ToList();
            }
            else
            {
                var clearRiskGetSurveyReportsResult = await ClearRisk.GetSurveyReports(new Query() { Filter = $@"i => i.COMPANY_ID == {Security.getCompanyId()}" });


                getSurveyReportsResult = (from x in clearRiskGetSurveyReportsResult
                                          select new SurveyReport
                                          {
                                              SURVEY_REPORT_ID = x.SURVEY_REPORT_ID,
                                              SURVEY_DATE = x.SURVEY_DATE,
                                              SURVEY_ID = x.SURVEY_ID,
                                              SURVEYOR_ID = x.SURVEYOR_ID,
                                              Surveyor = x.Surveyor,
                                              Assesment = x.Assesment,
                                              Survey = x.Survey,
                                              Order = x.Order,
                                              WarningLevel = x.WarningLevel,
                                              EntityStatus = x.EntityStatus,
                                              Company = x.Company,
                                              COMMENTS = x.COMMENTS,
                                              ASSESMENT_ID = x.ASSESMENT_ID,
                                              WARNING_LEVEL_ID = x.WARNING_LEVEL_ID,
                                              WORK_ORDER_ID = x.WORK_ORDER_ID,
                                              ENTITY_STATUS_ID = x.ENTITY_STATUS_ID,
                                              COMPANY_ID = x.COMPANY_ID,
                                          }).ToList();
            }
        }

        protected async System.Threading.Tasks.Task HelpClick(MouseEventArgs args)
        {
            string url = "/Help/12";
            await JSRuntime.InvokeAsync<object>("open", url, "_blank");
        }

        protected async System.Threading.Tasks.Task GridEditButtonClick(MouseEventArgs args, dynamic data)
        {
            UriHelper.NavigateTo("view-survey-report" + "/" + data.SURVEY_REPORT_ID.ToString());
        }

    }
}
