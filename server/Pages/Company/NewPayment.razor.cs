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
    public partial class NewPayment: ComponentBase
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

        protected RadzenTemplateForm<CompanyTransaction> form0;

        protected RadzenLabel amountpaidLabel;

        protected dynamic amountpaid;

        protected RadzenRequiredValidator amountpaidRequiredValidator;

        protected RadzenLabel personIdLabel;

        protected dynamic personId;

        protected RadzenRequiredValidator personIdRequiredValidator;

        protected RadzenLabel paymentrefnoLabel;

        protected RadzenTextBox paymentrefno;

        protected RadzenRequiredValidator paymentrefnoRequiredValidator;

        protected RadzenLabel remarksLabel;

        protected RadzenTextBox remarks;

        protected RadzenRequiredValidator remarksRequiredValidator;

        protected RadzenLabel taxamountLabel;

        protected dynamic taxamount;

        protected RadzenRequiredValidator taxamountRequiredValidator;

        protected RadzenLabel totalamountLabel;

        protected dynamic totalamount;

        protected RadzenRequiredValidator totalamountRequiredValidator;

        protected RadzenLabel transactiondateLabel;

        protected dynamic transactiondate;

        protected RadzenRequiredValidator transactiondateRequiredValidator;

        protected RadzenLabel transactionrefnoLabel;

        protected RadzenTextBox transactionrefno;

        protected RadzenRequiredValidator transactionrefnoRequiredValidator;

        protected RadzenLabel transactionStatusIdLabel;

        protected dynamic transactionStatusId;

        protected RadzenRequiredValidator transactionStatusIdRequiredValidator;

        protected RadzenLabel transactiontypeLabel;

        protected dynamic transactiontype;

        protected RadzenRequiredValidator transactiontypeRequiredValidator;

        protected RadzenLabel paymentdateLabel;

        protected dynamic paymentdate;

        protected RadzenLabel createdDateLabel;

        protected dynamic createdDate;

        protected RadzenLabel creatorIdLabel;

        protected dynamic creatorId;

        protected RadzenLabel updatedDateLabel;

        protected dynamic updatedDate;

        protected RadzenLabel updaterIdLabel;

        protected dynamic updaterId;

        protected RadzenLabel deletedDateLabel;

        protected dynamic deletedDate;

        protected RadzenLabel deleterIdLabel;

        protected dynamic deleterId;

        protected RadzenLabel isDeletedLabel;

        protected dynamic isDeleted;

        protected RadzenLabel currencyIdLabel;

        protected dynamic currencyId;

        protected RadzenRequiredValidator currencyIdRequiredValidator;

        protected RadzenButton button1;

        protected RadzenButton button2;



        protected int[] Years = { 2020, 2021, 2022, 2023, 2024, 2025, 2026, 2027, 2028 };
        protected int[] Months = { 01, 02, 03, 04, 05, 06, 07, 08, 09, 10, 11, 12 };

        CompanyTransaction _companytransaction;
        protected CompanyTransaction companytransaction
        {
            get
            {
                return _companytransaction;
            }
            set
            {
                if (!object.Equals(_companytransaction, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "companytransaction", NewValue = value, OldValue = _companytransaction };
                    _companytransaction = value;
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

        protected Applicence Licence { get; set; }

        protected Models.ClearConnection.Person company { get; set; }
        protected async System.Threading.Tasks.Task Load()
        {
              company = await ClearRisk.GetPersonByPersonId(Security.getCompanyId());

            Licence = company.Applicence;

            companytransaction = new CompanyTransaction() { 
            
                PERSON_ID = company.PERSON_ID,
                TRANSACTIONDATE = DateTime.Now,
                TRANSACTION_STATUS_ID = 1,
                PAYMENTDATE = DateTime.Now,
                CREATED_DATE = DateTime.Now,
                CREATOR_ID = Security.getUserId(),
                UPDATED_DATE = DateTime.Now,
                UPDATER_ID = Security.getUserId(),
                IS_DELETED = false,
                CURRENCY_ID = (int)company.CURRENCY_ID
            };
        }

        protected async System.Threading.Tasks.Task Form0Submit(CompanyTransaction args)
        {
            isLoading = true;
            StateHasChanged();
            await Task.Delay(1);


            if (companytransaction.AMOUNTPAID <= 0)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Amount cannot be zero",180000);
                isLoading = false;
                StateHasChanged();
                return;
            }else if (string.IsNullOrEmpty(companytransaction.CardNumder))
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Card number cannot be empty",180000);
                isLoading = false ;
                StateHasChanged();
                return;
            }else if (companytransaction.Month <= 0)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Please select Month",180000);
                isLoading = false;
                StateHasChanged();
            }
            else if (companytransaction.Year <= 0)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Please select Year",180000);
                isLoading = false;
                StateHasChanged();
            }
            else if (string.IsNullOrEmpty(companytransaction.CVC))
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Please Enter CVC",180000);
                isLoading = false;
                StateHasChanged();
            }


            try
            {
                System.Random rand = new System.Random((int)System.DateTime.Now.Ticks);
                int random = rand.Next(1, 100000000);

                args.PAYMENTREFNO = "INV-" + random.ToString();
                args.TAXAMOUNT = 0;
                args.TOTALAMOUNT = args.AMOUNTPAID;

                if (args.CompanyTransactionDetails == null)
                    args.CompanyTransactionDetails = new List<CompanyTransactionDetail>();

                args.CompanyTransactionDetails.Add(new CompanyTransactionDetail
                {
                    PRODUCT_ID = Licence.APPLICENCEID,
                    PRICE = args.AMOUNTPAID,
                    TAX_AMT =0M,
                    PRICE_TOTAL = args.AMOUNTPAID,
                    CREATED_DATE = DateTime.Now,
                    CREATOR_ID = Security.getUserId(),
                    UPDATED_DATE = DateTime.Now,
                    UPDATER_ID = Security.getUserId(),
                    IS_DELETED = false
                });

                var transaction = await ClearRisk.CreateCompanyTransaction(companytransaction);

                if(transaction.TRANSACTIONID > 0)
                {
                    PayModel payModel = new PayModel();

                    if (company != null)
                    {
                        payModel.CustomerName = company.COMPANY_NAME;
                        payModel.Email = company.PERSONAL_EMAIL;
                        payModel.Address = company.PERSONALADDRESS1 + ',' + company.PERSONAL_CITY + ',' + company.PERSONAL_POSTCODE + ',' + company.State.STATENAME;
                        payModel.CardNumder = args.CardNumder;
                        payModel.Month = args.Month;
                        payModel.Year = args.Year;
                        payModel.CVC = args.CVC;
                        payModel.Amount = args.AMOUNTPAID * 100;
                        payModel.Currency = Licence.Currency.ISO_CODE;
                    }

                    Charge charge = await ProcessPayment.PayAsync(payModel);

                    if(charge != null && charge.Paid)
                    {
                        //Create Company Account Transaction
                        transaction.TRANSACTION_STATUS_ID = 2;
                        transaction.TRANSACTIONREFNO = charge.InvoiceId;
                        transaction.PAYMENTDATE = DateTime.Now;

                        if (transaction.AccountTransactions == null)
                            transaction.AccountTransactions = new List<CompanyAccountTransaction>();

                        transaction.AccountTransactions.Add(new CompanyAccountTransaction
                        {
                            TRANSACTION_DATE = DateTime.Now,
                            PAYMENT_AMOUNT = 0M,
                            DEPOSITE_AMOUNT = transaction.AMOUNTPAID,
                            DESCRIPTION = "Payment Received",
                            COMPANY_ID = transaction.PERSON_ID,
                            TRXTYPE = "DEP",
                            CREATED_DATE = DateTime.Now,
                            CREATOR_ID = Security.getUserId(),
                            UPDATED_DATE = DateTime.Now,
                            UPDATER_ID = Security.getUserId(),
                            CURRENCY_ID = transaction.CURRENCY_ID
                        });

                        await ClearRisk.UpdateCompanyTransaction(transaction.TRANSACTIONID,companytransaction);
                        NotificationService.Notify(NotificationSeverity.Success, $"Payment Process", $"Your Payment process was successful, and the invoice has been sent on your email!",180000);
                        DialogService.Close(null);
                    }                     
                    else
                    {
                        transaction.TRANSACTION_STATUS_ID = 3;
                        await ClearRisk.UpdateCompanyTransaction(transaction.TRANSACTIONID, companytransaction);
                        NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to Process Payment!",180000);
                    }
                }

            }
            catch (System.Exception clearRiskCreateCompanyTransactionException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new CompanyTransaction!",180000);
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
                
            }
        }
        

        protected void ChangeMonthYear(object value, string name)
        {
            if (name == "Year")
                companytransaction.Year = (int)value;
            else if(name == "Month")
                companytransaction.Month = (int)value;
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }

        

    }
}
