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
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.Extensions.Logging.Abstractions;
using Novacode;
using System.Drawing;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;
using Syncfusion.Pdf;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using System.Net;
using System.Net.Http;
using Geocoding;
using Geocoding.Google;
using Syncfusion.Blazor.Gantt;
using DocumentFormat.OpenXml.VariantTypes;
//using Coravel.Scheduling.Schedule.Interfaces;
using Hangfire;

namespace Clear.Risk.Pages.RiskAssesment
{
    public partial class EditAssesmentRecord : ComponentBase
    {
        [Inject]
        protected IWebHostEnvironment _hosting { get; set; }
        protected async System.Threading.Tasks.Task ButtonBackClick(MouseEventArgs args)
        {
            UriHelper.NavigateTo("manage-assesments");
        }
        protected bool isTabOpen { get; set; }
        protected async System.Threading.Tasks.Task SaveAndRunOnScheduleClick(MouseEventArgs args)
        {
            try
            {
                assesment.UPDATED_DATE = DateTime.Now;
                assesment.UPDATER_ID = Security.getUserId();
                await ClearConnection.UpdateAssesment(assesment.ASSESMENTID, assesment);
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }

        }

        protected bool isLoading = true;
        protected async Task<Assesment> SaveAssesment()
        {
            if (assesment != null)
            {
                //if (assesment.WORK_SITE_ID == 0)
                //{
                //    NotificationService.Notify(NotificationSeverity.Error, $"Work Site should not left blank");
                //    return assesment;
                //}


                if (!assesment.ISSCHEDULE)
                {
                    NotificationService.Notify(NotificationSeverity.Error, $"Please Click on Run Now Option to Generate Assessment");
                    return assesment;
                }

                else
                {
                    if (assesment.SCHEDULE_TYPE_ID == null)
                    {
                        NotificationService.Notify(NotificationSeverity.Error, $"Schedule Type should not left blank");
                        return assesment;
                    }


                    if (!assesment.MON && !assesment.TUE && !assesment.WED && !assesment.THUS && !assesment.FRI
                        && !assesment.SAT && !assesment.SUN)
                    {
                        NotificationService.Notify(NotificationSeverity.Error, $"Day of Schedule required to select to Run and Generate Assessment");
                        return assesment;
                    }

                }

                if (assesment.SWMSTemplateNames.Count() == 0)
                {
                    NotificationService.Notify(NotificationSeverity.Error, $"Select one or more Templates to Generate Assessment");
                    return assesment;
                }


                if (assesment.EmployeeNames.Count() == 0)
                {
                    NotificationService.Notify(NotificationSeverity.Error, $"Select one or more Employee/s to Assigned Assessment");
                    return assesment;
                }

                //Clear.Risk.Models.ClearConnection.Template template = await ClearConnection.GetTemplateByTradeId(assesment.TRADECATEGORYID);
                //if (template == null)
                //{
                //    NotificationService.Notify(NotificationSeverity.Error, $"There is no template vailable for selected Trade Category");
                //    return assesment;
                //}

                if (assesment.WORK_SITE_ID == 0)
                {
                    //Create Work Site
                    await CreateWorkSite(assesment);

                }

                if (assesment.WORK_ORDER_ID == null)
                {
                    //Create Work Order
                    await CreateWorkOrder(assesment);
                }

                assesment.CREATED_DATE = DateTime.Now;
                assesment.CREATOR_ID = Security.getUserId();
                assesment.UPDATED_DATE = DateTime.Now;
                assesment.UPDATER_ID = Security.getUserId();
                assesment.STATUS = 1;
                assesment.ENTITY_STATUS_ID = 1;
                assesment.ESCALATION_LEVEL_ID = 1;
                assesment.WARNING_LEVEL_ID = 1;
                assesment.STATUS_LEVEL_ID = 1;

                var clearConnectionGetTradeCategoriesResult = await ClearConnection.GetTradeCategories();
                //Assigned Employee
                foreach (var employee in assesment.EmployeeNames)
                {
                    var item = new AssesmentEmployee()
                    {
                        EMPLOYEE_ID = employee
                    };

                    if (assesment.AssesmentEmployees == null)
                        assesment.AssesmentEmployees = new List<AssesmentEmployee>();

                    assesment.AssesmentEmployees.Add(item);
                }

                foreach (var swmsid in assesment.SWMSTemplateNames)
                {

                    var result = getswmsresults.Where(a => a.SWMSID == swmsid).FirstOrDefault();

                    if (result != null)
                    {
                        if (assesment.AssesmentAttachements == null)
                            assesment.AssesmentAttachements = new List<AssesmentAttachement>();

                        assesment.AssesmentAttachements.Add(new AssesmentAttachement()
                        {
                            SWMS_TEMPLATE_ID = swmsid,
                            ATTACHEMENTDATE = DateTime.Now,
                            DOCUMENTTEMPLATEURL = result.TEMPLATENAME
                        });
                    }

                }

                //Assigned Employee with Assesment Record

                //Generate PDF Template and Assigned To Employee
                // await ReplaceTextInDocFileForPdf(assesment);

                return await ClearConnection.CreateAssesment(assesment);
            }
            return assesment;
        }

        protected async Task CreateWorkSite(Assesment assesment)
        {
            PersonSite model = new PersonSite();
            model.CREATED_DATE = DateTime.Now;
            model.CREATOR_ID = Security.getUserId();
            model.UPDATED_DATE = DateTime.Now;
            model.UPDATER_ID = Security.getUserId();
            model.IS_DELETED = false;
            model.PERSON_ID = (int)assesment.CLIENTID;
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

            await ClearConnection.CreatePersonSite(model);

            if (model.PERSON_SITE_ID > 0)
                assesment.WORK_SITE_ID = model.PERSON_SITE_ID;
        }

        protected async Task CreateWorkOrder(Assesment assesment)
        {

            int maxID = await OrderService.GetMaxID(Security.getUserId());
            string current = "WO-0000000";
            assesment.WorkOrder = new WorkOrder()
            {
                DATE_RAISED = assesment.ASSESMENTDATE,
                CREATED_DATE = DateTime.Now,
                UPDATED_DATE = DateTime.Now,
                CREATOR_ID = Security.getUserId(),
                UPDATER_ID = Security.getUserId(),
                IS_DELETED = false,
                WARNING_LEVEL_ID = 1,
                ESCALATION_LEVEL_ID = 1,
                DUE_DATE = assesment.ASSESMENTDATE,
                WORK_ORDER_NUMBER = GetNextValue(current, maxID),
                COMPANY_ID = assesment.COMPANYID,
                CLIENT_ID = assesment.CLIENTID,
                CLIENT_CONTACT_ID = assesment.CLIENT_CONTACT_ID,
                CLIENT_WORK_ORDER_NUMBER = assesment.WORKORDERNUMBER,
                PURCHASE_ORDER_NUMBER = assesment.PURCHASEORDER,
                WORK_ORDER_TYPE = 11,
                PROCESSTYPE_ID = 4,
                PRIORITY_ID = 2,
                STATUS_ID = 3,
                REACTIVECRITICALITY_ID = 6,
                DESCRIPTION = assesment.SCOPEOFWORK,
                CONTRACTOR_ID = assesment.CONTRACTOR_ID,
                CONTRACTOR_CONTACT_ID = assesment.CONTRACTOR_CONTACT_ID,
                START_DATE = assesment.WORKSTARTDATE,
                END_DATE = assesment.WORKENDDATE,
                AUTHORIZED_COST = 0M,
                ESTIMETED_HOUR = 0,
                ESTIMATEDWORKORDER_COST = 0M,
                ACTUALWORKORDER_COST = 0M,
                WORK_LOCATION_ID = assesment.WORK_SITE_ID

            };

            //Assigned Worker
            foreach (int personid in assesment.EmployeeNames)
            {
                if (assesment.WorkOrder.WorkerPeople == null)
                    assesment.WorkOrder.WorkerPeople = new List<WorkerPerson>();

                assesment.WorkOrder.WorkerPeople.Add(new WorkerPerson()
                {
                    PERSON_ID = personid,
                    TOTAL_HOURES = 0,
                    CREATED_DATE = DateTime.Now,
                    CREATOR_ID = Security.getUserId(),
                    UPDATED_DATE = DateTime.Now,
                    UPDATER_ID = Security.getUserId(),
                    IS_DELETED = false,
                    ESCALATION_LEVEL_ID = 1,
                    WARNING_LEVEL_ID = 1,
                    STATUS_LEVEL_ID = 1,
                    WORKING_HOURES = 0M

                }
                 );
            }
        }

        private string GetNextValue(string s, int Maxnum)
        {
            return String.Format("WO-{0:D6}", Convert.ToInt64(s.Substring(3)) + Maxnum);
        }
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        [Inject]
        protected SecurityService Security { get; set; }

        [Inject]
        protected ClearConnectionService ClearConnection { get; set; }

        [Inject]
        protected AssesmentService AssesmentConnection { get; set; }

        [Inject]
        protected WorkOrderService OrderService { get; set; }

        Clear.Risk.Models.ClearConnection.Assesment _assesment;


        //[Inject]
        //protected Coravel.Scheduling.Schedule.Interfaces.IScheduler _schedule { get; set; }

        [Inject]
        protected NavigationManager UriHelper { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

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

        protected Clear.Risk.Models.ClearConnection.Assesment assesment
        {
            get
            {
                return _assesment;
            }
            set
            {
                if (!object.Equals(_assesment, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "assesment", NewValue = value, OldValue = _assesment };
                    _assesment = value;
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
            //await Load();

        }
        [Parameter]
        public dynamic ASSESMENTID { get; set; }

        protected async Task Load()
        {
            isTabOpen = false;
            isLoading = true;
            StateHasChanged();
            await Task.Delay(1);

            var clearRiskGetPersonContactsResult = await ClearConnection.GetPersonContacts();
            getPersonContactsResult = clearRiskGetPersonContactsResult;

            var clearRiskGetPeopleResult = await ClearConnection.GetContractors(Security.getUserId(), null);
            getContractorResult = clearRiskGetPeopleResult;

            getWorkSiteResult = await ClearConnection.GetPersonSites(new Query());

            var clearConnectionGetStatesResult = await ClearConnection.GetStates(new Query());
            getStatesResult = clearConnectionGetStatesResult;

            var clearConnectionGetCountriesResult = await ClearConnection.GetCountries(new Query());
            getCountriesResult = clearConnectionGetCountriesResult;

            getWorkOrderResult = await OrderService.GetWorkOrders(Security.getUserId(), null);

            getWarningResult = await ClearConnection.GetWarningLevels();
            getEntityResult = await ClearConnection.GetEntityStatuses();

            getTemplateResult = await ClearConnection.GetTemplateList(new Query() { Filter = $@"i => i.COMPANYID == {Security.getCompanyId()} || i.COUNTRY_ID == {Security.getCountryId()} " });



            assesment = await ClearConnection.GetAssesmentByAssesmentid(int.Parse($"{ASSESMENTID}"));

            if (assesment != null && assesment.PersonSite != null)
            {
                assesment.SiteName = assesment.PersonSite.SITE_NAME;
                assesment.BuildingName = assesment.PersonSite.BUILDING_NAME;
                assesment.Address1 = assesment.PersonSite.SITE_ADDRESS1;
                assesment.Address2 = assesment.PersonSite.SITE_ADDRESS2;
                assesment.City = assesment.PersonSite.CITY;
                assesment.PostCode = assesment.PersonSite.POST_CODE;
                assesment.StateId = assesment.PersonSite.STATE_ID;
                assesment.CountryId = assesment.PersonSite.COUNTRY_ID;
                assesment.Lat = (double)assesment.PersonSite.LATITUDE;
                assesment.Lon = (double)assesment.PersonSite.LONGITUDE;

                assesment.Floor = assesment.PersonSite.FLOOR;
                assesment.RoomNo = assesment.PersonSite.ROOMNO;
            }



            var clearConnectionGetSurveysResult = await AssesmentConnection.GetSurveys(new Query());
            getSurveysResult = clearConnectionGetSurveysResult;

            var clearConnectionGetScheduleTypesResult = await ClearConnection.GetScheduleTypes(new Query());
            getScheduleTypesResult = clearConnectionGetScheduleTypesResult;

            var clearConnectionGetSWMSResult = await ClearConnection.GetSwmsTemplates(new Query());
            getswmsresults = clearConnectionGetSWMSResult;

            var clearConnectionGetPeopleResult = await ClearConnection.GetClients(Security.getUserId(), new Query());
            getPeopleResult = clearConnectionGetPeopleResult;

            getEmployeeResult = await ClearConnection.GetEmployee(Security.getUserId(), new Query());

            var clearConnectionGetTradeCategoriesResult = await ClearConnection.GetTradeCategories();
            getTradeCategoriesResult = clearConnectionGetTradeCategoriesResult;

            var results = await ClearConnection.GetTemplateTypes();
            getAssessmentTypeResult = results;

            var clearConnectionGetAssesmentAttachementsResult = await ClearConnection.GetAssesmentAttachements(new Query() { Filter = $@"i => i.ASSESMENTID == {ASSESMENTID}" });

            AssesmentAttachements = clearConnectionGetAssesmentAttachementsResult.Select(x => new Clear.Risk.Models.ClearConnection.AssesmentAttachement
            {
                ATTACHEMENTID = x.ATTACHEMENTID,
                ATTACHEMENTDATE = x.ATTACHEMENTDATE,
                Attachments = x.Attachments,
                //DOCUMENTPDFURL = x.DOCUMENTPDFURL,
                DOCUMENTTEMPLATEURL = x.DOCUMENTTEMPLATEURL,
                SwmsTemplate = x.SwmsTemplate
            }).ToList();

            var clearConnectionGetAssesmentEmployeesResult = await ClearConnection.GetAssesmentEmployees(new Query() { Filter = $@"i => i.ASSESMENT_ID == {ASSESMENTID}" });
            AssesmentEmployees = clearConnectionGetAssesmentEmployeesResult.Select(x => new Clear.Risk.Models.ClearConnection.AssesmentEmployee
            {
                ASSESMENT_EMPLOYEE_ID = x.ASSESMENT_EMPLOYEE_ID,
                EMPLOYEE_ID = x.EMPLOYEE_ID,
                Employee = x.Employee ?? null,
                WarningLevel = x.WarningLevel ?? null,
                FileName = x.FileName,
                SignatureImageUrl = x.SignatureImageUrl,
                Sign_Date = x.Sign_Date,
                VersionNo = x.VersionNo,
                SignedStatus = x.SignedStatus,
                IS_ACTIVE = x.IS_ACTIVE
            }).ToList();

            IQueryable<AssesmentEmployeeAttachement> clearConnectionGetAssesmentEmployeeAttachementsResult = await ClearConnection.GetAssesmentEmployeeAttachementsByAssesmentId(int.Parse(ASSESMENTID));

            getAssesmentEmployeeAttachementsResult = clearConnectionGetAssesmentEmployeeAttachementsResult.Select(x =>
                                                        new AssesmentEmployeeAttachement
                                                        {
                                                            DOCUMENTNAME = x.DOCUMENTNAME,
                                                            DOCUMENT_URL = x.DOCUMENT_URL,
                                                            ASSIGNED_DATE = x.ASSIGNED_DATE,
                                                            ACCEPTED_DATE = x.ACCEPTED_DATE,
                                                            SINGNATURE_DATE = x.SINGNATURE_DATE,
                                                            AssesmentEmployeeStatus = x.AssesmentEmployeeStatus,
                                                            WarningLevel = x.WarningLevel,
                                                        }).ToList();

            //var clearConnectionGetAssesmentEmployeeAttachmentsResult = await ClearConnection.GetAssesmentEmployeeAttachementsByAssesmentId(int.Parse(ASSESMENTID), new Query());


            var scheduleResults = await ClearConnection.GetAssesmentSchedules(new Query() { Filter = $@"i => i.ASSESMENTID == {ASSESMENTID}" });
            AssesmentSchedules = scheduleResults.Select(x =>
                                    new AssesmentSchedule
                                    {
                                        ASSESSMENT_SCHEDULE_ID = x.ASSESSMENT_SCHEDULE_ID,
                                        ScheduleType = x.ScheduleType,
                                        INTERVAL = x.INTERVAL,
                                        SCHEDULE_TIME = x.SCHEDULE_TIME,
                                        MON = x.MON,
                                        TUE = x.TUE,
                                        WED = x.WED,
                                        THUS = x.THUS,
                                        FRI = x.FRI,
                                        SAT = x.SAT,
                                        SUN = x.SUN
                                    }).ToList();

            isLoading = false;
            StateHasChanged();

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
                    var args = new PropertyChangedEventArgs() { Name = "getTradeCategoriesResult", NewValue = value, OldValue = _getTradeCategoriesResult };
                    _getTradeCategoriesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.TemplateType> _getAssessmentTypeResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.TemplateType> getAssessmentTypeResult
        {
            get
            {
                return _getAssessmentTypeResult;
            }
            set
            {
                if (!object.Equals(_getTradeCategoriesResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getAssessmentTypeResult", NewValue = value, OldValue = _getAssessmentTypeResult };
                    _getAssessmentTypeResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<WorkOrder> _getWorkOrderResult;
        protected IEnumerable<WorkOrder> getWorkOrderResult
        {
            get
            {
                return _getWorkOrderResult;
            }
            set
            {
                if (!object.Equals(_getWorkOrderResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getWorkOrderResult", NewValue = value, OldValue = _getWorkOrderResult };
                    _getWorkOrderResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        private string GetFormNumber()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            var FormNumber = BitConverter.ToUInt32(buffer, 0) ^ BitConverter.ToUInt32(buffer, 4) ^ BitConverter.ToUInt32(buffer, 8) ^ BitConverter.ToUInt32(buffer, 12);
            return FormNumber.ToString("X");

        }

        IEnumerable<Clear.Risk.Models.ClearConnection.PersonSite> _getPersonSitesResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.PersonSite> getPersonSitesResult
        {
            get
            {
                return _getPersonSitesResult;
            }
            set
            {
                if (!object.Equals(_getPersonSitesResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getPersonSitesResult", NewValue = value, OldValue = _getPersonSitesResult };
                    _getPersonSitesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }


        }

        protected void Change(object value, string name)
        {

            StateHasChanged();
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.Survey> _getSurveysResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.Survey> getSurveysResult
        {
            get
            {
                return _getSurveysResult;
            }
            set
            {
                if (!object.Equals(_getSurveysResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getSurveysResult", NewValue = value, OldValue = _getSurveysResult };
                    _getSurveysResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.ScheduleType> _getScheduleTypesResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.ScheduleType> getScheduleTypesResult
        {
            get
            {
                return _getScheduleTypesResult;
            }
            set
            {
                if (!object.Equals(_getScheduleTypesResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getScheduleTypesResult", NewValue = value, OldValue = _getScheduleTypesResult };
                    _getScheduleTypesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }


        }

        IEnumerable<Clear.Risk.Models.ClearConnection.SwmsTemplate> _getswmsresults;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.SwmsTemplate> getswmsresults
        {
            get
            {
                return _getswmsresults;
            }
            set
            {
                if (!object.Equals(_getswmsresults, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getswmsresults", NewValue = value, OldValue = _getswmsresults };
                    _getswmsresults = value;
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

        IEnumerable<Clear.Risk.Models.ClearConnection.Person> _getEmployeeResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.Person> getEmployeeResult
        {
            get
            {
                return _getEmployeeResult;
            }
            set
            {
                if (!object.Equals(_getEmployeeResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getEmployeeResult", NewValue = value, OldValue = _getEmployeeResult };
                    _getEmployeeResult = value;
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

        DateTime? value = DateTime.Now;

        IEnumerable<DateTime> dates = new DateTime[] { DateTime.Today.AddDays(-1), DateTime.Today.AddDays(1) };

        Dictionary<DateTime, string> events = new Dictionary<DateTime, string>();

        protected void Change(DateTime? value, string name, string format)
        {
            events.Add(DateTime.Now, $"{name} value changed to {value?.ToString(format)}");
            StateHasChanged();
        }

        protected void DateRenderSpecial(DateRenderEventArgs args)
        {
            if (dates.Contains(args.Date))
            {
                args.Attributes.Add("style", "background-color: #ff6d41; border-color: white;");
            }
        }

        protected void DateRender(DateRenderEventArgs args)
        {
            args.Disabled = dates.Contains(args.Date);
        }

        

        protected async Task<string> ReplaceTextInDocFileForPdf(Assesment refViewModel)
        {
            if (refViewModel != null)
            {
                string filePath = _hosting.WebRootPath;

                foreach (var item in refViewModel.AssesmentAttachements)
                {
                    string fullPath = Path.Combine(filePath, (filePath + @"\Uploads\Templates\" + item.DOCUMENTTEMPLATEURL));

                    if (item.SwmsTemplate == null)
                        item.SwmsTemplate = await ClearConnection.GetSwmsBySwmsid(item.SWMS_TEMPLATE_ID);

                    if (refViewModel.Company == null)
                        refViewModel.Company = await ClearConnection.GetPersonByPersonId(refViewModel.COMPANYID);

                    if (refViewModel.Contractor == null)
                        refViewModel.Contractor = getContractorResult.Where(a => a.PERSON_ID == refViewModel.CONTRACTOR_ID).FirstOrDefault();

                    if (refViewModel.ContractorContact == null)
                        refViewModel.ContractorContact = getPersonContactsResult.Where(a => a.PERSON_CONTACT_ID == refViewModel.CONTRACTOR_CONTACT_ID).FirstOrDefault();

                    if (refViewModel.Client == null)
                        refViewModel.Client = getPeopleResult.Where(a => a.PERSON_ID == refViewModel.CLIENTID).FirstOrDefault();

                    if (refViewModel.ClientContact == null)
                        refViewModel.ClientContact = getPersonContactsResult.Where(a => a.PERSON_CONTACT_ID == refViewModel.CLIENT_CONTACT_ID).FirstOrDefault();

                    Novacode.DocX document = Novacode.DocX.Load(fullPath);

                    document.ReplaceText("&&Company_ComapnyName", refViewModel.Company != null ? refViewModel.Company.COMPANY_NAME : string.Empty);
                    document.ReplaceText("&&Contractor_FullName", refViewModel.Company != null ? refViewModel.Company.FullName : string.Empty);
                    document.ReplaceText("&&Company_ComapnyPhoneNo", refViewModel.Company != null ? refViewModel.Company.BUSINESS_PHONE : string.Empty);
                    document.ReplaceText("&&Assessment_DateCreated", DateTime.Now.ToString());
                    document.ReplaceText("&&Assessment_Version", item.SwmsTemplate != null ? item.SwmsTemplate.VERSION : "");
                    //document.ReplaceText("&&Assessment_WorkOrder", model.WorkOrderNumber != null ? model.WorkOrderNumber : "");
                    document.ReplaceText("&&Date", DateTime.Now.ToString());
                    document.ReplaceText("&&Contractor_PhoneNo", string.Empty);
                    document.ReplaceText("&&Contractor_ComapnyName", string.Empty);
                    document.ReplaceText("&&Company_Name", string.Empty);
                    document.ReplaceText("&&Customer_Manager", refViewModel.Company != null ? refViewModel.Company.COMPANY_NAME : string.Empty);
                    document.ReplaceText("&&Scope", refViewModel.SCOPEOFWORK);
                    document.ReplaceText("&&Assessment_ContractorSiteManager", refViewModel.CONTRACTORSITEMANAGER);
                    document.ReplaceText("&&CustomerManager_PhoneNo", refViewModel.CONTRACTORSITEMNGRMNO);
                    // document.ReplaceText("&&Logo", model.OrderCompany.CompanyLogoId != null ? model.OrderCompany.CompanyLogoId : "");
                    document.ReplaceText("&&Permit_Number", refViewModel.WORKORDERNUMBER);


                    #region Assessment Text Replace
                    document.ReplaceText("&&AssessmentTask_AssignedDate", DateTime.Now.ToString());
                    document.ReplaceText("&&Assessment_AssessmentDate", DateTime.Now.ToString());
                    document.ReplaceText("&&Assessment_AssessmentID", (refViewModel.ASSESMENTID.ToString()));
                    document.ReplaceText("&&Assessment_ClientID", (refViewModel.CLIENTID != null ? refViewModel.CLIENTID.ToString() : string.Empty));
                    document.ReplaceText("&&Assessment_CompanyID", (refViewModel.COMPANYID.ToString()));
                    document.ReplaceText("&&Assessment_ContractorSiteMngrMNo", (refViewModel.CONTRACTORSITEMNGRMNO));
                    document.ReplaceText("&&Assessment_CreationTime", DateTime.Now.TimeOfDay.ToString());
                    //document.ReplaceText("&&Assessment_EmployeeID", (refViewModel.model.EmployeeID != null ? refViewModel.model.EmployeeID.ToString() : ""));
                    document.ReplaceText("&&Assessment_IsCompleted", refViewModel.ISCOMPLETED ? "Completed" : "Pending");
                    document.ReplaceText("&&Assessment_OrderLocation", refViewModel.ORDERLOCATION);
                    document.ReplaceText("&&Assessment_PermitNumber", refViewModel.PURCHASEORDER);
                    document.ReplaceText("&&Assessment_PlaceofWorkAddress", refViewModel.PLACEOFWORKADDRESS);
                    document.ReplaceText("&&Assessment_ProjectName", refViewModel.PROJECTNAME);
                    document.ReplaceText("&&Assessment_PurchaseOrder", refViewModel.PURCHASEORDER);
                    document.ReplaceText("&&Assessment_ReferenceNumber", refViewModel.REFERENCENUMBER);
                    document.ReplaceText("&&Assessment_RiskAssessmentNo", refViewModel.RISKASSESSMENTNO);
                    document.ReplaceText("&&Assessment_ScopeOfWork", refViewModel.SCOPEOFWORK);
                    document.ReplaceText("&&Assessment_TemplateId", item.SWMS_TEMPLATE_ID.ToString());
                    document.ReplaceText("&&Company_Country", refViewModel.Company != null ? refViewModel.Company.Country1 != null ? refViewModel.Company.Country1.COUNTRYNAME : "" : "");
                    document.ReplaceText("&&Assessment_TypeofAssessment", refViewModel.TemplateType != null ? refViewModel.TemplateType.NAME : "");
                    document.ReplaceText("&&Assessment_WorkOrderNumber", refViewModel.WORKORDERNUMBER);
                    document.ReplaceText("&&Assessment_PrincipalContractor", refViewModel.PRINCIPALCONTRACTOR);
                    document.ReplaceText("&&Assessment_WorkingContractor", refViewModel.WORKINGCONTRACTOR);
                    document.ReplaceText("&&Assessment_WorkStartDate", string.Format("{0:d}", refViewModel.WORKSTARTDATE));
                    document.ReplaceText("&&Assessment_WorkEndDate", string.Format("{0:d}", refViewModel.WORKENDDATE));
                    #endregion

                    document.ReplaceText("&&Company_Address1", refViewModel.Company != null ? refViewModel.Company.BUSINESS_ADDRESS1 : "");
                    document.ReplaceText("&&Company_Address2", refViewModel.Company != null ? refViewModel.Company.BUSINESS_ADDRESS2 : "");
                    document.ReplaceText("&&Company_City", refViewModel.Company != null ? refViewModel.Company.BUSINESS_CITY : "");
                    document.ReplaceText("&&Company_CompanyName", refViewModel.Company != null ? refViewModel.Company.COMPANY_NAME : "");
                    document.ReplaceText("&&Company_CompanyNumber", refViewModel.Company != null ? refViewModel.Company.BUSINESS_PHONE : "");
                    document.ReplaceText("&&Company_CompanyOfficeNumber", refViewModel.Company != null ? refViewModel.Company.BUSINESS_MOBILE : "");
                    document.ReplaceText("&&Company_Country", refViewModel.Company != null ? refViewModel.Company.Country1 != null ? refViewModel.Company.Country1.COUNTRYNAME : "" : "");


                    #region "Logo ReplaceMent"
                    try
                    {
                        var headerparagraphs = document.Headers.odd.Paragraphs.Where(x => x.Text.Contains("&&LOGO"));
                        if (headerparagraphs != null)
                        {
                            foreach (var paragraph in headerparagraphs)
                            {
                                paragraph.ReplaceText("&&LOGO", "");
                                #region Replace Logo
                                string logourl;
                                if (!string.IsNullOrEmpty(refViewModel.Company.UPLOAD_PROFILE))
                                {
                                    logourl = "Uploads/Company/Logo/" + refViewModel.Company.UPLOAD_PROFILE;
                                }
                                else
                                {
                                    logourl = "app_development/images/logo.png";
                                }
                                string newpath = Path.Combine(filePath, logourl);

                                System.IO.FileInfo newlogofile = new System.IO.FileInfo(newpath);
                                if (newlogofile.Exists)
                                {
                                    Novacode.Image img = document.AddImage(newpath);
                                    // Insert a Paragraph into the default Header.
                                    Novacode.Header header_default = document.Headers.odd;
                                    Novacode.Picture pic1 = img.CreatePicture();
                                    pic1.Width = 100;
                                    pic1.Height = 50;
                                    header_default.Pictures.Add(pic1);

                                    Novacode.Paragraph p1 = header_default.InsertParagraph();
                                    //paragraph.Direction = Novacode.Direction.RightToLeft;
                                    paragraph.AppendPicture(pic1);
                                }

                                #endregion
                            }
                        }
                        else
                        {
                            return "headerparagraphs not found";
                        }



                    }


                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                    #endregion

                    //Add Employee
                    int count = 0;
                    foreach (var assinedemployee in refViewModel.AssesmentEmployees)
                    {
                        if (assinedemployee.Employee == null)
                            assinedemployee.Employee = await ClearConnection.GetPersonByPersonId(assinedemployee.EMPLOYEE_ID);
                        count++;
                        var nameparagraphs = document.Paragraphs.Where(x => x.Text.Contains("&&Employee" + count.ToString() + "_Name"));
                        if (nameparagraphs != null)
                        {
                            document.ReplaceText("&&Employee" + count.ToString() + "_Name", assinedemployee.Employee.FullName);
                        }
                        var singparagraphs = document.Paragraphs.Where(x => x.Text.Contains("&&Emp" + count.ToString() + "_Signature"));
                        if (singparagraphs != null)
                        {
                            document.ReplaceText("&&Emp" + count.ToString() + "_Signature", "&&" + assinedemployee.Employee.FIRST_NAME.Trim() + "_" + assinedemployee.Employee.LAST_NAME.Trim() + "_" + assinedemployee.Employee.PERSON_ID.ToString().Trim());
                        }
                        var dateparagraphs = document.Paragraphs.Where(x => x.Text.Contains("&&Signature" + count.ToString() + "_Date"));
                        if (dateparagraphs != null)
                        {
                            document.ReplaceText("&&Signature" + count.ToString() + "_Date", "&&Sign" + assinedemployee.Employee.FIRST_NAME.Trim() + "_" + assinedemployee.Employee.LAST_NAME.Trim() + "_date" + "_" + assinedemployee.Employee.PERSON_ID.ToString().Trim());
                        }
                    }

                    //SWMS TEMPLATE STEP
                    if (item.SwmsTemplate.SwmsTemplatesteps != null)
                    {
                        IList<SwmsTemplatestep> Swmssteps = item.SwmsTemplate.SwmsTemplatesteps.ToList();
                        var tableSetupparagraphs = document.Paragraphs.Where(x => x.Text.Contains("&&Steps-table"));
                        var t = document.AddTable(item.SwmsTemplate.SwmsTemplatesteps.Count + 2, 12);
                        //t.Design = TableDesign.LightGrid;

                        t.AutoFit = AutoFit.Contents;
                        t.Alignment = Alignment.center;
                        Novacode.Border b = new Novacode.Border(Novacode.BorderStyle.Tcbs_single, BorderSize.two, 0, Color.Black);
                        t.SetBorder(TableBorderType.Top, b);
                        t.SetBorder(TableBorderType.Bottom, b);
                        t.SetBorder(TableBorderType.Left, b);
                        t.SetBorder(TableBorderType.Right, b);
                        t.Rows[0].Cells[0].Paragraphs[0].Append("Item").FontSize(10);
                        t.Rows[0].Cells[0].Width = 10d;
                        t.Rows[0].Cells[0].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[0].Cells[0].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[0].Cells[0].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[0].Cells[0].SetBorder(TableCellBorderType.Bottom, b);
                        t.Rows[0].Cells[0].FillColor = Color.SlateGray;

                        t.Rows[0].Cells[1].Paragraphs[0].Append("Task & or Category of Hazard(Delete & Add items that are / not relevant)").FontSize(10);
                        t.Rows[0].Cells[1].Width = 100d;
                        t.Rows[0].Cells[1].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[0].Cells[1].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[0].Cells[1].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[0].Cells[1].SetBorder(TableCellBorderType.Bottom, b);
                        t.Rows[0].Cells[1].FillColor = Color.SlateGray;

                        t.Rows[0].Cells[2].Paragraphs[0].Append("What are the Specific Hazards?").FontSize(10);
                        t.Rows[0].Cells[2].Width = 100d;
                        t.Rows[0].Cells[2].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[0].Cells[2].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[0].Cells[2].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[0].Cells[2].SetBorder(TableCellBorderType.Bottom, b);
                        t.Rows[0].Cells[2].FillColor = Color.SlateGray;

                        //t.Rows[0].Cells[3].Paragraphs[0].Append("Impact:Health And Safety Environment Community Operations notification and approval must be obtained.Care taken on unsealed roads and property.").FontSize(10);
                        t.Rows[0].Cells[3].Paragraphs[0].Append("Area of Impact").FontSize(10);
                        t.Rows[0].Cells[3].Width = 100d;
                        t.Rows[0].Cells[3].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[0].Cells[3].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[0].Cells[3].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[0].Cells[3].SetBorder(TableCellBorderType.Bottom, b);
                        t.Rows[0].Cells[3].FillColor = Color.SlateGray;

                        t.Rows[0].Cells[4].Paragraphs[0].Append("Risk").FontSize(10);
                        t.Rows[0].Cells[4].Width = 50d;
                        //t.Rows[0].Cells[4].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[0].Cells[4].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[0].Cells[4].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[0].Cells[4].SetBorder(TableCellBorderType.Bottom, b);
                        t.Rows[0].Cells[4].FillColor = Color.SlateGray;

                        t.Rows[0].Cells[5].Paragraphs[0].Append("Before").FontSize(10);
                        t.Rows[0].Cells[5].Width = 50d;
                        //t.Rows[0].Cells[5].SetBorder(TableCellBorderType.Right, b);
                        // t.Rows[0].Cells[5].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[0].Cells[5].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[0].Cells[5].SetBorder(TableCellBorderType.Bottom, b);
                        t.Rows[0].Cells[5].FillColor = Color.SlateGray;

                        t.Rows[0].Cells[6].Paragraphs[0].Append("Controls").FontSize(10);
                        t.Rows[0].Cells[6].Width = 50d;
                        t.Rows[0].Cells[6].SetBorder(TableCellBorderType.Right, b);
                        //t.Rows[0].Cells[6].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[0].Cells[6].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[0].Cells[6].SetBorder(TableCellBorderType.Bottom, b);
                        t.Rows[0].Cells[6].FillColor = Color.SlateGray;

                        //t.Rows[0].Cells[7].Paragraphs[0].Append("Correct vehicles driven to site.Before any person enters site, whether it is to visit or to work,").FontSize(10);
                        t.Rows[0].Cells[7].Paragraphs[0].Append("Methods of Controlling Hazards").FontSize(10);

                        t.Rows[0].Cells[7].Width = 100d;
                        t.Rows[0].Cells[7].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[0].Cells[7].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[0].Cells[7].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[0].Cells[7].SetBorder(TableCellBorderType.Bottom, b);
                        t.Rows[0].Cells[7].FillColor = Color.SlateGray;

                        t.Rows[0].Cells[8].Paragraphs[0].Append("Risk").FontSize(10);
                        t.Rows[0].Cells[8].Width = 50d;
                        // t.Rows[0].Cells[8].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[0].Cells[8].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[0].Cells[8].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[0].Cells[8].SetBorder(TableCellBorderType.Bottom, b);
                        t.Rows[0].Cells[8].FillColor = Color.SlateGray;

                        t.Rows[0].Cells[9].Paragraphs[0].Append("After").FontSize(10);
                        t.Rows[0].Cells[9].Width = 50d;
                        // t.Rows[0].Cells[9].SetBorder(TableCellBorderType.Right, b);
                        // t.Rows[0].Cells[9].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[0].Cells[9].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[0].Cells[9].SetBorder(TableCellBorderType.Bottom, b);
                        t.Rows[0].Cells[9].FillColor = Color.SlateGray;

                        t.Rows[0].Cells[10].Paragraphs[0].Append("Controls").FontSize(10);
                        t.Rows[0].Cells[10].Width = 50d;
                        t.Rows[0].Cells[10].SetBorder(TableCellBorderType.Right, b);
                        // t.Rows[0].Cells[10].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[0].Cells[10].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[0].Cells[10].SetBorder(TableCellBorderType.Bottom, b);
                        t.Rows[0].Cells[10].FillColor = Color.SlateGray;

                        t.Rows[0].Cells[11].Paragraphs[0].Append("Who is responsible").FontSize(10);
                        t.Rows[0].Cells[11].Width = 80d;
                        t.Rows[0].Cells[11].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[0].Cells[11].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[0].Cells[11].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[0].Cells[11].SetBorder(TableCellBorderType.Bottom, b);
                        t.Rows[0].Cells[11].FillColor = Color.SlateGray;


                        t.Rows[1].Cells[0].Paragraphs[0].Append("").FontSize(10);
                        // t.Rows[1].Cells[0].Width = 20d;
                        t.Rows[1].Cells[0].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[1].Cells[0].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[1].Cells[0].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[1].Cells[0].SetBorder(TableCellBorderType.Bottom, b);

                        t.Rows[1].Cells[1].Paragraphs[0].Append("").FontSize(10);
                        // t.Rows[1].Cells[1].Width = 100d;
                        t.Rows[1].Cells[1].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[1].Cells[1].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[1].Cells[1].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[1].Cells[1].SetBorder(TableCellBorderType.Bottom, b);

                        t.Rows[1].Cells[2].Paragraphs[0].Append("").FontSize(10);
                        //t.Rows[1].Cells[2].Width = 100d;
                        t.Rows[1].Cells[2].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[1].Cells[2].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[1].Cells[2].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[1].Cells[2].SetBorder(TableCellBorderType.Bottom, b);

                        t.Rows[1].Cells[3].Paragraphs[0].Append("").FontSize(10);
                        //t.Rows[1].Cells[3].Width = 100d;
                        t.Rows[1].Cells[3].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[1].Cells[3].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[1].Cells[3].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[1].Cells[3].SetBorder(TableCellBorderType.Bottom, b);

                        t.Rows[1].Cells[4].Paragraphs[0].Append("L").FontSize(10);
                        //  t.Rows[1].Cells[4].Width = 50d;
                        t.Rows[1].Cells[4].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[1].Cells[4].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[1].Cells[4].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[1].Cells[4].SetBorder(TableCellBorderType.Bottom, b);
                        t.Rows[1].Cells[4].FillColor = Color.SlateGray;

                        t.Rows[1].Cells[5].Paragraphs[0].Append("C").FontSize(10);
                        // t.Rows[1].Cells[5].Width = 50d;
                        t.Rows[1].Cells[5].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[1].Cells[5].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[1].Cells[5].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[1].Cells[5].SetBorder(TableCellBorderType.Bottom, b);
                        t.Rows[1].Cells[5].FillColor = Color.SlateGray;

                        t.Rows[1].Cells[6].Paragraphs[0].Append("S").FontSize(10);
                        // t.Rows[1].Cells[6].Width = 50d;
                        t.Rows[1].Cells[6].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[1].Cells[6].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[1].Cells[6].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[1].Cells[6].SetBorder(TableCellBorderType.Bottom, b);
                        t.Rows[1].Cells[6].FillColor = Color.SlateGray;
                        t.Rows[1].Cells[7].Paragraphs[0].Append("").FontSize(10);
                        // t.Rows[1].Cells[7].Width = 100d;
                        t.Rows[1].Cells[7].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[1].Cells[7].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[1].Cells[7].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[1].Cells[7].SetBorder(TableCellBorderType.Bottom, b);
                        t.Rows[1].Cells[8].Paragraphs[0].Append("L").FontSize(10);
                        // t.Rows[1].Cells[8].Width = 50d;
                        t.Rows[1].Cells[8].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[1].Cells[8].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[1].Cells[8].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[1].Cells[8].SetBorder(TableCellBorderType.Bottom, b);
                        t.Rows[1].Cells[8].FillColor = Color.SlateGray;

                        t.Rows[1].Cells[9].Paragraphs[0].Append("C").FontSize(10);
                        // t.Rows[1].Cells[9].Width = 50d;
                        t.Rows[1].Cells[9].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[1].Cells[9].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[1].Cells[9].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[1].Cells[9].SetBorder(TableCellBorderType.Bottom, b);
                        t.Rows[1].Cells[9].FillColor = Color.SlateGray;

                        t.Rows[1].Cells[10].Paragraphs[0].Append("S").FontSize(10);
                        // t.Rows[1].Cells[10].Width = 50d;
                        t.Rows[1].Cells[10].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[1].Cells[10].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[1].Cells[10].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[1].Cells[10].SetBorder(TableCellBorderType.Bottom, b);
                        t.Rows[1].Cells[10].FillColor = Color.SlateGray;
                        t.Rows[1].Cells[11].Paragraphs[0].Append("").FontSize(10);
                        // t.Rows[1].Cells[11].Width = 80d;
                        t.Rows[1].Cells[11].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[1].Cells[11].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[1].Cells[11].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[1].Cells[11].SetBorder(TableCellBorderType.Bottom, b);

                        t.MergeCellsInColumn(0, 0, 1);
                        t.MergeCellsInColumn(1, 0, 1);
                        t.MergeCellsInColumn(2, 0, 1);
                        t.MergeCellsInColumn(3, 0, 1);
                        t.MergeCellsInColumn(7, 0, 1);
                        t.MergeCellsInColumn(11, 0, 1);

                        for (int i = 0; i < Swmssteps.Count; i++)
                        {
                            for (int j = 0; j < 12; j++)
                            {
                                try
                                {
                                    int currentitem = i + 1;
                                    if (j == 0)
                                        t.Rows[i + 2].Cells[j].Paragraphs[0].Append(Swmssteps[i].STEPNO.ToString()).FontSize(11);
                                    if (j == 1)
                                        t.Rows[i + 2].Cells[j].Paragraphs[0].Append(Swmssteps[i].TASKCATEGEORY).FontSize(11);
                                    if (j == 2)
                                        t.Rows[i + 2].Cells[j].Paragraphs[0].Append(Swmssteps[i].HAZARD).FontSize(11);
                                    if (j == 3)
                                        t.Rows[i + 2].Cells[j].Paragraphs[0].Append(Swmssteps[i].HEALTHIMPACT).FontSize(11);
                                    if (j == 4)
                                        t.Rows[i + 2].Cells[j].Paragraphs[0].Append(Swmssteps[i].RiskLikelyhood.NAME.ToString()).FontSize(11);
                                    if (j == 5)
                                        t.Rows[i + 2].Cells[j].Paragraphs[0].Append(Swmssteps[i].RISK_CONTRL_SCORE.ToString()).FontSize(11);
                                    if (j == 6)
                                    {
                                        t.Rows[i + 2].Cells[j].Paragraphs[0].Append(Swmssteps[i].RiskLikelyhood.NAME.ToString()).FontSize(11);
                                        if (Swmssteps[i].RISK_LIKELYHOOD_ID == 1)
                                        {
                                            if (Swmssteps[i].RISK_CONTRL_SCORE == 1)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.YellowGreen;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 2)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.YellowGreen;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 3)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Blue;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 4)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Blue;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 5)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Yellow;
                                            else
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Yellow;
                                        }
                                        else if (Swmssteps[i].RISK_LIKELYHOOD_ID == 2)
                                        {
                                            if (Swmssteps[i].RISK_CONTRL_SCORE == 1)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.YellowGreen;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 2)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Blue;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 3)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Blue;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 4)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Yellow;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 5)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                            else
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                        }
                                        else if (Swmssteps[i].RISK_LIKELYHOOD_ID == 3)
                                        {
                                            if (Swmssteps[i].RISK_CONTRL_SCORE == 1)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Blue;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 2)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Blue;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 3)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Yellow;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 4)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 5)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                            else
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                        }
                                        else if (Swmssteps[i].RISK_LIKELYHOOD_ID == 4)
                                        {
                                            if (Swmssteps[i].RISK_CONTRL_SCORE == 1)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Blue;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 2)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Yellow;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 3)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 4)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 5)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Red;
                                            else
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Red;
                                        }
                                        else if (Swmssteps[i].RISK_LIKELYHOOD_ID == 5)
                                        {
                                            if (Swmssteps[i].RISK_CONTRL_SCORE == 1)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Yellow;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 2)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 3)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 4)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Red;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 5)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Red;
                                            else
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Red;
                                        }
                                        else
                                        {
                                            if (Swmssteps[i].RISK_CONTRL_SCORE == 1)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.YellowGreen;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 2)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Blue;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 3)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Yellow;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 4)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 5)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Red;
                                            else
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Red;
                                        }
                                    }

                                    if (j == 7)
                                        t.Rows[i + 2].Cells[j].Paragraphs[0].Append(Swmssteps[i].CONTROLLINGHAZARDS.ToString()).FontSize(11);
                                    if (j == 8)
                                        t.Rows[i + 2].Cells[j].Paragraphs[0].Append(Swmssteps[i].RiskLikelyhood1.NAME.ToString()).FontSize(11);
                                    if (j == 9)
                                        t.Rows[i + 2].Cells[j].Paragraphs[0].Append(Swmssteps[i].AFTER_RISK_CONTROL_SCORE.ToString()).FontSize(11);
                                    if (j == 10)
                                    {
                                        t.Rows[i + 2].Cells[j].Paragraphs[0].Append(Swmssteps[i].AFTER_RISK_CONTROL_SCORE.ToString()).FontSize(11);
                                        if (Swmssteps[i].AFTER_RISK_CONTROL_SCORE == 1)
                                        {
                                            if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 1)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.YellowGreen;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 2)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.YellowGreen;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 3)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Blue;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 4)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Blue;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 5)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Yellow;
                                            else
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Yellow;
                                        }
                                        else if (Swmssteps[i].AFTER_RISK_CONTROL_SCORE == 2)
                                        {
                                            if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 1)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.YellowGreen;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 2)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Blue;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 3)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Blue;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 4)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Yellow;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 5)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                            else
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                        }
                                        else if (Swmssteps[i].AFTER_RISK_CONTROL_SCORE == 3)
                                        {
                                            if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 1)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Blue;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 2)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Blue;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 3)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Yellow;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 4)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 5)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                            else
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                        }
                                        else if (Swmssteps[i].AFTER_RISK_CONTROL_SCORE == 4)
                                        {
                                            if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 1)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Blue;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 2)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Yellow;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 3)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 4)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 5)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Red;
                                            else
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Red;
                                        }
                                        else if (Swmssteps[i].AFTER_RISK_CONTROL_SCORE == 4)
                                        {
                                            if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 1)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Yellow;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 2)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 3)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 4)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Red;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 5)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Red;
                                            else
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Red;
                                        }
                                        else
                                        {
                                            if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 1)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.YellowGreen;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 2)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Blue;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 3)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Yellow;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 4)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 5)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Red;
                                            else
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Red;
                                        }
                                    }
                                    if (j == 11)
                                        t.Rows[i + 2].Cells[j].Paragraphs[0].Append(Swmssteps[i].ResposnsibleType.NAME.ToString()).FontSize(11);


                                    t.Rows[i + 2].Cells[j].SetBorder(TableCellBorderType.Right, b);
                                    t.Rows[i + 2].Cells[j].SetBorder(TableCellBorderType.Left, b);
                                    t.Rows[i + 2].Cells[j].SetBorder(TableCellBorderType.Top, b);
                                    t.Rows[i + 2].Cells[j].SetBorder(TableCellBorderType.Bottom, b);
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }
                        }

                        t.SetWidths(new float[] { 40, 200, 150, 100, 40, 55, 65, 120, 40, 45, 65, 120 });
                        if (tableSetupparagraphs != null)
                        {
                            foreach (var pra in tableSetupparagraphs)
                            {
                                pra.ReplaceText("&&Steps-table", "Steps-table Section");
                                pra.SpacingAfter(20d);
                                pra.InsertTableAfterSelf(t);
                            }
                        }
                        else
                        {
                            return "tableSetupparagraphs Not Found";
                        }
                    }

                    //PPE Section

                    try
                    {
                        var ppeparagraphs = document.Paragraphs.Where(x => x.Text.Contains("&&PPESection"));
                        if (ppeparagraphs != null)
                        {
                            IList<SwmsPperequired> SWMSPPERequired = item.SwmsTemplate.SwmsPperequireds.ToList();
                            if (SWMSPPERequired != null)
                            {
                                Novacode.Border b = new Novacode.Border(Novacode.BorderStyle.Tcbs_single, BorderSize.two, 0, Color.Black);
                                var ppe = document.AddTable(SWMSPPERequired.Count + 1, 4);
                                ppe.Alignment = Alignment.center;
                                ppe.SetBorder(TableBorderType.Top, b);
                                ppe.SetBorder(TableBorderType.Bottom, b);
                                ppe.SetBorder(TableBorderType.Left, b);
                                ppe.SetBorder(TableBorderType.Right, b);
                                ppe.Rows[0].Cells[0].Paragraphs[0].Append("PPE Required").FontSize(10);
                                //ppe.Rows[0].Cells[0].Width = 350d;
                                //ppe.Rows[0].Cells[0].SetBorder(TableCellBorderType.Right, b);
                                ppe.Rows[0].Cells[0].SetBorder(TableCellBorderType.Left, b);
                                ppe.Rows[0].Cells[0].SetBorder(TableCellBorderType.Top, b);
                                ppe.Rows[0].Cells[0].SetBorder(TableCellBorderType.Bottom, b);
                                ppe.Rows[0].Cells[0].FillColor = Color.SlateGray;
                                ppe.Rows[0].Cells[1].Paragraphs[0].Append("").FontSize(11);
                                // ppe.Rows[0].Cells[1].Width = 50d;
                                ppe.Rows[0].Cells[1].SetBorder(TableCellBorderType.Right, b);
                                //ppe.Rows[0].Cells[1].SetBorder(TableCellBorderType.Left, b);
                                ppe.Rows[0].Cells[1].SetBorder(TableCellBorderType.Top, b);
                                ppe.Rows[0].Cells[1].SetBorder(TableCellBorderType.Bottom, b);
                                ppe.Rows[0].Cells[1].FillColor = Color.SlateGray;


                                ppe.Rows[0].Cells[2].Paragraphs[0].Append("PPE Required").FontSize(10);
                                ppe.Rows[0].Cells[2].Width = 350d;
                                //ppe.Rows[0].Cells[2].SetBorder(TableCellBorderType.Right, b);
                                ppe.Rows[0].Cells[2].SetBorder(TableCellBorderType.Left, b);
                                ppe.Rows[0].Cells[2].SetBorder(TableCellBorderType.Top, b);
                                ppe.Rows[0].Cells[2].SetBorder(TableCellBorderType.Bottom, b);
                                ppe.Rows[0].Cells[2].FillColor = Color.SlateGray;
                                ppe.Rows[0].Cells[3].Paragraphs[0].Append("").FontSize(11);
                                // ppe.Rows[0].Cells[3].Width = 50d;
                                ppe.Rows[0].Cells[3].SetBorder(TableCellBorderType.Right, b);
                                //ppe.Rows[0].Cells[3].SetBorder(TableCellBorderType.Left, b);
                                ppe.Rows[0].Cells[3].SetBorder(TableCellBorderType.Top, b);
                                ppe.Rows[0].Cells[3].SetBorder(TableCellBorderType.Bottom, b);
                                ppe.Rows[0].Cells[3].FillColor = Color.SlateGray;

                                var totalcoun = SWMSPPERequired.Count;
                                int k = 1;
                                for (int j = 0; j < SWMSPPERequired.Count;)
                                {
                                    var modulus = SWMSPPERequired.Count % 2;
                                    if (modulus == 0)
                                    {
                                        ppe.Rows[k].Cells[0].Paragraphs[0].Append(SWMSPPERequired[j].Ppevalue.KEY_DISPLAY).FontSize(10);
                                        //ppe.Rows[k].Cells[0].Width = 350d;
                                        ppe.Rows[k].Cells[0].SetBorder(TableCellBorderType.Right, b);
                                        ppe.Rows[k].Cells[0].SetBorder(TableCellBorderType.Left, b);
                                        ppe.Rows[k].Cells[0].SetBorder(TableCellBorderType.Left, b);
                                        ppe.Rows[k].Cells[0].SetBorder(TableCellBorderType.Bottom, b);
                                        //string pic1url = string.Empty;
                                        // var SWMSPPEImageIcon = await this.Storage.GetRepository<IPPEValueRepository>().GetByName(SWMSPPERequired[j].PPERequiredName);

                                        string newpath = Path.Combine(filePath, "PPEImageIcon/" + SWMSPPERequired[j].Ppevalue.ICONPATH);
                                        System.IO.FileInfo newlogofile = new System.IO.FileInfo(newpath);
                                        Novacode.Image img = document.AddImage(newpath);
                                        Novacode.Picture pic1 = img.CreatePicture();
                                        ppe.Rows[k].Cells[1].Paragraphs[0].AppendPicture(pic1).FontSize(11);
                                        // ppe.Rows[k].Cells[1].Width = 50d;
                                        ppe.Rows[k].Cells[1].SetBorder(TableCellBorderType.Right, b);
                                        ppe.Rows[k].Cells[1].SetBorder(TableCellBorderType.Left, b);
                                        ppe.Rows[k].Cells[1].SetBorder(TableCellBorderType.Top, b);
                                        ppe.Rows[k].Cells[1].SetBorder(TableCellBorderType.Bottom, b);

                                        ppe.Rows[k].Cells[2].Paragraphs[0].Append(SWMSPPERequired[j + 1].Ppevalue.KEY_DISPLAY).FontSize(10);
                                        // ppe.Rows[k].Cells[2].Width = 350d;
                                        ppe.Rows[k].Cells[2].SetBorder(TableCellBorderType.Right, b);
                                        ppe.Rows[k].Cells[2].SetBorder(TableCellBorderType.Left, b);
                                        ppe.Rows[k].Cells[2].SetBorder(TableCellBorderType.Left, b);
                                        ppe.Rows[k].Cells[2].SetBorder(TableCellBorderType.Bottom, b);
                                        string pic1ur2 = string.Empty;

                                        string newpath2 = Path.Combine(filePath, "PPEImageIcon/" + SWMSPPERequired[j + 1].Ppevalue.ICONPATH);
                                        System.IO.FileInfo newlogofile2 = new System.IO.FileInfo(newpath2);
                                        Novacode.Image img2 = document.AddImage(newpath2);
                                        Novacode.Picture pic2 = img2.CreatePicture();

                                        ppe.Rows[k].Cells[3].Paragraphs[0].AppendPicture(pic2).FontSize(11);
                                        // ppe.Rows[k].Cells[3].Width = 50d;
                                        ppe.Rows[k].Cells[3].SetBorder(TableCellBorderType.Right, b);
                                        ppe.Rows[k].Cells[3].SetBorder(TableCellBorderType.Left, b);
                                        ppe.Rows[k].Cells[3].SetBorder(TableCellBorderType.Top, b);
                                        ppe.Rows[k].Cells[3].SetBorder(TableCellBorderType.Bottom, b);



                                    }
                                    else
                                    {
                                        if (totalcoun == 1)
                                        {

                                            ppe.Rows[k].Cells[0].Paragraphs[0].Append(SWMSPPERequired[j].Ppevalue.KEY_DISPLAY).FontSize(11);
                                            // ppe.Rows[k].Cells[0].Width = 350d;
                                            ppe.Rows[k].Cells[0].SetBorder(TableCellBorderType.Right, b);
                                            ppe.Rows[k].Cells[0].SetBorder(TableCellBorderType.Left, b);
                                            ppe.Rows[k].Cells[0].SetBorder(TableCellBorderType.Left, b);
                                            ppe.Rows[k].Cells[0].SetBorder(TableCellBorderType.Bottom, b);
                                            string pic1url = string.Empty;

                                            string newpath = Path.Combine(filePath, "PPEImageIcon/" + SWMSPPERequired[j].Ppevalue.ICONPATH);
                                            System.IO.FileInfo newlogofile = new System.IO.FileInfo(newpath);
                                            Novacode.Image img = document.AddImage(newpath);
                                            Novacode.Picture pic1 = img.CreatePicture();
                                            ppe.Rows[k].Cells[1].Paragraphs[0].AppendPicture(pic1).FontSize(10);
                                            // ppe.Rows[k].Cells[1].Width = 50d;
                                            ppe.Rows[k].Cells[1].SetBorder(TableCellBorderType.Right, b);
                                            ppe.Rows[k].Cells[1].SetBorder(TableCellBorderType.Left, b);
                                            ppe.Rows[k].Cells[1].SetBorder(TableCellBorderType.Top, b);
                                            ppe.Rows[k].Cells[1].SetBorder(TableCellBorderType.Bottom, b);
                                        }
                                        else
                                        {
                                            ppe.Rows[k].Cells[0].Paragraphs[0].Append(SWMSPPERequired[j].Ppevalue.KEY_DISPLAY).FontSize(11);

                                            ppe.Rows[k].Cells[0].SetBorder(TableCellBorderType.Right, b);
                                            ppe.Rows[k].Cells[0].SetBorder(TableCellBorderType.Left, b);
                                            ppe.Rows[k].Cells[0].SetBorder(TableCellBorderType.Left, b);
                                            ppe.Rows[k].Cells[0].SetBorder(TableCellBorderType.Bottom, b);
                                            string pic1url = string.Empty;



                                            string newpath = Path.Combine(filePath, "PPEImageIcon/" + SWMSPPERequired[j].Ppevalue.ICONPATH);
                                            System.IO.FileInfo newlogofile = new System.IO.FileInfo(newpath);
                                            Novacode.Image img = document.AddImage(newpath);
                                            Novacode.Picture pic1 = img.CreatePicture();
                                            ppe.Rows[k].Cells[1].Paragraphs[0].AppendPicture(pic1).FontSize(10);
                                            // ppe.Rows[k].Cells[1].Width = 50d;
                                            ppe.Rows[k].Cells[1].SetBorder(TableCellBorderType.Right, b);
                                            ppe.Rows[k].Cells[1].SetBorder(TableCellBorderType.Left, b);
                                            ppe.Rows[k].Cells[1].SetBorder(TableCellBorderType.Top, b);
                                            ppe.Rows[k].Cells[1].SetBorder(TableCellBorderType.Bottom, b);

                                            ppe.Rows[k].Cells[2].Paragraphs[0].Append(SWMSPPERequired[k].Ppevalue.KEY_DISPLAY).FontSize(11);
                                            //ppe.Rows[k].Cells[2].Width = 350d;
                                            ppe.Rows[k].Cells[2].SetBorder(TableCellBorderType.Right, b);
                                            ppe.Rows[k].Cells[2].SetBorder(TableCellBorderType.Left, b);
                                            ppe.Rows[k].Cells[2].SetBorder(TableCellBorderType.Left, b);
                                            ppe.Rows[k].Cells[2].SetBorder(TableCellBorderType.Bottom, b);


                                            string pic1ur2 = string.Empty;


                                            string newpath2 = Path.Combine(filePath, "PPEImageIcon/" + SWMSPPERequired[j + 1].Ppevalue.ICONPATH);
                                            System.IO.FileInfo newlogofile2 = new System.IO.FileInfo(newpath2);
                                            Novacode.Image img2 = document.AddImage(newpath2);
                                            Novacode.Picture pic2 = img2.CreatePicture();

                                            ppe.Rows[k].Cells[3].Paragraphs[0].AppendPicture(pic2).FontSize(10);
                                            // ppe.Rows[k].Cells[3].Width = 50d;
                                            ppe.Rows[k].Cells[3].SetBorder(TableCellBorderType.Right, b);
                                            ppe.Rows[k].Cells[3].SetBorder(TableCellBorderType.Left, b);
                                            ppe.Rows[k].Cells[3].SetBorder(TableCellBorderType.Top, b);
                                            ppe.Rows[k].Cells[3].SetBorder(TableCellBorderType.Bottom, b);
                                        }
                                    }
                                    j = j + 2;
                                    totalcoun = totalcoun - 2;
                                    k++;
                                }
                                ppe.SetWidths(new float[] { 427, 100, 428, 100 });
                                foreach (var prappe in ppeparagraphs)
                                {
                                    prappe.ReplaceText("&&PPESection", "PPE SECTION");
                                    prappe.SpacingAfter(20d);
                                    prappe.InsertTableAfterSelf(ppe);
                                }
                            }
                        }
                        else
                        {
                            return "ppeparagraphs not found";
                        }

                    }
                    catch (Exception ex)
                    {
                        return ex.Message;

                    }

                    //Plant Euipment

                    try
                    {
                        var plantequipment = document.Paragraphs.Where(x => x.Text.Contains("&&Plant&Equipment"));
                        if (plantequipment != null)
                        {
                            string plantandreq = string.Empty;

                            if (item.SwmsTemplate.SwmsPlantequipments != null)
                            {
                                foreach (var plnt in item.SwmsTemplate.SwmsPlantequipments)
                                {
                                    plantandreq += plnt.PlantEquipment.NAME + ",";
                                }
                                foreach (var pra in plantequipment)
                                {
                                    pra.ReplaceText("&&Plant&Equipment", plantandreq);
                                }
                            }

                        }
                        else
                        {
                            return "plantequipment is Null";
                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;

                    }

                    try
                    {
                        var hazardmaterialspara = document.Paragraphs.Where(x => x.Text.Contains("&&HazardMaterials"));
                        if (hazardmaterialspara != null)
                        {
                            string _hazardousmaterial = string.Empty;

                            if (item.SwmsTemplate.SwmsHazardousmaterials != null)
                            {



                                foreach (var plnt in item.SwmsTemplate.SwmsHazardousmaterials)
                                {
                                    _hazardousmaterial += plnt.HazardMaterialValue.NAME + ",";
                                }
                            }
                            else
                            {
                                return "HazardousMaterials is Null";

                            }


                            foreach (var mpra in hazardmaterialspara)
                            {
                                mpra.ReplaceText("&&HazardMaterials", _hazardousmaterial);
                            }

                        }
                        else
                        {
                            return "hazardmaterialspara is Null";
                        }



                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }

                    try
                    {
                        var referencedlegislationpara = document.Paragraphs.Where(x => x.Text.Contains("&&ReferencedLegislation"));
                        if (referencedlegislationpara != null)
                        {


                            string _referencedlegislation = string.Empty;
                            if (item.SwmsTemplate.SwmsReferencedlegislations != null)
                            {
                                foreach (var plnt in item.SwmsTemplate.SwmsReferencedlegislations)
                                {
                                    _referencedlegislation += plnt.ReferencedLegislation.NAME + ",";
                                }
                            }

                            else
                            {
                                return "SwmsReferencedLegislations not found";
                            }

                            foreach (var rpra in referencedlegislationpara)
                            {
                                rpra.ReplaceText("&&ReferencedLegislation", _referencedlegislation);
                            }
                        }
                        else
                        {
                            return "licensesandpermitspara not found";

                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;

                    }


                    try

                    {
                        var licensesandpermitspara = document.Paragraphs.Where(x => x.Text.Contains("&&LicencedPermitsSection"));
                        if (licensesandpermitspara != null)
                        {

                            string _licensesandpermits = string.Empty;
                            if (item.SwmsTemplate.SwmsLicencespermits != null)
                            {
                                foreach (var plnt in item.SwmsTemplate.SwmsLicencespermits)
                                {
                                    _licensesandpermits += plnt.LicencePermit.NAME + ",";
                                }
                            }
                            else
                            {
                                return "SwmsLicencesPermit not found";
                            }

                            foreach (var pra in licensesandpermitspara)
                            {
                                pra.ReplaceText("&&LicencedPermitsSection", _licensesandpermits);
                            }
                        }
                        else
                        {
                            return "licensesandpermitspara not found";
                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;

                    }


                    try
                    {

                        string docPath = Path.Combine(filePath, "Uploads/Templates/Doc") + $@"\{item.SwmsTemplate.TEMPLATENAME + "-" + item.DOCUMENTTEMPLATEURL}";
                        document.SaveAs(docPath);
                        System.Random rand = new System.Random((int)System.DateTime.Now.Ticks);
                        int randompdf = rand.Next(1, 100000000);
                        string PdfName = refViewModel.REFERENCENUMBER + "-" + refViewModel.RISKASSESSMENTNO + "-" + randompdf + ".pdf";
                        string pdfPath = Path.Combine(filePath, "Uploads/Templates/Pdf") + $@"\{refViewModel.REFERENCENUMBER + "-" + refViewModel.RISKASSESSMENTNO + "-" + randompdf + ".pdf"}";
                        bool isConverted = ConverttoPdf(docPath, pdfPath);
                        if (isConverted)
                        {
                            item.DOCUMENTPDFURL = PdfName;
                            foreach (var employee in refViewModel.AssesmentEmployees)
                            {
                                AssesmentEmployeeAttachement model = new AssesmentEmployeeAttachement();
                                model.ASSESMENT_EMPLOYEE_ID = employee.ASSESMENT_EMPLOYEE_ID;
                                model.ATTACHEMENTID = item.ATTACHEMENTID;
                                model.DOCUMENTNAME = PdfName;
                                model.DOCUMENT_URL = "Uploads/Templates/Pdf";
                                model.ASSIGNED_DATE = DateTime.Now;
                                model.EMPLOYEE_STATUS = 1;

                                if (employee.AssignedEmployees == null)
                                    employee.AssignedEmployees = new List<AssesmentEmployeeAttachement>();

                                employee.AssignedEmployees.Add(model);
                            }

                        }
                        else
                        {
                            return "Document conversion failed";
                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;

                    }

                }
            }

            return string.Empty;
        }


        protected Boolean ConverttoPdf(string input, string output)
        {

            try
            {

                FileStream docStream = new FileStream(input, FileMode.Open);
                WordDocument document = new WordDocument(docStream, Syncfusion.DocIO.FormatType.Automatic);

                DocIORenderer render = new DocIORenderer();
                PdfDocument pdfDocument = render.ConvertToPDF(document);

                FileStream docStream1 = new FileStream(output, FileMode.OpenOrCreate);
                MemoryStream outputStream = new MemoryStream();

                pdfDocument.Save(docStream1);



                pdfDocument.Close();
                render.Dispose();

                document.Dispose();

                return true;

            }
            catch (Exception ex)
            {

                throw ex;


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

        protected async System.Threading.Tasks.Task GridEditButtonClick(MouseEventArgs args, dynamic data)
        {

            var dialogResult = await DialogService.OpenAsync<EditAssesmentSchedule>("Edit Assesment Schedule", new Dictionary<string, object>() { { "ASSESSMENT_SCHEDULE_ID", data.ASSESSMENT_SCHEDULE_ID } });

            await InvokeAsync(() => { StateHasChanged(); });

            var scheduleResults = await ClearConnection.GetAssesmentSchedules(new Query() { Filter = $@"i => i.ASSESMENTID == {ASSESMENTID}" });
            AssesmentSchedules = scheduleResults.Select(x =>
                                    new AssesmentSchedule
                                    {
                                        ASSESSMENT_SCHEDULE_ID = x.ASSESSMENT_SCHEDULE_ID,
                                        ScheduleType = x.ScheduleType,
                                        INTERVAL = x.INTERVAL,
                                        SCHEDULE_TIME = x.SCHEDULE_TIME,
                                        MON = x.MON,
                                        TUE = x.TUE,
                                        WED = x.WED,
                                        THUS = x.THUS,
                                        FRI = x.FRI,
                                        SAT = x.SAT,
                                        SUN = x.SUN
                                    }).ToList();
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

        protected async Task StopAssessment()
        {
            var person = await ClearConnection.GetPersonByPersonId(Security.getUserId());
            assesment.IsScheduleRunning = false;
            if (assesment.ENTITY_STATUS_ID == 5)
                assesment.ENTITY_STATUS_ID = 6;

            assesment.AssessmentActivity += $" Assessment Schedule Stopped By {person.FullName} on {DateTime.Now}.";
            await ClearConnection.UpdateAssesment(assesment.ASSESMENTID, assesment);
            NotificationService.Notify(NotificationSeverity.Success, $"Success", $"Assessment Schedule Stopped Successfully.", 1800000);
            RecurringJob.RemoveIfExists($"Assessment-{ASSESMENTID}");
        }

        protected async Task ChangeLocation(object value, string name)
        {
            var item = getWorkSiteResult.Where(a => a.PERSON_SITE_ID == assesment.WORK_SITE_ID).FirstOrDefault();
            if (item != null)
            {
                assesment.SiteName = item.SITE_NAME;
                assesment.BuildingName = item.BUILDING_NAME;
                assesment.Address1 = item.SITE_ADDRESS1;
                assesment.Address2 = item.SITE_ADDRESS2;
                assesment.City = item.CITY;
                assesment.PostCode = item.POST_CODE;
                assesment.StateId = item.STATE_ID;
                assesment.CountryId = item.COUNTRY_ID;
                assesment.Lat = (double?)item.LATITUDE;
                assesment.Lon = (double?)item.LONGITUDE;
                assesment.RoomNo = item.ROOMNO;
                assesment.Floor = item.FLOOR;
                if (item.LATITUDE == null && item.LONGITUDE == null)
                {
                    await GetGeolocation();
                    item.LATITUDE = (decimal)assesment.Lat;
                    item.LONGITUDE = (decimal)assesment.Lon;

                    await ClearConnection.UpdatePersonSite(item.PERSON_SITE_ID, item);

                }
            }
            StateHasChanged();
        }

        protected async Task ChangeOrder(object value, string name)
        {
            var item = getWorkOrderResult.Where(a => a.WORK_ORDER_ID == assesment.WORK_ORDER_ID).FirstOrDefault();

            if (item != null)
            {
                assesment.WORKORDERNUMBER = item.CLIENT_WORK_ORDER_NUMBER;
                assesment.WORK_SITE_ID = (int)item.WORK_LOCATION_ID;
                assesment.PURCHASEORDER = item.PURCHASE_ORDER_NUMBER;
                assesment.SCOPEOFWORK = item.DESCRIPTION;
                assesment.CONTRACTOR_ID = item.CONTRACTOR_ID;
                assesment.CONTRACTOR_CONTACT_ID = item.CONTRACTOR_CONTACT_ID;
            }
            StateHasChanged();
        }

        protected async Task GetGeolocation()
        {
            string address = assesment.Address1;
            if (!string.IsNullOrEmpty(assesment.Address2))
                address += "," + assesment.Address2;

            if (!string.IsNullOrEmpty(assesment.City))
                address += "," + assesment.City;

            if (!string.IsNullOrEmpty(assesment.PostCode))
                address += "," + assesment.PostCode;

            if (assesment.StateId != null)
            {
                var state = getStatesResult.Where(s => s.ID == assesment.StateId).FirstOrDefault();
                address += "," + state.STATENAME;
            }

            var country = getCountriesResult.Where(s => s.ID == assesment.CountryId).FirstOrDefault();
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
                        assesment.Lat = item.Coordinates.Latitude;
                        assesment.Lon = item.Coordinates.Longitude;
                    }
                    else
                    {
                        assesment.Lat = addresses.First().Coordinates.Latitude;
                        assesment.Lon = addresses.First().Coordinates.Longitude;
                    }

                }
                else
                {
                    assesment.Lat = addresses.First().Coordinates.Latitude;
                    assesment.Lon = addresses.First().Coordinates.Longitude;
                }




            }
            catch (Exception ex)
            {

            }




        }

        protected async Task ChangeState(object value, string name)
        {
            await GetGeolocation();
            StateHasChanged();
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.Template> _getTemplateResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.Template> getTemplateResult
        {
            get
            {
                return _getTemplateResult;
            }
            set
            {
                if (!object.Equals(_getTemplateResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getTemplateResult", NewValue = value, OldValue = _getTemplateResult };
                    _getTemplateResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        #region Assesment Template
        protected RadzenGrid<Clear.Risk.Models.ClearConnection.AssesmentAttachement> grid1;

        protected IList<Clear.Risk.Models.ClearConnection.AssesmentAttachement> AssesmentAttachements = new List<Clear.Risk.Models.ClearConnection.AssesmentAttachement>();

        protected async System.Threading.Tasks.Task AssesmentAttachementDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                var clearConnectionDeleteAssesmentAttachementResult = await ClearConnection.DeleteAssesmentAttachement(data.ATTACHEMENTID);
                if (clearConnectionDeleteAssesmentAttachementResult != null)
                {
                    AssesmentAttachements.Remove(AssesmentAttachements.FirstOrDefault(x => x.ATTACHEMENTID == data.ATTACHEMENTID));
                }
            }
            catch (System.Exception clearConnectionDeleteAssesmentAttachementException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete Assesment");
            }
        }

        #endregion

        #region Assigned Employee
        protected RadzenGrid<Clear.Risk.Models.ClearConnection.AssesmentEmployee> grid2;

        protected IList<Clear.Risk.Models.ClearConnection.AssesmentEmployee> AssesmentEmployees = new List<Clear.Risk.Models.ClearConnection.AssesmentEmployee>();




        protected IList<Clear.Risk.Models.ClearConnection.AssesmentSchedule> AssesmentSchedules;


        #endregion

        #region Risk Assesment

        protected IList<Clear.Risk.Models.ClearConnection.AssesmentEmployeeAttachement> getAssesmentEmployeeAttachementsResult;
        #endregion


        async Task RemoveEmployee(int empID)
        {
            if (await DialogService.Confirm("Are you sure you want to remove this employee.") == true)
            {
                //var item = AssesmentEmployees.FirstOrDefault(o => o.ASSESMENT_EMPLOYEE_ID == empID);

                //item.IS_ACTIVE = false;

                var deletedItem = await ClearConnection.RemoveAssesmentEmployee(empID);

                //UpdateAssesmentEmployee

                if (deletedItem == null)
                {
                    NotificationService.Notify(NotificationSeverity.Error, "Error", "Cannot Remove Employee, please try again later", 15000000);
                    return;
                }

                AssesmentEmployees.FirstOrDefault(x => x.ASSESMENT_EMPLOYEE_ID == empID).IS_ACTIVE = false;

                NotificationService.Notify(NotificationSeverity.Success, "Success", "Employee Removed Successfully.", 15000000);
                StateHasChanged();
            }
        }

        protected async Task ReActivateEmployee(int empid)
        {
            var emp = AssesmentEmployees.FirstOrDefault(i => i.ASSESMENT_EMPLOYEE_ID == empid);
            emp.IS_ACTIVE = true;

            var newItem = await ClearConnection.ActivateAssesmentEmployee(empid);

            if(newItem == null)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Error", "Cannot Reactivate Employee, please try again later", 15000000);
                return;
            }
            
            AssesmentEmployees.FirstOrDefault(i => i.ASSESMENT_EMPLOYEE_ID == empid).IS_ACTIVE = true;
            NotificationService.Notify(NotificationSeverity.Success, "Success", "Employee Activated Successfully.", 15000000);
            StateHasChanged();
        }


        protected async Task AddEmployeeClick()
        {
            var dialogResult = await DialogService.OpenAsync<AddAssessmentEmployee>("Add Assessment Employee", new Dictionary<string, object> { { "AssessmentId", ASSESMENTID.ToString() } });

            var clearConnectionGetAssesmentEmployeesResult = await ClearConnection.GetAssesmentEmployees(new Query() { Filter = $@"i => i.ASSESMENT_ID == {ASSESMENTID}" });

            AssesmentEmployees = clearConnectionGetAssesmentEmployeesResult.Select(x => new Clear.Risk.Models.ClearConnection.AssesmentEmployee
            {
                ASSESMENT_EMPLOYEE_ID = x.ASSESMENT_EMPLOYEE_ID,
                Employee = x.Employee ?? null,
                WarningLevel = x.WarningLevel ?? null,
                FileName = x.FileName,
                SignatureImageUrl = x.SignatureImageUrl,
                Sign_Date = x.Sign_Date,
                VersionNo = x.VersionNo,
                SignedStatus = x.SignedStatus,
                IS_ACTIVE=x.IS_ACTIVE
            }).ToList();
        }

    }
}
