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
    public partial class HazardValueComponent : ComponentBase
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
        protected bool IsLoading = true;
        protected IList<Clear.Risk.Models.ClearConnection.HazardValue> getHazardValuesResult = new List<Clear.Risk.Models.ClearConnection.HazardValue>();


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
            var clearConnectionGetHazardValuesResult = await ClearConnection.GetHazardValues();
            getHazardValuesResult = (from x in clearConnectionGetHazardValuesResult
                                     select new Models.ClearConnection.HazardValue
                                     {
                                         HAZARD_ID = x.HAZARD_ID,
                                         NAME = x.NAME,
                                         HAZARD_VALUE1 = x.HAZARD_VALUE1,
                                     }).ToList();


            //getHazardValuesResult = clearConnectionGetHazardValuesResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddHazardValue>("Add Hazard Value", null);
            await InvokeAsync(() => { StateHasChanged(); });

            await Load();
        }
        protected async System.Threading.Tasks.Task HelpClick(MouseEventArgs args)
        {
            string url = "/Help/17";
            await JSRuntime.InvokeAsync<object>("open", url, "_blank");

            // UriHelper.NavigateTo("Help" + "/" + 17);
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
                    var clearConnectionDeleteHazardValueResult = await ClearConnection.DeleteHazardValue(data.HAZARD_ID);
                    if (clearConnectionDeleteHazardValueResult != null)
                    {
                        getHazardValuesResult.Remove(getHazardValuesResult.FirstOrDefault(i => i.HAZARD_ID == data.HAZARD_ID));
                        NotificationService.Notify(NotificationSeverity.Success, $"Success", $"Hazard value deleted successfully.");
                    }
                }
            }
            catch (System.Exception clearConnectionDeleteHazardValueException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete HazardValue");
            }
            finally
            {
                IsLoading = false;
                StateHasChanged();
            }
        }
        protected async System.Threading.Tasks.Task GridEditButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<EditHazardValue>("Edit Hazard Value", new Dictionary<string, object>() { { "HAZARD_ID", data.HAZARD_ID } });
            await InvokeAsync(() => { StateHasChanged(); });

            await Load();
        }
    }
}
