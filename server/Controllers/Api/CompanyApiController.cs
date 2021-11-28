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

namespace Clear.Risk.Controllers.Api
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Produces("application/json")]
    [Route("api/Company")]
    [ApiController]
    public class CompanyApiController : ControllerBase
    {
        private readonly AssesmentService _aservice;
        private readonly WorkOrderService _wService;
        private readonly ClearConnectionService _clearService;
        private readonly SecurityService security;
        private readonly IWebHostEnvironment _hosting;
        public CompanyApiController(AssesmentService aservice, WorkOrderService wService,
            ClearConnectionService clearService, SecurityService securityService, IWebHostEnvironment webHostEnvironment)
        {
            this._aservice = aservice;
            this._wService = wService;
            this._clearService = clearService;
            this.security = securityService;
            this._hosting = webHostEnvironment;
        }

        [Route("GetCompanyStatstics")]
        [HttpGet]
        public async Task<IActionResult> GetCompanyStatstics()
        {

            var userId = User.Identity.GetUserId();
            if (userId > 0)
            {
               var result = _clearService.GetAccountStats();
                return Ok(result);
            }

            return BadRequest("Un Authorized Access");
        }


        [Route("GetWorkOrderStatstics")]
        [HttpGet]
        public async Task<IActionResult> GetWorkOrderStatstics()
        {

            var userId = User.Identity.GetUserId();
            if (userId > 0)
            {
                var result = _clearService.MonthyWorkOrderStatus();
                return Ok(result);
            }

            return BadRequest("Un Authorized Access");
        }

        [Route("GetMonthlySurveyStatstics")]
        [HttpGet]
        public async Task<IActionResult> GetMonthlySurveyStatstics()
        {

            var userId = User.Identity.GetUserId();
            if (userId > 0)
            {
                var result = _clearService.MonthySurveyConductStats();
                return Ok(result);
            }

            return BadRequest("Un Authorized Access");
        }


        [Route("GetInvoices")]
        [HttpGet]
        public async Task<IActionResult> GetInvoices()
        {

            var userId = User.Identity.GetUserId();
            if (userId > 0)
            {
                var result = await _clearService.GetCompanyTransactions(new Query() { Filter = $@"i => i.PERSON_ID == {userId} && i.TRANSACTIONDATE >= {DateTime.Now.AddDays(-30)} && i.TRANSACTIONDATE <= {DateTime.Now}" });
                return Ok(result);
            }

            return BadRequest("Un Authorized Access");
        }

    }
}
