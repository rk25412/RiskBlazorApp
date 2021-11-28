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
using System.Security.Cryptography;
using System.Text;
using System.Globalization;

namespace Clear.Risk.Pages.Company
{
    public partial class InvoiceHistory: ComponentBase
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

        protected RadzenContent content1;

        protected RadzenHeading pageTitle;

        protected RadzenButton button0;

        protected RadzenGrid<CompanyTransaction> grid0;

        IEnumerable<CompanyTransaction> _getCompanyTransactionsResult;
        protected IEnumerable<CompanyTransaction> getCompanyTransactionsResult
        {
            get
            {
                return _getCompanyTransactionsResult;
            }
            set
            {
                if (!object.Equals(_getCompanyTransactionsResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getCompanyTransactionsResult", NewValue = value, OldValue = _getCompanyTransactionsResult };
                    _getCompanyTransactionsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        dynamic _master;
        protected dynamic master
        {
            get
            {
                return _master;
            }
            set
            {
                if (!object.Equals(_master, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "master", NewValue = value, OldValue = _master };
                    _master = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
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
                var clearRiskGetCompanyTransactionsResult = await ClearRisk.GetCompanyTransactions();
                getCompanyTransactionsResult = clearRiskGetCompanyTransactionsResult;
            }
            else
            {
                var clearRiskGetCompanyTransactionsResult = await ClearRisk.GetCompanyTransactions(new Query() { Filter = $@"i => i.PERSON_ID == {Security.getCompanyId()}  " });
                getCompanyTransactionsResult = clearRiskGetCompanyTransactionsResult;
            }
               
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<NewPayment>("New Payment", null);
             grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }
        protected async System.Threading.Tasks.Task HelpClick(MouseEventArgs args)
        {
            string url = "/Help/11";
            await JSRuntime.InvokeAsync<object>("open", url, "_blank");

            //UriHelper.NavigateTo("Help" + "/" + 11);
        }


        protected async System.Threading.Tasks.Task Grid0RowExpand(CompanyTransaction args)
        {
            master = args;

            var clearRiskGetCompanyTransactionDetailsResult = await ClearRisk.GetCompanyTransactionDetails(new Query() { Filter = $@"i => i.TransactionID == {args.TRANSACTIONID}" });
            if (clearRiskGetCompanyTransactionDetailsResult != null)
            {
                args.CompanyTransactionDetails = clearRiskGetCompanyTransactionDetailsResult.ToList();
            }
        }

         
        
        
        
    }
}
