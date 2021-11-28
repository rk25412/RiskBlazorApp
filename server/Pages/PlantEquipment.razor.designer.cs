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
    public partial class PlantEquipmentComponent : ComponentBase
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
        protected IList<Clear.Risk.Models.ClearConnection.PlantEquipment> getPlantEquipmentsResult = new List<Clear.Risk.Models.ClearConnection.PlantEquipment>();

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
            var clearConnectionGetPlantEquipmentsResult = await ClearConnection.GetPlantEquipments();
            getPlantEquipmentsResult = clearConnectionGetPlantEquipmentsResult
                                                .Select(x =>
                                                    new Models.ClearConnection.PlantEquipment
                                                    {
                                                        PLANT_EQUIPMENT_ID = x.PLANT_EQUIPMENT_ID,
                                                        NAME = x.NAME,
                                                        EQUIPMENT_VALUE = x.EQUIPMENT_VALUE
                                                    }
                                                 ).ToList();


            //getPlantEquipmentsResult = clearConnectionGetPlantEquipmentsResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddPlantEquipment>("Add Plant Equipment", null);
            //grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });

            await Load();
        }
        protected async System.Threading.Tasks.Task HelpClick(MouseEventArgs args)
        {
            string url = "/Help/22";
            await JSRuntime.InvokeAsync<object>("open", url, "_blank");

            //UriHelper.NavigateTo("Help" + "/" + 22);
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
                    var clearConnectionDeletePlantEquipmentResult = await ClearConnection.DeletePlantEquipment(data.PLANT_EQUIPMENT_ID);
                    if (clearConnectionDeletePlantEquipmentResult != null) 
                    {
                        getPlantEquipmentsResult.Remove(getPlantEquipmentsResult.FirstOrDefault(x => x.PLANT_EQUIPMENT_ID == data.PLANT_EQUIPMENT_ID));
                    }
                }
            }
            catch (System.Exception clearConnectionDeletePlantEquipmentException)
            {
                 NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete PlantEquipment");
            }
            finally
            {
                IsLoading = false;
                StateHasChanged();
            }
        }
        protected async System.Threading.Tasks.Task GridEditButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<EditPlantEquipment>("Edit Plant Equipment", new Dictionary<string, object>() { { "PLANT_EQUIPMENT_ID", data.PLANT_EQUIPMENT_ID } });
            await InvokeAsync(() => { StateHasChanged(); });

            await Load();
        }
    }
}
