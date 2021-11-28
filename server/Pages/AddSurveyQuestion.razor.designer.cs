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
    public partial class AddSurveyQuestionComponent : ComponentBase
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
        protected ClearConnectionService ClearRisk { get; set; }

        [Parameter]
        public dynamic SURVEY_ID { get; set; }

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
                    var args = new PropertyChangedEventArgs() { Name = "getQuestionTypesResult", NewValue = value, OldValue = _getQuestionTypesResult };
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
                    var args = new PropertyChangedEventArgs() { Name = "getSurveyQuestionsResult", NewValue = value, OldValue = _getSurveyQuestionsResult };
                    _getSurveyQuestionsResult = value;
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
                    var args = new PropertyChangedEventArgs() { Name = "surveyquestion", NewValue = value, OldValue = _surveyquestion };
                    _surveyquestion = value;
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
            var clearConnectionGetQuestionTypesResult = await ClearConnection.GetQuestionTypes();
            getQuestionTypesResult = clearConnectionGetQuestionTypesResult;

            getWarningResult = await ClearRisk.GetWarningLevels();

            var clearConnectionGetSurveyQuestionsResult = await ClearConnection.GetSurveyQuestions();
            getSurveyQuestionsResult = clearConnectionGetSurveyQuestionsResult;

            surveyquestion = new Clear.Risk.Models.ClearConnection.SurveyQuestion()
            {
                CREATED_DATE = DateTime.Now,
                CREATOR_ID = Security.getUserId(),
                UPDATED_DATE = DateTime.Now,
                UPDATER_ID = Security.getUserId(),
                IS_DELETED = false,
                WARNING_LEVEL_ID = 1
            };
        }

        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.SurveyQuestion args)
        {
            surveyquestion.SURVEY_ID = int.Parse($"{SURVEY_ID}");

            try
            {
                //Get MAX Q.No from Record
                surveyquestion.SURVEYQ_ORDER = await ClearConnection.GetQuestionMaxID(args.SURVEY_ID);

                if (surveyquestion.QUESTION_TYPE_ID == 1)
                {
                    //Create Yes/No Answer

                    if (surveyquestion.SurveyAnswers == null)
                        surveyquestion.SurveyAnswers = new List<SurveyQuestionAnswer>();

                    surveyquestion.SurveyAnswers.Add(new SurveyQuestionAnswer()
                    {
                        CREATED_DATE = DateTime.Now,
                        CREATOR_ID = Security.getUserId(),
                        UPDATED_DATE = DateTime.Now,
                        UPDATER_ID = Security.getUserId(),
                        IS_DELETED = false,
                        SURVEY_ANSWER_TITLE = "Yes",
                        WARNING_LEVEL_ID = 1
                    });

                    surveyquestion.SurveyAnswers.Add(new SurveyQuestionAnswer()
                    {
                        CREATED_DATE = DateTime.Now,
                        CREATOR_ID = Security.getUserId(),
                        UPDATED_DATE = DateTime.Now,
                        UPDATER_ID = Security.getUserId(),
                        IS_DELETED = false,
                        SURVEY_ANSWER_TITLE = "No",
                        WARNING_LEVEL_ID = 1
                    });
                }

                var clearConnectionCreateSurveyQuestionResult = await ClearConnection.CreateSurveyQuestion(surveyquestion);
                DialogService.Close(surveyquestion);
            }
            catch (System.Exception clearConnectionCreateSurveyQuestionException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new SurveyQuestion!");
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
