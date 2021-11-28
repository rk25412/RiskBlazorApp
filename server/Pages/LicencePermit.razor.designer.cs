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
    public partial class LicencePermitComponent : ComponentBase
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

        //protected RadzenGrid<Clear.Risk.Models.ClearConnection.LicencePermit> grid0;


        protected bool IsLoading = true;
        protected IList<Clear.Risk.Models.ClearConnection.LicencePermit> getLicencePermitsResult = new List<Clear.Risk.Models.ClearConnection.LicencePermit>();

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            if (!Security.IsAuthenticated())
            {
                UriHelper.NavigateTo("Login", true);
            }
            else
            {
                IsLoading = true;
                StateHasChanged();
                await Task.Delay(1);
                await Load();
                IsLoading = false;
                StateHasChanged();
            }

        }
        protected async System.Threading.Tasks.Task Load()
        {
            var clearConnectionGetLicencePermitsResult = await ClearConnection.GetLicencePermits();

            getLicencePermitsResult = clearConnectionGetLicencePermitsResult
                                           .Select(x => new Models.ClearConnection.LicencePermit
                                           {
                                               PERMIT_ID = x.PERMIT_ID,
                                               NAME = x.NAME,
                                               PERMIT_VALUE = x.PERMIT_VALUE,
                                           }).ToList();


            //getLicencePermitsResult = clearConnectionGetLicencePermitsResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddLicencePermit>("Add Licence Permit", null);
            //grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });

            await Load();
        }
        protected async System.Threading.Tasks.Task HelpClick(MouseEventArgs args)
        {
            string url = "/Help/23";
            await JSRuntime.InvokeAsync<object>("open", url, "_blank");

            // UriHelper.NavigateTo("Help" + "/" + 23);
        }
        
        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            IsLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var clearConnectionDeleteLicencePermitResult = await ClearConnection.DeleteLicencePermit(data.PERMIT_ID);
                    if (clearConnectionDeleteLicencePermitResult != null) {
                        NotificationService.Notify(NotificationSeverity.Success, $"Success", $"License Permit successfully deleted.",1800000);
                    }
                }
            }
            catch (System.Exception clearConnectionDeleteLicencePermitException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete LicencePermit", 1800000);
            }
            finally
            {
                IsLoading = false;
                StateHasChanged();
            }
        }
        protected async System.Threading.Tasks.Task GridEditButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<EditLicencePermit>("Edit Licence Permit", new Dictionary<string, object>() { { "PERMIT_ID", data.PERMIT_ID } });
            await InvokeAsync(() => { StateHasChanged(); });

            await Load();
        }
    }
}
