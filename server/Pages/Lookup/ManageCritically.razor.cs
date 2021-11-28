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

namespace Clear.Risk.Pages.Lookup
{
    public partial class ManageCritically : ComponentBase
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
        protected bool IsLoading { get; set; }


        protected IList<CriticalityMaster> getCriticalityMastersResult = new List<CriticalityMaster>();

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
            var clearRiskGetCriticalityMastersResult = await ClearRisk.GetCriticalityMasters();
            getCriticalityMastersResult = (from x in clearRiskGetCriticalityMastersResult
                                           select new CriticalityMaster { 
                                           CRITICALITY_ID = x.CRITICALITY_ID,
                                           NAME = x.NAME
                                           }).ToList();
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddCriticalityMaster>("Add Criticality Master", null);
            //grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
            await Load();
        }
        protected async System.Threading.Tasks.Task HelpClick(MouseEventArgs args)
        {
            string url = "/Help/34";
            await JSRuntime.InvokeAsync<object>("open", url, "_blank");


            //UriHelper.NavigateTo("Help" + "/" + 34);
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
                    var clearRiskDeleteCriticalityMasterResult = await ClearRisk.DeleteCriticalityMaster(int.Parse($"{data.CRITICALITY_ID}"));
                    if (clearRiskDeleteCriticalityMasterResult != null)
                    {
                        getCriticalityMastersResult.Remove(getCriticalityMastersResult.FirstOrDefault(x => x.CRITICALITY_ID == data.CRITICALITY_ID));
                        IsLoading = false;
                        StateHasChanged();
                    }
                }
                IsLoading = false;
                StateHasChanged();
            }
            catch (System.Exception clearRiskDeleteCriticalityMasterException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete CriticalityMaster");
                IsLoading = false;
                StateHasChanged();
            }
        }
        protected async System.Threading.Tasks.Task GridEditButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<EditCriticalityMaster>("Edit Criticality Master", new Dictionary<string, object>() { { "CRITICALITY_ID", data.CRITICALITY_ID } });
            await InvokeAsync(() => { StateHasChanged(); });
            await Load();
        }
    }
}
