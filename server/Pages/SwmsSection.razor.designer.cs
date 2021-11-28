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
    public partial class SwmsSectionComponent : ComponentBase
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


        protected IList<Clear.Risk.Models.ClearConnection.SwmsSection> getSwmsSectionsResult = new List<Clear.Risk.Models.ClearConnection.SwmsSection>();

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
            var clearConnectionGetSwmsSectionsResult = await ClearConnection.GetSwmsSections();
            getSwmsSectionsResult = clearConnectionGetSwmsSectionsResult
                                    .Select(x => new Clear.Risk.Models.ClearConnection.SwmsSection
                                    {
                                        SWMS_SECTION_ID = x.SWMS_SECTION_ID,
                                        NAME = x.NAME,
                                    }).ToList();
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddSwmsSection>("Add Swms Section", null);
            //grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });

            await Load();
        }
        protected async System.Threading.Tasks.Task HelpClick(MouseEventArgs args)
        {
            string url = "/Help/25";
            await JSRuntime.InvokeAsync<object>("open", url, "_blank");

            //UriHelper.NavigateTo("Help" + "/" + 25);
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
                    var clearConnectionDeleteSwmsSectionResult = await ClearConnection.DeleteSwmsSection(data.SWMS_SECTION_ID);
                    if (clearConnectionDeleteSwmsSectionResult != null)
                    {
                        getSwmsSectionsResult.Remove(getSwmsSectionsResult.FirstOrDefault(x => x.SWMS_SECTION_ID == data.SWMS_SECTION_ID));
                        NotificationService.Notify(NotificationSeverity.Success, $"Success", $"SWMS Section Deleted Successfully");
                    }
                }
            }
            catch (System.Exception clearConnectionDeleteSwmsSectionException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete SwmsSection");
            }
            finally
            {
                IsLoading = false;
                StateHasChanged();
            }
        }
        protected async System.Threading.Tasks.Task GridEditButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<EditSwmsSection>("Edit Swms Section", new Dictionary<string, object>() { { "SWMS_SECTION_ID", data.SWMS_SECTION_ID } });
            await InvokeAsync(() => { StateHasChanged(); });


            await Load();
        }
    }
}
