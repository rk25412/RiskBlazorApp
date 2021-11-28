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
    public partial class EditSwmsLicencespermitComponent : ComponentBase
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
        public dynamic LPID { get; set; }

        Clear.Risk.Models.ClearConnection.SwmsLicencespermit _swmslicencespermit;
        protected Clear.Risk.Models.ClearConnection.SwmsLicencespermit swmslicencespermit
        {
            get
            {
                return _swmslicencespermit;
            }
            set
            {
                if (!object.Equals(_swmslicencespermit, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "swmslicencespermit", NewValue = value, OldValue = _swmslicencespermit };
                    _swmslicencespermit = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.LicencePermit> _getLicencePermitsResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.LicencePermit> getLicencePermitsResult
        {
            get
            {
                return _getLicencePermitsResult;
            }
            set
            {
                if (!object.Equals(_getLicencePermitsResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getLicencePermitsResult", NewValue = value, OldValue = _getLicencePermitsResult };
                    _getLicencePermitsResult = value;
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
            var clearConnectionGetSwmsLicencespermitByLpidResult = await ClearConnection.GetSwmsLicencespermitByLpid(LPID);
            swmslicencespermit = clearConnectionGetSwmsLicencespermitByLpidResult;

            var clearConnectionGetLicencePermitsResult = await ClearConnection.GetLicencePermits();
            getLicencePermitsResult = clearConnectionGetLicencePermitsResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.SwmsLicencespermit args)
        {
            try
            {
                var clearConnectionUpdateSwmsLicencespermitResult = await ClearConnection.UpdateSwmsLicencespermit(LPID, swmslicencespermit);
                DialogService.Close(swmslicencespermit);
            }
            catch (System.Exception clearConnectionUpdateSwmsLicencespermitException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to update SwmsLicencespermit");
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
