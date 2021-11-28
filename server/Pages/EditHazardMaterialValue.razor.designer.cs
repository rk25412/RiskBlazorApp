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
    public partial class EditHazardMaterialValueComponent : ComponentBase
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
        public dynamic HAZARD_MATERIAL_ID { get; set; }
        protected bool IsLoading { get; set; }
        Clear.Risk.Models.ClearConnection.HazardMaterialValue _hazardmaterialvalue;
        protected Clear.Risk.Models.ClearConnection.HazardMaterialValue hazardmaterialvalue
        {
            get
            {
                return _hazardmaterialvalue;
            }
            set
            {
                if (!object.Equals(_hazardmaterialvalue, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "hazardmaterialvalue", NewValue = value, OldValue = _hazardmaterialvalue };
                    _hazardmaterialvalue = value;
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
            var clearConnectionGetHazardMaterialValueByHazardMaterialIdResult = await ClearConnection.GetHazardMaterialValueByHazardMaterialId(HAZARD_MATERIAL_ID);
            hazardmaterialvalue = clearConnectionGetHazardMaterialValueByHazardMaterialIdResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.HazardMaterialValue args)
        {
            IsLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {
                var clearConnectionUpdateHazardMaterialValueResult = await ClearConnection.UpdateHazardMaterialValue(HAZARD_MATERIAL_ID, hazardmaterialvalue);
                IsLoading = false;
                StateHasChanged();
                DialogService.Close(hazardmaterialvalue);
            }
            catch (System.Exception clearConnectionUpdateHazardMaterialValueException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to update HazardMaterialValue");
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
