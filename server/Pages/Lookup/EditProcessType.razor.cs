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

namespace Clear.Risk.Pages.Lookup
{
    public partial class EditProcessType: ComponentBase
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
        protected ClearConnectionService ClearRisk { get; set; }

        [Parameter]
        public dynamic PROCESS_TYPE_ID { get; set; }
        protected bool IsLoading { get; set; }
        ProcessType _processtype;
        protected  ProcessType processtype
        {
            get
            {
                return _processtype;
            }
            set
            {
                if (!object.Equals(_processtype, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "processtype", NewValue = value, OldValue = _processtype };
                    _processtype = value;
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
            var clearRiskGetProcessTypeByProcessTypeIdResult = await ClearRisk.GetProcessTypeByProcessTypeId(int.Parse($"{PROCESS_TYPE_ID}"));
            processtype = clearRiskGetProcessTypeByProcessTypeIdResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit( ProcessType args)
        {
            IsLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {
                var clearRiskUpdateProcessTypeResult = await ClearRisk.UpdateProcessType(int.Parse($"{PROCESS_TYPE_ID}"), processtype);
                IsLoading = false;
                StateHasChanged();
                DialogService.Close(processtype);
            }
            catch (System.Exception clearRiskUpdateProcessTypeException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to update ProcessType");
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
