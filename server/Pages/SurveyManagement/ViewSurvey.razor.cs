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
using Clear.Risk.Pages;

namespace Clear.Risk.Pages.SurveyManagement
{
    public partial class ViewSurvey : ComponentBase
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

        IEnumerable<Clear.Risk.Models.ClearConnection.SurveyQuestion> _getSurveyQuestionResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.SurveyQuestion> getSurveyQuestionResult
        {
            get
            {
                return _getSurveyQuestionResult;
            }
            set
            {
                if (!object.Equals(_getSurveyQuestionResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getSurveyQuestionResult", NewValue = value, OldValue = _getSurveyQuestionResult };
                    _getSurveyQuestionResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }


        [Parameter]
        public dynamic SURVEY_ID { get; set; }

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

            getSurveyQuestionResult = await ClearConnection.GetSurveyQuestions(new Query() { Filter = $@"i => i.SURVEY_ID == {int.Parse($"{SURVEY_ID}")}" });


            var clearConnectionGetSurveyBySurveyIdResult = await ClearConnection.GetSurveyBySurveyId(int.Parse($"{SURVEY_ID}"));
            survey = clearConnectionGetSurveyBySurveyIdResult;


        }

        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.Survey args)
        {
            try
            {
                survey.UPDATER_ID = Security.getUserId();
                survey.UPDATED_DATE = DateTime.Now;
                var clearConnectionUpdateSurveyResult = await ClearConnection.UpdateSurvey(args.SURVEY_ID, survey);
                NotificationService.Notify(NotificationSeverity.Info, $"Success", $"Survey Template updated successfully!, ");
                DialogService.Close(survey);
            }
            catch (System.Exception clearConnectionUpdateSurveyException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to update Survey");
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            // DialogService.Close(null);
            UriHelper.NavigateTo("survey");
        }

        protected async System.Threading.Tasks.Task SurveyQuestionAddButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<AddSurveyQuestion>("Add Survey Question", new Dictionary<string, object>() { { "SURVEY_ID", data.SURVEY_ID } });

        }

        protected async System.Threading.Tasks.Task GridSelectButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<EditSurveyQuestion>("Edit Survey Question", new Dictionary<string, object>() { { "SURVEYQ_QUESTION_ID", data.SURVEYQ_QUESTION_ID } });

        }

        protected async System.Threading.Tasks.Task GridSelectDeleteButton(MouseEventArgs args, dynamic data)
        {
           
            try
            {
                if(await DialogService.Confirm("Are you sure you want to delete this record?")==true)
                {
                    var clearConnectionUpdateSurveyQuestionResult = await ClearConnection.DeleteSurveyQuestion(data.SURVEYQ_QUESTION_ID);
                    NotificationService.Notify(NotificationSeverity.Success, $"Success", $"Question Deleted", 180000);

                    foreach (var item in getSurveyQuestionResult)
                    {
                        if (item.SURVEYQ_ORDER > data.SURVEYQ_ORDER)
                        {
                            item.SURVEYQ_ORDER--;
                            //await ClearConnection.UpdateSurveyQuestion(item.SURVEYQ_QUESTION_ID, item);
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete, Please try again later.", 180000);
            }
        }


        protected async Task MoveQuestionUp(MouseEventArgs args, dynamic data)
        {
            if (data.SURVEYQ_ORDER > 1)
            {
                var now = getSurveyQuestionResult.FirstOrDefault(i => i.SURVEYQ_ORDER == data.SURVEYQ_ORDER);
                var prev = getSurveyQuestionResult.FirstOrDefault(i => i.SURVEYQ_ORDER == (data.SURVEYQ_ORDER - 1));
                now.SURVEYQ_ORDER--;
                prev.SURVEYQ_ORDER++;
                try
                {
                    ClearConnection.UpdateSurveyQuestion(now.SURVEYQ_QUESTION_ID, now);
                    ClearConnection.UpdateSurveyQuestion(prev.SURVEYQ_QUESTION_ID, prev);
                }
                catch (Exception ex)
                {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to rearrange.", 5000);
                }
                finally
                {
                    getSurveyQuestionResult = await ClearConnection.GetSurveyQuestions(new Query() { Filter = $@"i => i.SURVEY_ID == {int.Parse($"{SURVEY_ID}")}" });
                }
            }
        }

        protected async Task MoveQuestionDown(MouseEventArgs args, dynamic data)
        {
            if (data.SURVEYQ_ORDER < getSurveyQuestionResult.Count())
            {
                var now = getSurveyQuestionResult.FirstOrDefault(i => i.SURVEYQ_ORDER == data.SURVEYQ_ORDER);
                var next = getSurveyQuestionResult.FirstOrDefault(i => i.SURVEYQ_ORDER == (data.SURVEYQ_ORDER + 1));
                now.SURVEYQ_ORDER++;
                next.SURVEYQ_ORDER--;
                try
                {
                    ClearConnection.UpdateSurveyQuestion(now.SURVEYQ_QUESTION_ID, now);
                    ClearConnection.UpdateSurveyQuestion(next.SURVEYQ_QUESTION_ID, next);
                }
                catch (Exception ex)
                {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to rearrange.", 5000);
                }
                finally
                {
                    getSurveyQuestionResult = await ClearConnection.GetSurveyQuestions(new Query() { Filter = $@"i => i.SURVEY_ID == {int.Parse($"{SURVEY_ID}")}" });
                }
            }
        }


    }
}
