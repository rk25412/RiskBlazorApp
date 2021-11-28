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
    public partial class PpevalueComponent : ComponentBase
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
        protected IList<Clear.Risk.Models.ClearConnection.Ppevalue> getPpevaluesResult = new List<Clear.Risk.Models.ClearConnection.Ppevalue>();

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
            var clearConnectionGetPpevaluesResult = await ClearConnection.GetPpevalues();
            getPpevaluesResult = clearConnectionGetPpevaluesResult.Select(x => new Clear.Risk.Models.ClearConnection.Ppevalue
            {
                PPE_ID = x.PPE_ID,
                KEY_VALUE = x.KEY_VALUE,
                KEY_DISPLAY = x.KEY_DISPLAY,
                ACTIVE = x.ACTIVE,
                ICONPATH = x.ICONPATH

            }).ToList();
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddPpevalue>("Add Ppevalue", null);
            //grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
            await Load();
        }
        protected async System.Threading.Tasks.Task HelpClick(MouseEventArgs args)
        {
            string url = "/Help/27";
            await JSRuntime.InvokeAsync<object>("open", url, "_blank");

            //UriHelper.NavigateTo("Help" + "/" + 27);
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
                    var clearConnectionDeletePpevalueResult = await ClearConnection.DeletePpevalue(data.PPE_ID);
                    if (clearConnectionDeletePpevalueResult != null)
                    {
                        getPpevaluesResult.Remove(getPpevaluesResult.FirstOrDefault(x => x.PPE_ID == data.PPE_ID));
                        IsLoading = false;
                        StateHasChanged();
                    }
                }
                IsLoading = false;
                StateHasChanged();
            }
            catch (System.Exception clearConnectionDeletePpevalueException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete Ppevalue");
                IsLoading = false;
                StateHasChanged();
            }
        }
        protected async System.Threading.Tasks.Task GridEditButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<EditPpevalue>("Edit Ppevalue", new Dictionary<string, object>() { { "PPE_ID", data.PPE_ID } });
            await InvokeAsync(() => { StateHasChanged(); });
            await Load();
        }

    }
}
