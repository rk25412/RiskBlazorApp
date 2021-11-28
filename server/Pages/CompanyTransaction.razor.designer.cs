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
    public partial class CompanyTransactionComponent : ComponentBase
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

        protected RadzenGrid<Clear.Risk.Models.ClearConnection.CompanyTransaction> grid0;

        protected RadzenGrid<Clear.Risk.Models.ClearConnection.CompanyTransactionDetail> grid1;

        IEnumerable<Clear.Risk.Models.ClearConnection.CompanyTransaction> _getCompanyTransactionsResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.CompanyTransaction> getCompanyTransactionsResult
        {
            get
            {
                return _getCompanyTransactionsResult;
            }
            set
            {
                if (!object.Equals(_getCompanyTransactionsResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getCompanyTransactionsResult", NewValue = value, OldValue = _getCompanyTransactionsResult };
                    _getCompanyTransactionsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        Clear.Risk.Models.ClearConnection.CompanyTransaction _master;
        protected Clear.Risk.Models.ClearConnection.CompanyTransaction master
        {
            get
            {
                return _master;
            }
            set
            {
                if (!object.Equals(_master, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "master", NewValue = value, OldValue = _master };
                    _master = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.CompanyTransactionDetail> _CompanyTransactionDetails;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.CompanyTransactionDetail> CompanyTransactionDetails
        {
            get
            {
                return _CompanyTransactionDetails;
            }
            set
            {
                if (!object.Equals(_CompanyTransactionDetails, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "CompanyTransactionDetails", NewValue = value, OldValue = _CompanyTransactionDetails };
                    _CompanyTransactionDetails = value;
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
            if (Security.IsInRole("System Administrator"))
            { 
                var clearConnectionGetCompanyTransactionsResult = await ClearConnection.GetCompanyTransactions(new Query());
                getCompanyTransactionsResult = clearConnectionGetCompanyTransactionsResult;
            }
            else
            {
                 var clearConnectionGetCompanyTransactionsResult = await ClearConnection.GetCompanyTransactionByCompany(Security.getUserId(),new Query());
                getCompanyTransactionsResult = clearConnectionGetCompanyTransactionsResult;
            }
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(Clear.Risk.Models.ClearConnection.CompanyTransaction args)
        {
            master = args;

            var clearConnectionGetCompanyTransactionDetailsResult = await ClearConnection.GetCompanyTransactionDetails(new Query() { Filter = $@"i => i.TransactionID == {args.TRANSACTIONID}" });
            CompanyTransactionDetails = clearConnectionGetCompanyTransactionDetailsResult;
        }
    }
}
