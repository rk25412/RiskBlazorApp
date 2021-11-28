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
    public partial class AddSwmsPlantequipmentComponent : ComponentBase
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

        IEnumerable<Clear.Risk.Models.ClearConnection.PlantEquipment> _getPlantEquipmentsResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.PlantEquipment> getPlantEquipmentsResult
        {
            get
            {
                return _getPlantEquipmentsResult;
            }
            set
            {
                if (!object.Equals(_getPlantEquipmentsResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getPlantEquipmentsResult", NewValue = value, OldValue = _getPlantEquipmentsResult };
                    _getPlantEquipmentsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        Clear.Risk.Models.ClearConnection.SwmsPlantequipment _swmsplantequipment;
        protected Clear.Risk.Models.ClearConnection.SwmsPlantequipment swmsplantequipment
        {
            get
            {
                return _swmsplantequipment;
            }
            set
            {
                if (!object.Equals(_swmsplantequipment, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "swmsplantequipment", NewValue = value, OldValue = _swmsplantequipment };
                    _swmsplantequipment = value;
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
            var clearConnectionGetPlantEquipmentsResult = await ClearConnection.GetPlantEquipments();
            getPlantEquipmentsResult = clearConnectionGetPlantEquipmentsResult;

            swmsplantequipment = new Clear.Risk.Models.ClearConnection.SwmsPlantequipment(){};
        }

        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.SwmsPlantequipment args)
        {
            swmsplantequipment.SWMSID = int.Parse($"{SWMSID}");

            try
            {
                var clearConnectionCreateSwmsPlantequipmentResult = await ClearConnection.CreateSwmsPlantequipment(swmsplantequipment);
                DialogService.Close(swmsplantequipment);
            }
            catch (System.Exception clearConnectionCreateSwmsPlantequipmentException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new SwmsPlantequipment!");
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}