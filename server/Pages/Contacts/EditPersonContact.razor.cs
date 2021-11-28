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

namespace Clear.Risk.Pages.Contacts
{
    public partial class EditPersonContact: ComponentBase
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

        [Parameter]
        public dynamic PERSON_CONTACT_ID { get; set; }

        PersonContact _personcontact;
        protected PersonContact personcontact
        {
            get
            {
                return _personcontact;
            }
            set
            {
                if (!object.Equals(_personcontact, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "personcontact", NewValue = value, OldValue = _personcontact };
                    _personcontact = value;
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
                    var args = new PropertyChangedEventArgs() { Name = "getDesigationResult", NewValue = value, OldValue = _getDesigation };
                    _getDesigation = value;
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
                    var args = new PropertyChangedEventArgs() { Name = "getPeopleResult", NewValue = value, OldValue = _getPeopleResult };
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
                    var args = new PropertyChangedEventArgs() { Name = "getStatesResult", NewValue = value, OldValue = _getStatesResult };
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
                    var args = new PropertyChangedEventArgs() { Name = "getCountriesResult", NewValue = value, OldValue = _getCountriesResult };
                    _getCountriesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<StatusMaster> _getStatusMastersResult;
        protected IEnumerable<StatusMaster> getStatusMastersResult
        {
            get
            {
                return _getStatusMastersResult;
            }
            set
            {
                if (!object.Equals(_getStatusMastersResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getStatusMastersResult", NewValue = value, OldValue = _getStatusMastersResult };
                    _getStatusMastersResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        protected bool isLoading = true;
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
           

            //var clearRiskGetPeopleResult = await ClearRisk.GetPeople();
            //getPeopleResult = clearRiskGetPeopleResult;

            var clearRiskGetCountriesResult = await ClearRisk.GetCountries();
            getCountriesResult = clearRiskGetCountriesResult;

            var clearRiskGetStatesResult = await ClearRisk.GetStates(new Query());
            getStatesResult = clearRiskGetStatesResult;

            var clearConnectionGetDesigationsResult = await ClearRisk.GetDesigations();
            getDesigationResult = clearConnectionGetDesigationsResult;

            var clearRiskGetStatusMastersResult = await ClearRisk.GetStatusMasters();
            getStatusMastersResult = clearRiskGetStatusMastersResult;

            getGenders = new List<Gender>();
            getGenders.Add(new Gender { ID = 1, Name = "Male" });
            getGenders.Add(new Gender { ID = 2, Name = "Female" });

            var clearRiskGetPersonContactByPersonContactIdResult = await ClearRisk.GetPersonContactByPersonContactId(int.Parse($"{PERSON_CONTACT_ID}"));
            personcontact = clearRiskGetPersonContactByPersonContactIdResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit(PersonContact args)
        {
            isLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {
                personcontact.UPDATED_DATE = DateTime.Now;
                personcontact.UPDATER_ID = Security.getUserId();
                var clearRiskUpdatePersonContactResult = await ClearRisk.UpdatePersonContact(int.Parse($"{PERSON_CONTACT_ID}"), personcontact);
                DialogService.Close(personcontact);
            }
            catch (System.Exception clearRiskUpdatePersonContactException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to update PersonContact");
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
                
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }

        IList<Gender> _getGenders;

        protected IList<Gender> getGenders
        {
            get
            {
                return _getGenders;
            }
            set
            {
                if (!object.Equals(_getGenders, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getGenders", NewValue = value, OldValue = _getGenders };
                    _getGenders = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
    }
}
