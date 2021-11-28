using Radzen;
using System;
using System.Web;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components;
using Clear.Risk.Data;
using Clear.Risk.Models.ClearConnection;
using Clear.Risk.Models;
////using Coravel.Invocable;
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
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Clear.Risk
{
    //public class ChangeTransactionStatus : IInvocable
    public class ChangeTransactionStatus
    {
        private readonly ClearConnectionService context;
        private readonly SecurityService Security;
        private readonly NavigationManager navigationManager;
        private readonly WorkOrderService OrderService;
        private readonly IWebHostEnvironment _hosting;
        public ChangeTransactionStatus(ClearConnectionService context,
           NavigationManager navigationManager, SecurityService security, IWebHostEnvironment hostEnvironment,
           WorkOrderService workOrderService)
        {
            this.context = context;
            this.navigationManager = navigationManager;
            Security = security;
            OrderService = workOrderService;
            _hosting = hostEnvironment;
        }

        public async Task Invoke(int AssesmentId, int EmployeeId)
        {
            try
            {
                var item = await context.GetAssesmentByAssesmentid(AssesmentId);

                if (item.ENTITY_STATUS_ID == 2 && item.WARNING_LEVEL_ID == 1)
                {
                    item.WARNING_LEVEL_ID = 2;
                    item.UPDATED_DATE = DateTime.Now;

                    if (item.WorkOrder != null)
                    {
                        item.WorkOrder.WARNING_LEVEL_ID = 2;
                        item.WorkOrder.UPDATED_DATE = DateTime.Now;

                        await OrderService.UpdateWorkOrder(item.WORK_ORDER_ID, item.WorkOrder);


                    }

                    if (item.AssesmentEmployees != null)
                    {
                        var result = item.AssesmentEmployees.Where(a => a.EMPLOYEE_ID == EmployeeId).FirstOrDefault();

                        result.WARNING_LEVEL_ID = 2;

                        //Update Employee Warning Level
                        await context.UpdateAssesmentEmployee(result.ASSESMENT_EMPLOYEE_ID, result);
                        if (result.AssignedEmployees != null)
                        {
                            foreach (var attachement in result.AssignedEmployees)
                            {
                                attachement.WARNING_LEVEL_ID = 2;

                                await context.UpdateAssesmentEmployeeAttachement(attachement.ASSESMENT_EMPLOYEE_ID, attachement.ATTACHEMENTID, attachement);
                            }
                        }

                    }

                    await context.UpdateAssesment(item.ASSESMENTID, item);
                }
                else if (item.ENTITY_STATUS_ID == 2 && item.WARNING_LEVEL_ID == 2)
                {
                    item.WARNING_LEVEL_ID = 3;
                    item.UPDATED_DATE = DateTime.Now;

                    if (item.WorkOrder != null)
                    {
                        item.WorkOrder.WARNING_LEVEL_ID = 3;
                        item.WorkOrder.UPDATED_DATE = DateTime.Now;
                        await OrderService.UpdateWorkOrder(item.WORK_ORDER_ID, item.WorkOrder);
                    }

                    if (item.AssesmentEmployees != null)
                    {
                        var result = item.AssesmentEmployees.Where(a => a.EMPLOYEE_ID == EmployeeId).FirstOrDefault();

                        result.WARNING_LEVEL_ID = 3;
                        await context.UpdateAssesmentEmployee(result.ASSESMENT_EMPLOYEE_ID, result);



                        if (result.AssignedEmployees != null)
                        {
                            foreach (var attachement in result.AssignedEmployees)
                            {
                                attachement.WARNING_LEVEL_ID = 3;

                                await context.UpdateAssesmentEmployeeAttachement(attachement.ASSESMENT_EMPLOYEE_ID, attachement.ATTACHEMENTID, attachement);
                            }
                        }

                    }

                    await context.UpdateAssesment(item.ASSESMENTID, item);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
