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
    public partial class HazardMaterialValueComponent : ComponentBase
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

        //protected RadzenGrid<Clear.Risk.Models.ClearConnection.HazardMaterialValue> grid0;
        protected bool IsLoading = true;
        protected IList<Clear.Risk.Models.ClearConnection.HazardMaterialValue> getHazardMaterialValuesResult = new List<Clear.Risk.Models.ClearConnection.HazardMaterialValue>();
        

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
            var clearConnectionGetHazardMaterialValuesResult = await ClearConnection.GetHazardMaterialValues();

            getHazardMaterialValuesResult = (from x in clearConnectionGetHazardMaterialValuesResult
                                             select new Models.ClearConnection.HazardMaterialValue
                                             {
                                                 HAZARD_MATERIAL_ID = x.HAZARD_MATERIAL_ID,
                                                 NAME = x.NAME,
                                                 MATERIAL_VALUE = x.MATERIAL_VALUE,
                                             }).ToList();


            //getHazardMaterialValuesResult = clearConnectionGetHazardMaterialValuesResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddHazardMaterialValue>("Add Hazard Material Value", null);
            //grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });

            await Load();
        }
        protected async System.Threading.Tasks.Task HelpClick(MouseEventArgs args)
        {
            string url = "/Help/19";
            await JSRuntime.InvokeAsync<object>("open", url, "_blank");

            //SUriHelper.NavigateTo("Help" + "/" + 19);
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
                    var clearConnectionDeleteHazardMaterialValueResult = await ClearConnection.DeleteHazardMaterialValue(data.HAZARD_MATERIAL_ID);
                    if (clearConnectionDeleteHazardMaterialValueResult != null)
                    {
                        getHazardMaterialValuesResult.Remove(getHazardMaterialValuesResult.FirstOrDefault(x => x.HAZARD_MATERIAL_ID == data.HAZARD_MATERIAL_ID));
                        NotificationService.Notify(NotificationSeverity.Success, $"Success", $"Hazard Material Value successfully deleted.",180000);
                    }
                }
            }
            catch (System.Exception clearConnectionDeleteHazardMaterialValueException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete HazardMaterialValue");
            }
            finally
            {
                IsLoading = false;
                StateHasChanged();
            }
        }
        protected async System.Threading.Tasks.Task GridEditButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<EditHazardMaterialValue>("Edit Hazard Material Value", new Dictionary<string, object>() { { "HAZARD_MATERIAL_ID", data.HAZARD_MATERIAL_ID } });
            await InvokeAsync(() => { StateHasChanged(); });

            await Load();
        }
    }
}
