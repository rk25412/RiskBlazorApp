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

namespace Clear.Risk.Pages
{
    public partial class EditPersonSiteComponent : ComponentBase
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

        [Parameter]
        public dynamic PERSON_SITE_ID { get; set; }
        Dictionary<DateTime, string> events = new Dictionary<DateTime, string>();
        Clear.Risk.Models.ClearConnection.PersonSite _personsite;
        protected Clear.Risk.Models.ClearConnection.PersonSite personsite
        {
            get
            {
                return _personsite;
            }
            set
            {
                if (!object.Equals(_personsite, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "personsite", NewValue = value, OldValue = _personsite };
                    _personsite = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        protected int zoom = 3;

        protected void MarkerClick(RadzenGoogleMapMarker marker)
        {
            events.Add(DateTime.Now, $"Map {marker.Title} marker clicked. Marker position -> Lat: {marker.Position.Lat}, Lng: {marker.Position.Lng}");
            StateHasChanged();
        }
        protected void MapClick(GoogleMapClickEventArgs args)
        {
            events.Add(DateTime.Now, $"Map clicked at Lat: {args.Position.Lat}, Lng: {args.Position.Lng}");
            StateHasChanged();
        }
        protected bool showMadridMarker;
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
           

            var clearConnectionGetPeopleResult = await ClearConnection.GetPeople(new Query());
            getPeopleResult = clearConnectionGetPeopleResult;

            var clearConnectionGetStatesResult = await ClearConnection.GetStates(new Query());
            getStatesResult = clearConnectionGetStatesResult;

            var clearConnectionGetCountriesResult = await ClearConnection.GetCountries(new Query());
            getCountriesResult = clearConnectionGetCountriesResult;

            var clearConnectionGetPersonSiteByPersonSiteIdResult = await ClearConnection.GetPersonSiteByPersonSiteId(int.Parse($"{PERSON_SITE_ID}"));
            personsite = clearConnectionGetPersonSiteByPersonSiteIdResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.PersonSite args)
        {
            isLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {
                personsite.UPDATED_DATE = DateTime.Now;
                personsite.UPDATER_ID = Security.getUserId();
                var clearConnectionUpdatePersonSiteResult = await ClearConnection.UpdatePersonSite(int.Parse($"{PERSON_SITE_ID}"), personsite);
                DialogService.Close(personsite);
            }
            catch (System.Exception clearConnectionUpdatePersonSiteException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to update PersonSite");
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
    }
}
