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
    public partial class ManageHelp : ComponentBase
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

        protected IList<HelpReference> getHelpReferencesResult = new List<HelpReference>();
        
        protected bool isLoading { get; set; }
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
            var clearRiskGetHelpReferencesResult = await ClearRisk.GetHelpReferences();
            
            getHelpReferencesResult = (from x in clearRiskGetHelpReferencesResult
                                       select new HelpReference
                                       {
                                           HELP_ID = x.HELP_ID,
                                           SCREEN_ID = x.SCREEN_ID,
                                           SCREEN_NAME = x.SCREEN_NAME,
                                           HTML_CONTENT = x.HTML_CONTENT
                                       })
                                  .ToList();
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddHelpReference>("Add Help Reference", null);

            await InvokeAsync(() => { StateHasChanged(); });
            await Load();
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
                    var clearRiskDeleteHelpReferenceResult = await ClearRisk.DeleteHelpReference(int.Parse($"{data.HELP_ID}"));
                    if (clearRiskDeleteHelpReferenceResult != null)
                    {
                        getHelpReferencesResult.Remove(getHelpReferencesResult.FirstOrDefault(x => x.HELP_ID == data.HELP_ID));
                        isLoading = false;
                        StateHasChanged();
                    }
                }
                isLoading = false;
                StateHasChanged();
            }
            catch (System.Exception clearRiskDeleteHelpReferenceException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete HelpReference");
                isLoading = false;
                StateHasChanged();
            }
        }
        protected async System.Threading.Tasks.Task GridEditButtonClick(MouseEventArgs args, dynamic data)
        {

            var dialogResult = await DialogService.OpenAsync<EditHelpReference>("Edit Help Reference", new Dictionary<string, object>() { { "HELP_ID", data.HELP_ID } });

            await InvokeAsync(() => { StateHasChanged(); });
            await Load();
        }

    }
}
