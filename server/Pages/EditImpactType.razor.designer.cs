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
    public partial class EditImpactTypeComponent : ComponentBase
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
        public dynamic IMPACT_TYPE_ID { get; set; }
        protected bool IsLoading { get; set; }
        Clear.Risk.Models.ClearConnection.ImpactType _impacttype;
        protected Clear.Risk.Models.ClearConnection.ImpactType impacttype
        {
            get
            {
                return _impacttype;
            }
            set
            {
                if (!object.Equals(_impacttype, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "impacttype", NewValue = value, OldValue = _impacttype };
                    _impacttype = value;
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
            var clearConnectionGetImpactTypeByImpactTypeIdResult = await ClearConnection.GetImpactTypeByImpactTypeId(IMPACT_TYPE_ID);
            impacttype = clearConnectionGetImpactTypeByImpactTypeIdResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.ImpactType args)
        {
            IsLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {
                var clearConnectionUpdateImpactTypeResult = await ClearConnection.UpdateImpactType(IMPACT_TYPE_ID, impacttype);
                IsLoading = false;
                StateHasChanged();
                DialogService.Close(impacttype);
            }
            catch (System.Exception clearConnectionUpdateImpactTypeException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to update ImpactType");
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
