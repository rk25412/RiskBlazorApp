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
    public partial class ImpactTypeComponent : ComponentBase
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

        //protected RadzenGrid<Clear.Risk.Models.ClearConnection.ImpactType> grid0;
        protected bool IsLoading { get; set; }
        protected IList<Clear.Risk.Models.ClearConnection.ImpactType> getImpactTypesResult = new List<Clear.Risk.Models.ClearConnection.ImpactType>();

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
            var clearConnectionGetImpactTypesResult = await ClearConnection.GetImpactTypes();

            getImpactTypesResult = (from x in clearConnectionGetImpactTypesResult
                                    select new Models.ClearConnection.ImpactType
                                    {
                                        IMPACT_TYPE_ID = x.IMPACT_TYPE_ID,
                                        NAME = x.NAME,
                                        IMPACT_VALUE = x.IMPACT_VALUE,
                                    }).ToList();
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddImpactType>("Add Impact Type", null);
            //grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
            var clearConnectionGetImpactTypesResult = await ClearConnection.GetImpactTypes();

            getImpactTypesResult = (from x in clearConnectionGetImpactTypesResult
                                    select new Models.ClearConnection.ImpactType
                                    {
                                        IMPACT_TYPE_ID = x.IMPACT_TYPE_ID,
                                        NAME = x.NAME,
                                        IMPACT_VALUE = x.IMPACT_VALUE,
                                    }).ToList();

        }
        protected async System.Threading.Tasks.Task HelpClick(MouseEventArgs args)
        {
            string url = "/Help/15";
            await JSRuntime.InvokeAsync<object>("open", url, "_blank");

            //UriHelper.NavigateTo("Help" + "/" + 15);
        }
        //protected async System.Threading.Tasks.Task Grid0RowSelect(Clear.Risk.Models.ClearConnection.ImpactType args)
        //{
        //    var dialogResult = await DialogService.OpenAsync<EditImpactType>("Edit Impact Type", new Dictionary<string, object>() { { "IMPACT_TYPE_ID", args.IMPACT_TYPE_ID } });
        //    await InvokeAsync(() => { StateHasChanged(); });
        //}

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            IsLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var clearConnectionDeleteImpactTypeResult = await ClearConnection.DeleteImpactType(data.IMPACT_TYPE_ID);
                    if (clearConnectionDeleteImpactTypeResult != null)
                    {
                        getImpactTypesResult.Remove(getImpactTypesResult.FirstOrDefault(i => i.IMPACT_TYPE_ID == data.IMPACT_TYPE_ID));
                        NotificationService.Notify(NotificationSeverity.Success, $"Success", $"Impact Type Deleted Successfully.", 180000);
                    }
                }
            }
            catch (System.Exception clearConnectionDeleteImpactTypeException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete ImpactType", 180000);
            }
            finally
            {
                IsLoading = false;
                StateHasChanged();
            }
        }
        protected async System.Threading.Tasks.Task GridEditButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<EditImpactType>("Edit Impact Type", new Dictionary<string, object>() { { "IMPACT_TYPE_ID", data.IMPACT_TYPE_ID } });
            await InvokeAsync(() => { StateHasChanged(); });

            var clearConnectionGetImpactTypesResult = await ClearConnection.GetImpactTypes();

            getImpactTypesResult = (from x in clearConnectionGetImpactTypesResult
                                    select new Models.ClearConnection.ImpactType
                                    {
                                        IMPACT_TYPE_ID = x.IMPACT_TYPE_ID,
                                        NAME = x.NAME,
                                        IMPACT_VALUE = x.IMPACT_VALUE,
                                    }).ToList();
        }
    }
}
