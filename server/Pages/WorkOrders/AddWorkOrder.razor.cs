﻿using System;
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

namespace Clear.Risk.Pages.WorkOrders
{
    public partial class AddWorkOrder : ComponentBase
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
        protected WorkOrderService ClearRisk { get; set; }

        [Inject]
        protected ClearConnectionService ClearConnection { get; set; }

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

            var clearRiskGetPeopleResult = await ClearConnection.GetClients(Security.getCompanyId(), null);
            getPeopleResult = clearRiskGetPeopleResult;

            clearRiskGetPeopleResult = await ClearConnection.GetContractors(Security.getCompanyId(), null);
            getContractorResult = clearRiskGetPeopleResult;

            var clearRiskGetProcessTypesResult = await ClearConnection.GetProcessTypes();
            getProcessTypesResult = clearRiskGetProcessTypesResult;

            var clearRiskGetCriticalityMastersResult = await ClearConnection.GetCriticalityMasters();
            getCriticalityMastersResult = clearRiskGetCriticalityMastersResult;

            var clearConnectionGetStatesResult = await ClearConnection.GetStates(new Query());
            getStatesResult = clearConnectionGetStatesResult;

            var clearConnectionGetCountriesResult = await ClearConnection.GetCountries(new Query());
            getCountriesResult = clearConnectionGetCountriesResult;

            //getFacilitesResult = await ClearConnection.GetPeople(new Query() { Filter = $@"i => i.PARENT_PERSON_ID == {Security.getCompanyId()} && i.COMPANYTYPE == 6 && i.ISMANAGER == true" });
            getFacilitesResult = await ClearConnection.GetEmployee(Security.getCompanyId(), new Query() { Filter = "i => i.ISMANAGER == true" });

            getWorkSiteResult = await ClearConnection.GetPersonSites(new Query());

            var company = await ClearConnection.GetPersonByPersonId(Security.getCompanyId());
            int maxID = await ClearRisk.GetMaxID(Security.getCompanyId());
            string current = "WO-0000000";
            workorder = new WorkOrder()
            {
                DATE_RAISED = DateTime.Now,
                CREATED_DATE = DateTime.Now,
                UPDATED_DATE = DateTime.Now,
                CREATOR_ID = Security.getUserId(),
                UPDATER_ID = Security.getUserId(),
                IS_DELETED = false,
                WARNING_LEVEL_ID = 1,
                ESCALATION_LEVEL_ID = 1,
                STATUS_ID = 1,
                DUE_DATE = DateTime.Now.AddDays(1),
                WORK_ORDER_NUMBER = GetNextValue(current, maxID),
                COMPANY_ID = Security.getCompanyId(),
                STATUS_LEVEL_ID = 1,
                CountryId = (int)company.PERSONAL_COUNTRY_ID,
                StateId = null,
                PROCESSTYPE_ID = 4,
                REACTIVECRITICALITY_ID = 2,
                ISINTERNAL = true

            };
        }

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

                if (workorder.Lat == null && workorder.Lon == null)
                    await GetGeolocation();

                if (workorder.WORK_LOCATION_ID == null)
                {
                    //Create Work Site
                    await CreateWorkSite(workorder);

                }
                //else
                //{
                //    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Work Location cannot be empty",180000);
                //}

                var clearRiskCreateWorkOrderResult = await ClearRisk.CreateWorkOrder(workorder);

                if (workorder.WORK_ORDER_ID > 0)
                {
                    UriHelper.NavigateTo("edit-work-order" + "/" + workorder.WORK_ORDER_ID.ToString());
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new WorkOrder!");
                }

                //UriHelper.NavigateTo("work-order");
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

        protected async Task CreateWorkSite(WorkOrder assesment)
        {
            PersonSite model = new PersonSite();
            model.CREATED_DATE = DateTime.Now;
            model.CREATOR_ID = Security.getUserId();
            model.UPDATED_DATE = DateTime.Now;
            model.UPDATER_ID = Security.getUserId();
            model.IS_DELETED = false;
            model.PERSON_ID = assesment.CLIENT_ID;
            model.SITE_NAME = assesment.SiteName;
            model.BUILDING_NAME = assesment.BuildingName;
            model.SITE_ADDRESS1 = assesment.Address1;
            model.SITE_ADDRESS2 = assesment.Address2;
            model.CITY = assesment.City;
            model.POST_CODE = assesment.PostCode;
            model.STATE_ID = assesment.StateId;
            model.COUNTRY_ID = assesment.CountryId;
            model.LATITUDE = (decimal?)assesment.Lat;
            model.LONGITUDE = (decimal?)assesment.Lon;
            model.FLOOR = assesment.Floor;
            model.ROOMNO = assesment.RoomNo;

            assesment.WorkLocation = model;
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
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
        protected async Task ClientInternalChanged()
        {
            workorder.WORK_LOCATION_ID = 0;
            workorder.SiteName = "";
            workorder.BuildingName = "";
            workorder.Address1 = "";
            workorder.Address2 = "";
            workorder.City = "";
            workorder.PostCode = "";
            workorder.StateId = null;
            workorder.CountryId = 0;
            workorder.Lat = null;
            workorder.Lon = null;
            workorder.RoomNo = "";
            workorder.Floor = "";

        }

        protected async Task ChangeLocation(object value, string name)
        {
            if (value == null)
            {
                workorder.SiteName = "";
                workorder.BuildingName = "";
                workorder.Address1 = "";
                workorder.Address2 = "";
                workorder.City = "";
                workorder.PostCode = "";
                workorder.StateId = null;
                workorder.CountryId = 0;
                workorder.Lat = null;
                workorder.Lon = null;
                workorder.RoomNo = "";
                workorder.Floor = "";
            }
            else
            {
                var item = getWorkSiteResult.Where(a => a.PERSON_SITE_ID == (int)value).FirstOrDefault();
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
                else
                {
                    workorder.SiteName = "";
                    workorder.BuildingName = "";
                    workorder.Address1 = "";
                    workorder.Address2 = "";
                    workorder.City = "";
                    workorder.PostCode = "";
                    workorder.StateId = null;
                    workorder.CountryId = 0;
                    workorder.Lat = null;
                    workorder.Lon = null;
                    workorder.RoomNo = "";
                    workorder.Floor = "";
                }
            }
            StateHasChanged();
        }


        protected void OnDateChange(string field)
        {
            if (workorder.DATE_RAISED.Date < DateTime.Now.Date)
                workorder.DATE_RAISED = DateTime.Now.Date;

            if (workorder.DUE_DATE.Date < DateTime.Now.Date)
                workorder.DUE_DATE = DateTime.Now.AddDays(1).Date;


            if (field == "StartDate")
            {
                if (workorder.DATE_RAISED.Date >= workorder.DUE_DATE.Date)
                    workorder.DUE_DATE = workorder.DATE_RAISED.AddDays(1).Date;
            }
            else
            {
                if (workorder.DUE_DATE.Date <= workorder.DATE_RAISED.Date)
                    workorder.DUE_DATE = workorder.DATE_RAISED.AddDays(1).Date;
            }
        }

    }
}
