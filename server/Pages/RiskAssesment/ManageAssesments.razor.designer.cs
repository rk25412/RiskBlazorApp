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

namespace Clear.Risk.Pages
{
    public partial class ManageAssesmentsComponent : ComponentBase
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
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }

        [Inject]
        protected ClearConnectionService ClearConnection { get; set; }

        protected bool hasError = false;
        protected bool isLoading = true;

        protected RadzenGrid<Clear.Risk.Models.ClearConnection.Assesment> grid0;

        //IEnumerable<Clear.Risk.Models.ClearConnection.Assesment> _getAssesmentsResult;

        protected IList<Clear.Risk.Models.ClearConnection.Assesment> getAssesmentsResult = new List<Clear.Risk.Models.ClearConnection.Assesment>();

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
        protected async System.Threading.Tasks.Task Load()
        {
            isLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {
                if (Security.IsInRole("System Administrator"))
                {
                    var clearConnectionGetAssesmentsResult = await ClearConnection.GetAssesments(new Query() { Expand = "StatusMaster,TradeCategory,PersonSite,IndustryType,Client,Company" });

                    getAssesmentsResult = (from x in clearConnectionGetAssesmentsResult
                                           select new Clear.Risk.Models.ClearConnection.Assesment
                                           {
                                               ASSESMENTID = x.ASSESMENTID,
                                               RISKASSESSMENTNO = !string.IsNullOrEmpty(x.RISKASSESSMENTNO) ? x.RISKASSESSMENTNO : "",
                                               WORKORDERNUMBER = !string.IsNullOrEmpty(x.WORKORDERNUMBER) ? x.WORKORDERNUMBER : "",
                                               PROJECTNAME = !string.IsNullOrEmpty(x.PROJECTNAME) ? x.PROJECTNAME : "",
                                               ASSESMENTDATE = x.ASSESMENTDATE.Date,
                                               TemplateType = x.TemplateType ?? null,
                                               EntityStatus = x.EntityStatus,
                                               WarningLevel = x.WarningLevel,
                                               ISINTERNAL = x.ISINTERNAL,
                                               ISCOMPLETED = x.ISCOMPLETED,
                                               ISSCHEDULE = x.ISSCHEDULE,
                                               IsScheduleRunning = x.IsScheduleRunning,
                                               AssessmentActivity = !string.IsNullOrEmpty(x.AssessmentActivity) ? x.AssessmentActivity : "",
                                           }).ToList();
                }
                else
                {
                    var clearConnectionGetAssesmentsResult = await ClearConnection.GetAssesments(new Query() { Filter = $@"i => i.COMPANYID == {Security.getCompanyId()}", Expand = "StatusMaster,TradeCategory,PersonSite,IndustryType,Client,Company" });

                    getAssesmentsResult = (from x in clearConnectionGetAssesmentsResult
                                           select new Clear.Risk.Models.ClearConnection.Assesment
                                           {
                                               ASSESMENTID = x.ASSESMENTID,
                                               RISKASSESSMENTNO = !string.IsNullOrEmpty(x.RISKASSESSMENTNO) ? x.RISKASSESSMENTNO : "",
                                               WORKORDERNUMBER = !string.IsNullOrEmpty(x.WORKORDERNUMBER) ? x.WORKORDERNUMBER : "",
                                               PROJECTNAME = !string.IsNullOrEmpty(x.PROJECTNAME) ? x.PROJECTNAME : "",
                                               ASSESMENTDATE = x.ASSESMENTDATE,
                                               TemplateType = x.TemplateType ?? null,
                                               EntityStatus = x.EntityStatus,
                                               WarningLevel = x.WarningLevel,
                                               ISINTERNAL = x.ISINTERNAL,
                                               ISCOMPLETED = x.ISCOMPLETED,
                                               ISSCHEDULE = x.ISSCHEDULE
                                           }).ToList();






                    //getAssesmentsResult = clearConnectionGetAssesmentsResult;
                }
            }
            catch (Exception ex)
            {
                isLoading = false;
                StateHasChanged();
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }


        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            UriHelper.NavigateTo("create-assesment");
        }
        protected async System.Threading.Tasks.Task HelpClick(MouseEventArgs args)
        {
            string url = "/Help/1";
            await JSRuntime.InvokeAsync<object>("open", url, "_blank");

            //UriHelper.NavigateTo("Help" + "/"+ 1);
        }

        protected async System.Threading.Tasks.Task GridViewButtonClick(MouseEventArgs args, dynamic data)
        {
            UriHelper.NavigateTo("view-assesment" + "/" + data.ASSESMENTID.ToString());
        }

        protected async System.Threading.Tasks.Task GridEditButtonClick(MouseEventArgs args, dynamic data)
        {
            UriHelper.NavigateTo("edit-assesment-record" + "/" + data.ASSESMENTID.ToString());
        }

        protected async System.Threading.Tasks.Task GridViewAssessmentClick(MouseEventArgs args, dynamic data)
        {
            UriHelper.NavigateTo("view-assesment" + "/" + data.ASSESMENTID.ToString());
        }


    }
}
