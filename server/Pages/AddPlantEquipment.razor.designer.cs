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
    public partial class AddPlantEquipmentComponent : ComponentBase
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
        Clear.Risk.Models.ClearConnection.PlantEquipment _plantequipment;
        protected Clear.Risk.Models.ClearConnection.PlantEquipment plantequipment
        {
            get
            {
                return _plantequipment;
            }
            set
            {
                if (!object.Equals(_plantequipment, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "plantequipment", NewValue = value, OldValue = _plantequipment };
                    _plantequipment = value;
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
            plantequipment = new Clear.Risk.Models.ClearConnection.PlantEquipment(){};
        }

        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.PlantEquipment args)
        {
            IsLoading = true;
            StateHasChanged();
            await Task.Delay(1);

            try
            {
                var clearConnectionCreatePlantEquipmentResult = await ClearConnection.CreatePlantEquipment(plantequipment);
                IsLoading = false;
                StateHasChanged();
                DialogService.Close(plantequipment);
            }
            catch (System.Exception clearConnectionCreatePlantEquipmentException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new PlantEquipment!");
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
