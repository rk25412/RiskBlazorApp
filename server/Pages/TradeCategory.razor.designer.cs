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
    public partial class TradeCategoryComponent : ComponentBase
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

        //protected RadzenGrid<Clear.Risk.Models.ClearConnection.TradeCategory> grid0;
        protected bool IsLoading { get; set; }
        protected IList<Clear.Risk.Models.ClearConnection.TradeCategory> getTradeCategoriesResult = new List<Clear.Risk.Models.ClearConnection.TradeCategory>();

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
            var clearConnectionGetTradeCategoriesResult = await ClearConnection.GetTradeCategories();
            getTradeCategoriesResult = clearConnectionGetTradeCategoriesResult
                                            .Select(x =>
                                                new Clear.Risk.Models.ClearConnection.TradeCategory
                                                {
                                                    TRADE_CATEGORY_ID = x.TRADE_CATEGORY_ID,
                                                    TRADE_NAME = x.TRADE_NAME,
                                                    TradeCategory1 = x.TradeCategory1,
                                                    DESCRIPTION = x.DESCRIPTION,
                                                    HOURLY_COST = x.HOURLY_COST,
                                                }).ToList();
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddTradeCategory>("Add Trade Category", null);

            await InvokeAsync(() => { StateHasChanged(); });

            await Load();
        }
        protected async System.Threading.Tasks.Task HelpClick(MouseEventArgs args)
        {
            string url = "/Help/39";
            await JSRuntime.InvokeAsync<object>("open", url, "_blank");


        }
        protected async System.Threading.Tasks.Task Grid0RowSelect(Clear.Risk.Models.ClearConnection.TradeCategory args)
        {
            var dialogResult = await DialogService.OpenAsync<EditTradeCategory>("Edit Trade Category", new Dictionary<string, object>() { { "TRADE_CATEGORY_ID", args.TRADE_CATEGORY_ID } });
            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var clearConnectionDeleteTradeCategoryResult = await ClearConnection.DeleteTradeCategory(data.TRADE_CATEGORY_ID);
                    if (clearConnectionDeleteTradeCategoryResult != null)
                    {
                        getTradeCategoriesResult.Remove(getTradeCategoriesResult.FirstOrDefault(x => x.TRADE_CATEGORY_ID == data.TRADE_CATEGORY_ID));
                        NotificationService.Notify(NotificationSeverity.Success, $"Success", $"TradeCategory deleted successfully.",1800000);
                    }
                }
            }
            catch (System.Exception clearConnectionDeleteTradeCategoryException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete TradeCategory",1800000);
            }
        }
        protected async System.Threading.Tasks.Task GridEditButtonClick(MouseEventArgs args, dynamic data)
        {

            var dialogResult = await DialogService.OpenAsync<EditTradeCategory>("Edit Trade Category", new Dictionary<string, object>() { { "TRADE_CATEGORY_ID", data.TRADE_CATEGORY_ID } });
            await InvokeAsync(() => { StateHasChanged(); });

            await Load();
        }
    }
}
