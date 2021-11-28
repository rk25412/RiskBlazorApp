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
    public partial class EditApplicence: ComponentBase
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

        [Parameter]
        public dynamic APPLICENCEID { get; set; }

        protected RadzenContent content1;

        protected RadzenTemplateForm<Applicence> form0;

        protected RadzenLabel licenceNameLabel;

        protected RadzenTextBox licenceName;

        protected RadzenRequiredValidator licenceNameRequiredValidator;

        protected RadzenLabel versionLabel;

        protected RadzenTextBox version;

        protected RadzenRequiredValidator versionRequiredValidator;

        protected RadzenLabel descriptionLabel;

        protected RadzenTextBox description;

        protected RadzenRequiredValidator descriptionRequiredValidator;

        protected RadzenLabel helpLabel;

        protected RadzenTextBox help;

        protected RadzenRequiredValidator helpRequiredValidator;

        protected RadzenLabel urlLabel;

        protected RadzenTextBox url;

        protected RadzenRequiredValidator urlRequiredValidator;

        protected RadzenLabel isDefaultLabel;

        protected dynamic isDefault;

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

        protected RadzenLabel priceLabel;

        protected dynamic price;

        protected RadzenLabel discountLabel;

        protected dynamic discount;

        protected RadzenLabel netpriceLabel;

        protected dynamic netprice;

        protected RadzenLabel currencyIdLabel;

        protected dynamic currencyId;

        protected RadzenButton button1;

        protected RadzenButton button2;

        Applicence _applicence;
        protected Applicence applicence
        {
            get
            {
                return _applicence;
            }
            set
            {
                if (!object.Equals(_applicence, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "applicence", NewValue = value, OldValue = _applicence };
                    _applicence = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Currency> _getCurrenciesResult;
        protected IEnumerable<Currency> getCurrenciesResult
        {
            get
            {
                return _getCurrenciesResult;
            }
            set
            {
                if (!object.Equals(_getCurrenciesResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getCurrenciesResult", NewValue = value, OldValue = _getCurrenciesResult };
                    _getCurrenciesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected bool IsLoading { get; set; }

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
            var clearRiskGetApplicenceByApplicenceidResult = await ClearRisk.GetApplicenceByApplicenceid(int.Parse($"{APPLICENCEID}"));
            applicence = clearRiskGetApplicenceByApplicenceidResult;

            var clearRiskGetCurrenciesResult = await ClearRisk.GetCurrencies(new Query());
            getCurrenciesResult = clearRiskGetCurrenciesResult;

            var result = await ClearRisk.GetCountries(new Query());
            getCountryResult = result;
        }

        IEnumerable<Country> _getCountryResult;
        protected IEnumerable<Country> getCountryResult
        {
            get
            {
                return _getCountryResult;
            }
            set
            {
                if (!object.Equals(_getCountryResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getCountryResult", NewValue = value, OldValue = _getCountryResult };
                    _getCountryResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected async System.Threading.Tasks.Task Form0Submit(Applicence args)
        {
            IsLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {
                var clearRiskUpdateApplicenceResult = await ClearRisk.UpdateApplicence(int.Parse($"{APPLICENCEID}"), applicence);
                IsLoading = false;
                StateHasChanged();
                DialogService.Close(applicence);
            }
            catch (System.Exception clearRiskUpdateApplicenceException)
            {
                IsLoading = false;
                StateHasChanged();
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to update Applicence");
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
