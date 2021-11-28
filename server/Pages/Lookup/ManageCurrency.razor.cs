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
    public partial class ManageCurrency : ComponentBase
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

        //[Inject]
        //protected TooltipService TooltipService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }


        [Inject]
        protected ClearConnectionService ClearRisk { get; set; }

        protected RadzenContent content1;

        protected RadzenHeading pageTitle;

        protected RadzenCard card1;

        protected RadzenButton button0;

        //protected RadzenGrid<Currency> grid0;

        protected RadzenCard card2;

        protected RadzenTemplateForm<Currency> form0;

        protected RadzenLabel isoCodeLabel;

        protected RadzenTextBox isoCode;

        protected RadzenRequiredValidator isoCodeRequiredValidator;

        protected RadzenLabel cursymbolLabel;

        protected RadzenTextBox cursymbol;

        protected RadzenRequiredValidator cursymbolRequiredValidator;

        protected RadzenButton button2;

        protected RadzenButton button3;

        protected IList<Currency> getCurrenciesResult = new List<Currency>();

        protected bool IsLoading { get; set; }
        Currency _currency;
        protected Currency currency
        {
            get
            {
                return _currency;
            }
            set
            {
                if (!object.Equals(_currency, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "currency", NewValue = value, OldValue = _currency };
                    _currency = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        bool _isEdit;
        protected bool isEdit
        {
            get
            {
                return _isEdit;
            }
            set
            {
                if (!object.Equals(_isEdit, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "isEdit", NewValue = value, OldValue = _isEdit };
                    _isEdit = value;
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
            var clearRiskGetCurrenciesResult = await ClearRisk.GetCurrencies(new Query());

            getCurrenciesResult = (from x in clearRiskGetCurrenciesResult
                                   select new Currency
                                   {
                                       CURRENCY_ID = x.CURRENCY_ID,
                                       ISO_CODE = x.ISO_CODE,
                                       CURSYMBOL = x.CURSYMBOL,
                                   }).ToList();


            //getCurrenciesResult = clearRiskGetCurrenciesResult;

            currency = clearRiskGetCurrenciesResult.FirstOrDefault();

            isEdit = true;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            currency = new Currency();

            isEdit = false;
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(Currency args)
        {
            isEdit = true;

            currency = args;
        }
        protected async System.Threading.Tasks.Task HelpClick(MouseEventArgs args)
        {
            string url = "/Help/14";
            await JSRuntime.InvokeAsync<object>("open", url, "_blank");

            //UriHelper.NavigateTo("Help" + "/" + 14);
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
                    var clearRiskDeleteCurrencyResult = await ClearRisk.DeleteCurrency(int.Parse($"{data.CURRENCY_ID}"));
                    if (clearRiskDeleteCurrencyResult != null)
                    {
                        var clearRiskGetCurrenciesResult = await ClearRisk.GetCurrencies(new Query());

                        getCurrenciesResult = (from x in clearRiskGetCurrenciesResult
                                               select new Currency
                                               {
                                                   CURRENCY_ID = x.CURRENCY_ID,
                                                   ISO_CODE = x.ISO_CODE,
                                                   CURSYMBOL = x.CURSYMBOL,
                                               }).ToList();

                        NotificationService.Notify(NotificationSeverity.Success, $"Success", $"Currency deleted successfully.", 180000);
                    }
                }
            }
            catch (System.Exception clearRiskDeleteCurrencyException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete Currency", 180000);
            }
            finally
            {
                IsLoading = false;
                StateHasChanged();
            }
        }
        protected async System.Threading.Tasks.Task GridEditButtonClick(MouseEventArgs args, dynamic data)
        {
            //var dialogResult = await DialogService.OpenAsync<EditApplicence>("Edit Applicence", new Dictionary<string, object>() { { "APPLICENCEID", data.APPLICENCEID } }, new DialogOptions() { Width = $"{600}px" });
            //await InvokeAsync(() => { StateHasChanged(); });
            isEdit = true;

            currency = data;
        }

        protected async System.Threading.Tasks.Task Form0Submit(Currency args)
        {
            IsLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {
                if (isEdit)
                {
                    var clearRiskUpdateCurrencyResult = await ClearRisk.UpdateCurrency(int.Parse($"{currency.CURRENCY_ID}"), currency);
                    NotificationService.Notify(NotificationSeverity.Success, $"Success", $"Currency updated!", 180000);
                }
            }
            catch (System.Exception clearRiskUpdateCurrencyException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to update Currency");
            }
            finally
            {
                IsLoading = false;
                StateHasChanged();
            }

            try
            {
                if (!this.isEdit)
                {
                    var clearRiskCreateCurrencyResult = await ClearRisk.CreateCurrency(args);

                    //getCurrenciesResult.Add(new Currency
                    //{
                    //    CURRENCY_ID = clearRiskCreateCurrencyResult.CURRENCY_ID,
                    //    ISO_CODE = clearRiskCreateCurrencyResult.ISO_CODE,
                    //    CURSYMBOL = clearRiskCreateCurrencyResult.CURSYMBOL,
                    //});

                    getCurrenciesResult.Add(args);

                    currency = new Currency();

                    NotificationService.Notify(NotificationSeverity.Success, $"Success", $"Currency created!", 180000);
                }
            }
            catch (System.Exception clearRiskCreateCurrencyException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new Currency!");
            }
            finally
            {
                IsLoading = false;
                StateHasChanged();
            }
        }
    }
}
