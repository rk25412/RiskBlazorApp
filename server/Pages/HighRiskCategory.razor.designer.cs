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
    public partial class HighRiskCategoryComponent : ComponentBase
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

        //protected RadzenGrid<Clear.Risk.Models.ClearConnection.HighRiskCategory> grid0;
        protected bool IsLoading { get; set; }

        protected IList<Clear.Risk.Models.ClearConnection.HighRiskCategory> getHighRiskCategoriesResult = new List<Clear.Risk.Models.ClearConnection.HighRiskCategory>();

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
            var clearConnectionGetHighRiskCategoriesResult = await ClearConnection.GetHighRiskCategories();
            getHighRiskCategoriesResult = clearConnectionGetHighRiskCategoriesResult
                                            .Select(x => new Clear.Risk.Models.ClearConnection.HighRiskCategory
                                            {
                                                RISK_CATEGORY_ID = x.RISK_CATEGORY_ID,
                                                NAME = x.NAME,
                                            }).ToList();
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddHighRiskCategory>("Add High Risk Category", null);

            await InvokeAsync(() => { StateHasChanged(); });

            await Load();
        }
        protected async System.Threading.Tasks.Task HelpClick(MouseEventArgs args)
        {
            string url = "/Help/26";
            await JSRuntime.InvokeAsync<object>("open", url, "_blank");

            //UriHelper.NavigateTo("Help" + "/" + 26);
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
                    var clearConnectionDeleteHighRiskCategoryResult = await ClearConnection.DeleteHighRiskCategory(data.RISK_CATEGORY_ID);
                    if (clearConnectionDeleteHighRiskCategoryResult != null)
                    {
                        getHighRiskCategoriesResult.Remove(getHighRiskCategoriesResult.FirstOrDefault(o => o.RISK_CATEGORY_ID == data.RISK_CATEGORY_ID));
                        NotificationService.Notify(NotificationSeverity.Success, $"Success", $"High Risk Category deleted successfully.", 1800000);
                    }
                }
            }
            catch (System.Exception clearConnectionDeleteHighRiskCategoryException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete HighRiskCategory", 1800000);

            }
            finally
            {
                IsLoading = false;
                StateHasChanged();
            }
        }
        protected async System.Threading.Tasks.Task GridEditButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<EditHighRiskCategory>("Edit High Risk Category", new Dictionary<string, object>() { { "RISK_CATEGORY_ID", data.RISK_CATEGORY_ID } });
            await InvokeAsync(() => { StateHasChanged(); });

            await Load();
        }
    }
}
