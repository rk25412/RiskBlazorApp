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
    public partial class EditSurveyQuestionComponent : ComponentBase
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


        [Inject]
        protected ClearConnectionService Risk { get; set; }

        [Parameter]
        public dynamic SURVEYQ_QUESTION_ID { get; set; }

        protected RadzenGrid<Clear.Risk.Models.ClearConnection.SurveyQuestionAnswer> grid0;

        IEnumerable<Clear.Risk.Models.ClearConnection.SurveyQuestionAnswer> _getSurveyAnswersResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.SurveyQuestionAnswer> getSurveyAnswersResult
        {
            get
            {
                return _getSurveyAnswersResult;
            }
            set
            {
                if (!object.Equals(_getSurveyAnswersResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getSurveyAnswersResult", NewValue = value, OldValue = _getSurveyAnswersResult };
                    _getSurveyAnswersResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        Clear.Risk.Models.ClearConnection.SurveyQuestion _surveyquestion;
        protected Clear.Risk.Models.ClearConnection.SurveyQuestion surveyquestion
        {
            get
            {
                return _surveyquestion;
            }
            set
            {
                if (!object.Equals(_surveyquestion, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "surveyquestion", NewValue = value, OldValue = _surveyquestion };
                    _surveyquestion = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.QuestionType> _getQuestionTypesResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.QuestionType> getQuestionTypesResult
        {
            get
            {
                return _getQuestionTypesResult;
            }
            set
            {
                if (!object.Equals(_getQuestionTypesResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getQuestionTypesResult", NewValue = value, OldValue = _getQuestionTypesResult };
                    _getQuestionTypesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.SurveyQuestion> _getSurveyQuestionsResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.SurveyQuestion> getSurveyQuestionsResult
        {
            get
            {
                return _getSurveyQuestionsResult;
            }
            set
            {
                if (!object.Equals(_getSurveyQuestionsResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getSurveyQuestionsResult", NewValue = value, OldValue = _getSurveyQuestionsResult };
                    _getSurveyQuestionsResult = value;
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
        protected async System.Threading.Tasks.Task Load()
        {
            var clearConnectionGetSurveyQuestionBySurveyqQuestionIdResult = await ClearConnection.GetSurveyQuestionBySurveyqQuestionId(SURVEYQ_QUESTION_ID);
            surveyquestion = clearConnectionGetSurveyQuestionBySurveyqQuestionIdResult;

            var clearConnectionGetQuestionTypesResult = await ClearConnection.GetQuestionTypes();
            getQuestionTypesResult = clearConnectionGetQuestionTypesResult;

            var clearConnectionGetSurveyQuestionsResult = await ClearConnection.GetSurveyQuestions();
            getSurveyQuestionsResult = clearConnectionGetSurveyQuestionsResult;

            var clearConnectionGetWarningResult = await Risk.GetWarningLevels();
            getWarningResult = clearConnectionGetWarningResult;

            var clearConnectionGetSurveyAnswersResult = await ClearConnection.GetSurveyAnswers(new Query() { Filter = $@"i => i.SURVEY_QUESTION_ID == {SURVEYQ_QUESTION_ID}" });
            getSurveyAnswersResult = clearConnectionGetSurveyAnswersResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddSurveyAnswer>("Add Survey Answer", new Dictionary<string, object>() { { "SURVEYQ_QUESTION_ID", SURVEYQ_QUESTION_ID } });
            await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<EditSurveyAnswer>("Edit Survey Answer", new Dictionary<string, object>() { { "SURVEY_ANSWER_ID", data.SURVEY_ANSWER_ID } });
            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var clearConnectionDeleteSurveyAnswerResult = await ClearConnection.DeleteSurveyAnswer(int.Parse($"{data.SURVEY_ANSWER_ID}"));
                    if (clearConnectionDeleteSurveyAnswerResult != null)
                    {
                       // grid0.Reload();
                    }
                }
            }
            catch (System.Exception clearConnectionDeleteSurveyAnswerException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete SurveyAnswer");
            }
        }

        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.SurveyQuestion args)
        {
            try
            {
                var clearConnectionUpdateSurveyQuestionResult = await ClearConnection.UpdateSurveyQuestion(SURVEYQ_QUESTION_ID, surveyquestion);
                DialogService.Close(surveyquestion);
            }
            catch (System.Exception clearConnectionUpdateSurveyQuestionException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to update SurveyQuestion");
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
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
    }
}
