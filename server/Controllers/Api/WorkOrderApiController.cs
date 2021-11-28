using System;
using System.Linq;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Clear.Risk.Models;
using Clear.Risk.Authentication;
using Clear.Risk.ViewModels;
using Clear.Risk;
using Clear.Risk.Data;
using Clear.Risk.Models.ClearConnection;
using Radzen;
using Radzen.Blazor;
using Clear.Risk.Infrastructures.Helpers;
using System.Net;

using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;
using Syncfusion.Pdf;

using System.Runtime.InteropServices;
using Novacode;
using System.Drawing;
using System.IO;
//using Coravel;
//using Coravel.Scheduling;
//using Coravel.Scheduling.Schedule.Interfaces;
using Hangfire;

namespace Clear.Risk.Controllers.Api
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Produces("application/json")]
    [Route("api/Order")]
    [ApiController]
    public class WorkOrderApiController : ControllerBase
    {
        private readonly AssesmentService _aservice;
        private readonly WorkOrderService _wService;
        private readonly ClearConnectionService _clearService;
        private readonly SecurityService security;
        private readonly IWebHostEnvironment _hosting;
        private readonly ChangeTransactionStatus changeTransactionStatus;
        //private readonly Coravel.Scheduling.Schedule.Interfaces.IScheduler _schedule;
        private readonly SurveyServices _surveyservice;
        public WorkOrderApiController(AssesmentService aservice, WorkOrderService wService,
            ClearConnectionService clearService, SecurityService securityService,
            IWebHostEnvironment webHostEnvironment, /*Coravel.Scheduling.Schedule.Interfaces.IScheduler schedule,*/
            SurveyServices surveyservice, ChangeTransactionStatus transactionStatus)
        {
            _aservice = aservice;
            _wService = wService;
            _clearService = clearService;
            security = securityService;
            _hosting = webHostEnvironment;
            //_schedule = schedule;
            _surveyservice = surveyservice;
            changeTransactionStatus = transactionStatus;
        }



        [Route("GetActiveWorkOrders")]
        [HttpGet]
        public async Task<IActionResult> GetActiveWorkOrder()
        {
            var userId = User.Identity.GetUserId();
            if (userId > 0)
            {
                var items = await _wService.GetWorkOrders(new Radzen.Query() { Filter = $@"i => (i.STATUS_ID == 3 || i.STATUS_ID == 10) && i.WorkerPeople.Any(r =>  r.PERSON_ID == {userId})", Expand = $"Assessments" });
                var rowsCount = items.Count();

                var model = from record in items
                            where
       (record.Assessments/*.Where(i => i.WORKSTARTDATE <= DateTime.Now.Date && i.WORKENDDATE >= DateTime.Now.Date)*/
           .FirstOrDefault(i => i.ISSCHEDULE == false).ISSCHEDULE == false)
                            select new
                            {
                                WorkOrderId = record.WORK_ORDER_ID,
                                WorkOrderNo = record.WORK_ORDER_NUMBER,
                                ClientWorkOrder = record.CLIENT_WORK_ORDER_NUMBER,
                                ClientName = record.Client != null ? record.Client.COMPANY_NAME : string.Empty,
                                ClientContact = record.ClientContact != null ? record.ClientContact.FullName : string.Empty,
                                StatusId = record.STATUS_ID,
                                Status = record.OrderStatus != null ? record.OrderStatus.NAME : string.Empty,
                                OrderTypeId = record.WORK_ORDER_TYPE,
                                OrderType = record.WorkOrderType != null ? record.WorkOrderType.NAME : string.Empty,
                                Priority = record.PriorityMaster != null ? record.PriorityMaster.NAME : string.Empty,
                                WorkScope = record.DESCRIPTION,
                                WorkOrderDate = record.DATE_RAISED,
                                DueDate = record.DUE_DATE,
                                ProcessType = record.ProcessType != null ? record.ProcessType.NAME : string.Empty,
                                Contractor = record.Contractor != null ? record.Contractor.COMPANY_NAME : string.Empty,
                                ContractorContact = record.ContractorContact != null ? record.ContractorContact.FullName : string.Empty,
                                SiteName = record.WorkLocation != null ? record.WorkLocation.SITE_NAME : string.Empty,
                                BuildingName = record.WorkLocation != null ? record.WorkLocation.BUILDING_NAME : string.Empty,
                                Floor = record.WorkLocation != null ? record.WorkLocation.FLOOR : string.Empty,
                                Room = record.WorkLocation != null ? record.WorkLocation.ROOMNO : string.Empty,
                                Address1 = record.WorkLocation != null ? record.WorkLocation.SITE_ADDRESS1 : string.Empty,
                                Address2 = record.WorkLocation != null ? record.WorkLocation.SITE_ADDRESS2 : string.Empty,
                                City = record.WorkLocation != null ? record.WorkLocation.CITY : string.Empty,
                                PostCode = record.WorkLocation != null ? record.WorkLocation.POST_CODE : string.Empty,
                                Country = record.WorkLocation != null ? record.WorkLocation.Country != null ? record.WorkLocation.Country.COUNTRYNAME : string.Empty : string.Empty,
                                State = record.WorkLocation != null ? record.WorkLocation.State != null ? record.WorkLocation.State.STATENAME : string.Empty : string.Empty,
                                Lat = record.WorkLocation != null ? record.WorkLocation.LATITUDE : 0,
                                Lon = record.WorkLocation != null ? record.WorkLocation.LONGITUDE : 0,
                                WorkOrderStatus = record.EntityStatus != null ? record.EntityStatus.NAME : "N/A",
                                OrderWarningLevel = record.WarningLevel != null ? record.WarningLevel.NAME : "N/A",
                                completedForUser = _wService.IsWorkOrderCompleted(record.WORK_ORDER_ID, userId),
                            };

                if (model.Count() == 0)
                    return BadRequest();

                if (User.IsInRole("Employee"))
                {
                    var result = items.Where(a => a.STATUS_LEVEL_ID == 1).ToList();

                    foreach (var item in result)
                    {
                        item.STATUS_LEVEL_ID = 2;
                        item.UPDATED_DATE = DateTime.Now;
                        item.UPDATER_ID = User.Identity.GetUserId();
                        await _wService.UpdateWorkOrder(item.WORK_ORDER_ID, item);
                    }
                }

                //model = model.Where(i => i.completedForUser == false);

                return Ok(model);
            }

            return BadRequest();
        }


        [Route("GetOrderAssesments")]
        [HttpGet]
        public async Task<IActionResult> GetOrderAssesments(int orderId)
        {
            var userId = User.Identity.GetUserId();
            if (userId > 0 && orderId > 0)
            {
                var items = await _clearService.GetAssesments(new Radzen.Query() { Filter = $@"i => i.WORK_ORDER_ID == {orderId} && i.AssesmentEmployees.Any(r=>r.EMPLOYEE_ID == {userId}) " });
                var rowsCount = items.Count();

                bool inComplete = false;
                if (items.FirstOrDefault().ParentAssessmentId != null)
                {
                    var relatedAssessments = await _clearService.GetAssesments(new Radzen.Query() { Filter = $@"i => i.ParentAssessmentId == {items.FirstOrDefault().ParentAssessmentId} && i.ASSESMENTID < {items.FirstOrDefault().ASSESMENTID}" });

                    if (relatedAssessments.Count() > 0)
                    {

                        foreach (var assessment in relatedAssessments)
                        {
                            if (!_wService.IsWorkOrderCompleted((int)assessment.WORK_ORDER_ID, userId))
                            {
                                inComplete = true;
                                break;
                            }
                        }
                    }
                }

                // if (inComplete)
                //     return Ok(new
                //     {
                //         status = false,
                //         msg = "Please complete previous assessment of this schedule."
                //     });

                var allsurveyReports = await _surveyservice.GetSurveyReports(new Query() { Filter = $@"i => i.SURVEYOR_ID == { User.Identity.GetUserId()} && i.ASSESMENT_ID == { items.FirstOrDefault().ASSESMENTID }" });
                var surveyReport = allsurveyReports.FirstOrDefault();

                var model = from record in items
                            select new
                            {
                                AssesmentId = record.ASSESMENTID,
                                WorkOrderNo = record.WorkOrder.WORK_ORDER_NUMBER,
                                ClientWorkOrder = record.WORKORDERNUMBER,
                                ClientName = record.Client != null ? record.Client.COMPANY_NAME : string.Empty,
                                ClientContact = record.ClientContact != null ? record.ClientContact.FullName : string.Empty,
                                TraderCategoryId = record.TRADECATEGORYID,
                                TradeCategory = record.TradeCategory != null ? record.TradeCategory.TRADE_NAME : string.Empty,
                                AssesmentType = record.TemplateType != null ? record.TemplateType.NAME : string.Empty,
                                WorkScope = record.SCOPEOFWORK,
                                AssesmentDate = record.ASSESMENTDATE,
                                ProjectName = record.PROJECTNAME,
                                IsSurveyAttached = record.ISCOVIDSURVEY ? "Yes" : "No",
                                SurveyId = record.COVID_SURVEY_ID,
                                IsSurveySigned = record.COVID_SURVEY_ID != null ? (surveyReport != null ? "Yes" : "No") : "No",
                                AssesmentNo = record.RISKASSESSMENTNO,
                                SiteName = record.PersonSite != null ? record.PersonSite.SITE_NAME : string.Empty,
                                BuildingName = record.PersonSite != null ? record.PersonSite.BUILDING_NAME : string.Empty,
                                Floor = record.PersonSite != null ? record.PersonSite.FLOOR : string.Empty,
                                Room = record.PersonSite != null ? record.PersonSite.ROOMNO : string.Empty,
                                Address1 = record.PersonSite != null ? record.PersonSite.SITE_ADDRESS1 : string.Empty,
                                Address2 = record.PersonSite != null ? record.PersonSite.SITE_ADDRESS2 : string.Empty,
                                City = record.PersonSite != null ? record.PersonSite.CITY : string.Empty,
                                PostCode = record.PersonSite != null ? record.PersonSite.POST_CODE : string.Empty,
                                Country = record.PersonSite != null ? record.PersonSite.Country != null ? record.PersonSite.Country.COUNTRYNAME : string.Empty : string.Empty,
                                State = record.PersonSite != null ? record.PersonSite.State != null ? record.PersonSite.State.STATENAME : string.Empty : string.Empty,
                                Lat = record.PersonSite != null ? record.PersonSite.LATITUDE : 0,
                                Lon = record.PersonSite != null ? record.PersonSite.LONGITUDE : 0,
                                status = record.ISCOMPLETED ? "Complete" : "Pending",
                                SWMSStatus = record.EntityStatus != null ? record.EntityStatus.NAME : "NA",
                                SWMSWarningLevel = record.WarningLevel != null ? record.WarningLevel.NAME : "NA",
                                PrevStatus = !inComplete,
                            };

                if (User.IsInRole("Employee"))
                {
                    var result = items.Where(a => a.ENTITY_STATUS_ID == 1).ToList();

                    foreach (var item in result)
                    {
                        item.ENTITY_STATUS_ID = 2;
                        item.UPDATED_DATE = DateTime.Now;
                        item.UPDATER_ID = User.Identity.GetUserId();
                        await _clearService.UpdateAssesment(item.ASSESMENTID, item);

                    }
                }

                return Ok(model);
            }

            return BadRequest();
        }


        [Route("GetAssesmentTemplates")]
        [HttpGet]
        public async Task<IActionResult> GetAssesmentTemplates(int assesmentId)
        {
            var userId = User.Identity.GetUserId();
            if (userId > 0)
            {
                //var items = await _clearService.GetAssesmentAttachements(new Radzen.Query() { Filter = $@"i => i.ASSESMENTID == {assesmentId}" });
                var assesmentEmp = await _clearService.GetAssesmentEmployees(new Radzen.Query() { Filter = $@"i => i.EMPLOYEE_ID == {userId} && i.ASSESMENT_ID == {assesmentId}" });
                var emp = assesmentEmp.FirstOrDefault();

                var items = await _clearService.GetAssesmentAttachements(new Radzen.Query() { Filter = $@"i => i.ASSESMENTID == {assesmentId} && i.Attachments.Any(r=>r.AssignedEmployee.EMPLOYEE_ID == {userId} )" });
                var rowsCount = items.Count();

                var model = from record in items
                            select new
                            {
                                AssesmentTemplateId = record.ATTACHEMENTID,
                                AssesmentId = record.ASSESMENTID,
                                AssignedDate = record.ATTACHEMENTDATE,
                                Title = record.SwmsTemplate != null ? record.SwmsTemplate.TEMPLATENAME : string.Empty,
                                Version = record.SwmsTemplate != null ? record.SwmsTemplate.VERSION : string.Empty,
                                TemplateNumber = record.SwmsTemplate != null ? record.SwmsTemplate.SWMSTEMPLATENUMBER : string.Empty,
                                SWMSStatus = record.Assesment.EntityStatus != null ? record.Assesment.EntityStatus.NAME : "NA",
                                SWMSWarningLevel = record.Assesment.WarningLevel != null ? record.Assesment.WarningLevel.NAME : "NA",

                                Documents = from x in record.Assesment.Documents
                                            select new
                                            {
                                                DOCUMENTNAME = x.CompanyDocuments.DOCUMENTNAME,
                                                FILENAME = x.CompanyDocuments.FILENAME,
                                                DOCUMENT_URL = x.CompanyDocuments.DOCUMENT_URL,
                                                VERSION_NUMBER = x.CompanyDocuments.VERSION_NUMBER,
                                            },

                                SignedStatus = record.Assesment.AssesmentEmployees.Where(i => i.EMPLOYEE_ID == userId).FirstOrDefault().SignedStatus == 1 ? true : false,

                                Attachements = from file in record.Attachments.Where(i => i.ASSESMENT_EMPLOYEE_ID == emp.ASSESMENT_EMPLOYEE_ID)
                                               select new
                                               {
                                                   AssesmentTemplateId = file.ATTACHEMENTID,
                                                   AssignedEmployeeId = file.ASSESMENT_EMPLOYEE_ID,
                                                   AssignedDate = file.ASSIGNED_DATE,
                                                   Url = file.DOCUMENT_URL,
                                                   DocumentName = file.DOCUMENTNAME,
                                                   Status = file.AssesmentEmployeeStatus != null ? file.AssesmentEmployeeStatus.NAME : "Assigned",
                                               }
                            };
                // var results = items.ToList();
                return Ok(model);
            }

            return BadRequest();
        }

        [Route("DownloadPdfTemplate")]
        [HttpGet]
        public async Task<IActionResult> DownloadPdfTemplate(int AssesmentTemplateId)
        {
            var userId = User.Identity.GetUserId();
            if (userId > 0)
            {
                var items = await _clearService.GetAssesmentEmployeeAttachements(new Radzen.Query() { Filter = $@"i => i.ATTACHEMENTID == {AssesmentTemplateId} && i.ASSESMENT_EMPLOYEE_ID == {userId} " });
                var rowsCount = items.Count();

                var model = from record in items
                            select new
                            {
                                AssesmentTemplateId = record.ATTACHEMENTID,
                                AssignedEmployeeId = record.ASSESMENT_EMPLOYEE_ID,
                                AssignedDate = record.ASSIGNED_DATE,
                                Url = record.DOCUMENT_URL,
                                DocumentName = record.DOCUMENTNAME,
                                Status = record.AssesmentEmployeeStatus != null ? record.AssesmentEmployeeStatus.NAME : "Assigned",
                            };
                // var results = items.ToList();
                return Ok(model);
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("Createactivity")]
        public async Task<IActionResult> Create([FromBody] AssesmentSiteActivity model)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                model.EmployeeID = User.Identity.GetUserId();
                if (model != null)
                {
                    SiteActivity site = new SiteActivity()
                    {
                        ASSESMENT_ID = model.AssesmentSiteId,
                        START_DATE = DateTime.Now,
                        END_DATE = model.EndDate,
                        LATITUDE = model.Latitude,
                        LONGITUDE = model.Longitude,
                        STATUS = model.Status,
                        EMPLOYEE_ID = model.EmployeeID,
                        CREATOR_ID = User.Identity.GetUserId(),
                        UPDATER_ID = User.Identity.GetUserId()
                    };

                    await _aservice.CreateSiteActivity(site);

                    if (site.SITE_ACTIVITY_ID > 0)
                    {
                        if (model.Status == "On-Site") //Change the Work Order Status
                        {
                            Assesment assesment = await _clearService.GetAssesmentByAssesmentid(model.AssesmentSiteId);

                            if (assesment != null && assesment.WorkOrder != null)
                            {
                                assesment.WorkOrder.STATUS_ID = 10;
                                assesment.WorkOrder.UPDATED_DATE = DateTime.Now;
                                assesment.WorkOrder.UPDATER_ID = User.Identity.GetUserId();
                                await _wService.UpdateWorkOrder(assesment.WorkOrder.WORK_ORDER_ID, assesment.WorkOrder);

                                if (User.IsInRole("Employee"))
                                {
                                    //Run Schedular after 15 mins to change the status of warning Level

                                    //_schedule.ScheduleWithParams<ChangeTransactionStatus>(model.AssesmentSiteId, User.Identity.GetUserId()).EveryFifteenMinutes();

                                    RecurringJob.AddOrUpdate($"ChangeTransactionStatus-AssessmentId-{model.AssesmentSiteId}-Emp-{User.Identity.GetUserId()}", () => changeTransactionStatus.Invoke(model.AssesmentSiteId, User.Identity.GetUserId()), "*/15 * * * *");

                                }
                                //update Employee Status
                                AssesmentEmployee employee = assesment.AssesmentEmployees.Where(a => a.EMPLOYEE_ID == model.EmployeeID).FirstOrDefault();

                                if (employee != null)
                                {
                                    //employee.AssignedEmployees != null do later
                                }


                            }
                        }
                    }

                    var result = new
                    {

                        success = true,
                        id = site.SITE_ACTIVITY_ID,
                        msg = "Site Activity Recorded Successfully"
                    };

                    return Ok(result);
                }
                else
                {

                    return BadRequest("Error In Creation Of Site Activity");
                }


            }
            catch (Exception ex)
            {
                return BadRequest("Error In Creation Of Site Activity");
            }

        }

        [HttpGet]
        [Route("getactivity")]
        public async Task<IActionResult> GetStatus()
        {
            var userId = User.Identity.GetUserId();
            try
            {
                var sites = await _aservice.GetEmployeeSiteActivities(new Query() { Filter = $@"i => i.EMPLOYEE_ID ==   {userId} " });

                var model = from record in sites
                            select new
                            {
                                record.SITE_ACTIVITY_ID,
                                record.ASSESMENT_ID,
                                record.LATITUDE,
                                record.LONGITUDE,
                                record.START_DATE,
                                record.END_DATE,
                                record.STATUS,
                                record.CREATED_DATE,
                                record.CREATOR_ID,
                                record.UPDATED_DATE,
                                record.UPDATER_ID,
                                record.DELETED_DATE,
                                record.DELETER_ID,
                                record.IS_DELETED,
                                record.ESCALATION_LEVEL_ID,
                                record.WARNING_LEVEL_ID,
                                record.STATUS_LEVEL_ID,
                                record.EMPLOYEE_ID,
                                record.Assesment.WORK_ORDER_ID,
                            };

                if (model.Count() > 0)
                {
                    var site = model.Distinct().OrderByDescending(r => r.START_DATE).FirstOrDefault();

                    return Ok(site);
                }
                else
                    return Ok(new
                    {
                        SITE_ACTIVITY_ID = "",
                        ASSESMENT_ID = "",
                        LATITUDE = "",
                        LONGITUDE = "",
                        START_DATE = "",
                        END_DATE = "",
                        STATUS = "Off-Site",
                        CREATED_DATE = "",
                        CREATOR_ID = "",
                        UPDATED_DATE = "",
                        UPDATER_ID = "",
                        DELETED_DATE = "",
                        DELETER_ID = "",
                        IS_DELETED = "",
                        ESCALATION_LEVEL_ID = "",
                        WARNING_LEVEL_ID = "",
                        STATUS_LEVEL_ID = "",
                        EMPLOYEE_ID = "",
                    });

            }
            catch (Exception ex)
            {
                return BadRequest("Error In Getting Employee Status");
            }

        }

        [HttpPost]
        [Route("updateactivity")]
        public async Task<IActionResult> Update([FromBody] WorkActivity model)
        {
            try
            {
                model.EmployeeID = User.Identity.GetUserId();
                if (model.EmployeeID > 0)
                {
                    var sites = await _aservice.GetSiteActivities(new Query() { Filter = $@"i => i.EMPLOYEE_ID ==   {model.EmployeeID}
                                        && i.ASSESMENT_ID == {model.AssesmentSiteId}
                                        && i.LATITUDE == {model.UpdateLatitude}
                                        && i.LONGITUDE == {model.UpdateLongitude}
                                         " });

                    var result = sites.FirstOrDefault();

                    if (result != null)
                    {
                        result.END_DATE = model.EndDate;
                        result.UPDATER_ID = User.Identity.GetUserId();
                        result.UPDATED_DATE = DateTime.Now;
                        result.STATUS = model.Status;

                        if (result.Assesment != null && model.Status == "On-Site")
                        {
                            if (result.Assesment.WorkOrder == null)
                                result.Assesment.WorkOrder = await _wService.GetWorkOrderByWorkOrderId(result.Assesment.WORK_ORDER_ID);

                            result.Assesment.WorkOrder.STATUS_ID = 10;


                        }

                        //Update The Site Activity

                        await _aservice.UpdateSiteActivity(result.SITE_ACTIVITY_ID, result);

                        if (model.Status == "On-Site")
                        {
                            if (User.IsInRole("Employee"))
                            {
                                //Run Schedular after 15 mins to change the status of warning Level

                                //_schedule.ScheduleWithParams<ChangeTransactionStatus>(model.AssesmentSiteId, User.Identity.GetUserId()).EveryFifteenMinutes();

                                RecurringJob.AddOrUpdate($"ChangeTransactionStatus-AssessmentId-{model.AssesmentSiteId}-Emp-{User.Identity.GetUserId()}", () => changeTransactionStatus.Invoke(model.AssesmentSiteId, User.Identity.GetUserId()), "*/15 * * * *");
                            }
                        }

                    }
                    else
                    {
                        //Create new Site Activity
                        SiteActivity site = new SiteActivity()
                        {
                            ASSESMENT_ID = model.AssesmentSiteId,
                            START_DATE = DateTime.Now,
                            END_DATE = model.EndDate,
                            LATITUDE = model.UpdateLatitude,
                            LONGITUDE = model.UpdateLongitude,
                            STATUS = model.Status,
                            EMPLOYEE_ID = model.EmployeeID
                        };

                        await _aservice.CreateSiteActivity(site);

                        if (site.SITE_ACTIVITY_ID > 0)
                        {
                            if (model.Status == "On-Site") //Change the Work Order Status
                            {
                                Assesment assesment = await _clearService.GetAssesmentByAssesmentid(model.AssesmentSiteId);

                                if (assesment != null && assesment.WorkOrder != null)
                                {
                                    assesment.WorkOrder.STATUS_ID = 10;
                                    assesment.WorkOrder.UPDATED_DATE = DateTime.Now;
                                    assesment.WorkOrder.UPDATER_ID = User.Identity.GetUserId();
                                    await _wService.UpdateWorkOrder(assesment.WorkOrder.WORK_ORDER_ID, assesment.WorkOrder);

                                    if (User.IsInRole("Employee"))
                                    {
                                        //Run Schedular after 15 mins to change the status of warning Level

                                        //_schedule.ScheduleWithParams<ChangeTransactionStatus>(assesment.ASSESMENTID, User.Identity.GetUserId()).EveryFifteenMinutes();

                                        RecurringJob.AddOrUpdate($"ChangeTransactionStatus-AssessmentId-{model.AssesmentSiteId}-Emp-{User.Identity.GetUserId()}", () => changeTransactionStatus.Invoke(model.AssesmentSiteId, User.Identity.GetUserId()), "*/15 * * * *");


                                    }
                                    //update Employee Status
                                    AssesmentEmployee employee = assesment.AssesmentEmployees.Where(a => a.EMPLOYEE_ID == model.EmployeeID).FirstOrDefault();

                                    if (employee != null)
                                    {
                                        //employee.AssignedEmployees != null do later
                                    }


                                }
                            }
                        }
                    }




                    return Ok("Site Activity Updated Successfully");
                }
                else
                {
                    return BadRequest("Invalid Employee Record Found");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error In updation Of Site Activity");
            }

        }

        #region "Document Signed"
        [HttpPost]
        [Route("SignedDocument")]
        public async Task<IActionResult> SignedDocument([FromBody] SignedDto signed)
        {
            try
            {
                int AssignedId = int.Parse(signed.EmpID);
                AssesmentEmployeeAttachement task = await _clearService.GetAssesmentEmployeeAttachementByAssesmentEmployeeIdAndAttachementid(AssignedId, signed.attachementid);

                if (task != null)
                {
                    if (task.EMPLOYEE_STATUS == 4)
                    {
                        var result = new
                        {
                            status = false,
                            msg = "Document is Already Signed"
                        };

                        return Ok(result);
                    }

                    string[] split_1 = signed.imageUrl.Split(';');
                    string split_2 = split_1[0].Replace("base64,", "");
                    //Convert base64 to byte
                    byte[] byteConvertedImage = Convert.FromBase64String(split_2);
                    string tempSignName = $@"\{Guid.NewGuid() + ".png"}";
                    var SingPath = Path.Combine(_hosting.WebRootPath, _hosting.WebRootPath + @"\Uploads/Templates/Sing") + tempSignName;
                    System.IO.File.WriteAllBytes(SingPath, byteConvertedImage);

                    task.EMPLOYEE_STATUS = 4;
                    task.DEVICESINGNATURE_DATE = signed.SingNatureDate;
                    task.SINGNATURE_DATE = DateTime.Now;
                    task.SINGNATUREIMAGE = byteConvertedImage;
                    task.WARNING_LEVEL_ID = 1;

                    //_context.Update(task);
                    //await _context.SaveChangesAsync();

                    if ((!string.IsNullOrEmpty(task.SWMSTemplateAttachement.DOCUMENTPDFURL) && !string.IsNullOrEmpty(task.SWMSTemplateAttachement.DOCUMENTTEMPLATEURL)))
                    {
                        //foreach (var item in _attachements)
                        //{
                        string fullPath = Path.Combine(_hosting.WebRootPath, "Uploads/Templates/Doc") + $@"\{task.SWMSTemplateAttachement.DOCUMENTTEMPLATEURL}";
                        System.IO.FileInfo file = new System.IO.FileInfo(fullPath);
                        if (file.Exists)
                        {
                            Novacode.DocX document = Novacode.DocX.Load(fullPath);
                            var paragraphs = document.Paragraphs.Where(x => x.Text.Contains("&&" + task.AssignedEmployee.Employee.FIRST_NAME.Trim() + "_" + task.AssignedEmployee.Employee.LAST_NAME.Trim() + "_" + task.AssignedEmployee.Employee.PERSON_ID.ToString().Trim()));
                            foreach (var p in paragraphs)
                            {
                                p.ReplaceText("&&" + task.AssignedEmployee.Employee.FIRST_NAME.Trim() + "_" + task.AssignedEmployee.Employee.LAST_NAME.Trim() + "_" + task.AssignedEmployee.Employee.PERSON_ID.ToString().Trim(), "");
                                Novacode.Image img = document.AddImage(SingPath);
                                Novacode.Picture pic1 = img.CreatePicture();
                                pic1.Width = 100;
                                pic1.Height = 50;
                                p.AppendPicture(pic1);
                            }

                            document.ReplaceText("&&Sign" + task.AssignedEmployee.Employee.FIRST_NAME.Trim() + "_" + task.AssignedEmployee.Employee.LAST_NAME.Trim() + "_date" + "_" + task.AssignedEmployee.Employee.PERSON_ID.ToString().Trim(), DateTime.Now.ToString());
                            document.ReplaceText("&&Time" + task.SRNo.ToString(), string.Format("{0:t tt}", DateTime.Now));
                            document.ReplaceText("&&Employee" + task.SRNo.ToString() + "_Name", task.AssignedEmployee.Employee.FIRST_NAME + " " + task.AssignedEmployee.Employee.LAST_NAME);

                            document.SaveAs(fullPath);

                            string pdfPath = Path.Combine(_hosting.WebRootPath, _hosting.WebRootPath + "/Uploads/Templates/Pdf") + $@"\{task.DOCUMENTNAME}";
                            var docconvaertret = ConverttoPdf(fullPath, pdfPath);
                        }

                        await _clearService.UpdateAssesmentEmployeeAttachement(AssignedId, task.ATTACHEMENTID, task);

                        //Chage the Status
                        var attachements = await _clearService.GetAssesmentAttachements(new Query() { Filter = $@"i => i.ASSESMENTID ==   {task.SWMSTemplateAttachement.ASSESMENTID} 
                                &&   i.Attachments.Any(r=>r.EMPLOYEE_STATUS == 1)" });

                        if (attachements.Count() == 0)
                        {
                            //UPDATE The STatus of Assesment and Work Order

                            Assesment assesment = await _clearService.GetAssesmentByAssesmentid(task.SWMSTemplateAttachement.ASSESMENTID);
                            var allsurveyReports = await _surveyservice.GetSurveyReports(new Query() { Filter = $@"i => i.SURVEYOR_ID == {User.Identity.GetUserId()} && i.ASSESMENT_ID == {task.SWMSTemplateAttachement.ASSESMENTID}" });
                            SurveyReport surveyReport = allsurveyReports.FirstOrDefault();
                            if (assesment != null)
                            {
                                if (assesment.COVID_SURVEY_ID != null)
                                {
                                    if (surveyReport != null)
                                        assesment.ISCOMPLETED = true;
                                    else
                                        assesment.ISCOMPLETED = false;
                                }
                                else
                                    assesment.ISCOMPLETED = true;

                                assesment.WARNING_LEVEL_ID = 1;
                                assesment.ENTITY_STATUS_ID = 3;
                                if (assesment.WorkOrder != null)
                                {
                                    if (assesment.COVID_SURVEY_ID != null)
                                    {
                                        if (surveyReport != null)
                                            assesment.WorkOrder.STATUS_ID = 11;
                                        else
                                            assesment.WorkOrder.STATUS_ID = 10;
                                    }
                                    else
                                        assesment.WorkOrder.STATUS_ID = 11;

                                    assesment.WorkOrder.WARNING_LEVEL_ID = 1;
                                    assesment.WorkOrder.STATUS_LEVEL_ID = 4;
                                    await _wService.UpdateWorkOrder(assesment.WorkOrder.WORK_ORDER_ID, assesment.WorkOrder);
                                }

                                if (assesment.AssesmentEmployees != null)
                                {
                                    var empResult = assesment.AssesmentEmployees.Where(a => a.EMPLOYEE_ID == User.Identity.GetUserId()).FirstOrDefault();

                                    empResult.WARNING_LEVEL_ID = 1;

                                    await _clearService.UpdateAssesmentEmployee(empResult.ASSESMENT_EMPLOYEE_ID, empResult);
                                }
                                await _clearService.UpdateAssesment(assesment.ASSESMENTID, assesment);
                            }
                        }

                        //((Coravel.Scheduling.Schedule.Scheduler)_schedule).TryUnschedule("ChangeTransactionStatus");

                        RecurringJob.RemoveIfExists($"ChangeTransactionStatus-AssessmentId-{task.SWMSTemplateAttachement.ASSESMENTID}-Emp-{User.Identity.GetUserId()}");

                        var result = new
                        {
                            status = true,
                            msg = "Signature is Successfully Signed"
                        };
                        return Ok(result);
                    }
                }
                else
                {
                    var result = new
                    {
                        status = false,
                        msg = "No Record Found"

                    };
                    return BadRequest(result);
                }

            }
            catch (Exception ex)
            {
                var result = new
                {
                    status = false,
                    msg = "No Record Found",
                    Exception = ex.Message
                };
                return BadRequest(result);
                // res.Result = "Error";
            }

            var result1 = new
            {
                status = true,
                msg = "Signature is Successfully Signed"
            };
            return Ok(result1);

        }




        public Boolean ConverttoPdf(string input, string output)
        {

            try
            {
                using (FileStream docStream = new FileStream(input, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
                {
                    WordDocument document = new WordDocument(docStream, Syncfusion.DocIO.FormatType.Automatic);

                    DocIORenderer render = new DocIORenderer();
                    PdfDocument pdfDocument = render.ConvertToPDF(document);

                    FileStream docStream1 = new FileStream(output, FileMode.OpenOrCreate);
                    MemoryStream outputStream = new MemoryStream();

                    pdfDocument.Save(docStream1);

                    //Closes the instance of PDF document object
                    pdfDocument.Close();
                    render.Dispose();

                    document.Dispose();

                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }


        }
        #endregion

        #region "Company & Administration"
        [Route("SearchOrderAssesments")]
        [HttpGet]
        public async Task<IActionResult> SearchOrderAssesments([FromBody] JObject data)
        {
            var siteName = data.GetValue("SiteName", StringComparison.OrdinalIgnoreCase).ToString();
            var City = data.GetValue("City", StringComparison.OrdinalIgnoreCase).ToString();
            var PostCode = data.GetValue("PostCode", StringComparison.OrdinalIgnoreCase).ToString();


            var userId = User.Identity.GetUserId();
            if (userId > 0)
            {
                var items = await _clearService.SearchAssesments(userId, siteName, City, PostCode);
                var rowsCount = items.Count();

                var model = from record in items
                            select new
                            {
                                AssesmentId = record.ASSESMENTID,
                                WorkOrderNo = record.WorkOrder.WORK_ORDER_NUMBER,
                                ClientWorkOrder = record.WORKORDERNUMBER,
                                ClientName = record.Client != null ? record.Client.COMPANY_NAME : string.Empty,
                                ClientContact = record.ClientContact != null ? record.ClientContact.FullName : string.Empty,
                                TraderCategoryId = record.TRADECATEGORYID,
                                TradeCategory = record.TradeCategory != null ? record.TradeCategory.TRADE_NAME : string.Empty,
                                AssesmentType = record.TemplateType != null ? record.TemplateType.NAME : string.Empty,
                                WorkScope = record.SCOPEOFWORK,
                                AssesmentDate = record.ASSESMENTDATE,
                                ProjectName = record.PROJECTNAME,
                                IsSurveyAttached = record.ISCOVIDSURVEY ? "Yes" : "No",
                                SurveyId = record.COVID_SURVEY_ID,
                                AssesmentNo = record.RISKASSESSMENTNO,
                                SiteName = record.PersonSite != null ? record.PersonSite.SITE_NAME : string.Empty,
                                BuildingName = record.PersonSite != null ? record.PersonSite.BUILDING_NAME : string.Empty,
                                Floor = record.PersonSite != null ? record.PersonSite.FLOOR : string.Empty,
                                Room = record.PersonSite != null ? record.PersonSite.ROOMNO : string.Empty,
                                Address1 = record.PersonSite != null ? record.PersonSite.SITE_ADDRESS1 : string.Empty,
                                Address2 = record.PersonSite != null ? record.PersonSite.SITE_ADDRESS2 : string.Empty,
                                City = record.PersonSite != null ? record.PersonSite.CITY : string.Empty,
                                PostCode = record.PersonSite != null ? record.PersonSite.POST_CODE : string.Empty,
                                Country = record.PersonSite != null ? record.PersonSite.Country != null ? record.PersonSite.Country.COUNTRYNAME : string.Empty : string.Empty,
                                State = record.PersonSite != null ? record.PersonSite.State != null ? record.PersonSite.State.STATENAME : string.Empty : string.Empty,
                                Lat = record.PersonSite != null ? record.PersonSite.LATITUDE : 0,
                                Lon = record.PersonSite != null ? record.PersonSite.LONGITUDE : 0,
                                status = record.ISCOMPLETED ? "Complete" : "Pending"
                            };
                // var results = items.ToList();
                return Ok(model);
            }

            return BadRequest();
        }
        #endregion

        [HttpPost]
        [Route("SignCompanyDocument")]
        public async Task<IActionResult> SignCompanyDocument([FromBody] SignInstructionDto signed)
        {
            var userId = User.Identity.GetUserId();
            if (userId > 0)
            {
                var employees = await _clearService.GetAssesmentEmployees();
                AssesmentEmployee employee = employees.Where(a => a.ASSESMENT_ID == signed.AssesmentId && a.EMPLOYEE_ID == userId).FirstOrDefault();
                if (employee != null)
                {
                    if (employee.SignedStatus == 1)
                    {
                        return BadRequest(new
                        {
                            status = false,
                            msg = "Company document already signed."
                        });
                    }

                    string[] split_1 = signed.imageUrl.Split(';');
                    string split_2 = split_1[0].Replace("base64,", "");
                    //Convert base64 to byte
                    byte[] byteConvertedImage = Convert.FromBase64String(split_2);
                    string tempSignName = $@"\{Guid.NewGuid() + ".png"}";
                    var SingPath = Path.Combine(_hosting.WebRootPath, _hosting.WebRootPath + @"\UploadDocument/Sign") + tempSignName;
                    System.IO.File.WriteAllBytes(SingPath, byteConvertedImage);

                    employee.SignatureImageUrl = @"\UploadDocument/Sign" + tempSignName;
                    employee.Sign_Date = signed.SingnatureDate;
                    employee.Server_Sign_Date = DateTime.Now;

                    employee.SignedStatus = signed.IsSignedStatus;
                    employee.VersionNo = signed.VersionNo;
                    employee.FileName = signed.FileName;
                    try
                    {
                        await _clearService.UpdateAssesmentEmployee(employee.ASSESMENT_EMPLOYEE_ID, employee);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(new
                        {
                            status = false,
                            msg = ex.Message
                        });
                    }
                    return Ok(new
                    {
                        status = true,
                        msg = "Signature Saved"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        status = false,
                        msg = "Assesment Not Found"
                    });
                }
            }
            var result = new
            {
                status = false,
                msg = "A problem occured, try again later"
            };
            return BadRequest(result);
        }

        [HttpPost]
        [Route("SignDocumentByObserver")]
        public async Task<IActionResult> SignDocumentByObserver([FromBody] SignObserverDto dto)
        {
            try
            {
                AssesmentEmployeeAttachement task = await _clearService.GetAssesmentEmployeeAttachementByAssesmentEmployeeIdAndAttachementid(dto.EmpID, dto.attachementid);

                if (task != null)
                {
                    if (task.EMPLOYEE_STATUS == 4)
                    {
                        var result = new
                        {
                            status = false,
                            msg = "Document is Already Signed"
                        };
                        return Ok(result);
                    }

                    string[] split_1 = dto.ImageUrl.Split(';');
                    string split_2 = split_1[0].Replace("base64,", "");
                    byte[] byteConvertedImage = Convert.FromBase64String(split_2);
                    string tempSignName = $@"\{Guid.NewGuid() + ".png"}";
                    var SignPath = Path.Combine(_hosting.WebRootPath, _hosting.WebRootPath + @"\Uploads/Templates/Sing") + tempSignName;
                    System.IO.File.WriteAllBytes(SignPath, byteConvertedImage);
                    task.OBSERVER_SIGN_URL = @"\Uploads/Templates/Sing/" + tempSignName;


                    task.OBSERVER_NAME = dto.name;
                    task.OBSERVER_SIGN_DATE = dto.SignatureDate;
                    task.OBSERVER_SIGN_STATUS = 1;

                    task.EMPLOYEE_STATUS = 4;
                    task.SINGNATURE_DATE = DateTime.Now;
                    task.SINGNATUREIMAGE = byteConvertedImage;
                    task.WARNING_LEVEL_ID = 1;

                    if ((!string.IsNullOrEmpty(task.SWMSTemplateAttachement.DOCUMENTPDFURL) && !string.IsNullOrEmpty(task.SWMSTemplateAttachement.DOCUMENTTEMPLATEURL)))
                    {
                        string fullPath = Path.Combine(_hosting.WebRootPath, "Uploads/Templates/Doc") + $@"\{task.SWMSTemplateAttachement.DOCUMENTTEMPLATEURL}";
                        System.IO.FileInfo file = new System.IO.FileInfo(fullPath);
                        if (file.Exists)
                        {
                            Novacode.DocX document = Novacode.DocX.Load(fullPath);
                            var paragraphs = document.Paragraphs.Where(x => x.Text.Contains("&&" + task.AssignedEmployee.Employee.FIRST_NAME.Trim() + "_" + task.AssignedEmployee.Employee.LAST_NAME.Trim() + "_" + task.AssignedEmployee.Employee.PERSON_ID.ToString().Trim()));
                            foreach (var p in paragraphs)
                            {
                                p.ReplaceText("&&" + task.AssignedEmployee.Employee.FIRST_NAME.Trim() + "_" + task.AssignedEmployee.Employee.LAST_NAME.Trim() + "_" + task.AssignedEmployee.Employee.PERSON_ID.ToString().Trim(), "");
                                Novacode.Image img = document.AddImage(SignPath);
                                Novacode.Picture pic1 = img.CreatePicture();
                                pic1.Width = 100;
                                pic1.Height = 50;
                                p.AppendPicture(pic1);
                            }

                            document.ReplaceText("&&Sign" + task.AssignedEmployee.Employee.FIRST_NAME.Trim() + "_" + task.AssignedEmployee.Employee.LAST_NAME.Trim() + "_date" + "_" + task.AssignedEmployee.Employee.PERSON_ID.ToString().Trim(), DateTime.Now.ToString());
                            document.ReplaceText("&&Time" + task.SRNo.ToString(), string.Format("{0:t tt}", DateTime.Now));
                            document.ReplaceText("&&Employee" + task.SRNo.ToString() + "_Name", task.AssignedEmployee.Employee.FIRST_NAME + " " + task.AssignedEmployee.Employee.LAST_NAME);

                            document.SaveAs(fullPath);

                            string pdfPath = Path.Combine(_hosting.WebRootPath, _hosting.WebRootPath + "/Uploads/Templates/Pdf") + $@"\{task.DOCUMENTNAME}";
                            var docconvaertret = ConverttoPdf(fullPath, pdfPath);
                        }
                        await _clearService.UpdateAssesmentEmployeeAttachement(dto.EmpID, task.ATTACHEMENTID, task);

                        var attachements = await _clearService.GetAssesmentAttachements(new Query() { Filter = $@"i => i.ASSESMENTID ==   {task.SWMSTemplateAttachement.ASSESMENTID} &&   i.Attachments.Any(r=>r.EMPLOYEE_STATUS == 1)" });
                        if (attachements.Count() == 0)
                        {
                            Assesment assesment = await _clearService.GetAssesmentByAssesmentid(task.SWMSTemplateAttachement.ASSESMENTID);
                            var allsurveyReports = await _surveyservice.GetSurveyReports(new Query() { Filter = $@"i => i.SURVEYOR_ID == {User.Identity.GetUserId()} && i.ASSESMENT_ID == {task.SWMSTemplateAttachement.ASSESMENTID}" });
                            SurveyReport surveyReport = allsurveyReports.FirstOrDefault();
                            if (assesment != null)
                            {
                                if (assesment.COVID_SURVEY_ID != null)
                                {
                                    if (surveyReport != null)
                                        assesment.ISCOMPLETED = true;
                                    else
                                        assesment.ISCOMPLETED = false;
                                }
                                else
                                    assesment.ISCOMPLETED = true;


                                assesment.WARNING_LEVEL_ID = 1;
                                assesment.ENTITY_STATUS_ID = 3;
                                if (assesment.WorkOrder != null)
                                {
                                    if (assesment.COVID_SURVEY_ID != null)
                                    {
                                        if (surveyReport != null)
                                            assesment.WorkOrder.STATUS_ID = 11;
                                        else
                                            assesment.WorkOrder.STATUS_ID = 10;
                                    }
                                    else
                                        assesment.WorkOrder.STATUS_ID = 11;

                                    assesment.WorkOrder.WARNING_LEVEL_ID = 1;
                                    assesment.WorkOrder.STATUS_LEVEL_ID = 4;
                                    await _wService.UpdateWorkOrder(assesment.WorkOrder.WORK_ORDER_ID, assesment.WorkOrder);
                                }

                                if (assesment.AssesmentEmployees != null)
                                {
                                    var empResult = assesment.AssesmentEmployees.Where(a => a.EMPLOYEE_ID == User.Identity.GetUserId()).FirstOrDefault();

                                    empResult.WARNING_LEVEL_ID = 1;

                                    await _clearService.UpdateAssesmentEmployee(empResult.ASSESMENT_EMPLOYEE_ID, empResult);
                                }
                                await _clearService.UpdateAssesment(assesment.ASSESMENTID, assesment);
                            }

                            //((Coravel.Scheduling.Schedule.Scheduler)_schedule).TryUnschedule("ChangeTransactionStatus");

                            RecurringJob.RemoveIfExists($"ChangeTransactionStatus-AssessmentId-{task.SWMSTemplateAttachement.ASSESMENTID}-Emp-{User.Identity.GetUserId()}");

                            var result = new
                            {
                                status = true,
                                msg = "Signature is Successfully Signed"
                            };
                            return Ok(result);
                        }
                    }
                }
                else
                {
                    return BadRequest(new
                    {
                        status = false,
                        msg = "No record found"
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    status = false,
                    msg = ex.Message
                });
            }



            return Ok();
        }
    }
}
