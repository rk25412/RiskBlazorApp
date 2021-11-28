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
    public partial class AddControlMeasureValueComponent : ComponentBase
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
        protected bool IsLoading { get; set; }
        Clear.Risk.Models.ClearConnection.ControlMeasureValue _controlmeasurevalue;
        protected Clear.Risk.Models.ClearConnection.ControlMeasureValue controlmeasurevalue
        {
            get
            {
                return _controlmeasurevalue;
            }
            set
            {
                if (!object.Equals(_controlmeasurevalue, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "controlmeasurevalue", NewValue = value, OldValue = _controlmeasurevalue };
                    _controlmeasurevalue = value;
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
            controlmeasurevalue = new Clear.Risk.Models.ClearConnection.ControlMeasureValue(){};
        }

        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.ControlMeasureValue args)
        {
            IsLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {
                var clearConnectionCreateControlMeasureValueResult = await ClearConnection.CreateControlMeasureValue(controlmeasurevalue);
                IsLoading = false;
                StateHasChanged();

                DialogService.Close(controlmeasurevalue);
            }
            catch (System.Exception clearConnectionCreateControlMeasureValueException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new ControlMeasureValue!");
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