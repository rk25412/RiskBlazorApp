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
using Microsoft.AspNetCore.Identity;
using Clear.Risk.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Clear.Risk.Pages
{
    public partial class AddUserComponent : ComponentBase
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

        IEnumerable<Clear.Risk.Models.ClearConnection.PersonType> _getPersonTypesResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.PersonType> getPersonTypesResult
        {
            get
            {
                return _getPersonTypesResult;
            }
            set
            {
                if (!object.Equals(_getPersonTypesResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getPersonTypesResult", NewValue = value, OldValue = _getPersonTypesResult };
                    _getPersonTypesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.Applicence> _getApplicencesResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.Applicence> getApplicencesResult
        {
            get
            {
                return _getApplicencesResult;
            }
            set
            {
                if (!object.Equals(_getApplicencesResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getApplicencesResult", NewValue = value, OldValue = _getApplicencesResult };
                    _getApplicencesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        Clear.Risk.Models.ClearConnection.Person _person;
        protected Clear.Risk.Models.ClearConnection.Person person
        {
            get
            {
                return _person;
            }
            set
            {
                if (!object.Equals(_person, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "person", NewValue = value, OldValue = _person };
                    _person = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        IEnumerable<Clear.Risk.Models.ClearConnection.Desigation> _getDesigation;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.Desigation> getDesigationResult
        {
            get
            {
                return _getDesigation;
            }
            set
            {
                if (!object.Equals(_getDesigation, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getCountriesResult", NewValue = value, OldValue = _getDesigation };
                    _getDesigation = value;
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
            await Load();
        }
        protected async System.Threading.Tasks.Task Load()
        {
            var clearConnectionGetPeopleResult = await ClearConnection.GetPeople(new Query() { Expand = "State,Country,Person1,State1,Country1,PersonType,Applicence" });
            getPeopleResult = clearConnectionGetPeopleResult;

            var clearConnectionGetStatesResult = await ClearConnection.GetStates(new Query() { Expand = "Country" });
            getStatesResult = clearConnectionGetStatesResult;

            var clearConnectionGetCountriesResult = await ClearConnection.GetCountries(new Query());
            getCountriesResult = clearConnectionGetCountriesResult;

            var clearConnectionGetPersonTypesResult = await ClearConnection.GetPersonTypes(new Query());
            getPersonTypesResult = clearConnectionGetPersonTypesResult;

            var clearConnectionGetApplicencesResult = await ClearConnection.GetApplicences(new Query());
            getApplicencesResult = clearConnectionGetApplicencesResult;

            var clearConnectionGetDesigationsResult = await ClearConnection.GetDesigations();
            getDesigationResult = clearConnectionGetDesigationsResult;

            person = new Clear.Risk.Models.ClearConnection.Person(){
                CREATED_DATE = DateTime.Now,
                UPDATED_DATE = DateTime.Now,
                IS_DELETED = false,
                COMPANYTYPE = 1,
                CURRENT_BALANCE=0};
        }

       
        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.Person args)
        {
            try
            {
                
                var clearConnectionCreatePersonResult = await ClearConnection.CreatePerson(person);
                DialogService.Close(person);
 


            }
            catch (System.Exception clearConnectionCreatePersonException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new Person!");
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
