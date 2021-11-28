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
    public partial class EditOrderStatus: ComponentBase
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

        [Parameter]
        public dynamic ORDER_STATUS_ID { get; set; }
        protected bool IsLoading { get; set; }
        OrderStatus _orderstatus;
        protected OrderStatus orderstatus
        {
            get
            {
                return _orderstatus;
            }
            set
            {
                if (!object.Equals(_orderstatus, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "orderstatus", NewValue = value, OldValue = _orderstatus };
                    _orderstatus = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            if (!Security.IsAuthenticated())
            {
                UriHelper.NavigateTo("Login", true);
            }
            else
            {
                await Load();
            }

        }
        protected async System.Threading.Tasks.Task Load()
        {
            var clearRiskGetOrderStatusByOrderStatusIdResult = await ClearRisk.GetOrderStatusByOrderStatusId(int.Parse($"{ORDER_STATUS_ID}"));
            orderstatus = clearRiskGetOrderStatusByOrderStatusIdResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit(OrderStatus args)
        {
            IsLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {
                var clearRiskUpdateOrderStatusResult = await ClearRisk.UpdateOrderStatus(int.Parse($"{ORDER_STATUS_ID}"), orderstatus);
                IsLoading = false;
                StateHasChanged();
                DialogService.Close(orderstatus);
            }
            catch (System.Exception clearRiskUpdateOrderStatusException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to update OrderStatus");
                IsLoading = false;
                StateHasChanged();
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
