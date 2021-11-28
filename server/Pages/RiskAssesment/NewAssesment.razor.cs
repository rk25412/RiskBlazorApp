using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Radzen;
using Radzen.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Clear.Risk.Infrastructures.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;
using Microsoft.AspNetCore.Hosting;
using Clear.Risk.Models.ClearConnection;
//using Novacode;
//using System.IO;

namespace Clear.Risk.Pages
{
    public partial class NewAssesmentComponent
    {
        Clear.Risk.Models.ClearConnection.Assesment _assesment;

        [Inject]
        public IWebHostEnvironment hosting { get; }
        protected void Change(bool? value, string name)
        {
            if(value != null && value == true)
            {
                this.assesment.MON = true;
                this.assesment.TUE = true;
                this.assesment.THUS = true;
                this.assesment.FRI = true;
                this.assesment.WED = true;
            }
            StateHasChanged();
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
            await Load();

        }

        protected async Task Load()
        {
            int companyId = Security.getCompanyId();
            assesment = new Clear.Risk.Models.ClearConnection.Assesment()
            {
                CREATED_DATE = DateTime.Now,
                UPDATED_DATE = DateTime.Now,
                IS_DELETED = false ,
                ASSESMENTDATE = DateTime.Now,
                RISKASSESSMENTNO = GetFormNumber(),
                WORKSTARTDATE = DateTime.Now,
                WORKENDDATE = DateTime.Now,
                COMPANYID = companyId,
                SCOPEOFWORK = "Read the Covid Risk Assessment and then sign it to indicate you have read it. Complete the 5-minute survey if attached."

            };

             

            var clearConnectionGetPersonSitesResult = await ClearConnection.GetPersonSites(assesment.COMPANYID,new Query());
            getPersonSitesResult = clearConnectionGetPersonSitesResult;

            var clearConnectionGetSurveysResult = await AssesmentConnection.GetSurveys(new Query());
            getSurveysResult = clearConnectionGetSurveysResult;

            var clearConnectionGetScheduleTypesResult = await ClearConnection.GetScheduleTypes(new Query());
            getScheduleTypesResult = clearConnectionGetScheduleTypesResult;

            var clearConnectionGetSWMSResult = await ClearConnection.GetSwmsTemplates(new Query());
            getswmsresults = clearConnectionGetSWMSResult;

            var clearConnectionGetPeopleResult = await ClearConnection.GetPeople(new Query());
            getPeopleResult = clearConnectionGetPeopleResult;


        }

        private   string GetFormNumber()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            var FormNumber = BitConverter.ToUInt32(buffer, 0) ^ BitConverter.ToUInt32(buffer, 4) ^ BitConverter.ToUInt32(buffer, 8) ^ BitConverter.ToUInt32(buffer, 12);
            return FormNumber.ToString("X");

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

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            UriHelper.NavigateTo("manage-assesments");
        }

        protected async System.Threading.Tasks.Task SaveAndRunOnScheduleClick(MouseEventArgs args)
        {
            if(assesment != null)
            {
                if(assesment.WORK_SITE_ID ==0)
                    NotificationService.Notify(NotificationSeverity.Error, $"Work Site should not left blank");

                if(!assesment.ISSCHEDULE)
                    NotificationService.Notify(NotificationSeverity.Error, $"Please Click on Run Now Option to Generate Assessment");
                else
                {
                    if(assesment.SCHEDULE_TYPE_ID == null)
                        NotificationService.Notify(NotificationSeverity.Error, $"Schedule Type should not left blank");

                    if(!assesment.MON && !assesment.TUE && !assesment.WED && !assesment.THUS && !assesment.FRI
                        && !assesment.SAT && !assesment.SUN)
                        NotificationService.Notify(NotificationSeverity.Error, $"Day of Schedule required to select to Run and Generate Assessment");
                }

                if(assesment.SWMSTemplateNames.Count() ==0)
                    NotificationService.Notify(NotificationSeverity.Error, $"Select one or more Templates to Generate Assessment");

                if (assesment.EmployeeNames.Count() == 0)
                    NotificationService.Notify(NotificationSeverity.Error, $"Select one or more Employee/s to Assigned Assessment");

                assesment.CREATED_DATE = DateTime.Now;
                assesment.CREATOR_ID = Security.getUserId();
                assesment.UPDATED_DATE = DateTime.Now;
                assesment.UPDATER_ID = Security.getUserId();
                assesment.STATUS = 1;
                assesment.ENTITY_STATUS_ID = 1;
                assesment.ESCALATION_LEVEL_ID = 1;
                assesment.WARNING_LEVEL_ID = 1;
                assesment.STATUS_LEVEL_ID = 1;



            }
        }

        //private async Task<AssesmentAttachement> CreateAssesmentAttachement(Assesment amodel)
        //{
        //    AssesmentAttachement attachement = null;
        //    foreach (var swmsid in amodel.SWMSTemplateNames)
        //    {
        //        var model = await ClearConnection.GetSwmsBySwmsid(swmsid);

        //        if (model != null && model.Template != null)
        //        {
        //            attachement = new AssesmentAttachement();
        //            attachement.ATTACHEMENTDATE = DateTime.Now;
        //            //amodel.Person

        //            attachement.DOCUMENTPDFURL = "Uploads/Templates/" + model.Template.DOCUMENTURL;
        //            string fullPath = Path.Combine(hosting.WebRootPath, ("Uploads/Templates/" + model.Template.DOCUMENTURL));

        //            Novacode.DocX document = Novacode.DocX.Load(fullPath);
        //            document.ReplaceText("&&Company_ComapnyName", amodel.Person1 != null ? amodel.Person1.COMPANY_NAME  : "N/A");
        //            //document.ReplaceText("&&Contractor_FullName", amodel.Co != null ? persondetails.FullName != null ? persondetails.FullName : "Not Found" : "Not Found");
        //            document.ReplaceText("&&Company_ComapnyPhoneNo", amodel.Person1 != null ? amodel.Person1.BUSINESS_PHONE != null ? amodel.Person1.BUSINESS_PHONE : "Not Found" : "");
        //            document.ReplaceText("&&Assessment_DateCreated", string.Format("{0:d}", DateTime.Now));
        //            document.ReplaceText("&&Assessment_Version", model.VERSION);
        //            //document.ReplaceText("&&Assessment_WorkOrder", model.WorkOrderNumber != null ? model.WorkOrderNumber : "");
        //            document.ReplaceText("&&Date", string.Format("{0:d}", DateTime.Now));
        //           // document.ReplaceText("&&Contractor_PhoneNo", persondetails != null ? persondetails.BusinessMobile != null ? persondetails.BusinessMobile : "Not Found" : "");
        //           // document.ReplaceText("&&Contractor_ComapnyName", persondetails != null ? persondetails.CompanyName != null ? persondetails.CompanyName : "Not Found" : "");
        //           // document.ReplaceText("&&Company_Name", persondetails != null ? persondetails.CompanyName != null ? persondetails.CompanyName : "Not Found" : "");
        //           // document.ReplaceText("&&Customer_Manager", refViewModel.model.ClientName != null ? refViewModel.model.ClientName : "");
        //            document.ReplaceText("&&Scope", amodel.SCOPEOFWORK);
        //            //document.ReplaceText("&&Assessment_ContractorSiteManager", refViewModel.model.ContractorSiteManager != null ? refViewModel.model.ContractorSiteManager : "");
        //            //document.ReplaceText("&&CustomerManager_PhoneNo", refViewModel.model.PhoneMrgNumber != null ? refViewModel.model.PhoneMrgNumber : "");
        //            // document.ReplaceText("&&Logo", model.OrderCompany.CompanyLogoId != null ? model.OrderCompany.CompanyLogoId : "");
        //            //document.ReplaceText("&&Permit_Number", refViewModel.model.WorkOrderNumber != null ? refViewModel.model.WorkOrderNumber : "");


        //            #region Assessment Text Replace
        //            document.ReplaceText("&&AssessmentTask_AssignedDate", string.Format("{0:d}", DateTime.Now));
        //            document.ReplaceText("&&Assessment_AssessmentDate", string.Format("{0:d}", DateTime.Now));
        //            document.ReplaceText("&&Assessment_AssessmentID",  amodel.RISKASSESSMENTNO);
        //            //document.ReplaceText("&&Assessment_ClientID", (refViewModel.model.ClientId.ToString() != null ? refViewModel.model.ClientId.ToString() : "0"));
        //            document.ReplaceText("&&Assessment_CompanyID", amodel.COMPANYID.ToString());
        //           // document.ReplaceText("&&Assessment_ContractorSiteMngrMNo", (refViewModel.model.ContractorSiteMngrMNo.ToString() != null ? refViewModel.model.ContractorSiteMngrMNo.ToString() : "0"));
        //            document.ReplaceText("&&Assessment_CreationTime", string.Format("{0:t}", DateTime.Now));
        //           // document.ReplaceText("&&Assessment_EmployeeID", (refViewModel.model.EmployeeID != null ? refViewModel.model.EmployeeID.ToString() : ""));
        //            document.ReplaceText("&&Assessment_IsCompleted", amodel.ISCOMPLETED ? "Completed" : "Pending");
        //            document.ReplaceText("&&Assessment_OrderLocation", amodel.ORDERLOCATION);
        //            //document.ReplaceText("&&Assessment_PermitNumber", refViewModel.model.PurchaseOrder != null ? refViewModel.model.PurchaseOrder : "");
        //            document.ReplaceText("&&Assessment_PlaceofWorkAddress", amodel.PLACEOFWORKADDRESS);
        //            document.ReplaceText("&&Assessment_ProjectName", amodel.PROJECTNAME);
        //            document.ReplaceText("&&Assessment_PurchaseOrder",  amodel.PURCHASEORDER);
        //            document.ReplaceText("&&Assessment_ReferenceNumber", amodel.REFERENCENUMBER);
        //            document.ReplaceText("&&Assessment_RiskAssessmentNo", amodel.RISKASSESSMENTNO);
        //            document.ReplaceText("&&Assessment_ScopeOfWork", amodel.SCOPEOFWORK);
        //            document.ReplaceText("&&Assessment_TemplateId",  model.SWMSTEMPLATENUMBER);
        //            document.ReplaceText("&&Company_Country", amodel.Person1 != null ? amodel.Person1.Country1 != null ? amodel.Person1.Country1.COUNTRYNAME : "" : "");
        //            //document.ReplaceText("&&Assessment_TypeofAssessment", refViewModel.model.TypeofAssessment != null ? refViewModel.model.TypeofAssessment : "");
        //            document.ReplaceText("&&Assessment_WorkOrderNumber", amodel.WORKORDERNUMBER);
        //           // document.ReplaceText("&&Assessment_PrincipalContractor", refViewModel.model.PrincipalContractor != null ? refViewModel.model.PrincipalContractor : "");
        //           // document.ReplaceText("&&Assessment_WorkingContractor", refViewModel.model.WorkingContractor != null ? refViewModel.model.WorkingContractor : "");
        //            document.ReplaceText("&&Assessment_WorkStartDate", string.Format("{0:d}", amodel.WORKSTARTDATE));
        //            document.ReplaceText("&&Assessment_WorkEndDate", string.Format("{0:d}", amodel.WORKENDDATE));
        //            #endregion

        //        }
        //    }
           

        //    return attachement;
        //}

    }
}
