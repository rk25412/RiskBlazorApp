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
using Clear.Risk.Data;

namespace Clear.Risk.Pages.Lookup
{
    public partial class EditCriticalityMaster: ComponentBase
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
        public dynamic CRITICALITY_ID { get; set; }
        protected bool IsLoading { get; set; }
        CriticalityMaster _criticalitymaster;
        protected CriticalityMaster criticalitymaster
        {
            get
            {
                return _criticalitymaster;
            }
            set
            {
                if (!object.Equals(_criticalitymaster, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "criticalitymaster", NewValue = value, OldValue = _criticalitymaster };
                    _criticalitymaster = value;
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
            var clearRiskGetCriticalityMasterByCriticalityIdResult = await ClearRisk.GetCriticalityMasterByCriticalityId(int.Parse($"{CRITICALITY_ID}"));
            criticalitymaster = clearRiskGetCriticalityMasterByCriticalityIdResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit(CriticalityMaster args)
        {
            IsLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {
                var clearRiskUpdateCriticalityMasterResult = await ClearRisk.UpdateCriticalityMaster(int.Parse($"{CRITICALITY_ID}"), criticalitymaster);
                IsLoading = false;
                StateHasChanged();
                DialogService.Close(criticalitymaster);
            }
            catch (System.Exception clearRiskUpdateCriticalityMasterException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to update CriticalityMaster");
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
