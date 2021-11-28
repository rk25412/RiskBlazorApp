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
using DocumentFormat.OpenXml.Drawing;

namespace Clear.Risk
{
    public partial class WorkOrderService
    {
        private readonly ClearConnectionContext context;
        private readonly NavigationManager navigationManager;

        public WorkOrderService(ClearConnectionContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public async Task ExportWorkOrdersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/clearrisk/workorders/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')") : $"export/clearrisk/workorders/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')", true);
        }

        public async Task ExportWorkOrdersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/clearrisk/workorders/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')") : $"export/clearrisk/workorders/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')", true);
        }

        partial void OnWorkOrdersRead(ref IQueryable<WorkOrder> items);

        public async Task<IQueryable<WorkOrder>> GetWorkOrders(Query query = null)
        {
            var items = context.WorkOrders.AsQueryable();

            items = items.Include(i => i.OrderStatus);
            items = items.Include(i => i.PriorityMaster);
            items = items.Include(i => i.WorkOrderType);
            items = items.Include(i => i.BaseWorkOrder); ;
            items = items.Include(i => i.ClientContact);
            items = items.Include(i => i.Client);
            items = items.Include(i => i.ManagementCompany);
            items = items.Include(i => i.ContractorContact);
            items = items.Include(i => i.Contractor);
            items = items.Include(i => i.ProcessType);
            items = items.Include(i => i.CriticalityMaster);
            items = items.Include(i => i.WorkerPeople);
            items = items.Include(i => i.EntityStatus);
            items = items.Include(i => i.WarningLevel);
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    items = items.Where(query.Filter);
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnWorkOrdersRead(ref items);

            return await Task.FromResult(items);
        }

        public async Task<IQueryable<WorkOrder>> GetWorkOrders(int companyId, Query query = null)
        {
            var items = context.WorkOrders.AsQueryable().Where(a => a.COMPANY_ID == companyId);

            items = items.Include(i => i.OrderStatus);
            items = items.Include(i => i.PriorityMaster);
            items = items.Include(i => i.WorkOrderType);
            items = items.Include(i => i.BaseWorkOrder);
            items = items.Include(i => i.ClientContact);
            items = items.Include(i => i.Client);
            items = items.Include(i => i.ManagementCompany);
            items = items.Include(i => i.ContractorContact);
            items = items.Include(i => i.Contractor);
            items = items.Include(i => i.ProcessType);
            items = items.Include(i => i.CriticalityMaster);
            items = items.Include(i => i.EntityStatus);
            items = items.Include(i => i.WarningLevel);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    items = items.Where(query.Filter);
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnWorkOrdersRead(ref items);

            return await Task.FromResult(items);
        }
        partial void OnWorkOrderCreated(WorkOrder item);

        public async Task<WorkOrder> CreateWorkOrder(WorkOrder workOrder)
        {
            OnWorkOrderCreated(workOrder);

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.WorkOrders.Add(workOrder);
                    context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }



            return workOrder;
        }

        partial void OnWorkOrderDeleted(WorkOrder item);
        public async Task<WorkOrder> DeleteWorkOrder(int? workOrderId)
        {
            var item = context.WorkOrders
                              .Where(i => i.WORK_ORDER_ID == workOrderId)
                              .Include(i => i.BaseWorkOrder)
                              .Include(i => i.WorkerPeople)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnWorkOrderDeleted(item);

            context.WorkOrders.Remove(item);

            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                context.Entry(item).State = EntityState.Unchanged;
                throw ex;
            }

            return item;
        }

        partial void OnWorkOrderGet(WorkOrder item);

        public async Task<WorkOrder> GetWorkOrderByWorkOrderId(int? workOrderId)
        {
            var items = context.WorkOrders
                              .AsNoTracking()
                              .Where(i => i.WORK_ORDER_ID == workOrderId);

            items = items.Include(i => i.OrderStatus);

            items = items.Include(i => i.PriorityMaster);

            items = items.Include(i => i.WorkOrderType);

            items = items.Include(i => i.BaseWorkOrder);

            items = items.Include(i => i.ClientContact);

            items = items.Include(i => i.Client);

            items = items.Include(i => i.ContractorContact);

            items = items.Include(i => i.Contractor);

            items = items.Include(i => i.ManagementCompany);

            items = items.Include(i => i.RequestedBy);

            items = items.Include(i => i.ApproveBy);

            items = items.Include(i => i.ProcessType);

            items = items.Include(i => i.CriticalityMaster);
            items = items.Include(i => i.WorkLocation);


            var item = items.FirstOrDefault();

            OnWorkOrderGet(item);

            return await Task.FromResult(item);
        }

        partial void OnWorkOrderMaxIDGet(int item);
        public async Task<int> GetMaxID()
        {
            try
            {
                int maxID = 0;
                var items = context.WorkOrders.AsQueryable().ToList();

                if (items.Count > 0)
                    maxID = items.Max().WORK_ORDER_ID;
                else
                    maxID = 1;

                OnWorkOrderMaxIDGet(maxID);

                return await Task.FromResult(maxID);
            }
            catch (Exception ex)
            {
                return -1;
            }

        }

        public async Task<int> GetmaxAssesmentID(int companyId)
        {
            try
            {
                int maxId = 0;
                var items = context.Assesments.AsQueryable().Where(a => a.COMPANYID == companyId);

                maxId = items.Count() + 1;

                OnWorkOrderMaxIDGet(maxId);

                return await Task.FromResult(maxId);
            }
            catch (Exception ex)
            {
                return -1;
            }
        }


        public async Task<int> GetMaxID(int CompanyId)
        {
            try
            {
                int maxID = 0;
                var items = context.WorkOrders.AsQueryable().Where(a => a.COMPANY_ID == CompanyId);


                maxID = items.Count() + 1;

                OnWorkOrderMaxIDGet(maxID);

                return await Task.FromResult(maxID);
            }
            catch (Exception ex)
            {
                return -1;
            }

        }
        public async Task<WorkOrder> CancelWorkOrderChanges(WorkOrder item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnWorkOrderUpdated(WorkOrder item);

        public async Task<WorkOrder> UpdateWorkOrder(int? workOrderId, WorkOrder workOrder)
        {
            OnWorkOrderUpdated(workOrder);

            var item = context.WorkOrders
                              .Where(i => i.WORK_ORDER_ID == workOrderId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(workOrder);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return workOrder;
        }

        public async Task<WorkerPerson> CreateWorkerPeople(WorkerPerson order)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.WorkerPeople.Add(order);
                    context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
            return order;
        }

        public bool IsWorkOrderCompleted(int workOrderId, int userId)
        {
            bool result = false;

            var assessment = context.Assesments.OrderByDescending(i => i.ASSESMENTID).Where(i => i.WORK_ORDER_ID == workOrderId).FirstOrDefault();

            var assessmentAttachment = context.AssesmentAttachements.OrderByDescending(i => i.ATTACHEMENTID).Where(i => i.ASSESMENTID == assessment.ASSESMENTID).FirstOrDefault();

            var assessmentEmployee = context.AssesmentEmployees.Where(i => i.ASSESMENT_ID == assessment.ASSESMENTID && i.EMPLOYEE_ID == userId).FirstOrDefault();

            var assessmentEmployeeAttachment = context.AssesmentEmployeeAttachements.Where(i => i.ASSESMENT_EMPLOYEE_ID == assessmentEmployee.ASSESMENT_EMPLOYEE_ID && i.ATTACHEMENTID == assessmentAttachment.ATTACHEMENTID).FirstOrDefault();

            result = assessmentEmployeeAttachment?.EMPLOYEE_STATUS == 4 ? true : false;

            if (assessment.COVID_SURVEY_ID != null && result)
            {
                var surveyReport = context.SurveyReports.Where(i => i.SURVEYOR_ID == userId && i.ASSESMENT_ID == assessment.ASSESMENTID).FirstOrDefault();

                result = surveyReport != null ? true : false;
            }

            return result;
        }

    }
}
