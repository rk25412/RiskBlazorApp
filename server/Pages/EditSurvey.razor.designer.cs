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
    public partial class EditSurveyComponent : ComponentBase
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

        [Parameter]
        public dynamic SURVEY_ID { get; set; }

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
                    var args = new PropertyChangedEventArgs(){ Name = "survey", NewValue = value, OldValue = _survey };
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
                    var args = new PropertyChangedEventArgs(){ Name = "getSurveyTypesResult", NewValue = value, OldValue = _getSurveyTypesResult };
                    _getSurveyTypesResult = value;
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
            var clearConnectionGetSurveyBySurveyIdResult = await ClearConnection.GetSurveyBySurveyId(SURVEY_ID);
            survey = clearConnectionGetSurveyBySurveyIdResult;

            var clearConnectionGetSurveyTypesResult = await ClearConnection.GetSurveyTypes();
            getSurveyTypesResult = clearConnectionGetSurveyTypesResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.Survey args)
        {
            try
            {
                survey.UPDATER_ID = Security.getUserId();
                survey.UPDATED_DATE = DateTime.Now;
                var clearConnectionUpdateSurveyResult = await ClearConnection.UpdateSurvey(SURVEY_ID, survey);
                DialogService.Close(survey);
            }
            catch (System.Exception clearConnectionUpdateSurveyException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to update Survey");
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
