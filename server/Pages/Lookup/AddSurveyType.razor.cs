﻿using System;
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

namespace Clear.Risk.Pages.Lookup
{
    public partial class AddSurveyType: ComponentBase
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
        protected ClearConnectionService ClearRisk { get; set; }
        protected bool IsLoading { get; set; }
        SurveyType _surveytype;
        protected SurveyType surveytype
        {
            get
            {
                return _surveytype;
            }
            set
            {
                if (!object.Equals(_surveytype, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "surveytype", NewValue = value, OldValue = _surveytype };
                    _surveytype = value;
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
            surveytype = new SurveyType() { };
        }

        protected async System.Threading.Tasks.Task Form0Submit(SurveyType args)
        {
            IsLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {
                var clearRiskCreateSurveyTypeResult = await ClearRisk.CreateSurveyType(surveytype);
                IsLoading = false;
                StateHasChanged();
                DialogService.Close(surveytype);
            }
            catch (System.Exception clearRiskCreateSurveyTypeException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new SurveyType!");
                IsLoading = false;
                StateHasChanged();
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
