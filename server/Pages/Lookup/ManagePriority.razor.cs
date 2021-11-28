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
    public partial class ManagePriority : ComponentBase
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

        //protected RadzenGrid<PriorityMaster> grid0;
        protected bool IsLoading { get; set; }

        protected IList<PriorityMaster> getPriorityMastersResult = new List<PriorityMaster>();

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
            var clearRiskGetPriorityMastersResult = await ClearRisk.GetPriorityMasters();
            getPriorityMastersResult = (from x in clearRiskGetPriorityMastersResult
                                        select new PriorityMaster {
                                        PRIORITY_ID = x.PRIORITY_ID,
                                        NAME = x.NAME
                                        }).ToList();
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddPriorityMaster>("Add Priority Master", null);
            //grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
            await Load();
        }
        protected async System.Threading.Tasks.Task HelpClick(MouseEventArgs args)
        {
            string url = "/Help/33";
            await JSRuntime.InvokeAsync<object>("open", url, "_blank");

            //UriHelper.NavigateTo("Help" + "/" + 33);
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
                    var clearRiskDeletePriorityMasterResult = await ClearRisk.DeletePriorityMaster(int.Parse($"{data.PRIORITY_ID}"));
                    if (clearRiskDeletePriorityMasterResult != null)
                    {
                        getPriorityMastersResult.Remove(getPriorityMastersResult.FirstOrDefault(x => x.PRIORITY_ID == data.PRIORITY_ID));
                        IsLoading = false;
                        StateHasChanged();
                    }
                }
                IsLoading = false;
                StateHasChanged();
            }
            catch (System.Exception clearRiskDeletePriorityMasterException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete PriorityMaster");
                IsLoading = false;
                StateHasChanged();
            }
        }
        protected async System.Threading.Tasks.Task GridEditButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<EditPriorityMaster>("Edit Priority Master", new Dictionary<string, object>() { { "PRIORITY_ID", data.PRIORITY_ID } });
            await InvokeAsync(() => { StateHasChanged(); });
            await Load();
        }
    }
}
