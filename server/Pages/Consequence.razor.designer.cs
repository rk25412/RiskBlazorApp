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
    public partial class ConsequenceComponent : ComponentBase
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
        //protected RadzenGrid<Clear.Risk.Models.ClearConnection.Consequence> grid0;
        protected bool isLoading = true;
        protected IList<Clear.Risk.Models.ClearConnection.Consequence> getConsequencesResult = new List<Clear.Risk.Models.ClearConnection.Consequence>();
        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            if (!Security.IsAuthenticated())
            {
                UriHelper.NavigateTo("Login", true);
            }
            else
            {
                isLoading = true;
                StateHasChanged();
                await Task.Delay(1);
                await Load();
                isLoading = false;
                StateHasChanged();
            }

        }
        protected async System.Threading.Tasks.Task Load()
        {
            var clearConnectionGetConsequencesResult = await ClearConnection.GetConsequences();

            getConsequencesResult = (from x in clearConnectionGetConsequencesResult
                                     select new Clear.Risk.Models.ClearConnection.Consequence
                                     {
                                         CONSEQUENCE_ID = x.CONSEQUENCE_ID,
                                         NAME = x.NAME,
                                         CONSEQUENCE_VALUE = x.CONSEQUENCE_VALUE,
                                     }).ToList();


            //getConsequencesResult = clearConnectionGetConsequencesResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddConsequence>("Add Consequence", null);

            await InvokeAsync(() => { StateHasChanged(); });

            await Load();
        }
        protected async System.Threading.Tasks.Task HelpClick(MouseEventArgs args)
        {
            string url = "/Help/21";
            await JSRuntime.InvokeAsync<object>("open", url, "_blank");

            // UriHelper.NavigateTo("Help" + "/" + 21);
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            isLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var clearConnectionDeleteConsequenceResult = await ClearConnection.DeleteConsequence(data.CONSEQUENCE_ID);
                    if (clearConnectionDeleteConsequenceResult != null)
                    {

                        getConsequencesResult.Remove(getConsequencesResult.FirstOrDefault(x => x.CONSEQUENCE_ID == data.CONSEQUENCE_ID));
                        NotificationService.Notify(NotificationSeverity.Success, $"Success", $"Consequence deleted successfully.", 1800000);
                    }
                }
            }
            catch (System.Exception clearConnectionDeleteConsequenceException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete Consequence", 1800000);
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }
        protected async System.Threading.Tasks.Task GridEditButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<EditConsequence>("Edit Consequence", new Dictionary<string, object>() { { "CONSEQUENCE_ID", data.CONSEQUENCE_ID } });
            await InvokeAsync(() => { StateHasChanged(); });

            await Load();
        }
    }
}
