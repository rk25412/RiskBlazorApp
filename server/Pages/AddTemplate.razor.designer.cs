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
    public partial class AddTemplateComponent : ComponentBase
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

        IEnumerable<Clear.Risk.Models.ClearConnection.StatusMaster> _getStatusMastersResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.StatusMaster> getStatusMastersResult
        {
            get
            {
                return _getStatusMastersResult;
            }
            set
            {
                if (!object.Equals(_getStatusMastersResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getStatusMastersResult", NewValue = value, OldValue = _getStatusMastersResult };
                    _getStatusMastersResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.TradeCategory> _getTradeCategoriesResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.TradeCategory> getTradeCategoriesResult
        {
            get
            {
                return _getTradeCategoriesResult;
            }
            set
            {
                if (!object.Equals(_getTradeCategoriesResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getTradeCategoriesResult", NewValue = value, OldValue = _getTradeCategoriesResult };
                    _getTradeCategoriesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.Person> _getPeopleResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.Person> getPeopleResult
        {
            get
            {
                return _getPeopleResult;
            }
            set
            {
                if (!object.Equals(_getPeopleResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getPeopleResult", NewValue = value, OldValue = _getPeopleResult };
                    _getPeopleResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.SwmsTemplateCategory> _getSwmsTemplateCategoriesResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.SwmsTemplateCategory> getSwmsTemplateCategoriesResult
        {
            get
            {
                return _getSwmsTemplateCategoriesResult;
            }
            set
            {
                if (!object.Equals(_getSwmsTemplateCategoriesResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getSwmsTemplateCategoriesResult", NewValue = value, OldValue = _getSwmsTemplateCategoriesResult };
                    _getSwmsTemplateCategoriesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.Country> _getCountriesResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.Country> getCountriesResult
        {
            get
            {
                return _getCountriesResult;
            }
            set
            {
                if (!object.Equals(_getCountriesResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getCountriesResult", NewValue = value, OldValue = _getCountriesResult };
                    _getCountriesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.EscalationLevel> _getEscalationLevelsResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.EscalationLevel> getEscalationLevelsResult
        {
            get
            {
                return _getEscalationLevelsResult;
            }
            set
            {
                if (!object.Equals(_getEscalationLevelsResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getEscalationLevelsResult", NewValue = value, OldValue = _getEscalationLevelsResult };
                    _getEscalationLevelsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.WarningLevel> _getWarningLevelsResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.WarningLevel> getWarningLevelsResult
        {
            get
            {
                return _getWarningLevelsResult;
            }
            set
            {
                if (!object.Equals(_getWarningLevelsResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getWarningLevelsResult", NewValue = value, OldValue = _getWarningLevelsResult };
                    _getWarningLevelsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.StatusLevel> _getStatusLevelsResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.StatusLevel> getStatusLevelsResult
        {
            get
            {
                return _getStatusLevelsResult;
            }
            set
            {
                if (!object.Equals(_getStatusLevelsResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getStatusLevelsResult", NewValue = value, OldValue = _getStatusLevelsResult };
                    _getStatusLevelsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.State> _getStatesResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.State> getStatesResult
        {
            get
            {
                return _getStatesResult;
            }
            set
            {
                if (!object.Equals(_getStatesResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getStatesResult", NewValue = value, OldValue = _getStatesResult };
                    _getStatesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        Clear.Risk.Models.ClearConnection.Template _template;
        protected Clear.Risk.Models.ClearConnection.Template template
        {
            get
            {
                return _template;
            }
            set
            {
                if (!object.Equals(_template, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "template", NewValue = value, OldValue = _template };
                    _template = value;
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
            var clearConnectionGetStatusMastersResult = await ClearConnection.GetStatusMasters();
            getStatusMastersResult = clearConnectionGetStatusMastersResult;

            var clearConnectionGetTradeCategoriesResult = await ClearConnection.GetTradeCategories();
            getTradeCategoriesResult = clearConnectionGetTradeCategoriesResult;

            var clearConnectionGetPeopleResult = await ClearConnection.GetPeople();
            getPeopleResult = clearConnectionGetPeopleResult;

            var clearConnectionGetSwmsTemplateCategoriesResult = await ClearConnection.GetSwmsTemplateCategories();
            getSwmsTemplateCategoriesResult = clearConnectionGetSwmsTemplateCategoriesResult;

            var clearConnectionGetCountriesResult = await ClearConnection.GetCountries();
            getCountriesResult = clearConnectionGetCountriesResult;

            var clearConnectionGetEscalationLevelsResult = await ClearConnection.GetEscalationLevels();
            getEscalationLevelsResult = clearConnectionGetEscalationLevelsResult;

            var clearConnectionGetWarningLevelsResult = await ClearConnection.GetWarningLevels();
            getWarningLevelsResult = clearConnectionGetWarningLevelsResult;

            var clearConnectionGetStatusLevelsResult = await ClearConnection.GetStatusLevels();
            getStatusLevelsResult = clearConnectionGetStatusLevelsResult;

            var clearConnectionGetStatesResult = await ClearConnection.GetStates();
            getStatesResult = clearConnectionGetStatesResult;

            template = new Clear.Risk.Models.ClearConnection.Template(){};
        }

        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.Template args)
        {
            try
            {
                var clearConnectionCreateTemplateResult = await ClearConnection.CreateTemplate(template);
                DialogService.Close(template);
            }
            catch (System.Exception clearConnectionCreateTemplateException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new Template!");
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
