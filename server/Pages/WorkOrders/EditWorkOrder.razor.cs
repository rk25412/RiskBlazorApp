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
using System.Net;
using System.Net.Http;
using Geocoding;
using Geocoding.Google;
using Syncfusion.Blazor.Gantt;
//using Blazor.GoogleMap;
//using Blazor.GoogleMap.Components;
//using Blazor.GoogleMap.Map.Markers;
//using Blazor.GoogleMap.Map.InfoWindows;
//using Blazor.GoogleMap.Map;


namespace Clear.Risk.Pages.WorkOrders
{
    public partial class EditWorkOrder: ComponentBase
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

        //[Inject]
        //protected InfoWindow InfoWindow { get; set; }

        //[Inject]
        //protected MarkerCollectionFactory MarkerCollectionFactory { get; set; }

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
        protected WorkOrderService ClearRisk { get; set; }

        [Inject]
        protected ClearConnectionService ClearConnection { get; set; }
        [Parameter]
        public dynamic WorkOrderId { get; set; }

        IEnumerable<OrderStatus> _getOrderStatusesResult;
        protected IEnumerable<OrderStatus> getOrderStatusesResult
        {
            get
            {
                return _getOrderStatusesResult;
            }
            set
            {
                if (!object.Equals(_getOrderStatusesResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getOrderStatusesResult", NewValue = value, OldValue = _getOrderStatusesResult };
                    _getOrderStatusesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        [Parameter]
        public dynamic EmpId { get; set; }
        [Parameter]
        public dynamic tabNo { get; set; }

        IEnumerable<PriorityMaster> _getPriorityMastersResult;
        protected IEnumerable<PriorityMaster> getPriorityMastersResult
        {
            get
            {
                return _getPriorityMastersResult;
            }
            set
            {
                if (!object.Equals(_getPriorityMastersResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getPriorityMastersResult", NewValue = value, OldValue = _getPriorityMastersResult };
                    _getPriorityMastersResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<WorkOrderType> _getWorkOrderTypesResult;
        protected IEnumerable<WorkOrderType> getWorkOrderTypesResult
        {
            get
            {
                return _getWorkOrderTypesResult;
            }
            set
            {
                if (!object.Equals(_getWorkOrderTypesResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getWorkOrderTypesResult", NewValue = value, OldValue = _getWorkOrderTypesResult };
                    _getWorkOrderTypesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<WorkOrder> _getWorkOrdersResult;
        protected IEnumerable<WorkOrder> getWorkOrdersResult
        {
            get
            {
                return _getWorkOrdersResult;
            }
            set
            {
                if (!object.Equals(_getWorkOrdersResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getWorkOrdersResult", NewValue = value, OldValue = _getWorkOrdersResult };
                    _getWorkOrdersResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<PersonContact> _getPersonContactsResult;
        protected IEnumerable<PersonContact> getPersonContactsResult
        {
            get
            {
                return _getPersonContactsResult;
            }
            set
            {
                if (!object.Equals(_getPersonContactsResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getPersonContactsResult", NewValue = value, OldValue = _getPersonContactsResult };
                    _getPersonContactsResult = value;
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

         


        IEnumerable<Clear.Risk.Models.ClearConnection.Person> _getContractorResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.Person> getContractorResult
        {
            get
            {
                return _getContractorResult;
            }
            set
            {
                if (!object.Equals(_getContractorResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getContractorResult", NewValue = value, OldValue = _getContractorResult };
                    _getContractorResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.PersonSite> _getWorkSiteResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.PersonSite> getWorkSiteResult
        {
            get
            {
                return _getWorkSiteResult;
            }
            set
            {
                if (!object.Equals(_getWorkSiteResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getWorkSiteResult", NewValue = value, OldValue = _getWorkSiteResult };
                    _getWorkSiteResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<ProcessType> _getProcessTypesResult;
        protected IEnumerable<ProcessType> getProcessTypesResult
        {
            get
            {
                return _getProcessTypesResult;
            }
            set
            {
                if (!object.Equals(_getProcessTypesResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getProcessTypesResult", NewValue = value, OldValue = _getProcessTypesResult };
                    _getProcessTypesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<CriticalityMaster> _getCriticalityMastersResult;
        protected IEnumerable<CriticalityMaster> getCriticalityMastersResult
        {
            get
            {
                return _getCriticalityMastersResult;
            }
            set
            {
                if (!object.Equals(_getCriticalityMastersResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getCriticalityMastersResult", NewValue = value, OldValue = _getCriticalityMastersResult };
                    _getCriticalityMastersResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        WorkOrder _workorder;
        protected WorkOrder workorder
        {
            get
            {
                return _workorder;
            }
            set
            {
                if (!object.Equals(_workorder, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "workorder", NewValue = value, OldValue = _workorder };
                    _workorder = value;
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

        protected int currentCount = 0;
        //protected IMarkerCollection markers;
        //protected Marker selectedMarker;
        //protected InitialMapOptions initialMapOptions;

        [Inject]
        protected SurveyServices SurveyService { get; set; }
        protected async System.Threading.Tasks.Task Load()
        {
            var clearRiskGetOrderStatusesResult = await ClearConnection.GetOrderStatuses();
            getOrderStatusesResult = clearRiskGetOrderStatusesResult;

            var clearRiskGetPriorityMastersResult = await ClearConnection.GetPriorityMasters();
            getPriorityMastersResult = clearRiskGetPriorityMastersResult;

            var clearRiskGetWorkOrderTypesResult = await ClearConnection.GetWorkOrderTypes();
            getWorkOrderTypesResult = clearRiskGetWorkOrderTypesResult;

            var clearRiskGetWorkOrdersResult = await ClearRisk.GetWorkOrders();
            getWorkOrdersResult = clearRiskGetWorkOrdersResult;

            var clearRiskGetPersonContactsResult = await ClearConnection.GetPersonContacts();
            getPersonContactsResult = clearRiskGetPersonContactsResult;

            var clearRiskGetPeopleResult = await ClearConnection.GetClients(Security.getUserId(), null);
            getPeopleResult = clearRiskGetPeopleResult;

            clearRiskGetPeopleResult = await ClearConnection.GetContractors(Security.getUserId(), null);
            getContractorResult = clearRiskGetPeopleResult;

            var clearRiskGetProcessTypesResult = await ClearConnection.GetProcessTypes();
            getProcessTypesResult = clearRiskGetProcessTypesResult;

            var clearRiskGetCriticalityMastersResult = await ClearConnection.GetCriticalityMasters();
            getCriticalityMastersResult = clearRiskGetCriticalityMastersResult;

            var clearConnectionGetStatesResult = await ClearConnection.GetStates(new Query());
            getStatesResult = clearConnectionGetStatesResult;

            var clearConnectionGetCountriesResult = await ClearConnection.GetCountries(new Query());
            getCountriesResult = clearConnectionGetCountriesResult;

            getWorkSiteResult = await ClearConnection.GetPersonSites(new Query());

            var clearConnectionGetAssesmentsResult = await ClearConnection.GetAssesments(new Query() { Filter = $@"i => i.WORK_ORDER_ID == {int.Parse(WorkOrderId)} " });
            getAssesmentsResult = clearConnectionGetAssesmentsResult.Select(x => new Clear.Risk.Models.ClearConnection.Assesment 
            {
                ASSESMENTID = x.ASSESMENTID,
                RISKASSESSMENTNO = x.RISKASSESSMENTNO,
                ASSESMENTDATE = x.ASSESMENTDATE,
                PROJECTNAME = x.PROJECTNAME,
                TradeCategory = x.TradeCategory ?? null,
                TemplateType = x.TemplateType ?? null,
                EntityStatus = x.EntityStatus ?? null,
                WarningLevel = x.WarningLevel ?? null

            }).ToList();

            var clearConnectionGetSiteActivitiesResult = await ClearConnection.GetSiteActivities(new Query() { Filter = $@"i => i.Assesment.WORK_ORDER_ID == {int.Parse(WorkOrderId)} " });
            getSiteActivitiesResult = clearConnectionGetSiteActivitiesResult.Select(x => new SiteActivity 
            {
                SITE_ACTIVITY_ID =x.SITE_ACTIVITY_ID,
                Worker = x.Worker,
                Assesment = x.Assesment,
                START_DATE = x.START_DATE,
                STATUS = x.STATUS
            }).ToList();

            var clearRiskGetSurveyReportsResult = await SurveyService.GetSurveyReports(new Query() { Filter = $@"i => i.WORK_ORDER_ID == {int.Parse(WorkOrderId)}" });
            getSurveyReportsResult = clearRiskGetSurveyReportsResult.Select(x => new SurveyReport
            {
                SURVEY_REPORT_ID = x.SURVEY_REPORT_ID,
                SURVEY_DATE = x.SURVEY_DATE,
                Survey = x.Survey,
                Assesment = x.Assesment ?? null,
                Order = x.Order ?? null,
                EntityStatus = x.EntityStatus ?? null,
                WarningLevel =x.WarningLevel ?? null,
                COMMENTS = x.COMMENTS
            }).ToList();

            getWarningResult = await ClearConnection.GetWarningLevels();
            getEntityResult = await ClearConnection.GetEntityStatuses();

            //getFacilitesResult = await ClearConnection.GetPeople(new Query() { Filter = $@"i => i.PARENT_PERSON_ID == {Security.getCompanyId()} && i.COMPANYTYPE == 6 " });
            getFacilitesResult = await ClearConnection.GetEmployee(Security.getCompanyId(), new Query() { Filter = "i => i.ISMANAGER == true" });
            workorder =  await ClearRisk.GetWorkOrderByWorkOrderId(int.Parse($"{WorkOrderId}")) ;

            if(workorder.WorkLocation != null)
            {
                workorder.SiteName = workorder.WorkLocation.SITE_NAME;
                workorder.BuildingName = workorder.WorkLocation.BUILDING_NAME;
                workorder.Address1 = workorder.WorkLocation.SITE_ADDRESS1;
                workorder.Address2 = workorder.WorkLocation.SITE_ADDRESS2;
                workorder.City = workorder.WorkLocation.CITY;
                workorder.PostCode = workorder.WorkLocation.POST_CODE;
                workorder.StateId = workorder.WorkLocation.STATE_ID;
                workorder.CountryId = workorder.WorkLocation.COUNTRY_ID;
                workorder.Lat = (double)workorder.WorkLocation.LATITUDE;
                workorder.Lon = (double)workorder.WorkLocation.LONGITUDE;

                workorder.Floor = workorder.WorkLocation.FLOOR;
                workorder.RoomNo = workorder.WorkLocation.ROOMNO;




                

               
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.Person> _getFacilitesResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.Person> getFacilitesResult
        {
            get
            {
                return _getFacilitesResult;
            }
            set
            {
                if (!object.Equals(_getFacilitesResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getFacilitesResult", NewValue = value, OldValue = _getFacilitesResult };
                    _getFacilitesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        //protected void MapOnClick(Blazor.GoogleMap.Map.Events.MouseEventArgs mouseEvent)
        //{
        //    Console.WriteLine($"Clicked! {mouseEvent.LatLng.Lat}, {mouseEvent.LatLng.Lng}");
        //    markers.Add(new MarkerOptions(mouseEvent.LatLng)
        //    {
        //        Title = $"Test {DateTime.Now}",
        //        AssociatedInfoWindowId = markers.Count % 2 == 0 ? "infoWindow" : "infoWindowSecond",
        //        OnMarkerClick = EventCallback.Factory.Create<Marker>(this, MarkerClick)
        //    });
        //}

        //protected void MapOnDoubleClick(Blazor.GoogleMap.Map.Events.MouseEventArgs mouseEvent)
        //{
        //    Console.WriteLine($"DoubleClicked! {mouseEvent.LatLng.Lat}, {mouseEvent.LatLng.Lng}");
        //}

        //protected void MarkerClick(Marker marker)
        //{
        //    selectedMarker = marker;
        //    Console.WriteLine(marker.Options.Title);
        //}

        //protected async Task RemoveMarker()
        //{
        //    if (selectedMarker != null)
        //    {
        //        var removedResult = await markers.Remove(selectedMarker);
        //        Console.WriteLine($"Marker removed: {removedResult}");
        //    }
        //}

        protected void MapClick(GoogleMapClickEventArgs args)
        {
            //events.Add(DateTime.Now, $"Map clicked at Lat: {args.Position.Lat}, Lng: {args.Position.Lng}");
            StateHasChanged();
        }

        protected void MarkerClick(RadzenGoogleMapMarker marker)
        {
            //events.Add(DateTime.Now, $"Map {marker.Title} marker clicked. Marker position -> Lat: {marker.Position.Lat}, Lng: {marker.Position.Lng}");
            StateHasChanged();
        }

        protected int zoom = 3;

        protected bool showMadridMarker;
        protected List<RadzenGoogleMapMarker> radzenGoogleMapMarkers;

        private string GetNextValue(string s, int Maxnum)
        {
            return String.Format("WO-{0:D6}", Convert.ToInt64(s.Substring(3)) + Maxnum);
        }

        protected async System.Threading.Tasks.Task Form0Submit(WorkOrder args)
        {
            isLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {



                var clearRiskCreateWorkOrderResult = await ClearRisk.CreateWorkOrder(workorder);
                UriHelper.NavigateTo("work-order");
                // DialogService.Close(workorder);
            }
            catch (System.Exception clearRiskCreateWorkOrderException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new WorkOrder!");
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
                
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            if(EmpId != null)
                UriHelper.NavigateTo($@"edit-employee/{EmpId.ToString()}/3");
            else
                UriHelper.NavigateTo("work-order");
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

        #region Geolocation
        protected async Task ChangeState(object value, string name)
        {
            await GetGeolocation();
            StateHasChanged();
        }

        protected async Task GetGeolocation()
        {
            string address = workorder.Address1;
            if (!string.IsNullOrEmpty(workorder.Address2))
                address += "," + workorder.Address2;

            if (!string.IsNullOrEmpty(workorder.City))
                address += "," + workorder.City;

            if (!string.IsNullOrEmpty(workorder.PostCode))
                address += "," + workorder.PostCode;

            if (workorder.StateId != null)
            {
                var state = getStatesResult.Where(s => s.ID == workorder.StateId).FirstOrDefault();
                address += "," + state.STATENAME;
            }

            var country = getCountriesResult.Where(s => s.ID == workorder.CountryId).FirstOrDefault();
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

                if (addresses.Count() > 1)
                {
                    Address item = addresses.Where(a => a.FormattedAddress.Contains(address)).FirstOrDefault();
                    if (item != null)
                    {
                        workorder.Lat = item.Coordinates.Latitude;
                        workorder.Lon = item.Coordinates.Longitude;
                    }
                    else
                    {
                        workorder.Lat = addresses.First().Coordinates.Latitude;
                        workorder.Lon = addresses.First().Coordinates.Longitude;
                    }

                }
                else
                {
                    workorder.Lat = addresses.First().Coordinates.Latitude;
                    workorder.Lon = addresses.First().Coordinates.Longitude;
                }




            }
            catch (Exception ex)
            {

            }




        }
        #endregion

        protected async Task ChangeLocation(object value, string name)
        {
            var item = getWorkSiteResult.Where(a => a.PERSON_SITE_ID == workorder.WORK_LOCATION_ID).FirstOrDefault();
            if (item != null)
            {
                workorder.SiteName = item.SITE_NAME;
                workorder.BuildingName = item.BUILDING_NAME;
                workorder.Address1 = item.SITE_ADDRESS1;
                workorder.Address2 = item.SITE_ADDRESS2;
                workorder.City = item.CITY;
                workorder.PostCode = item.POST_CODE;
                workorder.StateId = item.STATE_ID;
                workorder.CountryId = item.COUNTRY_ID;
                workorder.Lat = (double?)item.LATITUDE;
                workorder.Lon = (double?)item.LONGITUDE;
                workorder.RoomNo = item.ROOMNO;
                workorder.Floor = item.FLOOR;
                if (item.LATITUDE == null && item.LONGITUDE == null)
                {
                    await GetGeolocation();
                    item.LATITUDE = (decimal)workorder.Lat;
                    item.LONGITUDE = (decimal)workorder.Lon;

                    await ClearConnection.UpdatePersonSite(item.PERSON_SITE_ID, item);

                }
            }
            StateHasChanged();
        }

        #region "Assessment History"

        protected IList<Clear.Risk.Models.ClearConnection.Assesment> getAssesmentsResult = new List<Clear.Risk.Models.ClearConnection.Assesment>();



        protected async System.Threading.Tasks.Task GridAssementViewButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {

                UriHelper.NavigateTo("edit-assesment-record" + "/" + data.ASSESMENTID.ToString());

            }
            catch (System.Exception clearRiskDeleteWorkOrderException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to find WorkOrder");
            }
        }

        protected IList<SiteActivity> getSiteActivitiesResult = new List<SiteActivity>();


        protected IList<SurveyReport> getSurveyReportsResult = new List<SurveyReport>();

        protected async System.Threading.Tasks.Task GridSurveyButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                //UriHelper.NavigateTo("view-survey-report" + "/" + data.SURVEY_REPORT_ID.ToString());
                UriHelper.NavigateTo($@"view-survey-report/{data.SURVEY_REPORT_ID.ToString()}/WO/{WorkOrderId}/5");

            }
            catch (System.Exception clearRiskDeleteWorkOrderException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to find WorkOrder");
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.WarningLevel> _getWarningResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.WarningLevel> getWarningResult
        {
            get
            {
                return _getWarningResult;
            }
            set
            {
                if (!object.Equals(_getWarningResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getWarningResult", NewValue = value, OldValue = _getWarningResult };
                    _getWarningResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.EntityStatus> _getEntityResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.EntityStatus> getEntityResult
        {
            get
            {
                return _getEntityResult;
            }
            set
            {
                if (!object.Equals(_getEntityResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getEntityResult", NewValue = value, OldValue = _getEntityResult };
                    _getEntityResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected async System.Threading.Tasks.Task ButtonAssesmentClick(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<GenerateAssessment>("Generate Assessment", new Dictionary<string, object>() { { "WorkOrderId", workorder.WORK_ORDER_ID } }, new DialogOptions() { Width = $"{800}px" });
           
           // await InvokeAsync(() => { StateHasChanged(); });
        }
        #endregion
    }
}
