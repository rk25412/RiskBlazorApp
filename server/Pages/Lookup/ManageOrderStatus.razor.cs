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
    public partial class ManageOrderStatus : ComponentBase
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

        protected IList<OrderStatus> getOrderStatusesResult = new List<OrderStatus>();

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
            var clearRiskGetOrderStatusesResult = await ClearRisk.GetOrderStatuses(new Query());
            getOrderStatusesResult = (from x in clearRiskGetOrderStatusesResult
                                      select new OrderStatus
                                      {
                                          ORDER_STATUS_ID = x.ORDER_STATUS_ID,
                                          NAME = x.NAME
                                      }).ToList();
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddOrderStatus>("Add Order Status", null);
            //grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
            await Load();
        }
        protected async System.Threading.Tasks.Task HelpClick(MouseEventArgs args)
        {
            string url = "/Help/32";
            await JSRuntime.InvokeAsync<object>("open", url, "_blank");

            //UriHelper.NavigateTo("Help" + "/" + 32);
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
                    var clearRiskDeleteOrderStatusResult = await ClearRisk.DeleteOrderStatus(int.Parse($"{data.ORDER_STATUS_ID}"));
                    if (clearRiskDeleteOrderStatusResult != null)
                    {
                        getOrderStatusesResult.Remove(getOrderStatusesResult.FirstOrDefault(x => x.ORDER_STATUS_ID == data.ORDER_STATUS_ID));
                        IsLoading = false;
                        StateHasChanged();

                    }
                }
                IsLoading = false;
                StateHasChanged();
            }
            catch (System.Exception clearRiskDeleteOrderStatusException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete OrderStatus");
                IsLoading = false;
                StateHasChanged();

            }
        }
        protected async System.Threading.Tasks.Task GridEditButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<EditOrderStatus>("Edit Order Status", new Dictionary<string, object>() { { "ORDER_STATUS_ID", data.ORDER_STATUS_ID } });
            await InvokeAsync(() => { StateHasChanged(); });
            await Load();
        }
    }
}
