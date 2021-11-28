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
using Stripe;
using System.Security.Claims;
using Clear.Risk.ViewModels;

namespace Clear.Risk.Pages.Company
{
    public partial class AddCredit : ComponentBase
    {
        [Parameter]
        public dynamic PersonId { get; set; }


        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        [Inject]
        protected DialogService DialogService { get; set; }
        [Inject]
        protected SecurityService Security { get; set; }
        [Inject]
        protected ClearConnectionService ClearRisk { get; set; }
        [Inject]
        protected NavigationManager UriHelper { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        protected bool isLoading { get; set; }
        
        CompanyAccountTransaction _CompanyAccountTransaction;
        protected CompanyAccountTransaction CompanyAccountTransaction
        {
            get
            {
                return _CompanyAccountTransaction;
            }
            set
            {
                if (!object.Equals(_CompanyAccountTransaction, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "CompanyAccountTransaction", NewValue = value, OldValue = _CompanyAccountTransaction };
                    _CompanyAccountTransaction = value;
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
           var company = await ClearRisk.GetPersonByPersonId(PersonId);
           
            CompanyAccountTransaction = new CompanyAccountTransaction()
            {
                COMPANY_ID = company.PERSON_ID,
                TRANSACTION_DATE = DateTime.Now,
                PAYMENT_AMOUNT = 0,               
                DESCRIPTION = "Company Credit By Super Admin",
                TRXTYPE = "DTD",
                CREATOR_ID= Security.getUserId(),
                CREATED_DATE = DateTime.Now,
                UPDATED_DATE = DateTime.Now,
                UPDATER_ID= Security.getUserId(),
                CURRENCY_ID = company.CURRENCY_ID != null ? (int)company.CURRENCY_ID : 0
            };
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }

        protected async System.Threading.Tasks.Task Form0Submit(CompanyAccountTransaction companyAccountTransaction)
        {
            isLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {
                var securityCreateAccountTransactionResult = await ClearRisk.CreateCompanyAccountTransaction(companyAccountTransaction);
                NotificationService.Notify(NotificationSeverity.Success, $"Success", $"Amount Add successfully !!!, ",180000);
                DialogService.Close(companyAccountTransaction);
                isLoading = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", ex.Message,180000);
                isLoading = false;
                StateHasChanged();
            }
            
        }
    }
}
