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
    public partial class EditWorkOrderType: ComponentBase
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
        public dynamic WORK_ORDER_TYPE_ID { get; set; }
        protected bool IsLoading { get; set; }
        Clear.Risk.Models.ClearConnection.WorkOrderType _workordertype;
        protected Clear.Risk.Models.ClearConnection.WorkOrderType workordertype
        {
            get
            {
                return _workordertype;
            }
            set
            {
                if (!object.Equals(_workordertype, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "workordertype", NewValue = value, OldValue = _workordertype };
                    _workordertype = value;
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
            var clearRiskGetWorkOrderTypeByWorkOrderTypeIdResult = await ClearRisk.GetWorkOrderTypeByWorkOrderTypeId(WORK_ORDER_TYPE_ID);
            workordertype = clearRiskGetWorkOrderTypeByWorkOrderTypeIdResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.WorkOrderType args)
        {
            IsLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {
                var clearRiskUpdateWorkOrderTypeResult = await ClearRisk.UpdateWorkOrderType(WORK_ORDER_TYPE_ID, workordertype);
                IsLoading = false;
                StateHasChanged();
                DialogService.Close(workordertype);
            }
            catch (System.Exception clearRiskUpdateWorkOrderTypeException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to update WorkOrderType");
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
