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
    public partial class ControlMeasureValueComponent : ComponentBase
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

        //protected RadzenGrid<Clear.Risk.Models.ClearConnection.ControlMeasureValue> grid0;
        protected bool IsLoading = true;
        protected IList<Clear.Risk.Models.ClearConnection.ControlMeasureValue> getControlMeasureValuesResult = new List<Models.ClearConnection.ControlMeasureValue>();

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
            var clearConnectionGetControlMeasureValuesResult = await ClearConnection.GetControlMeasureValues();
            getControlMeasureValuesResult = (from x in clearConnectionGetControlMeasureValuesResult
                                             select new Models.ClearConnection.ControlMeasureValue 
                                             {
                                                 CONTROL_MEASURE_ID = x.CONTROL_MEASURE_ID,
                                                 NAME = x.NAME,
                                                 CONTROL_VALUE = x.CONTROL_VALUE,
                                             }).ToList();


            //getControlMeasureValuesResult = clearConnectionGetControlMeasureValuesResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddControlMeasureValue>("Add Control Measure Value", null);

            await InvokeAsync(() => { StateHasChanged(); });

            await Load();
        }
        protected async System.Threading.Tasks.Task HelpClick(MouseEventArgs args)
        {
            string url = "/Help/18";
            await JSRuntime.InvokeAsync<object>("open", url, "_blank");

            //UriHelper.NavigateTo("Help" + "/" + 18);
        }


        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var clearConnectionDeleteControlMeasureValueResult = await ClearConnection.DeleteControlMeasureValue(data.CONTROL_MEASURE_ID);
                    if (clearConnectionDeleteControlMeasureValueResult != null)
                    {
                        getControlMeasureValuesResult.Remove(getControlMeasureValuesResult.FirstOrDefault(x => x.CONTROL_MEASURE_ID == data.CONTROL_MEASURE_ID));
                        NotificationService.Notify(NotificationSeverity.Success, $"Success", $"Control Measure Value is successfully deleted.", 180000);
                    }
                }
            }
            catch (System.Exception clearConnectionDeleteControlMeasureValueException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete ControlMeasureValue",180000);
            }
            finally
            {
                IsLoading = false;
                StateHasChanged();
            }
        }
        protected async System.Threading.Tasks.Task GridEditButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<EditControlMeasureValue>("Edit Control Measure Value", new Dictionary<string, object>() { { "CONTROL_MEASURE_ID", data.CONTROL_MEASURE_ID } });
            await InvokeAsync(() => { StateHasChanged(); });

            await Load();
        }
    }
}
