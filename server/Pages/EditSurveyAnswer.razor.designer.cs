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
using Microsoft.AspNetCore.Identity;
using Clear.Risk.Models;

namespace Clear.Risk.Pages
{
    public partial class EditSurveyAnswerComponent : ComponentBase
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
        public dynamic SURVEY_ANSWER_ID { get; set; }

        Clear.Risk.Models.ClearConnection.SurveyQuestionAnswer _surveyanswer;
        protected Clear.Risk.Models.ClearConnection.SurveyQuestionAnswer surveyanswer
        {
            get
            {
                return _surveyanswer;
            }
            set
            {
                if (!object.Equals(_surveyanswer, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "surveyanswer", NewValue = value, OldValue = _surveyanswer };
                    _surveyanswer = value;
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

            var clearConnectionGetWarningResult = await Risk.GetWarningLevels();
            getWarningResult = clearConnectionGetWarningResult;

            var clearConnectionGetSurveyAnswerBySurveyAnswerIdResult = await ClearConnection.GetSurveyAnswerBySurveyAnswerId(int.Parse($"{SURVEY_ANSWER_ID}"));
            surveyanswer = clearConnectionGetSurveyAnswerBySurveyAnswerIdResult;

            var clearConnectionGetSurveyQuestionsResult = await ClearConnection.GetSurveyQuestions(new Query() { Filter = $@"i => i.SURVEY_ID == {surveyanswer.Question.SURVEY_ID}" });
            getSurveyQuestionsResult = clearConnectionGetSurveyQuestionsResult;

        }

        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.SurveyQuestionAnswer args)
        {
            try
            {
                var clearConnectionUpdateSurveyAnswerResult = await ClearConnection.UpdateSurveyAnswer(int.Parse($"{SURVEY_ANSWER_ID}"), surveyanswer);
                DialogService.Close(surveyanswer);
            }
            catch (System.Exception clearConnectionUpdateSurveyAnswerException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to update SurveyAnswer");
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
