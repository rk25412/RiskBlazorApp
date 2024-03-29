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
    public partial class AddDesignation: ComponentBase
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

        [Parameter]
        public dynamic DESIGNATION_ID { get; set; }
        protected bool IsLoading { get; set; }
        Desigation _desigation;
        protected Desigation desigation
        {
            get
            {
                return _desigation;
            }
            set
            {
                if (!object.Equals(_desigation, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "desigation", NewValue = value, OldValue = _desigation };
                    _desigation = value;
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
            desigation = new Desigation() { };

        }
        protected async System.Threading.Tasks.Task Form0Submit(Desigation args)
        {
            IsLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {
                var clearRiskCreateDesignationResult = await ClearRisk.CreateDesignation(desigation);
                IsLoading = false;
                StateHasChanged();
                DialogService.Close(desigation);
            }
            catch (System.Exception clearRiskCreateSurveyTypeException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new Desigation!");
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
