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
    public partial class EditLicencePermitComponent : ComponentBase
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
        public dynamic PERMIT_ID { get; set; }
        protected bool IsLoading { get; set; }
        Clear.Risk.Models.ClearConnection.LicencePermit _licencepermit;
        protected Clear.Risk.Models.ClearConnection.LicencePermit licencepermit
        {
            get
            {
                return _licencepermit;
            }
            set
            {
                if (!object.Equals(_licencepermit, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "licencepermit", NewValue = value, OldValue = _licencepermit };
                    _licencepermit = value;
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
            var clearConnectionGetLicencePermitByPermitIdResult = await ClearConnection.GetLicencePermitByPermitId(PERMIT_ID);
            licencepermit = clearConnectionGetLicencePermitByPermitIdResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.LicencePermit args)
        {
            IsLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {
                var clearConnectionUpdateLicencePermitResult = await ClearConnection.UpdateLicencePermit(PERMIT_ID, licencepermit);
                IsLoading = false;
                StateHasChanged();
                DialogService.Close(licencepermit);
            }
            catch (System.Exception clearConnectionUpdateLicencePermitException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to update LicencePermit");
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
