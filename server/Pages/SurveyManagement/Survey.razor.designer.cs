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
using Clear.Risk.Pages.SurveyManagement;

namespace Clear.Risk.Pages
{
    public partial class SurveyComponent : ComponentBase
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
        protected SurveyServices ClearConnection { get; set; }

        protected RadzenGrid<Clear.Risk.Models.ClearConnection.Survey> grid0;

        protected RadzenGrid<Clear.Risk.Models.ClearConnection.SurveyQuestion> grid1;

        IEnumerable<Clear.Risk.Models.ClearConnection.Survey> _getSurveysResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.Survey> getSurveysResult
        {
            get
            {
                return _getSurveysResult;
            }
            set
            {
                if (!object.Equals(_getSurveysResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getSurveysResult", NewValue = value, OldValue = _getSurveysResult };
                    _getSurveysResult = value;
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
                    var args = new PropertyChangedEventArgs(){ Name = "master", NewValue = value, OldValue = _master };
                    _master = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
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
            var clearConnectionGetSurveysResult = await ClearConnection.GetSurveys(Security.IsInRole("System Administrator") ? new Query() : new Query() { Filter = $"i => i.CREATOR_ID == {Security.getCompanyId()} || i.CREATOR_ID == 2" });

            getSurveysResult = (from x in clearConnectionGetSurveysResult
                                select new Models.ClearConnection.Survey
                                {
                                    SURVEY_ID = x.SURVEY_ID,
                                    SurveyType = x.SurveyType,
                                    SURVEY_TITLE = x.SURVEY_TITLE,
                                    YES_NO_SCORE = x.YES_NO_SCORE,
                                    CHOICE_SCORE = x.CHOICE_SCORE,
                                    TEXT_SCORE = x.TEXT_SCORE,
                                    TOTAL_SCORE = x.TOTAL_SCORE,
                                }).ToList();


            //getSurveysResult = clearConnectionGetSurveysResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            UriHelper.NavigateTo("create-survey");
        }

        protected async System.Threading.Tasks.Task HelpClick(MouseEventArgs args)
        {
            string url = "/Help/3";
            await JSRuntime.InvokeAsync<object>("open", url, "_blank");

            //UriHelper.NavigateTo("Help" + "/" + 3);
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(Clear.Risk.Models.ClearConnection.Survey args)
        {
            var dialogResult = await DialogService.OpenAsync<EditSurvey>("Edit Survey", new Dictionary<string, object>() { {"SURVEY_ID", args.SURVEY_ID} });
            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Grid0RowExpand(Clear.Risk.Models.ClearConnection.Survey args)
        {
            master = args;

            var clearConnectionGetSurveyQuestionsResult = await ClearConnection.GetSurveyQuestions(new Query() { Filter = $@"i => i.SURVEY_ID == {args.SURVEY_ID}" });
            if (clearConnectionGetSurveyQuestionsResult != null) {
                args.SurveyQuestions = clearConnectionGetSurveyQuestionsResult.ToList();
                }
        }

        protected async System.Threading.Tasks.Task GridSelectButtonClick(MouseEventArgs args, dynamic data)
        {
            
                 UriHelper.NavigateTo("view-survey" + "/" + data.SURVEY_ID.ToString());
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var clearConnectionDeleteSurveyResult = await ClearConnection.DeleteSurvey(data.SURVEY_ID);
                    if (clearConnectionDeleteSurveyResult != null) {
                        //grid0.Reload();
}
                }
            }
            catch (System.Exception clearConnectionDeleteSurveyException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete Survey");
            }
        }

        protected async System.Threading.Tasks.Task SurveyQuestionAddButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<AddSurveyQuestion>("Add Survey Question", new Dictionary<string, object>() { {"SURVEY_ID", data.SURVEY_ID} });
            grid1.Reload();
        }

        protected async System.Threading.Tasks.Task Grid1RowSelect(Clear.Risk.Models.ClearConnection.SurveyQuestion args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<EditSurveyQuestion>("Edit Survey Question", new Dictionary<string, object>() { {"SURVEYQ_QUESTION_ID", args.SURVEYQ_QUESTION_ID} });
            grid1.Reload();
        }

        protected async System.Threading.Tasks.Task SurveyQuestionDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                var clearConnectionDeleteSurveyQuestionResult = await ClearConnection.DeleteSurveyQuestion(data.SURVEYQ_QUESTION_ID);
                if (clearConnectionDeleteSurveyQuestionResult != null) {
                    grid1.Reload();
}
            }
            catch (System.Exception clearConnectionDeleteSurveyQuestionException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete Survey");
            }
        }
    }
}
