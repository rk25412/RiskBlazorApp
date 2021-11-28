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
//using Newtonsoft.Json.Linq;
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
using Clear.Risk.Pages.WorkOrders;


namespace Clear.Risk.Controllers.Api
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/Survey")]
    [ApiController]
    public class SurveyApiController : Controller
    {
        private readonly SurveyServices _service;
        private readonly ClearConnectionService _clearService;
        private readonly WorkOrderService _wService;

        public SurveyApiController(SurveyServices services, ClearConnectionService clearService, WorkOrderService wservice)
        {
            this._service = services;
            this._clearService = clearService;
            this._wService = wservice;
        }

        [Route("GetSurveyTemplates")]
        [HttpGet]
        public async Task<IActionResult> GetSurveyTemplates(int assesmentId)
        {
            //var items = await _service.GetSurveys(new Radzen.Query());
            //return Ok(items);

            var assesment = await _clearService.GetAssesmentByAssesmentid(assesmentId);
            if (assesment.ISCOVIDSURVEY)
            {
                var allsurveyReports = await _service.GetSurveyReports(new Query() { Filter = $@"i => i.SURVEYOR_ID == { User.Identity.GetUserId()} && i.ASSESMENT_ID == {assesmentId}" });
                SurveyReport surveyReport = allsurveyReports.FirstOrDefault();
                if (surveyReport != null)
                {
                    return Ok(new
                    {
                        status = false,
                        msg = "Survey already submitted",
                    });
                }
                else
                {
                    var surveys = await _service.GetSurveys(new Radzen.Query() { Filter = $@"SURVEY_ID == {assesment.COVID_SURVEY_ID}" });
                    return Ok(surveys);
                }
            }
            return Ok(new
            {
                status = false,
                msg = "No Survey available",
            });
        }

        [Route("GetSurveyQuestions")]
        [HttpGet]
        public async Task<IActionResult> GetSurveyQuestions(int surveyId)
        {
            var items = await _service.GetLimitedSurveyQuestions(new Radzen.Query() { Filter = $@"i => i.SURVEY_ID == {surveyId}", Expand = "SurveyAnswers", OrderBy = $"i => i.SURVEYQ_ORDER" });
            return Ok(items);
        }

        [HttpPost]
        [Route("SubmitSurvey")]
        public async Task<IActionResult> SubmitSurvey([FromBody] SurveyReportViewModel model)
        {
            try
            {

                if (model.SurveyId > 0)
                {
                    SurveyReport survey = new SurveyReport();
                    survey.CREATED_DATE = DateTime.Now;
                    survey.UPDATED_DATE = DateTime.Now;
                    survey.CREATOR_ID = User.Identity.GetUserId();
                    survey.UPDATER_ID = User.Identity.GetUserId();
                    survey.IS_DELETED = false;
                    survey.SURVEYOR_ID = User.Identity.GetUserId();
                    survey.SURVEY_ID = model.SurveyId;
                    survey.ASSESMENT_ID = model.AssesmentId;
                    survey.WORK_ORDER_ID = model.WorkOrerId;
                    survey.ESCALATION_LEVEL_ID = 1;
                    survey.WARNING_LEVEL_ID = 1;
                    survey.ENTITY_STATUS_ID = 1;
                    survey.COMMENTS = model.Comments;
                    survey.COMPANY_ID = model.CompanyId;
                    survey.STATUS = "Submit";
                    survey.SURVEY_DATE = DateTime.Now;

                    foreach (var item in model.Questions)
                    {
                        if (survey.SurveyAnswerChecklists == null)
                            survey.SurveyAnswerChecklists = new List<SurveyAnswerChecklist>();

                        var question = await _service.GetSurveyQuestionBySurveyqQuestionId(item.QuestionId);

                        SurveyAnswerChecklist checkList = new SurveyAnswerChecklist();
                        checkList.CREATED_DATE = DateTime.Now;
                        checkList.CREATOR_ID = User.Identity.GetUserId();
                        checkList.UPDATED_DATE = DateTime.Now;
                        checkList.UPDATER_ID = User.Identity.GetUserId();
                        checkList.IS_DELETED = false;
                        checkList.SURVEY_QUESTION_ID = item.QuestionId;
                        checkList.PARENT_QUESTION_ID = item.ParentQuestionId;
                        checkList.ESCALATION_LEVEL_ID = 1;
                        checkList.WARNING_LEVEL_ID = question.WARNING_LEVEL_ID;
                        checkList.ENTITY_STATUS_ID = 4;
                        checkList.COMMENTS = item.FreeText;
                        checkList.SurveyorComments = item.AnswerComments;

                        if (item.YesNo != null)
                        {
                            if ((bool)item.YesNo)
                                checkList.YESNO = "Yes";
                            else
                                checkList.YESNO = "No";
                        }

                        if (survey.WARNING_LEVEL_ID < question.WARNING_LEVEL_ID)
                            survey.WARNING_LEVEL_ID = question.WARNING_LEVEL_ID;

                        if (item.Answers != null)
                        {
                            foreach (var i in item.Answers)
                            {
                                if (checkList.SurveyAnswerValues == null)
                                    checkList.SurveyAnswerValues = new List<SurveyAnswerValue>();

                                var answer = await _service.GetSurveyAnswerBySurveyAnswerId(i);

                                if (checkList.WARNING_LEVEL_ID < answer.WARNING_LEVEL_ID)
                                    checkList.WARNING_LEVEL_ID = answer.WARNING_LEVEL_ID;

                                if (survey.WARNING_LEVEL_ID < answer.WARNING_LEVEL_ID)
                                    survey.WARNING_LEVEL_ID = answer.WARNING_LEVEL_ID;

                                checkList.SurveyAnswerValues.Add(new SurveyAnswerValue
                                {
                                    SURVEY_ANSWER_ID = i
                                });
                            }
                        }

                        survey.SurveyAnswerChecklists.Add(checkList);
                    }

                    await _service.CreateSurveyReport(survey);

                    Assesment assesment = await _clearService.GetAssesmentByAssesmentid(survey.ASSESMENT_ID);

                    bool close = true;

                    foreach(var item in assesment.AssesmentEmployees)
                    {
                        var attach = item.AssignedEmployees.FirstOrDefault();

                        if (attach.EMPLOYEE_STATUS != 4)
                            close = false;
                    }


                    var surveyReports = await _service.GetSurveyReports(new Query() { Filter = $"i => i.ASSESMENT_ID == {survey.ASSESMENT_ID}" });

                    if (close)
                    {
                        if (surveyReports.Count() != assesment.AssesmentEmployees.Count())
                            close = false;
                    }


                    AssesmentEmployeeAttachement attachement = assesment.AssesmentEmployees.FirstOrDefault(i => i.ASSESMENT_ID == survey.ASSESMENT_ID && i.EMPLOYEE_ID == User.Identity.GetUserId())
                                                                .AssignedEmployees.FirstOrDefault();

                    if (survey.SURVEY_REPORT_ID > 0)
                    {
                        if (attachement.EMPLOYEE_STATUS == 4 && close)
                        {
                            assesment.ISCOMPLETED = true;
                            assesment.WorkOrder.STATUS_ID = 11;
                        }
                        else
                        {
                            assesment.ISCOMPLETED = false;
                            assesment.WorkOrder.STATUS_ID = 10;
                        }
                        await _wService.UpdateWorkOrder(assesment.WorkOrder.WORK_ORDER_ID, assesment.WorkOrder);
                        await _clearService.UpdateAssesment(assesment.ASSESMENTID, assesment);

                        return Ok("Your Survey Submitted Successfully");
                    }
                    else
                        return BadRequest("There is problem to submit Survey, Please Try after Some Time");
                }
                else
                {
                    return BadRequest("Invalid Survey Record Found");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}
