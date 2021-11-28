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

namespace Clear.Risk.Pages
{
    public partial class AddSwmsPperequiredComponent : ComponentBase
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

        [Parameter]
        public dynamic SWMSID { get; set; }

        IEnumerable<Clear.Risk.Models.ClearConnection.Ppevalue> _getPpevaluesResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.Ppevalue> getPpevaluesResult
        {
            get
            {
                return _getPpevaluesResult;
            }
            set
            {
                if (!object.Equals(_getPpevaluesResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getPpevaluesResult", NewValue = value, OldValue = _getPpevaluesResult };
                    _getPpevaluesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        Clear.Risk.Models.ClearConnection.SwmsPperequired _swmspperequired;
        protected Clear.Risk.Models.ClearConnection.SwmsPperequired swmspperequired
        {
            get
            {
                return _swmspperequired;
            }
            set
            {
                if (!object.Equals(_swmspperequired, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "swmspperequired", NewValue = value, OldValue = _swmspperequired };
                    _swmspperequired = value;
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
            var clearConnectionGetPpevaluesResult = await ClearConnection.GetPpevalues();
            getPpevaluesResult = clearConnectionGetPpevaluesResult;

            swmspperequired = new Clear.Risk.Models.ClearConnection.SwmsPperequired(){};
        }

        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.SwmsPperequired args)
        {
            swmspperequired.SWMSID = int.Parse($"{SWMSID}");

            try
            {
                var clearConnectionCreateSwmsPperequiredResult = await ClearConnection.CreateSwmsPperequired(swmspperequired);
                DialogService.Close(swmspperequired);
            }
            catch (System.Exception clearConnectionCreateSwmsPperequiredException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new SwmsPperequired!");
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
