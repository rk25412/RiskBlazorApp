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
    public partial class ReferencedLegislationComponent : ComponentBase
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


        protected bool IsLoading { get; set; }
        protected IList<Clear.Risk.Models.ClearConnection.ReferencedLegislation> getReferencedLegislationsResult = new List<Clear.Risk.Models.ClearConnection.ReferencedLegislation>();

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
            var clearConnectionGetReferencedLegislationsResult = await ClearConnection.GetReferencedLegislations();
            getReferencedLegislationsResult = clearConnectionGetReferencedLegislationsResult
                                                .Select(x => new Models.ClearConnection.ReferencedLegislation
                                                {
                                                    LEGISLATION_ID = x.LEGISLATION_ID,
                                                    NAME = x.NAME,
                                                    LEGISLATION_VALUE = x.LEGISLATION_VALUE,
                                                }).ToList();
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddReferencedLegislation>("Add Referenced Legislation", null);
            //grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });

            await Load();
        }
        protected async System.Threading.Tasks.Task HelpClick(MouseEventArgs args)
        {
            string url = "/Help/24";
            await JSRuntime.InvokeAsync<object>("open", url, "_blank");

            //UriHelper.NavigateTo("Help" + "/" + 24);
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
                    var clearConnectionDeleteReferencedLegislationResult = await ClearConnection.DeleteReferencedLegislation(data.LEGISLATION_ID);
                    if (clearConnectionDeleteReferencedLegislationResult != null)
                    {
                        getReferencedLegislationsResult.Remove(getReferencedLegislationsResult.FirstOrDefault(x => x.LEGISLATION_ID == data.LEGISLATION_ID));
                        NotificationService.Notify(NotificationSeverity.Success, $"Success", $"Referenced Legislation Deleted successfully", 1800000);
                    }
                }
            }
            catch (System.Exception clearConnectionDeleteReferencedLegislationException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete ReferencedLegislation", 1800000);
            }
            finally
            {
                IsLoading = false;
                StateHasChanged();
            }
        }
        protected async System.Threading.Tasks.Task GridEditButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<EditReferencedLegislation>("Edit Referenced Legislation", new Dictionary<string, object>() { { "LEGISLATION_ID", data.LEGISLATION_ID } });
            await InvokeAsync(() => { StateHasChanged(); });

            await Load();
        }
    }
}
