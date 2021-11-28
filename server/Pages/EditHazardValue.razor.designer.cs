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
    public partial class EditHazardValueComponent : ComponentBase
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
        public dynamic HAZARD_ID { get; set; }

        Clear.Risk.Models.ClearConnection.HazardValue _hazardvalue;
        protected bool IsLoading { get; set; }
        protected Clear.Risk.Models.ClearConnection.HazardValue hazardvalue
        {
            get
            {
                return _hazardvalue;
            }
            set
            {
                if (!object.Equals(_hazardvalue, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "hazardvalue", NewValue = value, OldValue = _hazardvalue };
                    _hazardvalue = value;
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
            var clearConnectionGetHazardValueByHazardIdResult = await ClearConnection.GetHazardValueByHazardId(HAZARD_ID);
            hazardvalue = clearConnectionGetHazardValueByHazardIdResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.HazardValue args)
        {
            IsLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {
                var clearConnectionUpdateHazardValueResult = await ClearConnection.UpdateHazardValue(HAZARD_ID, hazardvalue);
                IsLoading = false;
                StateHasChanged();

                DialogService.Close(hazardvalue);
            }
            catch (System.Exception clearConnectionUpdateHazardValueException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to update HazardValue");
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
