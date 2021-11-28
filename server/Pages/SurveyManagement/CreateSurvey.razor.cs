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
using System.Net;
using System.Net.Http;

namespace Clear.Risk.Pages.SurveyManagement
{
    public partial class CreateSurvey: ComponentBase
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

        IEnumerable<Clear.Risk.Models.ClearConnection.SurveyType> _getSurveyTypesResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.SurveyType> getSurveyTypesResult
        {
            get
            {
                return _getSurveyTypesResult;
            }
            set
            {
                if (!object.Equals(_getSurveyTypesResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getSurveyTypesResult", NewValue = value, OldValue = _getSurveyTypesResult };
                    _getSurveyTypesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        IEnumerable<Clear.Risk.Models.ClearConnection.Survey> _getBaseSurveyResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.Survey> getBaseSurveyResult
        {
            get
            {
                return _getBaseSurveyResult;
            }
            set
            {
                if (!object.Equals(_getBaseSurveyResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getBaseSurveyResult", NewValue = value, OldValue = _getBaseSurveyResult };
                    _getBaseSurveyResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        

        Clear.Risk.Models.ClearConnection.Survey _survey;
        protected Clear.Risk.Models.ClearConnection.Survey survey
        {
            get
            {
                return _survey;
            }
            set
            {
                if (!object.Equals(_survey, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "survey", NewValue = value, OldValue = _survey };
                    _survey = value;
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
            var clearConnectionGetSurveyTypesResult = await ClearConnection.GetSurveyTypes();
            getSurveyTypesResult = clearConnectionGetSurveyTypesResult;

            getBaseSurveyResult = await ClearConnection.GetSurveys(new Query() { Filter = $@"i => i.ISBASED == {true}" });

            survey = new Clear.Risk.Models.ClearConnection.Survey()
            {
                CREATED_DATE = DateTime.Now,
                CREATOR_ID = Security.getUserId(),
                UPDATED_DATE = DateTime.Now,
                UPDATER_ID = Security.getUserId(),
                WARNING_LEVEL_ID = 1,
                ENTITY_STATUS_ID = 1,
                ESCALATION_LEVEL_ID = 1,
                COMPANY_ID = Security.IsInRole("Administrator") ? Security.getCompanyId(): (int?)null
                

            };
        }

        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.Survey args)
        {
            isLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {
                if(args.BASE_SURVEY_ID != null)
                {
                    //Create Question of Based Survey and inster into Sevey Question/Answer
                    var result = await ClearConnection.GetSurveyQuestions(new Query() { Filter = $@"i => i.SURVEY_ID == {args.BASE_SURVEY_ID}" });

                    if(result != null)
                    {
                        int orderNo = 1;
                        foreach(var item in result)
                        {
                            if(args.SurveyQuestions == null)                             
                                args.SurveyQuestions = new List<SurveyQuestion>();

                            SurveyQuestion question = new SurveyQuestion(){
                                CREATED_DATE = DateTime.Now,
                                CREATOR_ID = Security.getUserId(),
                                UPDATED_DATE = DateTime.Now,
                                UPDATER_ID = Security.getUserId(),
                                IS_DELETED = false,
                                SURVEYQ_ORDER = orderNo,
                                QUESTION_TYPE_ID = item.QUESTION_TYPE_ID,
                                QUESTION_TITLE = item.QUESTION_TITLE,
                                QUESTION_DESC = item.QUESTION_DESC,
                                SCORE = item.SCORE,
                                PARENT_Q_ID = item.PARENT_Q_ID,
                                EXTERNAL_REF = item.EXTERNAL_REF,
                            };

                            if(item.SurveyAnswers != null)
                            {
                                foreach(var answer in item.SurveyAnswers)
                                {
                                    if (question.SurveyAnswers == null)
                                        question.SurveyAnswers = new List<SurveyQuestionAnswer>();

                                    question.SurveyAnswers.Add(new SurveyQuestionAnswer
                                    {
                                        CREATED_DATE = DateTime.Now,
                                        CREATOR_ID = Security.getUserId(),
                                        UPDATED_DATE = DateTime.Now,
                                        UPDATER_ID = Security.getUserId(),
                                        IS_DELETED = false,
                                        SURVEY_ANSWER_TITLE = answer.SURVEY_ANSWER_TITLE,
                                        SURVEY_A_COMMENT = answer.SURVEY_A_COMMENT,
                                        SURVEYA_VALUE = answer.SURVEYA_VALUE,
                                        GOTO_QUESTION_ID = answer.GOTO_QUESTION_ID
                                    });
                                }
                            }
                            

                            args.SurveyQuestions.Add(question);

                            orderNo++;
                             
                        }
                    }

                }
                var clearConnectionCreateSurveyResult = await ClearConnection.CreateSurvey(survey);
                
                if(survey.SURVEY_ID > 0)
                {
                    NotificationService.Notify(NotificationSeverity.Info, $"Success", $"Survey record created successfully!");
                    UriHelper.NavigateTo("view-survey" + "/" + survey.SURVEY_ID.ToString());
                }
            }
            catch (System.Exception clearConnectionCreateSurveyException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new Survey!");
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
                
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            UriHelper.NavigateTo("survey");
        }
        protected async System.Threading.Tasks.Task ButtonBackClick(MouseEventArgs args)
        {
            UriHelper.NavigateTo("survey");
        }
    }
}
