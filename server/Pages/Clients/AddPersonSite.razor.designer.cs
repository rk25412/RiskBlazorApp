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
using AspNetMonsters.Blazor.Geolocation;
using DocumentFormat.OpenXml.Office.CustomUI;
using System.Net;
using System.Net.Http;
using Geocoding;
using Geocoding.Google;

namespace Clear.Risk.Pages
{
    public partial class AddPersonSiteComponent : ComponentBase
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
        protected LocationService LocationService { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }
        [Parameter]
        public dynamic ClientId { get; set; }

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
        IList<RadzenGoogleMapMarker> markers;
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
        protected AspNetMonsters.Blazor.Geolocation.Location location;

        protected async System.Threading.Tasks.Task Load()
        {
            try
            {
                 
                var clearConnectionGetPeopleResult = await ClearConnection.GetPeople(new Query());
                getPeopleResult = clearConnectionGetPeopleResult;

                var admin = getPeopleResult.FirstOrDefault(x => x.PERSON_ID == Security.getUserId());

                var clearConnectionGetStatesResult = await ClearConnection.GetStates(new Query());
                getStatesResult = clearConnectionGetStatesResult;

                var clearConnectionGetCountriesResult = await ClearConnection.GetCountries(new Query());
                getCountriesResult = clearConnectionGetCountriesResult;

                personsite = new Clear.Risk.Models.ClearConnection.PersonSite()
                {
                    CREATED_DATE = DateTime.Now,
                    CREATOR_ID = Security.getUserId(),
                    UPDATED_DATE = DateTime.Now,
                    UPDATER_ID = Security.getUserId(),
                    IS_DELETED = false,
                    PERSON_ID = ClientId,
                    COUNTRY_ID = (int)admin.PERSONAL_COUNTRY_ID
                };

                location = await LocationService.GetLocationAsync();
            }
            catch(Exception ex)
            {

            }
            
        }

        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.PersonSite args)
        {
            isLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {
                await GetGeolocation();
                var clearConnectionCreatePersonSiteResult = await ClearConnection.CreatePersonSite(personsite);
                DialogService.Close(personsite);
            }
            catch (System.Exception clearConnectionCreatePersonSiteException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new PersonSite!");
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


        #region google Map
        protected int zoom = 3;

        protected bool showMadridMarker;

        [Inject]
        protected LocationService _location { get; set; }

        protected List<RadzenGoogleMapMarker> radzenGoogleMapMarkers;

        Dictionary<DateTime, string> events = new Dictionary<DateTime, string>();

        
        protected void MapClick(GoogleMapClickEventArgs args)
        {
            events.Add(DateTime.Now, $"Map clicked at Lat: {args.Position.Lat}, Lng: {args.Position.Lng}");
            StateHasChanged();
        }

        protected void MarkerClick(RadzenGoogleMapMarker marker)
        {
            events.Add(DateTime.Now, $"Map {marker.Title} marker clicked. Marker position -> Lat: {marker.Position.Lat}, Lng: {marker.Position.Lng}");
            StateHasChanged();
        }

        
        protected async Task GetGeolocation()
        {
            string address = personsite.SITE_ADDRESS1;
            if (!string.IsNullOrEmpty(personsite.SITE_ADDRESS2))
                address += "," + personsite.SITE_ADDRESS2;

            if (!string.IsNullOrEmpty(personsite.CITY))
                address += "," + personsite.CITY;

            if (!string.IsNullOrEmpty(personsite.POST_CODE))
                address += "," + personsite.POST_CODE;

            if(personsite.STATE_ID != null)
            {
                var state = getStatesResult.Where(s => s.ID == personsite.STATE_ID).FirstOrDefault();
                address += "," + state.STATENAME;
            }

            var country = getCountriesResult.Where(s => s.ID == personsite.COUNTRY_ID).FirstOrDefault();
            address += "," + country.COUNTRYNAME;
            


            try
            {
                //string requestUri = string.Format("https://maps.googleapis.com/maps/api/geocode/json?key={1}&address={0}&sensor=false", Uri.EscapeDataString(address), "AIzaSyAqp3wbBhtOg9f4hmxNbRZm_ZITPS8fQ8I");

                //using (var client = new HttpClient())
                //{
                //    var request = await client.GetAsync(requestUri);
                //    var response = await request.Content.ReadAsStringAsync();


                //}

                IGeocoder geocoder = new GoogleGeocoder() { ApiKey = "AIzaSyAqp3wbBhtOg9f4hmxNbRZm_ZITPS8fQ8I" };
                
                IEnumerable<Address> addresses = await geocoder.GeocodeAsync(address);

                personsite.LATITUDE = decimal.Parse(addresses.First().Coordinates.Latitude.ToString());
                personsite.LONGITUDE = decimal.Parse(addresses.First().Coordinates.Longitude.ToString());

                if (radzenGoogleMapMarkers == null)
                    radzenGoogleMapMarkers = new List<RadzenGoogleMapMarker>();

                if (radzenGoogleMapMarkers != null)
                {
                    radzenGoogleMapMarkers.Add(new RadzenGoogleMapMarker
                    {
                        Title = personsite.SITE_NAME,
                        Label = personsite.SITE_NAME,
                        Position = new GoogleMapPosition() { Lat = (double)personsite.LATITUDE, Lng = (double)personsite.LONGITUDE }
                    });
                }
            }
            catch(Exception ex)
            {

            }

            
        }

        protected async Task ChangeState(object value, string name)
        {
            isLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            await GetGeolocation(); 
            isLoading = false;
            StateHasChanged();
            
        }

        #endregion
    }
}
