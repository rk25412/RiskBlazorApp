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
    public partial class TransactionHistory: ComponentBase
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

        //protected RadzenGrid<CompanyAccountTransaction> grid0;

        IEnumerable<CompanyAccountTransaction> _getCompanyAccountTransactionsResult;
        protected IEnumerable<CompanyAccountTransaction> getCompanyAccountTransactionsResult
        {
            get
            {
                return _getCompanyAccountTransactionsResult;
            }
            set
            {
                if (!object.Equals(_getCompanyAccountTransactionsResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getCompanyAccountTransactionsResult", NewValue = value, OldValue = _getCompanyAccountTransactionsResult };
                    _getCompanyAccountTransactionsResult = value;
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
            var clearRiskGetCompanyAccountTransactionsResult = await ClearRisk.GetCompanyAccountTransactions(Security.IsInRole("System Administrator") ? new Query() : new Query() { Filter = $@"i => i.COMPANY_ID == {Security.getCompanyId()}" });
            decimal intialBalance = 0;

            foreach (var item in clearRiskGetCompanyAccountTransactionsResult)
            {
                intialBalance = intialBalance + (item.DEPOSITE_AMOUNT - item.PAYMENT_AMOUNT);
                item.Balance = intialBalance;
            }

            getCompanyAccountTransactionsResult = (from x in clearRiskGetCompanyAccountTransactionsResult
                                                   select new CompanyAccountTransaction
                                                   {
                                                       COMPANY_TRANSACTION_ID = x.COMPANY_TRANSACTION_ID,
                                                       Person = x.Person,
                                                       TRANSACTION_DATE = x.TRANSACTION_DATE,
                                                       PAYMENT_AMOUNT = x.PAYMENT_AMOUNT,
                                                       DEPOSITE_AMOUNT = x.DEPOSITE_AMOUNT,
                                                       Balance = x.Balance,
                                                       DESCRIPTION = x.DESCRIPTION,
                                                       TRXTYPE = x.TRXTYPE,
                                                       Currency = x.Currency,
                                                   }).ToList();


            //getCompanyAccountTransactionsResult = clearRiskGetCompanyAccountTransactionsResult;

        }

        protected async System.Threading.Tasks.Task HelpClick(MouseEventArgs args)
        {
            string url = "/Help/10";
            await JSRuntime.InvokeAsync<object>("open", url, "_blank");

            //UriHelper.NavigateTo("Help" + "/" + 10);
        }

    }
}
