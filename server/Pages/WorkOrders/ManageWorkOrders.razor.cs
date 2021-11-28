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

namespace Clear.Risk.Pages.WorkOrders
{
    public partial class ManageWorkOrders : ComponentBase
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
        protected WorkOrderService ClearRisk { get; set; }

        protected RadzenGrid<WorkOrder> grid0;

        //IEnumerable<WorkOrder> _getWorkOrdersResult;
        protected IList<WorkOrder> getWorkOrdersResult = new List<WorkOrder>();

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
            if (Security.IsInRole("System Administrator"))
            {
                var clearRiskGetWorkOrdersResult = await ClearRisk.GetWorkOrders(new Query());
                getWorkOrdersResult = (from x in clearRiskGetWorkOrdersResult
                                       select new WorkOrder
                                       {
                                           WORK_ORDER_ID = x.WORK_ORDER_ID,
                                           WORK_ORDER_NUMBER = x.WORK_ORDER_NUMBER,
                                           DATE_RAISED = x.DATE_RAISED.Date,
                                           DUE_DATE = x.DUE_DATE.Date,
                                           OrderStatus = x.OrderStatus ?? null,
                                           PriorityMaster = x.PriorityMaster ?? null,
                                           EntityStatus = x.EntityStatus ?? null,
                                           WarningLevel = x.WarningLevel ?? null,
                                           DESCRIPTION = x.DESCRIPTION
                                       }).ToList();
            }
            else
            {
                var clearRiskGetWorkOrdersResult = await ClearRisk.GetWorkOrders(Security.getCompanyId(), new Query());
                getWorkOrdersResult = (from x in clearRiskGetWorkOrdersResult
                                       select new WorkOrder
                                       {
                                           WORK_ORDER_ID = x.WORK_ORDER_ID,
                                           WORK_ORDER_NUMBER = x.WORK_ORDER_NUMBER,
                                           DATE_RAISED = x.DATE_RAISED,
                                           DUE_DATE = x.DUE_DATE,
                                           OrderStatus = x.OrderStatus ?? null,
                                           PriorityMaster = x.PriorityMaster ?? null,
                                           EntityStatus = x.EntityStatus ?? null,
                                           WarningLevel = x.WarningLevel ?? null,
                                           DESCRIPTION = x.DESCRIPTION
                                       })
                                 .ToList();
            }
        }



        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            UriHelper.NavigateTo("add-work-order");
        }
        protected async System.Threading.Tasks.Task HelpClick(MouseEventArgs args)
        {

            string url = "/Help/2";
            await JSRuntime.InvokeAsync<object>("open", url, "_blank");


            //UriHelper.NavigateTo("Help" + "/" + 2);
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(WorkOrder args)
        {
            // var dialogResult = await DialogService.OpenAsync<EditWorkOrder>("Edit Work Order", new Dictionary<string, object>() { { "WORK_ORDER_ID", args.WORK_ORDER_ID } });
            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {

                UriHelper.NavigateTo("edit-work-order" + "/" + data.WORK_ORDER_ID.ToString());

            }
            catch (System.Exception clearRiskDeleteWorkOrderException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to find WorkOrder");
            }
        }
    }
}
