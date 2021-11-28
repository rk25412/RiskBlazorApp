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
    public partial class ManageCountry: ComponentBase
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

        protected RadzenButton button0;

        protected RadzenGrid<Country> grid0;

        protected RadzenHeading heading1;

        protected RadzenButton stateAddButton;

        protected RadzenGrid<State> grid1;

        IEnumerable<Country> _getCountriesResult;
        protected IEnumerable<Country> getCountriesResult
        {
            get
            {
                return _getCountriesResult;
            }
            set
            {
                if (!object.Equals(_getCountriesResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getCountriesResult", NewValue = value, OldValue = _getCountriesResult };
                    _getCountriesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        Country _master;
        protected Country master
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

        IEnumerable<State> _States;
        protected IEnumerable<State> States
        {
            get
            {
                return _States;
            }
            set
            {
                if (!object.Equals(_States, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "States", NewValue = value, OldValue = _States };
                    _States = value;
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
            var clearRiskGetCountriesResult = await ClearRisk.GetCountries();
            getCountriesResult = clearRiskGetCountriesResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddCountry>("Add Country", null);
              grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Grid0RowDoubleClick(dynamic args)
        {
            DialogService.Open<EditCountry>("Edit Country", new Dictionary<string, object>() { { "ID", args.ID } });
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(Country args)
        {
            master = args;

            var clearRiskGetStatesResult = await ClearRisk.GetStates(new Query() { Filter = $@"i => i.COUNTRYID == {args.ID}" });
            States = clearRiskGetStatesResult;
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var clearRiskDeleteCountryResult = await ClearRisk.DeleteCountry(data.ID);
                    if (clearRiskDeleteCountryResult != null)
                    {
                          grid0.Reload();
                    }
                }
            }
            catch (System.Exception clearRiskDeleteCountryException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete Country");
            }
        }

        protected async System.Threading.Tasks.Task StateAddButtonClick(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddState>("Add State", new Dictionary<string, object>() { { "COUNTRYID", master.ID } });
              grid1.Reload();
        }

        protected async System.Threading.Tasks.Task Grid1RowSelect(State args)
        {
            var dialogResult = await DialogService.OpenAsync<EditState>("Edit State", new Dictionary<string, object>() { { "ID", args.ID } });
              grid1.Reload();
        }

        protected async System.Threading.Tasks.Task StateDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                var clearRiskDeleteStateResult = await ClearRisk.DeleteState(data.ID);
                if (clearRiskDeleteStateResult != null)
                {
                      grid1.Reload();
                }
            }
            catch (System.Exception clearRiskDeleteStateException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete Country");
            }
        }
    }
}
