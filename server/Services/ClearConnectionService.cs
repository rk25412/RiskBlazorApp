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

namespace Clear.Risk
{
    public partial class ClearConnectionService
    {
        private readonly ClearConnectionContext context;
        private readonly NavigationManager navigationManager;

        public ClearConnectionService(ClearConnectionContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public async Task ExportApplicencesToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/applicences/excel") : "export/clearconnection/applicences/excel", true);
        }

        public async Task ExportApplicencesToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/applicences/csv") : "export/clearconnection/applicences/csv", true);
        }

        partial void OnApplicencesRead(ref IQueryable<Models.ClearConnection.Applicence> items);

        public async Task<IQueryable<Models.ClearConnection.Applicence>> GetApplicences(Query query = null)
        {
            var items = context.Applicences.AsQueryable();

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

            OnApplicencesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnApplicenceCreated(Models.ClearConnection.Applicence item);

        public async Task<Models.ClearConnection.Applicence> CreateApplicence(Models.ClearConnection.Applicence applicence)
        {
            OnApplicenceCreated(applicence);

            context.Applicences.Add(applicence);
            context.SaveChanges();

            return applicence;
        }
        public async Task ExportAssesmentsToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/assesments/excel") : "export/clearconnection/assesments/excel", true);
        }

        public async Task ExportAssesmentsToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/assesments/csv") : "export/clearconnection/assesments/csv", true);
        }

        partial void OnAssesmentsRead(ref IQueryable<Models.ClearConnection.Assesment> items);

        public async Task<IQueryable<Models.ClearConnection.Assesment>> GetAssesments(Query query = null)
        {
            var items = context.Assesments.AsQueryable();

            // items = items.Include(i => i.Assesment1);

            items = items.Include(i => i.Client);

            items = items.Include(i => i.Company);

            items = items.Include(i => i.TradeCategory);

            items = items.Include(i => i.StatusMaster);

            items = items.Include(i => i.PersonSite);

            items = items.Include(i => i.IndustryType);

            items = items.Include(i => i.WarningLevel);
            items = items.Include(i => i.EscalationLevel);
            items = items.Include(i => i.StatusLevel);
            items = items.Include(i => i.EntityStatus);

            items = items.Include(i => i.AssesmentEmployees);


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

            OnAssesmentsRead(ref items);

            return await Task.FromResult(items);
        }




        partial void OnAssesmentCreated(Models.ClearConnection.Assesment item);


        public async Task<IQueryable<Models.ClearConnection.Assesment>> SearchAssesments(int CompanyId, string SiteName, string City, string PostCode, Query query = null)
        {
            var items = context.Assesments.AsQueryable();

            // items = items.Include(i => i.Assesment1);

            items = items.Include(i => i.Client);

            items = items.Include(i => i.Company);

            items = items.Include(i => i.TradeCategory);

            items = items.Include(i => i.StatusMaster);

            items = items.Include(i => i.PersonSite);

            items = items.Include(i => i.IndustryType);

            // items = items.Include(i => i.ScheduleAssesments);

            items = items.Include(i => i.AssesmentEmployees).Where(i => i.COMPANYID == CompanyId &&
                                (string.IsNullOrEmpty(SiteName) || i.PersonSite.SITE_NAME.StartsWith(SiteName))
                                && (string.IsNullOrEmpty(City) || i.PersonSite.CITY.StartsWith(City))
                                && (string.IsNullOrEmpty(PostCode) || i.PersonSite.POST_CODE == PostCode));

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

            OnAssesmentsRead(ref items);

            return await Task.FromResult(items);
        }

        public async Task<Models.ClearConnection.Assesment> CreateAssesment(Models.ClearConnection.Assesment assesment)
        {
            OnAssesmentCreated(assesment);
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.Assesments.Add(assesment);
                    context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }


            return assesment;
        }
        public async Task ExportAssesmentAttachementsToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/assesmentattachements/excel") : "export/clearconnection/assesmentattachements/excel", true);
        }

        public async Task ExportAssesmentAttachementsToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/assesmentattachements/csv") : "export/clearconnection/assesmentattachements/csv", true);
        }

        partial void OnAssesmentAttachementsRead(ref IQueryable<Models.ClearConnection.AssesmentAttachement> items);

        public async Task<IQueryable<Models.ClearConnection.AssesmentAttachement>> GetAssesmentAttachements(Query query = null)
        {
            var items = context.AssesmentAttachements.AsQueryable();

            items = items.Include(i => i.Assesment).ThenInclude(i => i.Documents).ThenInclude(i => i.CompanyDocuments);

            items = items.Include(i => i.SwmsTemplate);

            items = items.Include(i => i.Attachments);

            items = items.Include(i => i.Attachments).ThenInclude(b => b.AssignedEmployee);

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

            OnAssesmentAttachementsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAssesmentAttachementCreated(Models.ClearConnection.AssesmentAttachement item);

        public async Task<Models.ClearConnection.AssesmentAttachement> CreateAssesmentAttachement(Models.ClearConnection.AssesmentAttachement assesmentAttachement)
        {
            OnAssesmentAttachementCreated(assesmentAttachement);

            context.AssesmentAttachements.Add(assesmentAttachement);
            context.SaveChanges();

            return assesmentAttachement;
        }
        public async Task ExportAssesmentEmployeesToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/assesmentemployees/excel") : "export/clearconnection/assesmentemployees/excel", true);
        }

        public async Task ExportAssesmentEmployeesToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/assesmentemployees/csv") : "export/clearconnection/assesmentemployees/csv", true);
        }

        partial void OnAssesmentEmployeesRead(ref IQueryable<Models.ClearConnection.AssesmentEmployee> items);

        public async Task<IQueryable<Models.ClearConnection.AssesmentEmployee>> GetAssesmentEmployees(Query query = null)
        {
            var items = context.AssesmentEmployees.AsQueryable();

            items = items.Include(i => i.Assesment);

            items = items.Include(i => i.Employee);

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

            OnAssesmentEmployeesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAssesmentEmployeeCreated(Models.ClearConnection.AssesmentEmployee item);

        public async Task<Models.ClearConnection.AssesmentEmployee> CreateAssesmentEmployee(Models.ClearConnection.AssesmentEmployee assesmentEmployee)
        {
            OnAssesmentEmployeeCreated(assesmentEmployee);

            context.AssesmentEmployees.Add(assesmentEmployee);
            try
            {
                context.SaveChanges();
                return assesmentEmployee;
            }
            catch (Exception ex)
            {
                context.Entry(assesmentEmployee).State = EntityState.Unchanged;
                throw ex;
            }
        }
        public async Task ExportAssesmentEmployeeAttachementsToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/assesmentemployeeattachements/excel") : "export/clearconnection/assesmentemployeeattachements/excel", true);
        }

        public async Task ExportAssesmentEmployeeAttachementsToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/assesmentemployeeattachements/csv") : "export/clearconnection/assesmentemployeeattachements/csv", true);
        }

        partial void OnAssesmentEmployeeAttachementsRead(ref IQueryable<Models.ClearConnection.AssesmentEmployeeAttachement> items);

        public async Task<IQueryable<Models.ClearConnection.AssesmentEmployeeAttachement>> GetAssesmentEmployeeAttachements(Query query = null)
        {

            try
            {
                var items = context.AssesmentEmployeeAttachements.AsQueryable();

                items = items.Include(i => i.SWMSTemplateAttachement);
                items = items.Include(i => i.AssesmentEmployeeStatus);
                items = items.Include(i => i.AssignedEmployee);
                items = items.Include(i => i.AssignedEmployee).ThenInclude(i => i.Employee);



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


                OnAssesmentEmployeeAttachementsRead(ref items);

                return await Task.FromResult(items);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IQueryable<Models.ClearConnection.AssesmentEmployeeAttachement>> GetAssesmentEmployeeAttachementsByAssesmentId(int AssesmentId, Query query = null)
        {
            var items = context.AssesmentEmployeeAttachements.AsQueryable();

            items = items.Include(i => i.SWMSTemplateAttachement).Where(b => b.SWMSTemplateAttachement.ASSESMENTID == AssesmentId);

            items = items.Include(i => i.AssesmentEmployeeStatus);
            items = items.Include(i => i.AssignedEmployee);
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

            OnAssesmentEmployeeAttachementsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAssesmentEmployeeAttachementCreated(Models.ClearConnection.AssesmentEmployeeAttachement item);

        public async Task<Models.ClearConnection.AssesmentEmployeeAttachement> CreateAssesmentEmployeeAttachement(Models.ClearConnection.AssesmentEmployeeAttachement assesmentEmployeeAttachement)
        {
            OnAssesmentEmployeeAttachementCreated(assesmentEmployeeAttachement);

            context.AssesmentEmployeeAttachements.Add(assesmentEmployeeAttachement);
            context.SaveChanges();

            return assesmentEmployeeAttachement;
        }
        public async Task ExportAssesmentEmployeeStatusesToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/assesmentemployeestatuses/excel") : "export/clearconnection/assesmentemployeestatuses/excel", true);
        }

        public async Task ExportAssesmentEmployeeStatusesToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/assesmentemployeestatuses/csv") : "export/clearconnection/assesmentemployeestatuses/csv", true);
        }

        partial void OnAssesmentEmployeeStatusesRead(ref IQueryable<Models.ClearConnection.AssesmentEmployeeStatus> items);

        public async Task<IQueryable<Models.ClearConnection.AssesmentEmployeeStatus>> GetAssesmentEmployeeStatuses(Query query = null)
        {
            var items = context.AssesmentEmployeeStatuses.AsQueryable();

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

            OnAssesmentEmployeeStatusesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAssesmentEmployeeStatusCreated(Models.ClearConnection.AssesmentEmployeeStatus item);

        public async Task<Models.ClearConnection.AssesmentEmployeeStatus> CreateAssesmentEmployeeStatus(Models.ClearConnection.AssesmentEmployeeStatus assesmentEmployeeStatus)
        {
            OnAssesmentEmployeeStatusCreated(assesmentEmployeeStatus);

            context.AssesmentEmployeeStatuses.Add(assesmentEmployeeStatus);
            context.SaveChanges();

            return assesmentEmployeeStatus;
        }
        public async Task ExportAssesmenttasksToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/assesmenttasks/excel") : "export/clearconnection/assesmenttasks/excel", true);
        }

        public async Task ExportAssesmenttasksToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/assesmenttasks/csv") : "export/clearconnection/assesmenttasks/csv", true);
        }

        partial void OnAssesmenttasksRead(ref IQueryable<Models.ClearConnection.Assesmenttask> items);

        public async Task<IQueryable<Models.ClearConnection.Assesmenttask>> GetAssesmenttasks(Query query = null)
        {
            var items = context.Assesmenttasks.AsQueryable();

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

            OnAssesmenttasksRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAssesmenttaskCreated(Models.ClearConnection.Assesmenttask item);

        public async Task<Models.ClearConnection.Assesmenttask> CreateAssesmenttask(Models.ClearConnection.Assesmenttask assesmenttask)
        {
            OnAssesmenttaskCreated(assesmenttask);

            context.Assesmenttasks.Add(assesmenttask);
            context.SaveChanges();

            return assesmenttask;
        }
        public async Task ExportConsequencesToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/consequences/excel") : "export/clearconnection/consequences/excel", true);
        }

        public async Task ExportConsequencesToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/consequences/csv") : "export/clearconnection/consequences/csv", true);
        }

        partial void OnConsequencesRead(ref IQueryable<Models.ClearConnection.Consequence> items);

        public async Task<IQueryable<Models.ClearConnection.Consequence>> GetConsequences(Query query = null)
        {
            var items = context.Consequences.AsQueryable();

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

            OnConsequencesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnConsequenceCreated(Models.ClearConnection.Consequence item);

        public async Task<Models.ClearConnection.Consequence> CreateConsequence(Models.ClearConnection.Consequence consequence)
        {
            OnConsequenceCreated(consequence);

            context.Consequences.Add(consequence);
            context.SaveChanges();

            return consequence;
        }
        public async Task ExportControlMeasureValuesToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/controlmeasurevalues/excel") : "export/clearconnection/controlmeasurevalues/excel", true);
        }

        public async Task ExportControlMeasureValuesToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/controlmeasurevalues/csv") : "export/clearconnection/controlmeasurevalues/csv", true);
        }

        partial void OnControlMeasureValuesRead(ref IQueryable<Models.ClearConnection.ControlMeasureValue> items);

        public async Task<IQueryable<Models.ClearConnection.ControlMeasureValue>> GetControlMeasureValues(Query query = null)
        {
            var items = context.ControlMeasureValues.AsQueryable();

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

            OnControlMeasureValuesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnControlMeasureValueCreated(Models.ClearConnection.ControlMeasureValue item);

        public async Task<Models.ClearConnection.ControlMeasureValue> CreateControlMeasureValue(Models.ClearConnection.ControlMeasureValue controlMeasureValue)
        {
            OnControlMeasureValueCreated(controlMeasureValue);

            context.ControlMeasureValues.Add(controlMeasureValue);
            context.SaveChanges();

            return controlMeasureValue;
        }
        public async Task ExportCountriesToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/countries/excel") : "export/clearconnection/countries/excel", true);
        }

        public async Task ExportCountriesToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/countries/csv") : "export/clearconnection/countries/csv", true);
        }

        partial void OnDesigationRead(ref IQueryable<Models.ClearConnection.Desigation> items);
        public async Task<IQueryable<Models.ClearConnection.Desigation>> GetDesigations(Query query = null)
        {
            var items = context.Desigations.AsQueryable();

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

            OnDesigationRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCompanyDoumentRead(ref IQueryable<Models.ClearConnection.CompanyDocumentFile> items);
        public async Task<IQueryable<Models.ClearConnection.CompanyDocumentFile>> GetCompanyDocumentFiles(Query query = null)
        {
            var items = context.CompanyDocumentFiles.AsQueryable();
            items = items.Include(i => i.Person);
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

            OnCompanyDoumentRead(ref items);

            return await Task.FromResult(items);
        }
        partial void OnCountriesRead(ref IQueryable<Models.ClearConnection.Country> items);

        public async Task<IQueryable<Models.ClearConnection.Country>> GetCountries(Query query = null)
        {
            var items = context.Countries.AsQueryable();

            try
            {

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
            }
            catch (Exception ex)
            {
                throw ex;
            }

            OnCountriesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCountryCreated(Models.ClearConnection.Country item);

        public async Task<Models.ClearConnection.Country> CreateCountry(Models.ClearConnection.Country country)
        {
            OnCountryCreated(country);

            context.Countries.Add(country);
            context.SaveChanges();

            return country;
        }
        public async Task ExportEntityStatusesToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/entitystatuses/excel") : "export/clearconnection/entitystatuses/excel", true);
        }

        public async Task ExportEntityStatusesToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/entitystatuses/csv") : "export/clearconnection/entitystatuses/csv", true);
        }

        partial void OnEntityStatusesRead(ref IQueryable<Models.ClearConnection.EntityStatus> items);

        public async Task<IQueryable<Models.ClearConnection.EntityStatus>> GetEntityStatuses(Query query = null)
        {
            var items = context.EntityStatuses.AsQueryable();

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

            OnEntityStatusesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnEntityStatusCreated(Models.ClearConnection.EntityStatus item);

        public async Task<Models.ClearConnection.EntityStatus> CreateEntityStatus(Models.ClearConnection.EntityStatus entityStatus)
        {
            OnEntityStatusCreated(entityStatus);

            context.EntityStatuses.Add(entityStatus);
            context.SaveChanges();

            return entityStatus;
        }
        public async Task ExportEscalationLevelsToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/escalationlevels/excel") : "export/clearconnection/escalationlevels/excel", true);
        }

        public async Task ExportEscalationLevelsToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/escalationlevels/csv") : "export/clearconnection/escalationlevels/csv", true);
        }

        partial void OnEscalationLevelsRead(ref IQueryable<Models.ClearConnection.EscalationLevel> items);

        public async Task<IQueryable<Models.ClearConnection.EscalationLevel>> GetEscalationLevels(Query query = null)
        {
            var items = context.EscalationLevels.AsQueryable();

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

            OnEscalationLevelsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnEscalationLevelCreated(Models.ClearConnection.EscalationLevel item);

        public async Task<Models.ClearConnection.EscalationLevel> CreateEscalationLevel(Models.ClearConnection.EscalationLevel escalationLevel)
        {
            OnEscalationLevelCreated(escalationLevel);

            context.EscalationLevels.Add(escalationLevel);
            context.SaveChanges();

            return escalationLevel;
        }
        public async Task ExportHazardMaterialValuesToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/hazardmaterialvalues/excel") : "export/clearconnection/hazardmaterialvalues/excel", true);
        }

        public async Task ExportHazardMaterialValuesToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/hazardmaterialvalues/csv") : "export/clearconnection/hazardmaterialvalues/csv", true);
        }

        partial void OnHazardMaterialValuesRead(ref IQueryable<Models.ClearConnection.HazardMaterialValue> items);

        public async Task<IQueryable<Models.ClearConnection.HazardMaterialValue>> GetHazardMaterialValues(Query query = null)
        {
            var items = context.HazardMaterialValues.AsQueryable();

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

            OnHazardMaterialValuesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnHazardMaterialValueCreated(Models.ClearConnection.HazardMaterialValue item);

        public async Task<Models.ClearConnection.HazardMaterialValue> CreateHazardMaterialValue(Models.ClearConnection.HazardMaterialValue hazardMaterialValue)
        {
            OnHazardMaterialValueCreated(hazardMaterialValue);

            context.HazardMaterialValues.Add(hazardMaterialValue);
            context.SaveChanges();

            return hazardMaterialValue;
        }
        public async Task ExportHazardValuesToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/hazardvalues/excel") : "export/clearconnection/hazardvalues/excel", true);
        }

        public async Task ExportHazardValuesToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/hazardvalues/csv") : "export/clearconnection/hazardvalues/csv", true);
        }

        partial void OnHazardValuesRead(ref IQueryable<Models.ClearConnection.HazardValue> items);

        public async Task<IQueryable<Models.ClearConnection.HazardValue>> GetHazardValues(Query query = null)
        {
            var items = context.HazardValues.AsQueryable();

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

            OnHazardValuesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnHazardValueCreated(Models.ClearConnection.HazardValue item);

        public async Task<Models.ClearConnection.HazardValue> CreateHazardValue(Models.ClearConnection.HazardValue hazardValue)
        {
            OnHazardValueCreated(hazardValue);

            context.HazardValues.Add(hazardValue);
            context.SaveChanges();

            return hazardValue;
        }
        public async Task ExportHighRiskCategoriesToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/highriskcategories/excel") : "export/clearconnection/highriskcategories/excel", true);
        }

        public async Task ExportHighRiskCategoriesToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/highriskcategories/csv") : "export/clearconnection/highriskcategories/csv", true);
        }

        partial void OnHighRiskCategoriesRead(ref IQueryable<Models.ClearConnection.HighRiskCategory> items);

        public async Task<IQueryable<Models.ClearConnection.HighRiskCategory>> GetHighRiskCategories(Query query = null)
        {
            var items = context.HighRiskCategories.AsQueryable();

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

            OnHighRiskCategoriesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnHighRiskCategoryCreated(Models.ClearConnection.HighRiskCategory item);

        public async Task<Models.ClearConnection.HighRiskCategory> CreateHighRiskCategory(Models.ClearConnection.HighRiskCategory highRiskCategory)
        {
            OnHighRiskCategoryCreated(highRiskCategory);

            context.HighRiskCategories.Add(highRiskCategory);
            context.SaveChanges();

            return highRiskCategory;
        }
        public async Task ExportImpactTypesToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/impacttypes/excel") : "export/clearconnection/impacttypes/excel", true);
        }

        public async Task ExportImpactTypesToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/impacttypes/csv") : "export/clearconnection/impacttypes/csv", true);
        }
        partial void OnImpactTypesRead(ref IQueryable<Models.ClearConnection.ImpactType> items);

        public async Task<IQueryable<Models.ClearConnection.ImpactType>> GetImpactTypes(Query query = null)
        {
            var items = context.ImpactTypes.AsQueryable();

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

            OnImpactTypesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnImpactTypeCreated(Models.ClearConnection.ImpactType item);

        public async Task<Models.ClearConnection.ImpactType> CreateImpactType(Models.ClearConnection.ImpactType impactType)
        {
            OnImpactTypeCreated(impactType);

            context.ImpactTypes.Add(impactType);
            context.SaveChanges();

            return impactType;
        }
        public async Task ExportIndustryTypesToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/industrytypes/excel") : "export/clearconnection/industrytypes/excel", true);
        }

        public async Task ExportIndustryTypesToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/industrytypes/csv") : "export/clearconnection/industrytypes/csv", true);
        }

        partial void OnIndustryTypesRead(ref IQueryable<Models.ClearConnection.IndustryType> items);

        public async Task<IQueryable<Models.ClearConnection.IndustryType>> GetIndustryTypes(Query query = null)
        {
            var items = context.IndustryTypes.AsQueryable();

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

            OnIndustryTypesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnIndustryTypeCreated(Models.ClearConnection.IndustryType item);

        public async Task<Models.ClearConnection.IndustryType> CreateIndustryType(Models.ClearConnection.IndustryType industryType)
        {
            OnIndustryTypeCreated(industryType);

            context.IndustryTypes.Add(industryType);
            context.SaveChanges();

            return industryType;
        }
        public async Task ExportLicencePermitsToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/licencepermits/excel") : "export/clearconnection/licencepermits/excel", true);
        }

        public async Task ExportLicencePermitsToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/licencepermits/csv") : "export/clearconnection/licencepermits/csv", true);
        }

        partial void OnLicencePermitsRead(ref IQueryable<Models.ClearConnection.LicencePermit> items);

        public async Task<IQueryable<Models.ClearConnection.LicencePermit>> GetLicencePermits(Query query = null)
        {
            var items = context.LicencePermits.AsQueryable();

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

            OnLicencePermitsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnLicencePermitCreated(Models.ClearConnection.LicencePermit item);

        public async Task<Models.ClearConnection.LicencePermit> CreateLicencePermit(Models.ClearConnection.LicencePermit licencePermit)
        {
            OnLicencePermitCreated(licencePermit);

            context.LicencePermits.Add(licencePermit);
            context.SaveChanges();

            return licencePermit;
        }
        public async Task ExportPeopleToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/people/excel") : "export/clearconnection/people/excel", true);
        }

        public async Task ExportPeopleToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/people/csv") : "export/clearconnection/people/csv", true);
        }

        partial void OnPeopleRead(ref IQueryable<Models.ClearConnection.Person> items);

        public async Task<IQueryable<Models.ClearConnection.Person>> GetPeople(Query query = null)
        {
            var items = context.People.AsQueryable();

            items = items.Include(i => i.Person1);

            items = items.Include(i => i.State);

            items = items.Include(i => i.Country);

            items = items.Include(i => i.State1);

            items = items.Include(i => i.Country1);

            items = items.Include(i => i.PersonType);

            items = items.Include(i => i.Applicence);

            items = items.Include(i => i.Applicence).ThenInclude(c => c.Currency);

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

            OnPeopleRead(ref items);

            return await Task.FromResult(items);
        }


        //============================================================================================================================

        public async Task<IQueryable<Models.ClearConnection.Person>> GetAllPerson(Query query = null)
        {
            var items = context.People.AsQueryable();

            items = items.Include(i => i.Person1);

            items = items.Include(i => i.State);

            items = items.Include(i => i.Country);

            items = items.Include(i => i.State1);

            items = items.Include(i => i.Country1);

            items = items.Include(i => i.PersonType);

            items = items.Include(i => i.Applicence);

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

            OnPeopleRead(ref items);

            return await Task.FromResult(items);
        }

        //===========================================================================================================================


        public async Task<IQueryable<Models.ClearConnection.Person>> GetEmployee(Query query = null)
        {
            var items = context.People.AsQueryable().Where(p => p.COMPANYTYPE == 3);

            items = items.Include(i => i.Person1);

            items = items.Include(i => i.State);

            items = items.Include(i => i.Country);

            items = items.Include(i => i.State1);

            items = items.Include(i => i.Country1);

            items = items.Include(i => i.PersonType);

            items = items.Include(i => i.Applicence);

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

            OnPeopleRead(ref items);

            return await Task.FromResult(items);
        }


        public async Task<IQueryable<Models.ClearConnection.Person>> GetClients(Query query = null)
        {
            var items = context.People.AsQueryable().Where(p => p.COMPANYTYPE == 4);

            items = items.Include(i => i.Person1);

            items = items.Include(i => i.State);

            items = items.Include(i => i.Country);

            items = items.Include(i => i.State1);

            items = items.Include(i => i.Country1);

            items = items.Include(i => i.PersonType);

            items = items.Include(i => i.Applicence);

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

            OnPeopleRead(ref items);

            return await Task.FromResult(items);
        }

        public async Task<IQueryable<Models.ClearConnection.Person>> GetContractors(Query query = null)
        {
            var items = context.People.AsQueryable().Where(p => p.COMPANYTYPE == 5);

            items = items.Include(i => i.Person1);

            items = items.Include(i => i.State);

            items = items.Include(i => i.Country);

            items = items.Include(i => i.State1);

            items = items.Include(i => i.Country1);

            items = items.Include(i => i.PersonType);

            items = items.Include(i => i.Applicence);

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

            OnPeopleRead(ref items);

            return await Task.FromResult(items);
        }

        public async Task<IQueryable<Models.ClearConnection.Person>> GetEmployee(int companyId, Query query = null)
        {
            var items = context.People.AsQueryable().Where(p => p.COMPANYTYPE == 3 && p.PARENT_PERSON_ID == companyId);

            items = items.Include(i => i.Person1);

            items = items.Include(i => i.State);

            items = items.Include(i => i.Country);

            items = items.Include(i => i.State1);

            items = items.Include(i => i.Country1);

            items = items.Include(i => i.PersonType);

            items = items.Include(i => i.Applicence);

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

            OnPeopleRead(ref items);

            return await Task.FromResult(items);
        }

        public async Task<IQueryable<Models.ClearConnection.Person>> GetClients(int companyId, Query query = null)
        {
            var items = context.People.AsQueryable().Where(p => p.COMPANYTYPE == 4 && p.PARENT_PERSON_ID == companyId);

            items = items.Include(i => i.Person1);

            items = items.Include(i => i.State);

            items = items.Include(i => i.Country);

            items = items.Include(i => i.State1);

            items = items.Include(i => i.Country1);

            items = items.Include(i => i.PersonType);

            items = items.Include(i => i.Applicence);

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

            OnPeopleRead(ref items);

            return await Task.FromResult(items);
        }


        public async Task<IQueryable<Models.ClearConnection.Person>> GetContractors(int companyId, Query query = null)
        {
            var items = context.People.AsQueryable().Where(p => p.COMPANYTYPE == 5 && p.PARENT_PERSON_ID == companyId);

            items = items.Include(i => i.Person1);

            items = items.Include(i => i.State);

            items = items.Include(i => i.Country);

            items = items.Include(i => i.State1);

            items = items.Include(i => i.Country1);

            items = items.Include(i => i.PersonType);

            items = items.Include(i => i.Applicence);

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

            OnPeopleRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnPersonCreated(Models.ClearConnection.Person item);

        public async Task<Models.ClearConnection.Person> CreatePerson(Models.ClearConnection.Person person)
        {
            OnPersonCreated(person);

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.People.Add(person);
                    context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }



            return person;
        }
        public async Task ExportPersonRolesToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/personroles/excel") : "export/clearconnection/personroles/excel", true);
        }

        public async Task ExportPersonRolesToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/personroles/csv") : "export/clearconnection/personroles/csv", true);
        }

        partial void OnPersonRolesRead(ref IQueryable<Models.ClearConnection.PersonRole> items);

        public async Task<IQueryable<Models.ClearConnection.PersonRole>> GetPersonRoles(Query query = null)
        {
            var items = context.PersonRoles.AsQueryable();

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

            OnPersonRolesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnPersonRoleCreated(Models.ClearConnection.PersonRole item);

        public async Task<Models.ClearConnection.PersonRole> CreatePersonRole(Models.ClearConnection.PersonRole personRole)
        {
            OnPersonRoleCreated(personRole);

            context.PersonRoles.Add(personRole);
            context.SaveChanges();

            return personRole;
        }
        public async Task ExportPersonSitesToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/personsites/excel") : "export/clearconnection/personsites/excel", true);
        }

        public async Task ExportPersonSitesToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/personsites/csv") : "export/clearconnection/personsites/csv", true);
        }

        partial void OnPersonSitesRead(ref IQueryable<Models.ClearConnection.PersonSite> items);

        public async Task<IQueryable<Models.ClearConnection.PersonSite>> GetPersonSites(Query query = null)
        {
            var items = context.PersonSites.AsQueryable();

            items = items.Include(i => i.Person);

            items = items.Include(i => i.State);

            items = items.Include(i => i.Country);

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

            OnPersonSitesRead(ref items);

            return await Task.FromResult(items);
        }


        public async Task<IQueryable<Models.ClearConnection.PersonSite>> GetCompanySites(int companyId, Query query = null)
        {
            var items = context.PersonSites.AsQueryable().Where(ps => ps.PERSON_ID == companyId);

            items = items.Include(i => i.Person);

            items = items.Include(i => i.State);

            items = items.Include(i => i.Country);

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

            OnPersonSitesRead(ref items);

            return await Task.FromResult(items);
        }
        public async Task<IQueryable<Models.ClearConnection.PersonSite>> GetPersonSites(int personId, Query query = null)
        {
            var items = context.PersonSites.AsQueryable().Where(x => x.PERSON_ID == personId);

            items = items.Include(i => i.Person);

            items = items.Include(i => i.State);

            items = items.Include(i => i.Country);

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

            OnPersonSitesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnPersonSiteCreated(Models.ClearConnection.PersonSite item);

        public async Task<Models.ClearConnection.PersonSite> CreatePersonSite(Models.ClearConnection.PersonSite personSite)
        {
            OnPersonSiteCreated(personSite);

            context.PersonSites.Add(personSite);
            context.SaveChanges();

            return personSite;
        }
        public async Task ExportPersonTypesToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/persontypes/excel") : "export/clearconnection/persontypes/excel", true);
        }

        public async Task ExportPersonTypesToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/persontypes/csv") : "export/clearconnection/persontypes/csv", true);
        }

        partial void OnPersonTypesRead(ref IQueryable<Models.ClearConnection.PersonType> items);

        public async Task<IQueryable<Models.ClearConnection.PersonType>> GetPersonTypes(Query query = null)
        {
            var items = context.PersonTypes.AsQueryable();

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

            OnPersonTypesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnPersonTypeCreated(Models.ClearConnection.PersonType item);

        public async Task<Models.ClearConnection.PersonType> CreatePersonType(Models.ClearConnection.PersonType personType)
        {
            OnPersonTypeCreated(personType);

            context.PersonTypes.Add(personType);
            context.SaveChanges();

            return personType;
        }
        public async Task ExportPlantEquipmentsToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/plantequipments/excel") : "export/clearconnection/plantequipments/excel", true);
        }

        public async Task ExportPlantEquipmentsToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/plantequipments/csv") : "export/clearconnection/plantequipments/csv", true);
        }

        partial void OnPlantEquipmentsRead(ref IQueryable<Models.ClearConnection.PlantEquipment> items);

        public async Task<IQueryable<Models.ClearConnection.PlantEquipment>> GetPlantEquipments(Query query = null)
        {
            var items = context.PlantEquipments.AsQueryable();

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

            OnPlantEquipmentsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnPlantEquipmentCreated(Models.ClearConnection.PlantEquipment item);

        public async Task<Models.ClearConnection.PlantEquipment> CreatePlantEquipment(Models.ClearConnection.PlantEquipment plantEquipment)
        {
            OnPlantEquipmentCreated(plantEquipment);

            context.PlantEquipments.Add(plantEquipment);
            context.SaveChanges();

            return plantEquipment;
        }
        public async Task ExportPpevaluesToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/ppevalues/excel") : "export/clearconnection/ppevalues/excel", true);
        }

        public async Task ExportPpevaluesToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/ppevalues/csv") : "export/clearconnection/ppevalues/csv", true);
        }

        partial void OnPpevaluesRead(ref IQueryable<Models.ClearConnection.Ppevalue> items);

        public async Task<IQueryable<Models.ClearConnection.Ppevalue>> GetPpevalues(Query query = null)
        {
            var items = context.Ppevalues.AsQueryable();

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

            OnPpevaluesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnPpevalueCreated(Models.ClearConnection.Ppevalue item);

        public async Task<Models.ClearConnection.Ppevalue> CreatePpevalue(Models.ClearConnection.Ppevalue ppevalue)
        {
            OnPpevalueCreated(ppevalue);

            context.Ppevalues.Add(ppevalue);
            context.SaveChanges();

            return ppevalue;
        }
        public async Task ExportReferencedLegislationsToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/referencedlegislations/excel") : "export/clearconnection/referencedlegislations/excel", true);
        }

        public async Task ExportReferencedLegislationsToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/referencedlegislations/csv") : "export/clearconnection/referencedlegislations/csv", true);
        }

        partial void OnReferencedLegislationsRead(ref IQueryable<Models.ClearConnection.ReferencedLegislation> items);

        public async Task<IQueryable<Models.ClearConnection.ReferencedLegislation>> GetReferencedLegislations(Query query = null)
        {
            var items = context.ReferencedLegislations.AsQueryable();

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

            OnReferencedLegislationsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnReferencedLegislationCreated(Models.ClearConnection.ReferencedLegislation item);

        public async Task<Models.ClearConnection.ReferencedLegislation> CreateReferencedLegislation(Models.ClearConnection.ReferencedLegislation referencedLegislation)
        {
            OnReferencedLegislationCreated(referencedLegislation);

            context.ReferencedLegislations.Add(referencedLegislation);
            context.SaveChanges();

            return referencedLegislation;
        }
        public async Task ExportResposnsibleTypesToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/resposnsibletypes/excel") : "export/clearconnection/resposnsibletypes/excel", true);
        }

        public async Task ExportResposnsibleTypesToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/resposnsibletypes/csv") : "export/clearconnection/resposnsibletypes/csv", true);
        }

        partial void OnResposnsibleTypesRead(ref IQueryable<Models.ClearConnection.ResposnsibleType> items);

        public async Task<IQueryable<Models.ClearConnection.ResposnsibleType>> GetResposnsibleTypes(Query query = null)
        {
            var items = context.ResposnsibleTypes.AsQueryable();

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

            OnResposnsibleTypesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnResposnsibleTypeCreated(Models.ClearConnection.ResposnsibleType item);

        public async Task<Models.ClearConnection.ResposnsibleType> CreateResposnsibleType(Models.ClearConnection.ResposnsibleType resposnsibleType)
        {
            OnResposnsibleTypeCreated(resposnsibleType);

            context.ResposnsibleTypes.Add(resposnsibleType);
            context.SaveChanges();

            return resposnsibleType;
        }
        public async Task ExportRiskLikelyhoodsToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/risklikelyhoods/excel") : "export/clearconnection/risklikelyhoods/excel", true);
        }

        public async Task ExportRiskLikelyhoodsToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/risklikelyhoods/csv") : "export/clearconnection/risklikelyhoods/csv", true);
        }

        partial void OnRiskLikelyhoodsRead(ref IQueryable<Models.ClearConnection.RiskLikelyhood> items);

        public async Task<IQueryable<Models.ClearConnection.RiskLikelyhood>> GetRiskLikelyhoods(Query query = null)
        {
            var items = context.RiskLikelyhoods.AsQueryable();

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

            OnRiskLikelyhoodsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnRiskLikelyhoodCreated(Models.ClearConnection.RiskLikelyhood item);

        public async Task<Models.ClearConnection.RiskLikelyhood> CreateRiskLikelyhood(Models.ClearConnection.RiskLikelyhood riskLikelyhood)
        {
            OnRiskLikelyhoodCreated(riskLikelyhood);

            context.RiskLikelyhoods.Add(riskLikelyhood);
            context.SaveChanges();

            return riskLikelyhood;
        }
        public async Task ExportScheduleTypesToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/scheduletypes/excel") : "export/clearconnection/scheduletypes/excel", true);
        }

        public async Task ExportScheduleTypesToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/scheduletypes/csv") : "export/clearconnection/scheduletypes/csv", true);
        }

        partial void OnScheduleTypesRead(ref IQueryable<Models.ClearConnection.ScheduleType> items);

        public async Task<IQueryable<Models.ClearConnection.ScheduleType>> GetScheduleTypes(Query query = null)
        {
            var items = context.ScheduleTypes.AsQueryable();

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

            OnScheduleTypesRead(ref items);

            return await Task.FromResult(items);
        }


        partial void OnScheduleTimesRead(ref IQueryable<Models.ClearConnection.ScheduleTime> items);

        public async Task<IQueryable<Models.ClearConnection.ScheduleTime>> GetScheduleTimes(Query query = null)
        {
            var items = context.ScheduleTimes.AsQueryable();

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

            OnScheduleTimesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnScheduleTypeCreated(Models.ClearConnection.ScheduleType item);

        public async Task<Models.ClearConnection.ScheduleType> CreateScheduleType(Models.ClearConnection.ScheduleType scheduleType)
        {
            OnScheduleTypeCreated(scheduleType);

            context.ScheduleTypes.Add(scheduleType);
            context.SaveChanges();

            return scheduleType;
        }
        public async Task ExportSmtpsetupsToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/smtpsetups/excel") : "export/clearconnection/smtpsetups/excel", true);
        }

        public async Task ExportSmtpsetupsToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/smtpsetups/csv") : "export/clearconnection/smtpsetups/csv", true);
        }

        partial void OnSmtpsetupsRead(ref IQueryable<Models.ClearConnection.Smtpsetup> items);

        public async Task<IQueryable<Models.ClearConnection.Smtpsetup>> GetSmtpsetups(Query query = null)
        {
            var items = context.Smtpsetups.AsQueryable();

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

            OnSmtpsetupsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSmtpsetupCreated(Models.ClearConnection.Smtpsetup item);

        public async Task<Models.ClearConnection.Smtpsetup> CreateSmtpsetup(Models.ClearConnection.Smtpsetup smtpsetup)
        {
            OnSmtpsetupCreated(smtpsetup);

            context.Smtpsetups.Add(smtpsetup);
            context.SaveChanges();

            return smtpsetup;
        }
        public async Task ExportStatesToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/states/excel") : "export/clearconnection/states/excel", true);
        }

        public async Task ExportStatesToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/states/csv") : "export/clearconnection/states/csv", true);
        }

        partial void OnStatesRead(ref IQueryable<Models.ClearConnection.State> items);

        public async Task<IQueryable<Models.ClearConnection.State>> GetStates(Query query = null)
        {
            var items = context.States.AsQueryable();

            items = items.Include(i => i.Country);

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

            OnStatesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStateCreated(Models.ClearConnection.State item);

        public async Task<Models.ClearConnection.State> CreateState(Models.ClearConnection.State state)
        {
            OnStateCreated(state);

            context.States.Add(state);
            context.SaveChanges();

            return state;
        }
        public async Task ExportStatusLevelsToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/statuslevels/excel") : "export/clearconnection/statuslevels/excel", true);
        }

        public async Task ExportStatusLevelsToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/statuslevels/csv") : "export/clearconnection/statuslevels/csv", true);
        }

        partial void OnStatusLevelsRead(ref IQueryable<Models.ClearConnection.StatusLevel> items);

        public async Task<IQueryable<Models.ClearConnection.StatusLevel>> GetStatusLevels(Query query = null)
        {
            var items = context.StatusLevels.AsQueryable();

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

            OnStatusLevelsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStatusLevelCreated(Models.ClearConnection.StatusLevel item);

        public async Task<Models.ClearConnection.StatusLevel> CreateStatusLevel(Models.ClearConnection.StatusLevel statusLevel)
        {
            OnStatusLevelCreated(statusLevel);

            context.StatusLevels.Add(statusLevel);
            context.SaveChanges();

            return statusLevel;
        }
        public async Task ExportStatusMastersToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/statusmasters/excel") : "export/clearconnection/statusmasters/excel", true);
        }

        public async Task ExportStatusMastersToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/statusmasters/csv") : "export/clearconnection/statusmasters/csv", true);
        }

        partial void OnStatusMastersRead(ref IQueryable<Models.ClearConnection.StatusMaster> items);

        public async Task<IQueryable<Models.ClearConnection.StatusMaster>> GetStatusMasters(Query query = null)
        {
            var items = context.StatusMasters.AsQueryable();

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

            OnStatusMastersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStatusMasterCreated(Models.ClearConnection.StatusMaster item);

        public async Task<Models.ClearConnection.StatusMaster> CreateStatusMaster(Models.ClearConnection.StatusMaster statusMaster)
        {
            OnStatusMasterCreated(statusMaster);

            context.StatusMasters.Add(statusMaster);
            context.SaveChanges();

            return statusMaster;
        }
        public async Task ExportSwmsHazardousmaterialsToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/swmshazardousmaterials/excel") : "export/clearconnection/swmshazardousmaterials/excel", true);
        }

        public async Task ExportSwmsHazardousmaterialsToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/swmshazardousmaterials/csv") : "export/clearconnection/swmshazardousmaterials/csv", true);
        }

        partial void OnSwmsHazardousmaterialsRead(ref IQueryable<Models.ClearConnection.SwmsHazardousmaterial> items);

        public async Task<IQueryable<Models.ClearConnection.SwmsHazardousmaterial>> GetSwmsHazardousmaterials(Query query = null)
        {
            var items = context.SwmsHazardousmaterials.AsQueryable();

            items = items.Include(i => i.SwmsTemplate);

            items = items.Include(i => i.HazardMaterialValue);

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

            OnSwmsHazardousmaterialsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSwmsHazardousmaterialCreated(Models.ClearConnection.SwmsHazardousmaterial item);

        public async Task<Models.ClearConnection.SwmsHazardousmaterial> CreateSwmsHazardousmaterial(Models.ClearConnection.SwmsHazardousmaterial swmsHazardousmaterial)
        {
            OnSwmsHazardousmaterialCreated(swmsHazardousmaterial);

            context.SwmsHazardousmaterials.Add(swmsHazardousmaterial);
            context.SaveChanges();

            return swmsHazardousmaterial;
        }
        public async Task ExportSwmsLicencespermitsToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/swmslicencespermits/excel") : "export/clearconnection/swmslicencespermits/excel", true);
        }

        public async Task ExportSwmsLicencespermitsToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/swmslicencespermits/csv") : "export/clearconnection/swmslicencespermits/csv", true);
        }

        partial void OnSwmsLicencespermitsRead(ref IQueryable<Models.ClearConnection.SwmsLicencespermit> items);

        public async Task<IQueryable<Models.ClearConnection.SwmsLicencespermit>> GetSwmsLicencespermits(Query query = null)
        {
            var items = context.SwmsLicencespermits.AsQueryable();

            items = items.Include(i => i.SwmsTemplate);

            items = items.Include(i => i.LicencePermit);

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

            OnSwmsLicencespermitsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSwmsLicencespermitCreated(Models.ClearConnection.SwmsLicencespermit item);

        public async Task<Models.ClearConnection.SwmsLicencespermit> CreateSwmsLicencespermit(Models.ClearConnection.SwmsLicencespermit swmsLicencespermit)
        {
            OnSwmsLicencespermitCreated(swmsLicencespermit);

            context.SwmsLicencespermits.Add(swmsLicencespermit);
            context.SaveChanges();

            return swmsLicencespermit;
        }
        public async Task ExportSwmsPlantequipmentsToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/swmsplantequipments/excel") : "export/clearconnection/swmsplantequipments/excel", true);
        }

        public async Task ExportSwmsPlantequipmentsToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/swmsplantequipments/csv") : "export/clearconnection/swmsplantequipments/csv", true);
        }

        partial void OnSwmsPlantequipmentsRead(ref IQueryable<Models.ClearConnection.SwmsPlantequipment> items);

        public async Task<IQueryable<Models.ClearConnection.SwmsPlantequipment>> GetSwmsPlantequipments(Query query = null)
        {
            var items = context.SwmsPlantequipments.AsQueryable();

            items = items.Include(i => i.SwmsTemplate);

            items = items.Include(i => i.PlantEquipment);

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

            OnSwmsPlantequipmentsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSwmsPlantequipmentCreated(Models.ClearConnection.SwmsPlantequipment item);

        public async Task<Models.ClearConnection.SwmsPlantequipment> CreateSwmsPlantequipment(Models.ClearConnection.SwmsPlantequipment swmsPlantequipment)
        {
            OnSwmsPlantequipmentCreated(swmsPlantequipment);

            context.SwmsPlantequipments.Add(swmsPlantequipment);
            context.SaveChanges();

            return swmsPlantequipment;
        }
        public async Task ExportSwmsPperequiredsToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/swmspperequireds/excel") : "export/clearconnection/swmspperequireds/excel", true);
        }

        public async Task ExportSwmsPperequiredsToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/swmspperequireds/csv") : "export/clearconnection/swmspperequireds/csv", true);
        }

        partial void OnSwmsPperequiredsRead(ref IQueryable<Models.ClearConnection.SwmsPperequired> items);

        public async Task<IQueryable<Models.ClearConnection.SwmsPperequired>> GetSwmsPperequireds(Query query = null)
        {
            var items = context.SwmsPperequireds.AsQueryable();

            items = items.Include(i => i.SwmsTemplate);

            items = items.Include(i => i.Ppevalue);

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

            OnSwmsPperequiredsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSwmsPperequiredCreated(Models.ClearConnection.SwmsPperequired item);

        public async Task<Models.ClearConnection.SwmsPperequired> CreateSwmsPperequired(Models.ClearConnection.SwmsPperequired swmsPperequired)
        {
            OnSwmsPperequiredCreated(swmsPperequired);

            context.SwmsPperequireds.Add(swmsPperequired);
            context.SaveChanges();

            return swmsPperequired;
        }
        public async Task ExportSwmsReferencedlegislationsToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/swmsreferencedlegislations/excel") : "export/clearconnection/swmsreferencedlegislations/excel", true);
        }

        public async Task ExportSwmsReferencedlegislationsToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/swmsreferencedlegislations/csv") : "export/clearconnection/swmsreferencedlegislations/csv", true);
        }

        partial void OnSwmsReferencedlegislationsRead(ref IQueryable<Models.ClearConnection.SwmsReferencedlegislation> items);

        public async Task<IQueryable<Models.ClearConnection.SwmsReferencedlegislation>> GetSwmsReferencedlegislations(Query query = null)
        {
            var items = context.SwmsReferencedlegislations.AsQueryable();

            items = items.Include(i => i.SwmsTemplate);

            items = items.Include(i => i.ReferencedLegislation);

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

            OnSwmsReferencedlegislationsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSwmsReferencedlegislationCreated(Models.ClearConnection.SwmsReferencedlegislation item);

        public async Task<Models.ClearConnection.SwmsReferencedlegislation> CreateSwmsReferencedlegislation(Models.ClearConnection.SwmsReferencedlegislation swmsReferencedlegislation)
        {
            OnSwmsReferencedlegislationCreated(swmsReferencedlegislation);

            context.SwmsReferencedlegislations.Add(swmsReferencedlegislation);
            context.SaveChanges();

            return swmsReferencedlegislation;
        }
        public async Task ExportSwmsSectionsToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/swmssections/excel") : "export/clearconnection/swmssections/excel", true);
        }

        public async Task ExportSwmsSectionsToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/swmssections/csv") : "export/clearconnection/swmssections/csv", true);
        }

        partial void OnSwmsSectionsRead(ref IQueryable<Models.ClearConnection.SwmsSection> items);

        public async Task<IQueryable<Models.ClearConnection.SwmsSection>> GetSwmsSections(Query query = null)
        {
            var items = context.SwmsSections.AsQueryable();

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

            OnSwmsSectionsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSwmsSectionCreated(Models.ClearConnection.SwmsSection item);

        public async Task<Models.ClearConnection.SwmsSection> CreateSwmsSection(Models.ClearConnection.SwmsSection swmsSection)
        {
            OnSwmsSectionCreated(swmsSection);

            context.SwmsSections.Add(swmsSection);
            context.SaveChanges();

            return swmsSection;
        }
        public async Task ExportSwmsTemplatesToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/swmstemplates/excel") : "export/clearconnection/swmstemplates/excel", true);
        }

        public async Task ExportSwmsTemplatesToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/swmstemplates/csv") : "export/clearconnection/swmstemplates/csv", true);
        }

        partial void OnSwmsTemplatesRead(ref IQueryable<Models.ClearConnection.SwmsTemplate> items);

        public async Task<IQueryable<Models.ClearConnection.SwmsTemplate>> GetSwmsTemplates(Query query = null)
        {
            var items = context.SwmsTemplates.AsQueryable();

            items = items.Include(i => i.Person);

            items = items.Include(i => i.TemplateType);

            items = items.Include(i => i.Template);

            items = items.Include(i => i.StatusMaster);

            items = items.Include(i => i.SwmsTemplateCategory);

            items = items.Include(i => i.Country);

            items = items.Include(i => i.EscalationLevel);

            items = items.Include(i => i.WarningLevel);

            items = items.Include(i => i.StatusLevel);

            items = items.Include(i => i.State);

            items = items.Include(i => i.TradeCategory);



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

            OnSwmsTemplatesRead(ref items);

            return await Task.FromResult(items);
        }


        public async Task<IQueryable<Models.ClearConnection.SwmsTemplate>> GetLimitedSwmsTemplates(Query query = null)
        {
            var items = context.SwmsTemplates.AsQueryable();





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

            OnSwmsTemplatesRead(ref items);

            return await Task.FromResult(items);
        }

        public async Task<IQueryable<Models.ClearConnection.SwmsTemplate>> GetSwmsTemplatestALll(Query query = null)
        {
            var items = context.SwmsTemplates.AsQueryable();

            items = items.Include(i => i.Person);

            items = items.Include(i => i.TemplateType);

            items = items.Include(i => i.Template);

            items = items.Include(i => i.StatusMaster);

            items = items.Include(i => i.SwmsTemplateCategory);

            items = items.Include(i => i.Country);

            items = items.Include(i => i.EscalationLevel);

            items = items.Include(i => i.WarningLevel);

            items = items.Include(i => i.StatusLevel);

            items = items.Include(i => i.State);

            items = items.Include(i => i.TradeCategory);
            items = items.Include(i => i.SwmsPperequireds)
                  .Include(i => i.SwmsHazardousmaterials)
                  .Include(i => i.SwmsLicencespermits)
                  .Include(i => i.SwmsPlantequipments)
                  .Include(i => i.SwmsReferencedlegislations)
                  .Include(i => i.SwmsTemplatesteps);



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

            OnSwmsTemplatesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSwmsTemplateCreated(Models.ClearConnection.SwmsTemplate item);

        public async Task<Models.ClearConnection.SwmsTemplate> CreateSwmsTemplate(Models.ClearConnection.SwmsTemplate swmsTemplate)
        {
            OnSwmsTemplateCreated(swmsTemplate);

            context.SwmsTemplates.Add(swmsTemplate);
            context.SaveChanges();

            return swmsTemplate;
        }
        public async Task ExportSwmsTemplateCategoriesToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/swmstemplatecategories/excel") : "export/clearconnection/swmstemplatecategories/excel", true);
        }

        public async Task ExportSwmsTemplateCategoriesToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/swmstemplatecategories/csv") : "export/clearconnection/swmstemplatecategories/csv", true);
        }

        partial void OnSwmsTemplateCategoriesRead(ref IQueryable<Models.ClearConnection.SwmsTemplateCategory> items);

        public async Task<IQueryable<Models.ClearConnection.SwmsTemplateCategory>> GetSwmsTemplateCategories(Query query = null)
        {
            var items = context.SwmsTemplateCategories.AsQueryable();

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

            OnSwmsTemplateCategoriesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSwmsTemplateCategoryCreated(Models.ClearConnection.SwmsTemplateCategory item);

        public async Task<Models.ClearConnection.SwmsTemplateCategory> CreateSwmsTemplateCategory(Models.ClearConnection.SwmsTemplateCategory swmsTemplateCategory)
        {
            OnSwmsTemplateCategoryCreated(swmsTemplateCategory);

            context.SwmsTemplateCategories.Add(swmsTemplateCategory);
            context.SaveChanges();

            return swmsTemplateCategory;
        }
        public async Task ExportSwmsTemplatestepsToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/swmstemplatesteps/excel") : "export/clearconnection/swmstemplatesteps/excel", true);
        }

        public async Task ExportSwmsTemplatestepsToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/swmstemplatesteps/csv") : "export/clearconnection/swmstemplatesteps/csv", true);
        }

        partial void OnSwmsTemplatestepsRead(ref IQueryable<Models.ClearConnection.SwmsTemplatestep> items);

        public async Task<IQueryable<Models.ClearConnection.SwmsTemplatestep>> GetSwmsTemplatesteps(Query query = null)
        {
            var items = context.SwmsTemplatesteps.AsQueryable();

            items = items.Include(i => i.SwmsTemplate);

            items = items.Include(i => i.RiskLikelyhood);

            items = items.Include(i => i.Consequence);

            items = items.Include(i => i.RiskLikelyhood1);

            items = items.Include(i => i.Consequence1);

            items = items.Include(i => i.ResposnsibleType);

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

            OnSwmsTemplatestepsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSwmsTemplatestepCreated(Models.ClearConnection.SwmsTemplatestep item);

        public async Task<Models.ClearConnection.SwmsTemplatestep> CreateSwmsTemplatestep(Models.ClearConnection.SwmsTemplatestep swmsTemplatestep)
        {
            OnSwmsTemplatestepCreated(swmsTemplatestep);

            context.SwmsTemplatesteps.Add(swmsTemplatestep);
            context.SaveChanges();

            return swmsTemplatestep;
        }
        public async Task ExportSystemrolesToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/systemroles/excel") : "export/clearconnection/systemroles/excel", true);
        }

        public async Task ExportSystemrolesToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/systemroles/csv") : "export/clearconnection/systemroles/csv", true);
        }

        partial void OnSystemrolesRead(ref IQueryable<Models.ClearConnection.Systemrole> items);

        public async Task<IQueryable<Models.ClearConnection.Systemrole>> GetSystemroles(Query query = null)
        {
            var items = context.Systemroles.AsQueryable();

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

            OnSystemrolesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSystemroleCreated(Models.ClearConnection.Systemrole item);

        public async Task<Models.ClearConnection.Systemrole> CreateSystemrole(Models.ClearConnection.Systemrole systemrole)
        {
            OnSystemroleCreated(systemrole);

            context.Systemroles.Add(systemrole);
            context.SaveChanges();

            return systemrole;
        }
        public async Task ExportTemplatesToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/templates/excel") : "export/clearconnection/templates/excel", true);
        }

        public async Task ExportTemplatesToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/templates/csv") : "export/clearconnection/templates/csv", true);
        }

        partial void OnTemplatesRead(ref IQueryable<Models.ClearConnection.Template> items);

        public async Task<IQueryable<Models.ClearConnection.Template>> GetTemplates(Query query = null)
        {
            var items = context.Templates.AsQueryable();

            items = items.Include(i => i.StatusMaster);

            items = items.Include(i => i.TradeCategory);

            items = items.Include(i => i.Person);

            items = items.Include(i => i.SwmsTemplateCategory);

            items = items.Include(i => i.Country);

            items = items.Include(i => i.EscalationLevel);

            items = items.Include(i => i.WarningLevel);

            items = items.Include(i => i.StatusLevel);

            items = items.Include(i => i.State);

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

            OnTemplatesRead(ref items);

            return await Task.FromResult(items);
        }

        public async Task<IQueryable<Models.ClearConnection.Template>> GetTemplateList(Query query = null)
        {
            var items = context.Templates.AsQueryable();

            items = items.Include(i => i.Templateattachments);






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

            OnTemplatesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTemplateCreated(Models.ClearConnection.Template item);

        public async Task<Models.ClearConnection.Template> CreateTemplate(Models.ClearConnection.Template template)
        {
            OnTemplateCreated(template);

            context.Templates.Add(template);
            context.SaveChanges();

            return template;
        }
        public async Task ExportTemplateTypesToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/templatetypes/excel") : "export/clearconnection/templatetypes/excel", true);
        }

        public async Task ExportTemplateTypesToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/templatetypes/csv") : "export/clearconnection/templatetypes/csv", true);
        }

        partial void OnTemplateTypesRead(ref IQueryable<Models.ClearConnection.TemplateType> items);

        public async Task<IQueryable<Models.ClearConnection.TemplateType>> GetTemplateTypes(Query query = null)
        {
            var items = context.TemplateTypes.AsQueryable();

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

            OnTemplateTypesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTemplateTypeCreated(Models.ClearConnection.TemplateType item);

        public async Task<Models.ClearConnection.TemplateType> CreateTemplateType(Models.ClearConnection.TemplateType templateType)
        {
            OnTemplateTypeCreated(templateType);

            context.TemplateTypes.Add(templateType);
            context.SaveChanges();

            return templateType;
        }
        public async Task ExportTemplateattachmentsToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/templateattachments/excel") : "export/clearconnection/templateattachments/excel", true);
        }

        public async Task ExportTemplateattachmentsToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/templateattachments/csv") : "export/clearconnection/templateattachments/csv", true);
        }

        partial void OnTemplateattachmentsRead(ref IQueryable<Models.ClearConnection.Templateattachment> items);

        public async Task<IQueryable<Models.ClearConnection.Templateattachment>> GetTemplateattachments(Query query = null)
        {
            var items = context.Templateattachments.AsQueryable();

            items = items.Include(i => i.Template);

            items = items.Include(i => i.TemplateType);

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

            OnTemplateattachmentsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTemplateattachmentCreated(Models.ClearConnection.Templateattachment item);

        public async Task<Models.ClearConnection.Templateattachment> CreateTemplateattachment(Models.ClearConnection.Templateattachment templateattachment)
        {
            OnTemplateattachmentCreated(templateattachment);

            context.Templateattachments.Add(templateattachment);
            context.SaveChanges();

            return templateattachment;
        }
        public async Task ExportTradeCategoriesToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/tradecategories/excel") : "export/clearconnection/tradecategories/excel", true);
        }

        public async Task ExportTradeCategoriesToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/tradecategories/csv") : "export/clearconnection/tradecategories/csv", true);
        }

        partial void OnTradeCategoriesRead(ref IQueryable<Models.ClearConnection.TradeCategory> items);

        public async Task<IQueryable<Models.ClearConnection.TradeCategory>> GetTradeCategories(Query query = null)
        {
            var items = context.TradeCategories.AsQueryable();

            items = items.Include(i => i.TradeCategory1);

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

            OnTradeCategoriesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTradeCategoryCreated(Models.ClearConnection.TradeCategory item);

        public async Task<Models.ClearConnection.TradeCategory> CreateTradeCategory(Models.ClearConnection.TradeCategory tradeCategory)
        {
            OnTradeCategoryCreated(tradeCategory);

            context.TradeCategories.Add(tradeCategory);
            context.SaveChanges();

            return tradeCategory;
        }
        public async Task ExportWarningLevelsToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/warninglevels/excel") : "export/clearconnection/warninglevels/excel", true);
        }

        public async Task ExportWarningLevelsToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/clearconnection/warninglevels/csv") : "export/clearconnection/warninglevels/csv", true);
        }

        partial void OnWarningLevelsRead(ref IQueryable<Models.ClearConnection.WarningLevel> items);

        public async Task<IQueryable<Models.ClearConnection.WarningLevel>> GetWarningLevels(Query query = null)
        {
            var items = context.WarningLevels.AsQueryable();

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

            OnWarningLevelsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnWarningLevelCreated(Models.ClearConnection.WarningLevel item);

        public async Task<Models.ClearConnection.WarningLevel> CreateWarningLevel(Models.ClearConnection.WarningLevel warningLevel)
        {
            OnWarningLevelCreated(warningLevel);

            context.WarningLevels.Add(warningLevel);
            context.SaveChanges();

            return warningLevel;
        }

        partial void OnApplicenceDeleted(Models.ClearConnection.Applicence item);

        public async Task<Models.ClearConnection.Applicence> DeleteApplicence(int? applicenceid)
        {
            var item = context.Applicences
                              .Where(i => i.APPLICENCEID == applicenceid)
                              .Include(i => i.People)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnApplicenceDeleted(item);

            context.Applicences.Remove(item);

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

        partial void OnApplicenceGet(Models.ClearConnection.Applicence item);

        public async Task<Models.ClearConnection.Applicence> GetApplicenceByApplicenceid(int? applicenceid)
        {
            var items = context.Applicences
                              .AsNoTracking()
                              .Where(i => i.APPLICENCEID == applicenceid);

            var item = items.FirstOrDefault();

            OnApplicenceGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.Applicence> GetDefaultApplicence()
        {
            var items = context.Applicences
                              .AsNoTracking()
                              .Where(i => i.IS_DEFAULT == true);

            var item = items.FirstOrDefault();

            OnApplicenceGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.Applicence> GetApplicenceByCountry(int countryId)
        {
            var items = context.Applicences
                              .AsNoTracking()
                              .Where(i => i.COUNTRY_ID == countryId);

            var item = items.FirstOrDefault();

            OnApplicenceGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.Applicence> CancelApplicenceChanges(Models.ClearConnection.Applicence item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnApplicenceUpdated(Models.ClearConnection.Applicence item);

        public async Task<Models.ClearConnection.Applicence> UpdateApplicence(int? applicenceid, Models.ClearConnection.Applicence applicence)
        {
            OnApplicenceUpdated(applicence);

            var item = context.Applicences
                              .Where(i => i.APPLICENCEID == applicenceid)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(applicence);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return applicence;
        }

        partial void OnAssesmentDeleted(Models.ClearConnection.Assesment item);

        public async Task<Models.ClearConnection.Assesment> DeleteAssesment(int? assesmentid)
        {
            var item = context.Assesments
                              .Where(i => i.ASSESMENTID == assesmentid)
                              //.Include(i => i.Assesments1)
                              .Include(i => i.AssesmentAttachements)
                              .Include(i => i.AssesmentEmployees)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnAssesmentDeleted(item);

            context.Assesments.Remove(item);

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

        partial void OnAssesmentGet(Models.ClearConnection.Assesment item);

        public async Task<Models.ClearConnection.Assesment> GetAssesmentByAssesmentid(int? assesmentid)
        {
            var items = context.Assesments
                              .AsNoTracking()
                              .Where(i => i.ASSESMENTID == assesmentid);

            // items = items.Include(i => i.Assesment1);

            items = items.Include(i => i.Client);

            items = items.Include(i => i.Company);

            items = items.Include(i => i.TradeCategory);

            items = items.Include(i => i.StatusMaster);

            items = items.Include(i => i.PersonSite);

            items = items.Include(i => i.IndustryType);

            items = items.Include(i => i.Documents);

            items = items.Include(i => i.ScheduleAssesments);

            items = items.Include(i => i.WorkOrder);
            items = items.Include(i => i.AssesmentEmployees).ThenInclude(b => b.AssignedEmployees);

            var item = items.FirstOrDefault();

            OnAssesmentGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.Assesment> CopyAssesmentByAssesmentid(int? assesmentid)
        {
            var items = context.Assesments
                              .AsNoTracking()
                              .Where(i => i.ASSESMENTID == assesmentid);

            items = items.Include(i => i.AssesmentEmployees);
            items = items.Include(i => i.AssesmentAttachements);
            //items = items.Include(i => i.CompanyDocumentFile);
            items = items.Include(i => i.Documents);

            var item = items.FirstOrDefault();

            this.DetachEntity(item);
            //this.DetachEntity(item.Company);


            OnAssesmentGet(item);

            return await Task.FromResult(item);
        }

        private T DetachEntity<T>(T entity) where T : class
        {
            context.Entry(entity).State = EntityState.Detached;
            if (entity.GetType().GetProperty("Id") != null)
            {
                entity.GetType().GetProperty("Id").SetValue(entity, 0);
            }
            return entity;
        }

        private List<T> DetachEntities<T>(List<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                this.DetachEntity(entity);
            }
            return entities;
        }

        public async Task<Models.ClearConnection.Assesment> CancelAssesmentChanges(Models.ClearConnection.Assesment item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnAssesmentUpdated(Models.ClearConnection.Assesment item);

        public async Task<Models.ClearConnection.Assesment> UpdateAssesment(int? assesmentid, Models.ClearConnection.Assesment assesment)
        {
            OnAssesmentUpdated(assesment);

            var item = context.Assesments
                              .Where(i => i.ASSESMENTID == assesmentid)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(assesment);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return assesment;
        }

        partial void OnAssesmentAttachementDeleted(Models.ClearConnection.AssesmentAttachement item);

        public async Task<Models.ClearConnection.AssesmentAttachement> DeleteAssesmentAttachement(int? attachementid)
        {
            var item = context.AssesmentAttachements
                              .Where(i => i.ATTACHEMENTID == attachementid)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnAssesmentAttachementDeleted(item);

            context.AssesmentAttachements.Remove(item);

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

        partial void OnAssesmentAttachementGet(Models.ClearConnection.AssesmentAttachement item);

        public async Task<Models.ClearConnection.AssesmentAttachement> GetAssesmentAttachementByAttachementid(int? attachementid)
        {
            var items = context.AssesmentAttachements
                              .AsNoTracking()
                              .Where(i => i.ATTACHEMENTID == attachementid);

            items = items.Include(i => i.Assesment);

            items = items.Include(i => i.SwmsTemplate);

            var item = items.FirstOrDefault();

            OnAssesmentAttachementGet(item);

            return await Task.FromResult(item);
        }



        public async Task<Models.ClearConnection.AssesmentAttachement> CancelAssesmentAttachementChanges(Models.ClearConnection.AssesmentAttachement item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnAssesmentAttachementUpdated(Models.ClearConnection.AssesmentAttachement item);

        public async Task<Models.ClearConnection.AssesmentAttachement> UpdateAssesmentAttachement(int? attachementid, Models.ClearConnection.AssesmentAttachement assesmentAttachement)
        {
            OnAssesmentAttachementUpdated(assesmentAttachement);

            var item = context.AssesmentAttachements
                              .Where(i => i.ATTACHEMENTID == attachementid)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(assesmentAttachement);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return assesmentAttachement;
        }

        partial void OnAssesmentEmployeeDeleted(Models.ClearConnection.AssesmentEmployee item);

        public async Task<Models.ClearConnection.AssesmentEmployee> DeleteAssesmentEmployee(int? assesmentEmployeeId)
        {
            var item = context.AssesmentEmployees
                              .Where(i => i.ASSESMENT_EMPLOYEE_ID == assesmentEmployeeId).Include(i => i.AssignedEmployees)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnAssesmentEmployeeDeleted(item);

            context.AssesmentEmployees.Remove(item);

            try
            {
                context.SaveChanges();
                return item;
            }
            catch (Exception ex)
            {
                context.Entry(item).State = EntityState.Unchanged;
                throw ex;
            }

            //return item;
        }

        partial void OnAssesmentEmployeeGet(Models.ClearConnection.AssesmentEmployee item);

        public async Task<Models.ClearConnection.AssesmentEmployee> GetAssesmentEmployeeByAssesmentEmployeeId(int? assesmentEmployeeId)
        {
            var items = context.AssesmentEmployees
                              .AsNoTracking()
                              .Where(i => i.ASSESMENT_EMPLOYEE_ID == assesmentEmployeeId);

            items = items.Include(i => i.Assesment);

            items = items.Include(i => i.Employee);

            var item = items.FirstOrDefault();

            OnAssesmentEmployeeGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.AssesmentEmployee> CancelAssesmentEmployeeChanges(Models.ClearConnection.AssesmentEmployee item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnAssesmentEmployeeUpdated(Models.ClearConnection.AssesmentEmployee item);

        public async Task<Models.ClearConnection.AssesmentEmployee> UpdateAssesmentEmployee(int? assesmentEmployeeId, Models.ClearConnection.AssesmentEmployee assesmentEmployee)
        {
            OnAssesmentEmployeeUpdated(assesmentEmployee);

            var item = context.AssesmentEmployees
                              .Where(i => i.ASSESMENT_EMPLOYEE_ID == assesmentEmployeeId)
                              .FirstOrDefault();
            if (assesmentEmployee == null)
            {
                throw new Exception("Item no longer available");
            }

            try
            {
                var entry = context.Entry(item);
                entry.CurrentValues.SetValues(assesmentEmployee);
                entry.State = EntityState.Modified;
                //context.AssesmentEmployees.Update(assesmentEmployee);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return assesmentEmployee;
        }

        public async Task<Models.ClearConnection.AssesmentEmployee> RemoveAssesmentEmployee(int? assesmentEmployeeId)
        {
            var item = context.AssesmentEmployees
                             .Where(i => i.ASSESMENT_EMPLOYEE_ID == assesmentEmployeeId)
                             .FirstOrDefault();

            OnAssesmentEmployeeUpdated(item);

            
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            try
            {
                item.IS_ACTIVE = false;
                var entry = context.Entry(item);
                //entry.CurrentValues.SetValues(assesmentEmployee);
                entry.State = EntityState.Modified;
                context.AssesmentEmployees.Update(item);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return item;
        }

        public async Task<Models.ClearConnection.AssesmentEmployee> ActivateAssesmentEmployee(int? assesmentEmployeeId)
        {
            var item = context.AssesmentEmployees
                             .Where(i => i.ASSESMENT_EMPLOYEE_ID == assesmentEmployeeId)
                             .FirstOrDefault();

            OnAssesmentEmployeeUpdated(item);

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            try
            {
                item.IS_ACTIVE = true;
                var entry = context.Entry(item);
                //entry.CurrentValues.SetValues(assesmentEmployee);
                entry.State = EntityState.Modified;
                context.AssesmentEmployees.Update(item);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return item;
        }

        partial void OnAssesmentEmployeeAttachementDeleted(Models.ClearConnection.AssesmentEmployeeAttachement item);

        public async Task<Models.ClearConnection.AssesmentEmployeeAttachement> DeleteAssesmentEmployeeAttachement(int? assesmentEmployeeId, int? attachementid)
        {
            var item = context.AssesmentEmployeeAttachements
                              .Where(i => i.ASSESMENT_EMPLOYEE_ID == assesmentEmployeeId && i.ATTACHEMENTID == attachementid)

                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnAssesmentEmployeeAttachementDeleted(item);

            context.AssesmentEmployeeAttachements.Remove(item);

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

        partial void OnAssesmentEmployeeAttachementGet(Models.ClearConnection.AssesmentEmployeeAttachement item);

        public async Task<Models.ClearConnection.AssesmentEmployeeAttachement> GetAssesmentEmployeeAttachementByAssesmentEmployeeIdAndAttachementid(int? assesmentEmployeeId, int? attachementid)
        {
            var items = context.AssesmentEmployeeAttachements
                              .AsNoTracking()
                              .Where(i => i.ASSESMENT_EMPLOYEE_ID == assesmentEmployeeId && i.ATTACHEMENTID == attachementid);

            items = items.Include(i => i.SWMSTemplateAttachement);

            items = items.Include(i => i.AssesmentEmployeeStatus);

            items = items.Include(i => i.AssignedEmployee).ThenInclude(a => a.Employee);

            var item = items.FirstOrDefault();

            OnAssesmentEmployeeAttachementGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.AssesmentEmployeeAttachement> CancelAssesmentEmployeeAttachementChanges(Models.ClearConnection.AssesmentEmployeeAttachement item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnAssesmentEmployeeAttachementUpdated(Models.ClearConnection.AssesmentEmployeeAttachement item);

        public async Task<Models.ClearConnection.AssesmentEmployeeAttachement> UpdateAssesmentEmployeeAttachement(int? assesmentEmployeeId, int? attachementid, Models.ClearConnection.AssesmentEmployeeAttachement assesmentEmployeeAttachement)
        {
            OnAssesmentEmployeeAttachementUpdated(assesmentEmployeeAttachement);

            var item = context.AssesmentEmployeeAttachements
                              .Where(i => i.ASSESMENT_EMPLOYEE_ID == assesmentEmployeeId && i.ATTACHEMENTID == attachementid)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(assesmentEmployeeAttachement);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return assesmentEmployeeAttachement;
        }

        partial void OnAssesmentEmployeeStatusDeleted(Models.ClearConnection.AssesmentEmployeeStatus item);

        public async Task<Models.ClearConnection.AssesmentEmployeeStatus> DeleteAssesmentEmployeeStatus(int? assesmentStatusId)
        {
            var item = context.AssesmentEmployeeStatuses
                              .Where(i => i.ASSESMENT_STATUS_ID == assesmentStatusId)
                              .Include(i => i.AssesmentEmployeeAttachements)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnAssesmentEmployeeStatusDeleted(item);

            context.AssesmentEmployeeStatuses.Remove(item);

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

        partial void OnAssesmentEmployeeStatusGet(Models.ClearConnection.AssesmentEmployeeStatus item);

        public async Task<Models.ClearConnection.AssesmentEmployeeStatus> GetAssesmentEmployeeStatusByAssesmentStatusId(int? assesmentStatusId)
        {
            var items = context.AssesmentEmployeeStatuses
                              .AsNoTracking()
                              .Where(i => i.ASSESMENT_STATUS_ID == assesmentStatusId);

            var item = items.FirstOrDefault();

            OnAssesmentEmployeeStatusGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.AssesmentEmployeeStatus> CancelAssesmentEmployeeStatusChanges(Models.ClearConnection.AssesmentEmployeeStatus item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnAssesmentEmployeeStatusUpdated(Models.ClearConnection.AssesmentEmployeeStatus item);

        public async Task<Models.ClearConnection.AssesmentEmployeeStatus> UpdateAssesmentEmployeeStatus(int? assesmentStatusId, Models.ClearConnection.AssesmentEmployeeStatus assesmentEmployeeStatus)
        {
            OnAssesmentEmployeeStatusUpdated(assesmentEmployeeStatus);

            var item = context.AssesmentEmployeeStatuses
                              .Where(i => i.ASSESMENT_STATUS_ID == assesmentStatusId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(assesmentEmployeeStatus);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return assesmentEmployeeStatus;
        }

        partial void OnAssesmenttaskDeleted(Models.ClearConnection.Assesmenttask item);

        public async Task<Models.ClearConnection.Assesmenttask> DeleteAssesmenttask(Int64? assesmenttaskid)
        {
            var item = context.Assesmenttasks
                              .Where(i => i.ASSESMENTTASKID == assesmenttaskid)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnAssesmenttaskDeleted(item);

            context.Assesmenttasks.Remove(item);

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

        partial void OnAssesmenttaskGet(Models.ClearConnection.Assesmenttask item);

        public async Task<Models.ClearConnection.Assesmenttask> GetAssesmenttaskByAssesmenttaskid(Int64? assesmenttaskid)
        {
            var items = context.Assesmenttasks
                              .AsNoTracking()
                              .Where(i => i.ASSESMENTTASKID == assesmenttaskid);

            var item = items.FirstOrDefault();

            OnAssesmenttaskGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.Assesmenttask> CancelAssesmenttaskChanges(Models.ClearConnection.Assesmenttask item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnAssesmenttaskUpdated(Models.ClearConnection.Assesmenttask item);

        public async Task<Models.ClearConnection.Assesmenttask> UpdateAssesmenttask(Int64? assesmenttaskid, Models.ClearConnection.Assesmenttask assesmenttask)
        {
            OnAssesmenttaskUpdated(assesmenttask);

            var item = context.Assesmenttasks
                              .Where(i => i.ASSESMENTTASKID == assesmenttaskid)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(assesmenttask);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return assesmenttask;
        }

        partial void OnConsequenceDeleted(Models.ClearConnection.Consequence item);

        public async Task<Models.ClearConnection.Consequence> DeleteConsequence(int? consequenceId)
        {
            var item = context.Consequences
                              .Where(i => i.CONSEQUENCE_ID == consequenceId)
                              .Include(i => i.SwmsTemplatesteps)
                              .Include(i => i.SwmsTemplatesteps1)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnConsequenceDeleted(item);

            context.Consequences.Remove(item);

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

        partial void OnConsequenceGet(Models.ClearConnection.Consequence item);

        public async Task<Models.ClearConnection.Consequence> GetConsequenceByConsequenceId(int? consequenceId)
        {
            var items = context.Consequences
                              .AsNoTracking()
                              .Where(i => i.CONSEQUENCE_ID == consequenceId);

            var item = items.FirstOrDefault();

            OnConsequenceGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.Consequence> CancelConsequenceChanges(Models.ClearConnection.Consequence item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnConsequenceUpdated(Models.ClearConnection.Consequence item);

        public async Task<Models.ClearConnection.Consequence> UpdateConsequence(int? consequenceId, Models.ClearConnection.Consequence consequence)
        {
            OnConsequenceUpdated(consequence);

            var item = context.Consequences
                              .Where(i => i.CONSEQUENCE_ID == consequenceId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(consequence);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return consequence;
        }

        partial void OnControlMeasureValueDeleted(Models.ClearConnection.ControlMeasureValue item);

        public async Task<Models.ClearConnection.ControlMeasureValue> DeleteControlMeasureValue(int? controlMeasureId)
        {
            var item = context.ControlMeasureValues
                              .Where(i => i.CONTROL_MEASURE_ID == controlMeasureId)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnControlMeasureValueDeleted(item);

            context.ControlMeasureValues.Remove(item);

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

        partial void OnControlMeasureValueGet(Models.ClearConnection.ControlMeasureValue item);

        public async Task<Models.ClearConnection.ControlMeasureValue> GetControlMeasureValueByControlMeasureId(int? controlMeasureId)
        {
            var items = context.ControlMeasureValues
                              .AsNoTracking()
                              .Where(i => i.CONTROL_MEASURE_ID == controlMeasureId);

            var item = items.FirstOrDefault();

            OnControlMeasureValueGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.ControlMeasureValue> CancelControlMeasureValueChanges(Models.ClearConnection.ControlMeasureValue item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnControlMeasureValueUpdated(Models.ClearConnection.ControlMeasureValue item);

        public async Task<Models.ClearConnection.ControlMeasureValue> UpdateControlMeasureValue(int? controlMeasureId, Models.ClearConnection.ControlMeasureValue controlMeasureValue)
        {
            OnControlMeasureValueUpdated(controlMeasureValue);

            var item = context.ControlMeasureValues
                              .Where(i => i.CONTROL_MEASURE_ID == controlMeasureId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(controlMeasureValue);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return controlMeasureValue;
        }

        partial void OnCountryDeleted(Models.ClearConnection.Country item);

        public async Task<Models.ClearConnection.Country> DeleteCountry(int? id)
        {
            var item = context.Countries
                              .Where(i => i.ID == id)
                              .Include(i => i.People)
                              .Include(i => i.People1)
                              .Include(i => i.Templates)
                              .Include(i => i.PersonSites)
                              .Include(i => i.SwmsTemplates)
                              .Include(i => i.States)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnCountryDeleted(item);

            context.Countries.Remove(item);

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

        partial void OnCountryGet(Models.ClearConnection.Country item);

        public async Task<Models.ClearConnection.Country> GetCountryById(int? id)
        {
            var items = context.Countries
                              .AsNoTracking()
                              .Where(i => i.ID == id);

            var item = items.FirstOrDefault();

            OnCountryGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.Country> CancelCountryChanges(Models.ClearConnection.Country item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnCountryUpdated(Models.ClearConnection.Country item);

        public async Task<Models.ClearConnection.Country> UpdateCountry(int? id, Models.ClearConnection.Country country)
        {
            OnCountryUpdated(country);

            var item = context.Countries
                              .Where(i => i.ID == id)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(country);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return country;
        }

        partial void OnEntityStatusDeleted(Models.ClearConnection.EntityStatus item);

        public async Task<Models.ClearConnection.EntityStatus> DeleteEntityStatus(int? entityStatusId)
        {
            var item = context.EntityStatuses
                              .Where(i => i.ENTITY_STATUS_ID == entityStatusId)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnEntityStatusDeleted(item);

            context.EntityStatuses.Remove(item);

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

        partial void OnEntityStatusGet(Models.ClearConnection.EntityStatus item);

        public async Task<Models.ClearConnection.EntityStatus> GetEntityStatusByEntityStatusId(int? entityStatusId)
        {
            var items = context.EntityStatuses
                              .AsNoTracking()
                              .Where(i => i.ENTITY_STATUS_ID == entityStatusId);

            var item = items.FirstOrDefault();

            OnEntityStatusGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.EntityStatus> CancelEntityStatusChanges(Models.ClearConnection.EntityStatus item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnEntityStatusUpdated(Models.ClearConnection.EntityStatus item);

        public async Task<Models.ClearConnection.EntityStatus> UpdateEntityStatus(int? entityStatusId, Models.ClearConnection.EntityStatus entityStatus)
        {
            OnEntityStatusUpdated(entityStatus);

            var item = context.EntityStatuses
                              .Where(i => i.ENTITY_STATUS_ID == entityStatusId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(entityStatus);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return entityStatus;
        }

        partial void OnEscalationLevelDeleted(Models.ClearConnection.EscalationLevel item);

        public async Task<Models.ClearConnection.EscalationLevel> DeleteEscalationLevel(int? escalationLevelId)
        {
            var item = context.EscalationLevels
                              .Where(i => i.ESCALATION_LEVEL_ID == escalationLevelId)
                              .Include(i => i.Templates)
                              .Include(i => i.SwmsTemplates)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnEscalationLevelDeleted(item);

            context.EscalationLevels.Remove(item);

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

        partial void OnEscalationLevelGet(Models.ClearConnection.EscalationLevel item);

        public async Task<Models.ClearConnection.EscalationLevel> GetEscalationLevelByEscalationLevelId(int? escalationLevelId)
        {
            var items = context.EscalationLevels
                              .AsNoTracking()
                              .Where(i => i.ESCALATION_LEVEL_ID == escalationLevelId);

            var item = items.FirstOrDefault();

            OnEscalationLevelGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.EscalationLevel> CancelEscalationLevelChanges(Models.ClearConnection.EscalationLevel item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnEscalationLevelUpdated(Models.ClearConnection.EscalationLevel item);

        public async Task<Models.ClearConnection.EscalationLevel> UpdateEscalationLevel(int? escalationLevelId, Models.ClearConnection.EscalationLevel escalationLevel)
        {
            OnEscalationLevelUpdated(escalationLevel);

            var item = context.EscalationLevels
                              .Where(i => i.ESCALATION_LEVEL_ID == escalationLevelId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(escalationLevel);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return escalationLevel;
        }

        partial void OnHazardMaterialValueDeleted(Models.ClearConnection.HazardMaterialValue item);

        public async Task<Models.ClearConnection.HazardMaterialValue> DeleteHazardMaterialValue(int? hazardMaterialId)
        {
            var item = context.HazardMaterialValues
                              .Where(i => i.HAZARD_MATERIAL_ID == hazardMaterialId)
                              .Include(i => i.SwmsHazardousmaterials)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnHazardMaterialValueDeleted(item);

            context.HazardMaterialValues.Remove(item);

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

        partial void OnHazardMaterialValueGet(Models.ClearConnection.HazardMaterialValue item);

        public async Task<Models.ClearConnection.HazardMaterialValue> GetHazardMaterialValueByHazardMaterialId(int? hazardMaterialId)
        {
            var items = context.HazardMaterialValues
                              .AsNoTracking()
                              .Where(i => i.HAZARD_MATERIAL_ID == hazardMaterialId);

            var item = items.FirstOrDefault();

            OnHazardMaterialValueGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.HazardMaterialValue> CancelHazardMaterialValueChanges(Models.ClearConnection.HazardMaterialValue item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnHazardMaterialValueUpdated(Models.ClearConnection.HazardMaterialValue item);

        public async Task<Models.ClearConnection.HazardMaterialValue> UpdateHazardMaterialValue(int? hazardMaterialId, Models.ClearConnection.HazardMaterialValue hazardMaterialValue)
        {
            OnHazardMaterialValueUpdated(hazardMaterialValue);

            var item = context.HazardMaterialValues
                              .Where(i => i.HAZARD_MATERIAL_ID == hazardMaterialId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(hazardMaterialValue);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return hazardMaterialValue;
        }

        partial void OnHazardValueDeleted(Models.ClearConnection.HazardValue item);

        public async Task<Models.ClearConnection.HazardValue> DeleteHazardValue(int? hazardId)
        {
            var item = context.HazardValues
                              .Where(i => i.HAZARD_ID == hazardId)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnHazardValueDeleted(item);

            context.HazardValues.Remove(item);

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

        partial void OnHazardValueGet(Models.ClearConnection.HazardValue item);

        public async Task<Models.ClearConnection.HazardValue> GetHazardValueByHazardId(int? hazardId)
        {
            var items = context.HazardValues
                              .AsNoTracking()
                              .Where(i => i.HAZARD_ID == hazardId);

            var item = items.FirstOrDefault();

            OnHazardValueGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.HazardValue> CancelHazardValueChanges(Models.ClearConnection.HazardValue item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnHazardValueUpdated(Models.ClearConnection.HazardValue item);

        public async Task<Models.ClearConnection.HazardValue> UpdateHazardValue(int? hazardId, Models.ClearConnection.HazardValue hazardValue)
        {
            OnHazardValueUpdated(hazardValue);

            var item = context.HazardValues
                              .Where(i => i.HAZARD_ID == hazardId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(hazardValue);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return hazardValue;
        }

        partial void OnHighRiskCategoryDeleted(Models.ClearConnection.HighRiskCategory item);

        public async Task<Models.ClearConnection.HighRiskCategory> DeleteHighRiskCategory(int? riskCategoryId)
        {
            var item = context.HighRiskCategories
                              .Where(i => i.RISK_CATEGORY_ID == riskCategoryId)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnHighRiskCategoryDeleted(item);

            context.HighRiskCategories.Remove(item);

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

        partial void OnHighRiskCategoryGet(Models.ClearConnection.HighRiskCategory item);

        public async Task<Models.ClearConnection.HighRiskCategory> GetHighRiskCategoryByRiskCategoryId(int? riskCategoryId)
        {
            var items = context.HighRiskCategories
                              .AsNoTracking()
                              .Where(i => i.RISK_CATEGORY_ID == riskCategoryId);

            var item = items.FirstOrDefault();

            OnHighRiskCategoryGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.HighRiskCategory> CancelHighRiskCategoryChanges(Models.ClearConnection.HighRiskCategory item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnHighRiskCategoryUpdated(Models.ClearConnection.HighRiskCategory item);

        public async Task<Models.ClearConnection.HighRiskCategory> UpdateHighRiskCategory(int? riskCategoryId, Models.ClearConnection.HighRiskCategory highRiskCategory)
        {
            OnHighRiskCategoryUpdated(highRiskCategory);

            var item = context.HighRiskCategories
                              .Where(i => i.RISK_CATEGORY_ID == riskCategoryId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(highRiskCategory);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return highRiskCategory;
        }

        partial void OnImpactTypeDeleted(Models.ClearConnection.ImpactType item);

        public async Task<Models.ClearConnection.ImpactType> DeleteImpactType(int? impactTypeId)
        {
            var item = context.ImpactTypes
                              .Where(i => i.IMPACT_TYPE_ID == impactTypeId)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnImpactTypeDeleted(item);

            context.ImpactTypes.Remove(item);

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

        partial void OnImpactTypeGet(Models.ClearConnection.ImpactType item);

        public async Task<Models.ClearConnection.ImpactType> GetImpactTypeByImpactTypeId(int? impactTypeId)
        {
            var items = context.ImpactTypes
                              .AsNoTracking()
                              .Where(i => i.IMPACT_TYPE_ID == impactTypeId);

            var item = items.FirstOrDefault();

            OnImpactTypeGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.ImpactType> CancelImpactTypeChanges(Models.ClearConnection.ImpactType item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnImpactTypeUpdated(Models.ClearConnection.ImpactType item);

        public async Task<Models.ClearConnection.ImpactType> UpdateImpactType(int? impactTypeId, Models.ClearConnection.ImpactType impactType)
        {
            OnImpactTypeUpdated(impactType);

            var item = context.ImpactTypes
                              .Where(i => i.IMPACT_TYPE_ID == impactTypeId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(impactType);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return impactType;
        }

        partial void OnIndustryTypeDeleted(Models.ClearConnection.IndustryType item);

        public async Task<Models.ClearConnection.IndustryType> DeleteIndustryType(int? industryTypeId)
        {
            var item = context.IndustryTypes
                              .Where(i => i.INDUSTRY_TYPE_ID == industryTypeId)
                              .Include(i => i.Assesments)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnIndustryTypeDeleted(item);

            context.IndustryTypes.Remove(item);

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

        partial void OnIndustryTypeGet(Models.ClearConnection.IndustryType item);

        public async Task<Models.ClearConnection.IndustryType> GetIndustryTypeByIndustryTypeId(int? industryTypeId)
        {
            var items = context.IndustryTypes
                              .AsNoTracking()
                              .Where(i => i.INDUSTRY_TYPE_ID == industryTypeId);

            var item = items.FirstOrDefault();

            OnIndustryTypeGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.IndustryType> CancelIndustryTypeChanges(Models.ClearConnection.IndustryType item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnIndustryTypeUpdated(Models.ClearConnection.IndustryType item);

        public async Task<Models.ClearConnection.IndustryType> UpdateIndustryType(int? industryTypeId, Models.ClearConnection.IndustryType industryType)
        {
            OnIndustryTypeUpdated(industryType);

            var item = context.IndustryTypes
                              .Where(i => i.INDUSTRY_TYPE_ID == industryTypeId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(industryType);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return industryType;
        }

        partial void OnLicencePermitDeleted(Models.ClearConnection.LicencePermit item);

        public async Task<Models.ClearConnection.LicencePermit> DeleteLicencePermit(int? permitId)
        {
            var item = context.LicencePermits
                              .Where(i => i.PERMIT_ID == permitId)
                              .Include(i => i.SwmsLicencespermits)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnLicencePermitDeleted(item);

            context.LicencePermits.Remove(item);

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

        partial void OnLicencePermitGet(Models.ClearConnection.LicencePermit item);

        public async Task<Models.ClearConnection.LicencePermit> GetLicencePermitByPermitId(int? permitId)
        {
            var items = context.LicencePermits
                              .AsNoTracking()
                              .Where(i => i.PERMIT_ID == permitId);

            var item = items.FirstOrDefault();

            OnLicencePermitGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.LicencePermit> CancelLicencePermitChanges(Models.ClearConnection.LicencePermit item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnLicencePermitUpdated(Models.ClearConnection.LicencePermit item);

        public async Task<Models.ClearConnection.LicencePermit> UpdateLicencePermit(int? permitId, Models.ClearConnection.LicencePermit licencePermit)
        {
            OnLicencePermitUpdated(licencePermit);

            var item = context.LicencePermits
                              .Where(i => i.PERMIT_ID == permitId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(licencePermit);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return licencePermit;
        }

        partial void OnPersonDeleted(Models.ClearConnection.Person item);

        public async Task<Models.ClearConnection.Person> DeletePerson(int? personId)
        {
            var item = context.People
                              .Where(i => i.PERSON_ID == personId)
                              .Include(i => i.Assesments)
                              .Include(i => i.People1)
                              .Include(i => i.Templates)
                              .Include(i => i.PersonSites)
                              .Include(i => i.AssesmentEmployees)
                              .Include(i => i.SwmsTemplates)
                              .Include(i => i.SwmsTemplates1)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnPersonDeleted(item);

            context.People.Remove(item);

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

        partial void OnPersonGet(Models.ClearConnection.Person item);

        public async Task<Models.ClearConnection.Person> GetPersonByPersonId(int? personId)
        {
            var items = context.People
                              .AsNoTracking()
                              .Where(i => i.PERSON_ID == personId);

            items = items.Include(i => i.Person1);

            items = items.Include(i => i.State);

            items = items.Include(i => i.Country);

            items = items.Include(i => i.State1);

            items = items.Include(i => i.Country1);

            items = items.Include(i => i.PersonType);

            items = items.Include(i => i.Currency);

            items = items.Include(i => i.Applicence).ThenInclude(c => c.Currency);

            var item = items.FirstOrDefault();

            OnPersonGet(item);

            return await Task.FromResult(item);
        }


        public async Task<Models.ClearConnection.Person> GetPersonByPersonEmail(string email)
        {
            var items = context.People
                              .AsNoTracking()
                              .Where(i => i.PERSONAL_EMAIL == email);

            items = items.Include(i => i.Person1);

            items = items.Include(i => i.State);

            items = items.Include(i => i.Country);

            items = items.Include(i => i.State1);

            items = items.Include(i => i.Country1);

            items = items.Include(i => i.PersonType);

            items = items.Include(i => i.Applicence);

            var item = items.FirstOrDefault();

            OnPersonGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.Person> FindByAsync(Expression<Func<Models.ClearConnection.Person, bool>> predicate)
        {
            var items = await context.People.Where(predicate).FirstOrDefaultAsync();


            OnPersonGet(items);

            return await Task.FromResult(items);
        }


        public async Task<Models.ClearConnection.Person> CancelPersonChanges(Models.ClearConnection.Person item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnPersonUpdated(Models.ClearConnection.Person item);

        public async Task<Models.ClearConnection.Person> UpdatePerson(int? personId, Models.ClearConnection.Person person)
        {
            OnPersonUpdated(person);

            var item = context.People
                              .Where(i => i.PERSON_ID == personId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(person);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return person;
        }

        partial void OnPersonRoleDeleted(Models.ClearConnection.PersonRole item);

        public async Task<Models.ClearConnection.PersonRole> DeletePersonRole(int? personId, int? roleId)
        {
            var item = context.PersonRoles
                              .Where(i => i.PERSON_ID == personId && i.ROLE_ID == roleId)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnPersonRoleDeleted(item);

            context.PersonRoles.Remove(item);

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

        partial void OnPersonRoleGet(Models.ClearConnection.PersonRole item);

        public async Task<Models.ClearConnection.PersonRole> GetPersonRoleByPersonIdAndRoleId(int? personId, int? roleId)
        {
            var items = context.PersonRoles
                              .AsNoTracking()
                              .Where(i => i.PERSON_ID == personId && i.ROLE_ID == roleId);

            var item = items.FirstOrDefault();

            OnPersonRoleGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.PersonRole> CancelPersonRoleChanges(Models.ClearConnection.PersonRole item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnPersonRoleUpdated(Models.ClearConnection.PersonRole item);

        public async Task<Models.ClearConnection.PersonRole> UpdatePersonRole(int? personId, int? roleId, Models.ClearConnection.PersonRole personRole)
        {
            OnPersonRoleUpdated(personRole);

            var item = context.PersonRoles
                              .Where(i => i.PERSON_ID == personId && i.ROLE_ID == roleId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(personRole);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return personRole;
        }

        partial void OnPersonSiteDeleted(Models.ClearConnection.PersonSite item);

        public async Task<Models.ClearConnection.PersonSite> DeletePersonSite(int? personSiteId)
        {
            var item = context.PersonSites
                              .Where(i => i.PERSON_SITE_ID == personSiteId)
                              .Include(i => i.Assesments)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnPersonSiteDeleted(item);

            context.PersonSites.Remove(item);

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

        partial void OnPersonSiteGet(Models.ClearConnection.PersonSite item);

        public async Task<Models.ClearConnection.PersonSite> GetPersonSiteByPersonSiteId(int? personSiteId)
        {
            var items = context.PersonSites
                              .AsNoTracking()
                              .Where(i => i.PERSON_SITE_ID == personSiteId);

            items = items.Include(i => i.Person);

            items = items.Include(i => i.State);

            items = items.Include(i => i.Country);

            var item = items.FirstOrDefault();

            OnPersonSiteGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.PersonSite> CancelPersonSiteChanges(Models.ClearConnection.PersonSite item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnPersonSiteUpdated(Models.ClearConnection.PersonSite item);

        public async Task<Models.ClearConnection.PersonSite> UpdatePersonSite(int? personSiteId, Models.ClearConnection.PersonSite personSite)
        {
            OnPersonSiteUpdated(personSite);

            var item = context.PersonSites
                              .Where(i => i.PERSON_SITE_ID == personSiteId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            try
            {
                var entry = context.Entry(item);
                entry.CurrentValues.SetValues(personSite);
                entry.State = EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return personSite;
        }

        partial void OnPersonTypeDeleted(Models.ClearConnection.PersonType item);

        public async Task<Models.ClearConnection.PersonType> DeletePersonType(int? personTypeId)
        {
            var item = context.PersonTypes
                              .Where(i => i.PERSON_TYPE_ID == personTypeId)
                              .Include(i => i.People)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnPersonTypeDeleted(item);

            context.PersonTypes.Remove(item);

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

        partial void OnPersonTypeGet(Models.ClearConnection.PersonType item);

        public async Task<Models.ClearConnection.PersonType> GetPersonTypeByPersonTypeId(int? personTypeId)
        {
            var items = context.PersonTypes
                              .AsNoTracking()
                              .Where(i => i.PERSON_TYPE_ID == personTypeId);

            var item = items.FirstOrDefault();

            OnPersonTypeGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.PersonType> GetPersonTypeByRegistration()
        {
            var items = context.PersonTypes
                              .AsNoTracking()
                              .Where(i => i.REGDEFAULT == true);

            var item = items.FirstOrDefault();

            OnPersonTypeGet(item);

            return await Task.FromResult(item);
        }
        public async Task<Models.ClearConnection.Smtpsetup> GetSmtpsetup()
        {
            var items = context.Smtpsetups
                              .AsNoTracking()
                              .Where(x => x.IS_DELETED == false);

            var item = items.FirstOrDefault();

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.PersonType> CancelPersonTypeChanges(Models.ClearConnection.PersonType item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnPersonTypeUpdated(Models.ClearConnection.PersonType item);

        public async Task<Models.ClearConnection.PersonType> UpdatePersonType(int? personTypeId, Models.ClearConnection.PersonType personType)
        {
            OnPersonTypeUpdated(personType);

            var item = context.PersonTypes
                              .Where(i => i.PERSON_TYPE_ID == personTypeId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(personType);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return personType;
        }

        partial void OnPlantEquipmentDeleted(Models.ClearConnection.PlantEquipment item);

        public async Task<Models.ClearConnection.PlantEquipment> DeletePlantEquipment(int? plantEquipmentId)
        {
            var item = context.PlantEquipments
                              .Where(i => i.PLANT_EQUIPMENT_ID == plantEquipmentId)
                              .Include(i => i.SwmsPlantequipments)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnPlantEquipmentDeleted(item);

            context.PlantEquipments.Remove(item);

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

        partial void OnPlantEquipmentGet(Models.ClearConnection.PlantEquipment item);

        public async Task<Models.ClearConnection.PlantEquipment> GetPlantEquipmentByPlantEquipmentId(int? plantEquipmentId)
        {
            var items = context.PlantEquipments
                              .AsNoTracking()
                              .Where(i => i.PLANT_EQUIPMENT_ID == plantEquipmentId);

            var item = items.FirstOrDefault();

            OnPlantEquipmentGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.PlantEquipment> CancelPlantEquipmentChanges(Models.ClearConnection.PlantEquipment item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnPlantEquipmentUpdated(Models.ClearConnection.PlantEquipment item);

        public async Task<Models.ClearConnection.PlantEquipment> UpdatePlantEquipment(int? plantEquipmentId, Models.ClearConnection.PlantEquipment plantEquipment)
        {
            OnPlantEquipmentUpdated(plantEquipment);

            var item = context.PlantEquipments
                              .Where(i => i.PLANT_EQUIPMENT_ID == plantEquipmentId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(plantEquipment);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return plantEquipment;
        }

        partial void OnPpevalueDeleted(Models.ClearConnection.Ppevalue item);

        public async Task<Models.ClearConnection.Ppevalue> DeletePpevalue(int? ppeId)
        {
            var item = context.Ppevalues
                              .Where(i => i.PPE_ID == ppeId)
                              .Include(i => i.SwmsPperequireds)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnPpevalueDeleted(item);

            context.Ppevalues.Remove(item);

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

        partial void OnPpevalueGet(Models.ClearConnection.Ppevalue item);

        public async Task<Models.ClearConnection.Ppevalue> GetPpevalueByPpeId(int? ppeId)
        {
            var items = context.Ppevalues
                              .AsNoTracking()
                              .Where(i => i.PPE_ID == ppeId);

            var item = items.FirstOrDefault();

            OnPpevalueGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.Ppevalue> CancelPpevalueChanges(Models.ClearConnection.Ppevalue item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnPpevalueUpdated(Models.ClearConnection.Ppevalue item);

        public async Task<Models.ClearConnection.Ppevalue> UpdatePpevalue(int? ppeId, Models.ClearConnection.Ppevalue ppevalue)
        {
            OnPpevalueUpdated(ppevalue);

            var item = context.Ppevalues
                              .Where(i => i.PPE_ID == ppeId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(ppevalue);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return ppevalue;
        }

        partial void OnReferencedLegislationDeleted(Models.ClearConnection.ReferencedLegislation item);

        public async Task<Models.ClearConnection.ReferencedLegislation> DeleteReferencedLegislation(int? legislationId)
        {
            var item = context.ReferencedLegislations
                              .Where(i => i.LEGISLATION_ID == legislationId)
                              .Include(i => i.SwmsReferencedlegislations)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnReferencedLegislationDeleted(item);

            context.ReferencedLegislations.Remove(item);

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

        partial void OnReferencedLegislationGet(Models.ClearConnection.ReferencedLegislation item);

        public async Task<Models.ClearConnection.ReferencedLegislation> GetReferencedLegislationByLegislationId(int? legislationId)
        {
            var items = context.ReferencedLegislations
                              .AsNoTracking()
                              .Where(i => i.LEGISLATION_ID == legislationId);

            var item = items.FirstOrDefault();

            OnReferencedLegislationGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.ReferencedLegislation> CancelReferencedLegislationChanges(Models.ClearConnection.ReferencedLegislation item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnReferencedLegislationUpdated(Models.ClearConnection.ReferencedLegislation item);

        public async Task<Models.ClearConnection.ReferencedLegislation> UpdateReferencedLegislation(int? legislationId, Models.ClearConnection.ReferencedLegislation referencedLegislation)
        {
            OnReferencedLegislationUpdated(referencedLegislation);

            var item = context.ReferencedLegislations
                              .Where(i => i.LEGISLATION_ID == legislationId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(referencedLegislation);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return referencedLegislation;
        }

        partial void OnResposnsibleTypeDeleted(Models.ClearConnection.ResposnsibleType item);

        public async Task<Models.ClearConnection.ResposnsibleType> DeleteResposnsibleType(int? responsibleId)
        {
            var item = context.ResposnsibleTypes
                              .Where(i => i.RESPONSIBLE_ID == responsibleId)
                              .Include(i => i.SwmsTemplatesteps)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnResposnsibleTypeDeleted(item);

            context.ResposnsibleTypes.Remove(item);

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

        partial void OnResposnsibleTypeGet(Models.ClearConnection.ResposnsibleType item);

        public async Task<Models.ClearConnection.ResposnsibleType> GetResposnsibleTypeByResponsibleId(int? responsibleId)
        {
            var items = context.ResposnsibleTypes
                              .AsNoTracking()
                              .Where(i => i.RESPONSIBLE_ID == responsibleId);

            var item = items.FirstOrDefault();

            OnResposnsibleTypeGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.ResposnsibleType> CancelResposnsibleTypeChanges(Models.ClearConnection.ResposnsibleType item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnResposnsibleTypeUpdated(Models.ClearConnection.ResposnsibleType item);

        public async Task<Models.ClearConnection.ResposnsibleType> UpdateResposnsibleType(int? responsibleId, Models.ClearConnection.ResposnsibleType resposnsibleType)
        {
            OnResposnsibleTypeUpdated(resposnsibleType);

            var item = context.ResposnsibleTypes
                              .Where(i => i.RESPONSIBLE_ID == responsibleId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(resposnsibleType);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return resposnsibleType;
        }

        partial void OnRiskLikelyhoodDeleted(Models.ClearConnection.RiskLikelyhood item);

        public async Task<Models.ClearConnection.RiskLikelyhood> DeleteRiskLikelyhood(int? riskValueId)
        {
            var item = context.RiskLikelyhoods
                              .Where(i => i.RISK_VALUE_ID == riskValueId)
                              .Include(i => i.SwmsTemplatesteps)
                              .Include(i => i.SwmsTemplatesteps1)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnRiskLikelyhoodDeleted(item);

            context.RiskLikelyhoods.Remove(item);

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

        partial void OnRiskLikelyhoodGet(Models.ClearConnection.RiskLikelyhood item);

        public async Task<Models.ClearConnection.RiskLikelyhood> GetRiskLikelyhoodByRiskValueId(int? riskValueId)
        {
            var items = context.RiskLikelyhoods
                              .AsNoTracking()
                              .Where(i => i.RISK_VALUE_ID == riskValueId);

            var item = items.FirstOrDefault();

            OnRiskLikelyhoodGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.RiskLikelyhood> CancelRiskLikelyhoodChanges(Models.ClearConnection.RiskLikelyhood item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnRiskLikelyhoodUpdated(Models.ClearConnection.RiskLikelyhood item);

        public async Task<Models.ClearConnection.RiskLikelyhood> UpdateRiskLikelyhood(int? riskValueId, Models.ClearConnection.RiskLikelyhood riskLikelyhood)
        {
            OnRiskLikelyhoodUpdated(riskLikelyhood);

            var item = context.RiskLikelyhoods
                              .Where(i => i.RISK_VALUE_ID == riskValueId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(riskLikelyhood);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return riskLikelyhood;
        }

        partial void OnScheduleTypeDeleted(Models.ClearConnection.ScheduleType item);

        public async Task<Models.ClearConnection.ScheduleType> DeleteScheduleType(int? scheduleTypeId)
        {
            var item = context.ScheduleTypes
                              .Where(i => i.SCHEDULE_TYPE_ID == scheduleTypeId)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnScheduleTypeDeleted(item);

            context.ScheduleTypes.Remove(item);

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

        partial void OnScheduleTypeGet(Models.ClearConnection.ScheduleType item);

        public async Task<Models.ClearConnection.ScheduleType> GetScheduleTypeByScheduleTypeId(int? scheduleTypeId)
        {
            var items = context.ScheduleTypes
                              .AsNoTracking()
                              .Where(i => i.SCHEDULE_TYPE_ID == scheduleTypeId);

            var item = items.FirstOrDefault();

            OnScheduleTypeGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.ScheduleType> CancelScheduleTypeChanges(Models.ClearConnection.ScheduleType item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnScheduleTypeUpdated(Models.ClearConnection.ScheduleType item);

        public async Task<Models.ClearConnection.ScheduleType> UpdateScheduleType(int? scheduleTypeId, Models.ClearConnection.ScheduleType scheduleType)
        {
            OnScheduleTypeUpdated(scheduleType);

            var item = context.ScheduleTypes
                              .Where(i => i.SCHEDULE_TYPE_ID == scheduleTypeId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(scheduleType);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return scheduleType;
        }

        partial void OnSmtpsetupDeleted(Models.ClearConnection.Smtpsetup item);

        public async Task<Models.ClearConnection.Smtpsetup> DeleteSmtpsetup(int? smtpSetupId)
        {
            var item = context.Smtpsetups
                              .Where(i => i.SMTP_SETUP_ID == smtpSetupId)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnSmtpsetupDeleted(item);

            context.Smtpsetups.Remove(item);

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

        partial void OnSmtpsetupGet(Models.ClearConnection.Smtpsetup item);

        public async Task<Models.ClearConnection.Smtpsetup> GetSmtpsetupBySmtpSetupId(int? smtpSetupId)
        {
            var items = context.Smtpsetups
                              .AsNoTracking()
                              .Where(i => i.SMTP_SETUP_ID == smtpSetupId);

            var item = items.FirstOrDefault();

            OnSmtpsetupGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.Smtpsetup> CancelSmtpsetupChanges(Models.ClearConnection.Smtpsetup item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnSmtpsetupUpdated(Models.ClearConnection.Smtpsetup item);

        public async Task<Models.ClearConnection.Smtpsetup> UpdateSmtpsetup(int? smtpSetupId, Models.ClearConnection.Smtpsetup smtpsetup)
        {
            OnSmtpsetupUpdated(smtpsetup);

            var item = context.Smtpsetups
                              .Where(i => i.SMTP_SETUP_ID == smtpSetupId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(smtpsetup);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return smtpsetup;
        }

        partial void OnStateDeleted(Models.ClearConnection.State item);

        public async Task<Models.ClearConnection.State> DeleteState(int? id)
        {
            var item = context.States
                              .Where(i => i.ID == id)


                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnStateDeleted(item);

            context.States.Remove(item);

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

        partial void OnStateGet(Models.ClearConnection.State item);

        public async Task<Models.ClearConnection.State> GetStateById(int? id)
        {
            var items = context.States
                              .AsNoTracking()
                              .Where(i => i.ID == id);

            items = items.Include(i => i.Country);

            var item = items.FirstOrDefault();

            OnStateGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.State> CancelStateChanges(Models.ClearConnection.State item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnStateUpdated(Models.ClearConnection.State item);

        public async Task<Models.ClearConnection.State> UpdateState(int? id, Models.ClearConnection.State state)
        {
            OnStateUpdated(state);

            var item = context.States
                              .Where(i => i.ID == id)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(state);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return state;
        }

        partial void OnStatusLevelDeleted(Models.ClearConnection.StatusLevel item);

        public async Task<Models.ClearConnection.StatusLevel> DeleteStatusLevel(int? statusLevelId)
        {
            var item = context.StatusLevels
                              .Where(i => i.STATUS_LEVEL_ID == statusLevelId)
                              .Include(i => i.Templates)
                              .Include(i => i.SwmsTemplates)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnStatusLevelDeleted(item);

            context.StatusLevels.Remove(item);

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

        partial void OnStatusLevelGet(Models.ClearConnection.StatusLevel item);

        public async Task<Models.ClearConnection.StatusLevel> GetStatusLevelByStatusLevelId(int? statusLevelId)
        {
            var items = context.StatusLevels
                              .AsNoTracking()
                              .Where(i => i.STATUS_LEVEL_ID == statusLevelId);

            var item = items.FirstOrDefault();

            OnStatusLevelGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.StatusLevel> CancelStatusLevelChanges(Models.ClearConnection.StatusLevel item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnStatusLevelUpdated(Models.ClearConnection.StatusLevel item);

        public async Task<Models.ClearConnection.StatusLevel> UpdateStatusLevel(int? statusLevelId, Models.ClearConnection.StatusLevel statusLevel)
        {
            OnStatusLevelUpdated(statusLevel);

            var item = context.StatusLevels
                              .Where(i => i.STATUS_LEVEL_ID == statusLevelId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(statusLevel);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return statusLevel;
        }

        partial void OnStatusMasterDeleted(Models.ClearConnection.StatusMaster item);

        public async Task<Models.ClearConnection.StatusMaster> DeleteStatusMaster(int? statusId)
        {
            var item = context.StatusMasters
                              .Where(i => i.STATUS_ID == statusId)
                              .Include(i => i.Assesments)
                              .Include(i => i.Templates)
                              .Include(i => i.SwmsTemplates)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnStatusMasterDeleted(item);

            context.StatusMasters.Remove(item);

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

        partial void OnStatusMasterGet(Models.ClearConnection.StatusMaster item);

        public async Task<Models.ClearConnection.StatusMaster> GetStatusMasterByStatusId(int? statusId)
        {
            var items = context.StatusMasters
                              .AsNoTracking()
                              .Where(i => i.STATUS_ID == statusId);

            var item = items.FirstOrDefault();

            OnStatusMasterGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.StatusMaster> CancelStatusMasterChanges(Models.ClearConnection.StatusMaster item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnStatusMasterUpdated(Models.ClearConnection.StatusMaster item);

        public async Task<Models.ClearConnection.StatusMaster> UpdateStatusMaster(int? statusId, Models.ClearConnection.StatusMaster statusMaster)
        {
            OnStatusMasterUpdated(statusMaster);

            var item = context.StatusMasters
                              .Where(i => i.STATUS_ID == statusId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(statusMaster);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return statusMaster;
        }

        partial void OnSwmsHazardousmaterialDeleted(Models.ClearConnection.SwmsHazardousmaterial item);

        public async Task<Models.ClearConnection.SwmsHazardousmaterial> DeleteSwmsHazardousmaterial(int? hazardousmaterialid)
        {
            var item = context.SwmsHazardousmaterials
                              .Where(i => i.HAZARDOUSMATERIALID == hazardousmaterialid)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnSwmsHazardousmaterialDeleted(item);

            context.SwmsHazardousmaterials.Remove(item);

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

        partial void OnSwmsHazardousmaterialGet(Models.ClearConnection.SwmsHazardousmaterial item);

        public async Task<Models.ClearConnection.SwmsHazardousmaterial> GetSwmsHazardousmaterialByHazardousmaterialid(int? hazardousmaterialid)
        {
            var items = context.SwmsHazardousmaterials
                              .AsNoTracking()
                              .Where(i => i.HAZARDOUSMATERIALID == hazardousmaterialid);

            items = items.Include(i => i.SwmsTemplate);

            items = items.Include(i => i.HazardMaterialValue);

            var item = items.FirstOrDefault();

            OnSwmsHazardousmaterialGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.SwmsHazardousmaterial> CancelSwmsHazardousmaterialChanges(Models.ClearConnection.SwmsHazardousmaterial item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnSwmsHazardousmaterialUpdated(Models.ClearConnection.SwmsHazardousmaterial item);

        public async Task<Models.ClearConnection.SwmsHazardousmaterial> UpdateSwmsHazardousmaterial(int? hazardousmaterialid, Models.ClearConnection.SwmsHazardousmaterial swmsHazardousmaterial)
        {
            OnSwmsHazardousmaterialUpdated(swmsHazardousmaterial);

            var item = context.SwmsHazardousmaterials
                              .Where(i => i.HAZARDOUSMATERIALID == hazardousmaterialid)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(swmsHazardousmaterial);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return swmsHazardousmaterial;
        }

        partial void OnSwmsLicencespermitDeleted(Models.ClearConnection.SwmsLicencespermit item);

        public async Task<Models.ClearConnection.SwmsLicencespermit> DeleteSwmsLicencespermit(int? lpid)
        {
            var item = context.SwmsLicencespermits
                              .Where(i => i.LPID == lpid)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnSwmsLicencespermitDeleted(item);

            context.SwmsLicencespermits.Remove(item);

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

        partial void OnSwmsLicencespermitGet(Models.ClearConnection.SwmsLicencespermit item);

        public async Task<Models.ClearConnection.SwmsLicencespermit> GetSwmsLicencespermitByLpid(int? lpid)
        {
            var items = context.SwmsLicencespermits
                              .AsNoTracking()
                              .Where(i => i.LPID == lpid);

            items = items.Include(i => i.SwmsTemplate);

            items = items.Include(i => i.LicencePermit);

            var item = items.FirstOrDefault();

            OnSwmsLicencespermitGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.SwmsLicencespermit> CancelSwmsLicencespermitChanges(Models.ClearConnection.SwmsLicencespermit item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnSwmsLicencespermitUpdated(Models.ClearConnection.SwmsLicencespermit item);

        public async Task<Models.ClearConnection.SwmsLicencespermit> UpdateSwmsLicencespermit(int? lpid, Models.ClearConnection.SwmsLicencespermit swmsLicencespermit)
        {
            OnSwmsLicencespermitUpdated(swmsLicencespermit);

            var item = context.SwmsLicencespermits
                              .Where(i => i.LPID == lpid)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(swmsLicencespermit);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return swmsLicencespermit;
        }

        partial void OnSwmsPlantequipmentDeleted(Models.ClearConnection.SwmsPlantequipment item);

        public async Task<Models.ClearConnection.SwmsPlantequipment> DeleteSwmsPlantequipment(int? peid)
        {
            var item = context.SwmsPlantequipments
                              .Where(i => i.PEID == peid)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnSwmsPlantequipmentDeleted(item);

            context.SwmsPlantequipments.Remove(item);

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

        partial void OnSwmsPlantequipmentGet(Models.ClearConnection.SwmsPlantequipment item);

        public async Task<Models.ClearConnection.SwmsPlantequipment> GetSwmsPlantequipmentByPeid(int? peid)
        {
            var items = context.SwmsPlantequipments
                              .AsNoTracking()
                              .Where(i => i.PEID == peid);

            items = items.Include(i => i.SwmsTemplate);

            items = items.Include(i => i.PlantEquipment);

            var item = items.FirstOrDefault();

            OnSwmsPlantequipmentGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.SwmsPlantequipment> CancelSwmsPlantequipmentChanges(Models.ClearConnection.SwmsPlantequipment item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnSwmsPlantequipmentUpdated(Models.ClearConnection.SwmsPlantequipment item);

        public async Task<Models.ClearConnection.SwmsPlantequipment> UpdateSwmsPlantequipment(int? peid, Models.ClearConnection.SwmsPlantequipment swmsPlantequipment)
        {
            OnSwmsPlantequipmentUpdated(swmsPlantequipment);

            var item = context.SwmsPlantequipments
                              .Where(i => i.PEID == peid)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(swmsPlantequipment);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return swmsPlantequipment;
        }

        partial void OnSwmsPperequiredDeleted(Models.ClearConnection.SwmsPperequired item);

        public async Task<Models.ClearConnection.SwmsPperequired> DeleteSwmsPperequired(int? ppeid)
        {
            var item = context.SwmsPperequireds
                              .Where(i => i.PPEID == ppeid)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnSwmsPperequiredDeleted(item);

            context.SwmsPperequireds.Remove(item);

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

        partial void OnSwmsPperequiredGet(Models.ClearConnection.SwmsPperequired item);

        public async Task<Models.ClearConnection.SwmsPperequired> GetSwmsPperequiredByPpeid(int? ppeid)
        {
            var items = context.SwmsPperequireds
                              .AsNoTracking()
                              .Where(i => i.PPEID == ppeid);

            items = items.Include(i => i.SwmsTemplate);

            items = items.Include(i => i.Ppevalue);

            var item = items.FirstOrDefault();

            OnSwmsPperequiredGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.SwmsPperequired> CancelSwmsPperequiredChanges(Models.ClearConnection.SwmsPperequired item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnSwmsPperequiredUpdated(Models.ClearConnection.SwmsPperequired item);

        public async Task<Models.ClearConnection.SwmsPperequired> UpdateSwmsPperequired(int? ppeid, Models.ClearConnection.SwmsPperequired swmsPperequired)
        {
            OnSwmsPperequiredUpdated(swmsPperequired);

            var item = context.SwmsPperequireds
                              .Where(i => i.PPEID == ppeid)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(swmsPperequired);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return swmsPperequired;
        }

        partial void OnSwmsReferencedlegislationDeleted(Models.ClearConnection.SwmsReferencedlegislation item);

        public async Task<Models.ClearConnection.SwmsReferencedlegislation> DeleteSwmsReferencedlegislation(int? reflid)
        {
            var item = context.SwmsReferencedlegislations
                              .Where(i => i.REFLID == reflid)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnSwmsReferencedlegislationDeleted(item);

            context.SwmsReferencedlegislations.Remove(item);

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

        partial void OnSwmsReferencedlegislationGet(Models.ClearConnection.SwmsReferencedlegislation item);

        public async Task<Models.ClearConnection.SwmsReferencedlegislation> GetSwmsReferencedlegislationByReflid(int? reflid)
        {
            var items = context.SwmsReferencedlegislations
                              .AsNoTracking()
                              .Where(i => i.REFLID == reflid);

            items = items.Include(i => i.SwmsTemplate);

            items = items.Include(i => i.ReferencedLegislation);

            var item = items.FirstOrDefault();

            OnSwmsReferencedlegislationGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.SwmsReferencedlegislation> CancelSwmsReferencedlegislationChanges(Models.ClearConnection.SwmsReferencedlegislation item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnSwmsReferencedlegislationUpdated(Models.ClearConnection.SwmsReferencedlegislation item);

        public async Task<Models.ClearConnection.SwmsReferencedlegislation> UpdateSwmsReferencedlegislation(int? reflid, Models.ClearConnection.SwmsReferencedlegislation swmsReferencedlegislation)
        {
            OnSwmsReferencedlegislationUpdated(swmsReferencedlegislation);

            var item = context.SwmsReferencedlegislations
                              .Where(i => i.REFLID == reflid)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(swmsReferencedlegislation);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return swmsReferencedlegislation;
        }

        partial void OnSwmsSectionDeleted(Models.ClearConnection.SwmsSection item);

        public async Task<Models.ClearConnection.SwmsSection> DeleteSwmsSection(int? swmsSectionId)
        {
            var item = context.SwmsSections
                              .Where(i => i.SWMS_SECTION_ID == swmsSectionId)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnSwmsSectionDeleted(item);

            context.SwmsSections.Remove(item);

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

        partial void OnSwmsSectionGet(Models.ClearConnection.SwmsSection item);

        public async Task<Models.ClearConnection.SwmsSection> GetSwmsSectionBySwmsSectionId(int? swmsSectionId)
        {
            var items = context.SwmsSections
                              .AsNoTracking()
                              .Where(i => i.SWMS_SECTION_ID == swmsSectionId);

            var item = items.FirstOrDefault();

            OnSwmsSectionGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.SwmsSection> CancelSwmsSectionChanges(Models.ClearConnection.SwmsSection item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnSwmsSectionUpdated(Models.ClearConnection.SwmsSection item);

        public async Task<Models.ClearConnection.SwmsSection> UpdateSwmsSection(int? swmsSectionId, Models.ClearConnection.SwmsSection swmsSection)
        {
            OnSwmsSectionUpdated(swmsSection);

            var item = context.SwmsSections
                              .Where(i => i.SWMS_SECTION_ID == swmsSectionId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(swmsSection);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return swmsSection;
        }

        partial void OnSwmsTemplateDeleted(Models.ClearConnection.SwmsTemplate item);

        public async Task<Models.ClearConnection.SwmsTemplate> DeleteSwmsTemplate(int? swmsid)
        {
            var item = context.SwmsTemplates
                              .Where(i => i.SWMSID == swmsid)
                              .Include(i => i.AssesmentAttachements)
                              .Include(i => i.SwmsTemplatesteps)
                              .Include(i => i.SwmsReferencedlegislations)
                              .Include(i => i.SwmsPperequireds)
                              .Include(i => i.SwmsPlantequipments)
                              .Include(i => i.SwmsLicencespermits)
                              .Include(i => i.SwmsHazardousmaterials)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnSwmsTemplateDeleted(item);

            context.SwmsTemplates.Remove(item);

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

        partial void OnSwmsTemplateGet(Models.ClearConnection.SwmsTemplate item);

        public async Task<Models.ClearConnection.SwmsTemplate> GetSwmsTemplateBySwmsid(int? swmsid)
        {
            var items = context.SwmsTemplates
                              .AsNoTracking()
                              .Where(i => i.SWMSID == swmsid);

            items = items.Include(i => i.Person);

            items = items.Include(i => i.TemplateType);

            items = items.Include(i => i.Template);

            items = items.Include(i => i.StatusMaster);

            items = items.Include(i => i.SwmsTemplateCategory);

            items = items.Include(i => i.Country);

            items = items.Include(i => i.EscalationLevel);

            items = items.Include(i => i.WarningLevel);

            items = items.Include(i => i.StatusLevel);

            items = items.Include(i => i.State);

            items = items.Include(i => i.TradeCategory);

            items = items.Include(i => i.Person1);

            var item = items.FirstOrDefault();

            OnSwmsTemplateGet(item);

            return await Task.FromResult(item);
        }


        public async Task<Models.ClearConnection.SwmsTemplate> GetSwmsBySwmsid(int? swmsid)
        {
            var items = context.SwmsTemplates
                              .AsNoTracking()
                              .Where(i => i.SWMSID == swmsid);

            items = items.Include(i => i.Person);

            items = items.Include(i => i.TemplateType);

            items = items.Include(i => i.Template).ThenInclude(i => i.Templateattachments);

            items = items.Include(i => i.StatusMaster);

            items = items.Include(i => i.SwmsTemplateCategory);

            items = items.Include(i => i.Country);

            items = items.Include(i => i.EscalationLevel);

            items = items.Include(i => i.WarningLevel);

            items = items.Include(i => i.StatusLevel);

            items = items.Include(i => i.State);



            items = items.Include(i => i.TradeCategory);

            items = items.Include(i => i.Person1);
            items = items.Include(i => i.SwmsPperequireds)
                    .Include(i => i.SwmsHazardousmaterials)
                    .Include(i => i.SwmsLicencespermits)
                    .Include(i => i.SwmsPlantequipments)
                    .Include(i => i.SwmsReferencedlegislations)
                    .Include(i => i.SwmsTemplatesteps).ThenInclude(i => i.Consequence)
                    .Include(i => i.SwmsTemplatesteps).ThenInclude(i => i.RiskLikelyhood)
                    .Include(i => i.SwmsTemplatesteps).ThenInclude(i => i.RiskLikelyhood1)
                    .Include(i => i.SwmsTemplatesteps).ThenInclude(i => i.Consequence1)
                    .Include(i => i.SwmsTemplatesteps).ThenInclude(i => i.ResposnsibleType);


            var item = items.FirstOrDefault();

            OnSwmsTemplateGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.SwmsTemplate> CancelSwmsTemplateChanges(Models.ClearConnection.SwmsTemplate item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnSwmsTemplateUpdated(Models.ClearConnection.SwmsTemplate item);


        public async Task<Models.ClearConnection.SwmsTemplate> UpdateSwmsTemplate(int? swmsid, Models.ClearConnection.SwmsTemplate swmsTemplate)
        {
            OnSwmsTemplateUpdated(swmsTemplate);

            var item = context.SwmsTemplates
                              .Where(i => i.SWMSID == swmsid)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(swmsTemplate);
            entry.State = EntityState.Modified;
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {

            }

            return swmsTemplate;
        }


        public async Task<Models.ClearConnection.SwmsTemplatestep> UpdateSwmsTemplateSteps(int? stepId, Models.ClearConnection.SwmsTemplatestep swmsTemplatestep)
        {
            var item = context.SwmsTemplatesteps
                              .Where(i => i.STEPID == stepId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(swmsTemplatestep);
            entry.State = EntityState.Modified;
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {

            }

            return swmsTemplatestep;
        }


        partial void OnSwmsTemplateCategoryDeleted(Models.ClearConnection.SwmsTemplateCategory item);

        public async Task<Models.ClearConnection.SwmsTemplateCategory> DeleteSwmsTemplateCategory(int? templateCategoryId)
        {
            var item = context.SwmsTemplateCategories
                              .Where(i => i.TEMPLATE_CATEGORY_ID == templateCategoryId)
                              .Include(i => i.Templates)
                              .Include(i => i.SwmsTemplates)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnSwmsTemplateCategoryDeleted(item);

            context.SwmsTemplateCategories.Remove(item);

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

        partial void OnSwmsTemplateCategoryGet(Models.ClearConnection.SwmsTemplateCategory item);

        public async Task<Models.ClearConnection.SwmsTemplateCategory> GetSwmsTemplateCategoryByTemplateCategoryId(int? templateCategoryId)
        {
            var items = context.SwmsTemplateCategories
                              .AsNoTracking()
                              .Where(i => i.TEMPLATE_CATEGORY_ID == templateCategoryId);

            var item = items.FirstOrDefault();

            OnSwmsTemplateCategoryGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.SwmsTemplateCategory> CancelSwmsTemplateCategoryChanges(Models.ClearConnection.SwmsTemplateCategory item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnSwmsTemplateCategoryUpdated(Models.ClearConnection.SwmsTemplateCategory item);

        public async Task<Models.ClearConnection.SwmsTemplateCategory> UpdateSwmsTemplateCategory(int? templateCategoryId, Models.ClearConnection.SwmsTemplateCategory swmsTemplateCategory)
        {
            OnSwmsTemplateCategoryUpdated(swmsTemplateCategory);

            var item = context.SwmsTemplateCategories
                              .Where(i => i.TEMPLATE_CATEGORY_ID == templateCategoryId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(swmsTemplateCategory);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return swmsTemplateCategory;
        }

        partial void OnSwmsTemplatestepDeleted(Models.ClearConnection.SwmsTemplatestep item);

        public async Task<Models.ClearConnection.SwmsTemplatestep> DeleteSwmsTemplatestep(int? stepid)
        {
            var item = context.SwmsTemplatesteps
                              .Where(i => i.STEPID == stepid)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnSwmsTemplatestepDeleted(item);

            context.SwmsTemplatesteps.Remove(item);

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

        partial void OnSwmsTemplatestepGet(Models.ClearConnection.SwmsTemplatestep item);

        public async Task<Models.ClearConnection.SwmsTemplatestep> GetSwmsTemplatestepByStepid(int? stepid)
        {
            var items = context.SwmsTemplatesteps
                              .AsNoTracking()
                              .Where(i => i.STEPID == stepid);

            items = items.Include(i => i.SwmsTemplate);

            items = items.Include(i => i.RiskLikelyhood);

            items = items.Include(i => i.Consequence);

            items = items.Include(i => i.RiskLikelyhood1);

            items = items.Include(i => i.Consequence1);

            items = items.Include(i => i.ResposnsibleType);

            var item = items.FirstOrDefault();

            OnSwmsTemplatestepGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.SwmsTemplatestep> CancelSwmsTemplatestepChanges(Models.ClearConnection.SwmsTemplatestep item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnSwmsTemplatestepUpdated(Models.ClearConnection.SwmsTemplatestep item);

        public async Task<Models.ClearConnection.SwmsTemplatestep> UpdateSwmsTemplatestep(int? stepid, Models.ClearConnection.SwmsTemplatestep swmsTemplatestep)
        {
            OnSwmsTemplatestepUpdated(swmsTemplatestep);

            var item = context.SwmsTemplatesteps
                              .Where(i => i.STEPID == stepid)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(swmsTemplatestep);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return swmsTemplatestep;
        }

        partial void OnSystemroleDeleted(Models.ClearConnection.Systemrole item);

        public async Task<Models.ClearConnection.Systemrole> DeleteSystemrole(int? roleId)
        {
            var item = context.Systemroles
                              .Where(i => i.ROLE_ID == roleId)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnSystemroleDeleted(item);

            context.Systemroles.Remove(item);

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

        partial void OnSystemroleGet(Models.ClearConnection.Systemrole item);

        public async Task<Models.ClearConnection.Systemrole> GetSystemroleByRoleId(int? roleId)
        {
            var items = context.Systemroles
                              .AsNoTracking()
                              .Where(i => i.ROLE_ID == roleId);

            var item = items.FirstOrDefault();

            OnSystemroleGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.Systemrole> CancelSystemroleChanges(Models.ClearConnection.Systemrole item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnSystemroleUpdated(Models.ClearConnection.Systemrole item);

        public async Task<Models.ClearConnection.Systemrole> UpdateSystemrole(int? roleId, Models.ClearConnection.Systemrole systemrole)
        {
            OnSystemroleUpdated(systemrole);

            var item = context.Systemroles
                              .Where(i => i.ROLE_ID == roleId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(systemrole);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return systemrole;
        }

        partial void OnTemplateDeleted(Models.ClearConnection.Template item);

        public async Task<Models.ClearConnection.Template> DeleteTemplate(int? id)
        {
            var item = context.Templates
                              .Where(i => i.ID == id)
                              .Include(i => i.Templateattachments)
                              .Include(i => i.SwmsTemplates)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnTemplateDeleted(item);

            context.Templates.Remove(item);

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

        partial void OnTemplateGet(Models.ClearConnection.Template item);

        public async Task<Models.ClearConnection.Template> GetTemplateById(int? id)
        {
            var items = context.Templates
                              .AsNoTracking()
                              .Where(i => i.ID == id);

            items = items.Include(i => i.StatusMaster);

            items = items.Include(i => i.TradeCategory);

            items = items.Include(i => i.Person);

            items = items.Include(i => i.SwmsTemplateCategory);

            items = items.Include(i => i.Country);

            items = items.Include(i => i.EscalationLevel);

            items = items.Include(i => i.WarningLevel);

            items = items.Include(i => i.StatusLevel);

            items = items.Include(i => i.State);

            var item = items.FirstOrDefault();

            OnTemplateGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.Template> GetTemplateByTradeId(int? id)
        {
            var items = context.Templates
                              .AsNoTracking()
                              .Where(i => i.TRADECATEGORYID == id);

            items = items.Include(i => i.StatusMaster);

            items = items.Include(i => i.TradeCategory);

            items = items.Include(i => i.Person);

            items = items.Include(i => i.SwmsTemplateCategory);

            items = items.Include(i => i.Country);

            items = items.Include(i => i.EscalationLevel);

            items = items.Include(i => i.WarningLevel);

            items = items.Include(i => i.StatusLevel);

            items = items.Include(i => i.State);

            items = items.Include(i => i.Templateattachments);

            var item = items.FirstOrDefault();

            OnTemplateGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.Template> CancelTemplateChanges(Models.ClearConnection.Template item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnTemplateUpdated(Models.ClearConnection.Template item);

        public async Task<Models.ClearConnection.Template> UpdateTemplate(int? id, Models.ClearConnection.Template template)
        {
            OnTemplateUpdated(template);

            var item = context.Templates
                              .Where(i => i.ID == id)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(template);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return template;
        }

        partial void OnTemplateTypeDeleted(Models.ClearConnection.TemplateType item);

        public async Task<Models.ClearConnection.TemplateType> DeleteTemplateType(int? templateTypeId)
        {
            var item = context.TemplateTypes
                              .Where(i => i.TEMPLATE_TYPE_ID == templateTypeId)
                              .Include(i => i.Templateattachments)
                              .Include(i => i.SwmsTemplates)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnTemplateTypeDeleted(item);

            context.TemplateTypes.Remove(item);

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

        partial void OnTemplateTypeGet(Models.ClearConnection.TemplateType item);

        public async Task<Models.ClearConnection.TemplateType> GetTemplateTypeByTemplateTypeId(int? templateTypeId)
        {
            var items = context.TemplateTypes
                              .AsNoTracking()
                              .Where(i => i.TEMPLATE_TYPE_ID == templateTypeId);

            var item = items.FirstOrDefault();

            OnTemplateTypeGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.TemplateType> CancelTemplateTypeChanges(Models.ClearConnection.TemplateType item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnTemplateTypeUpdated(Models.ClearConnection.TemplateType item);

        public async Task<Models.ClearConnection.TemplateType> UpdateTemplateType(int? templateTypeId, Models.ClearConnection.TemplateType templateType)
        {
            OnTemplateTypeUpdated(templateType);

            var item = context.TemplateTypes
                              .Where(i => i.TEMPLATE_TYPE_ID == templateTypeId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(templateType);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return templateType;
        }

        partial void OnTemplateattachmentDeleted(Models.ClearConnection.Templateattachment item);

        public async Task<Models.ClearConnection.Templateattachment> DeleteTemplateattachment(int? id)
        {
            var item = context.Templateattachments
                              .Where(i => i.ID == id)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnTemplateattachmentDeleted(item);

            context.Templateattachments.Remove(item);

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

        partial void OnTemplateattachmentGet(Models.ClearConnection.Templateattachment item);

        public async Task<Models.ClearConnection.Templateattachment> GetTemplateattachmentById(int? id)
        {
            var items = context.Templateattachments
                              .AsNoTracking()
                              .Where(i => i.ID == id);

            items = items.Include(i => i.Template);

            items = items.Include(i => i.TemplateType);

            var item = items.FirstOrDefault();

            OnTemplateattachmentGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.Templateattachment> CancelTemplateattachmentChanges(Models.ClearConnection.Templateattachment item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnTemplateattachmentUpdated(Models.ClearConnection.Templateattachment item);

        public async Task<Models.ClearConnection.Templateattachment> UpdateTemplateattachment(int? id, Models.ClearConnection.Templateattachment templateattachment)
        {
            OnTemplateattachmentUpdated(templateattachment);

            var item = context.Templateattachments
                              .Where(i => i.ID == id)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(templateattachment);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return templateattachment;
        }

        partial void OnTradeCategoryDeleted(Models.ClearConnection.TradeCategory item);

        public async Task<Models.ClearConnection.TradeCategory> DeleteTradeCategory(int? tradeCategoryId)
        {
            var item = context.TradeCategories
                              .Where(i => i.TRADE_CATEGORY_ID == tradeCategoryId)
                              .Include(i => i.Assesments)
                              .Include(i => i.Templates)
                              .Include(i => i.SwmsTemplates)
                              .Include(i => i.TradeCategories1)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnTradeCategoryDeleted(item);

            context.TradeCategories.Remove(item);

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

        partial void OnTradeCategoryGet(Models.ClearConnection.TradeCategory item);

        public async Task<Models.ClearConnection.TradeCategory> GetTradeCategoryByTradeCategoryId(int? tradeCategoryId)
        {
            var items = context.TradeCategories
                              .AsNoTracking()
                              .Where(i => i.TRADE_CATEGORY_ID == tradeCategoryId);

            items = items.Include(i => i.TradeCategory1);

            var item = items.FirstOrDefault();

            OnTradeCategoryGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.TradeCategory> CancelTradeCategoryChanges(Models.ClearConnection.TradeCategory item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnTradeCategoryUpdated(Models.ClearConnection.TradeCategory item);

        public async Task<Models.ClearConnection.TradeCategory> UpdateTradeCategory(int? tradeCategoryId, Models.ClearConnection.TradeCategory tradeCategory)
        {
            OnTradeCategoryUpdated(tradeCategory);

            var item = context.TradeCategories
                              .Where(i => i.TRADE_CATEGORY_ID == tradeCategoryId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(tradeCategory);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return tradeCategory;
        }

        partial void OnWarningLevelDeleted(Models.ClearConnection.WarningLevel item);

        public async Task<Models.ClearConnection.WarningLevel> DeleteWarningLevel(int? warningLevelId)
        {
            var item = context.WarningLevels
                              .Where(i => i.WARNING_LEVEL_ID == warningLevelId)
                              .Include(i => i.Templates)
                              .Include(i => i.SwmsTemplates)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnWarningLevelDeleted(item);

            context.WarningLevels.Remove(item);

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

        partial void OnWarningLevelGet(Models.ClearConnection.WarningLevel item);

        public async Task<Models.ClearConnection.WarningLevel> GetWarningLevelByWarningLevelId(int? warningLevelId)
        {
            var items = context.WarningLevels
                              .AsNoTracking()
                              .Where(i => i.WARNING_LEVEL_ID == warningLevelId);

            var item = items.FirstOrDefault();

            OnWarningLevelGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.WarningLevel> CancelWarningLevelChanges(Models.ClearConnection.WarningLevel item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnWarningLevelUpdated(Models.ClearConnection.WarningLevel item);

        public async Task<Models.ClearConnection.WarningLevel> UpdateWarningLevel(int? warningLevelId, Models.ClearConnection.WarningLevel warningLevel)
        {
            OnWarningLevelUpdated(warningLevel);

            var item = context.WarningLevels
                              .Where(i => i.WARNING_LEVEL_ID == warningLevelId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(warningLevel);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return warningLevel;
        }


        partial void OnSurveyAnswerChecklistsRead(ref IQueryable<Models.ClearConnection.SurveyAnswerChecklist> items);

        public async Task<IQueryable<Models.ClearConnection.SurveyAnswerChecklist>> GetSurveyAnswerChecklists(Query query = null)
        {
            var items = context.SurveyAnswerChecklists.AsQueryable();



            items = items.Include(i => i.Question);

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

            OnSurveyAnswerChecklistsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSurveyAnswerValuesRead(ref IQueryable<Models.ClearConnection.SurveyAnswerValue> items);

        public async Task<IQueryable<Models.ClearConnection.SurveyAnswerValue>> GetSurveyAnswerValues(Query query = null)
        {
            var items = context.SurveyAnswerValues.AsQueryable();

            items = items.Include(i => i.AnswerChecklist);

            items = items.Include(i => i.SurveyAnswer);

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

            OnSurveyAnswerValuesRead(ref items);

            return await Task.FromResult(items);
        }




        #region Company Transaction
        partial void OnCompanyTransactionsRead(ref IQueryable<Models.ClearConnection.CompanyTransaction> items);

        public async Task<IQueryable<Models.ClearConnection.CompanyTransaction>> GetCompanyTransactions(Query query = null)
        {
            var items = context.CompanyTransactions.AsQueryable();

            items = items.Include(i => i.TransactionStatus);

            items = items.Include(i => i.Currency);

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

            OnCompanyTransactionsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCompanyTransactionCreated(CompanyTransaction item);

        public async Task<CompanyTransaction> CreateCompanyTransaction(CompanyTransaction companyTransaction)
        {
            OnCompanyTransactionCreated(companyTransaction);

            context.CompanyTransactions.Add(companyTransaction);
            context.SaveChanges();

            return companyTransaction;
        }

        partial void OnCompanyTransactionUpdated(CompanyTransaction item);

        public async Task<CompanyTransaction> UpdateCompanyTransaction(int? transactionid, CompanyTransaction companyTransaction)
        {
            OnCompanyTransactionUpdated(companyTransaction);

            var item = context.CompanyTransactions
                              .Where(i => i.TRANSACTIONID == transactionid)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(companyTransaction);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return companyTransaction;
        }

        public async Task<IQueryable<Models.ClearConnection.CompanyTransaction>> GetCompanyTransactionByCompany(int companyId, Query query = null)
        {
            var items = context.CompanyTransactions.AsQueryable().Where(a => a.PERSON_ID == companyId);

            items = items.Include(i => i.TransactionStatus);

            items = items.Include(i => i.Currency);

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

            OnCompanyTransactionsRead(ref items);

            return await Task.FromResult(items);
        }
        partial void OnCompanyTransactionDetailsRead(ref IQueryable<Models.ClearConnection.CompanyTransactionDetail> items);

        public async Task<IQueryable<Models.ClearConnection.CompanyTransactionDetail>> GetCompanyTransactionDetails(Query query = null)
        {
            var items = context.CompanyTransactionDetails.AsQueryable();

            items = items.Include(i => i.CompanyTransaction);

            items = items.Include(i => i.Applicence);

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

            OnCompanyTransactionDetailsRead(ref items);

            return await Task.FromResult(items);
        }

        #endregion

        #region Work Order Type
        public async Task ExportWorkOrderTypesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/clearrisk/workordertypes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')") : $"export/clearrisk/workordertypes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')", true);
        }

        public async Task ExportWorkOrderTypesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/clearrisk/workordertypes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')") : $"export/clearrisk/workordertypes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')", true);
        }

        partial void OnWorkOrderTypesRead(ref IQueryable<Models.ClearConnection.WorkOrderType> items);

        public async Task<IQueryable<Models.ClearConnection.WorkOrderType>> GetWorkOrderTypes(Query query = null)
        {
            var items = context.WorkOrderTypes.AsQueryable();

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

            OnWorkOrderTypesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnWorkOrderTypeCreated(Models.ClearConnection.WorkOrderType item);

        public async Task<Models.ClearConnection.WorkOrderType> CreateWorkOrderType(Models.ClearConnection.WorkOrderType workOrderType)
        {
            OnWorkOrderTypeCreated(workOrderType);

            context.WorkOrderTypes.Add(workOrderType);
            context.SaveChanges();

            return workOrderType;
        }

        partial void OnWorkOrderTypeDeleted(Models.ClearConnection.WorkOrderType item);

        public async Task<Models.ClearConnection.WorkOrderType> DeleteWorkOrderType(int? workOrderTypeId)
        {
            var item = context.WorkOrderTypes
                              .Where(i => i.WORK_ORDER_TYPE_ID == workOrderTypeId)
                              .Include(i => i.WorkOrders)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnWorkOrderTypeDeleted(item);

            context.WorkOrderTypes.Remove(item);

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

        partial void OnWorkOrderTypeGet(Models.ClearConnection.WorkOrderType item);

        public async Task<Models.ClearConnection.WorkOrderType> GetWorkOrderTypeByWorkOrderTypeId(int? workOrderTypeId)
        {
            var items = context.WorkOrderTypes
                              .AsNoTracking()
                              .Where(i => i.WORK_ORDER_TYPE_ID == workOrderTypeId);

            var item = items.FirstOrDefault();

            OnWorkOrderTypeGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ClearConnection.WorkOrderType> CancelWorkOrderTypeChanges(Models.ClearConnection.WorkOrderType item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnWorkOrderTypeUpdated(Models.ClearConnection.WorkOrderType item);

        public async Task<Models.ClearConnection.WorkOrderType> UpdateWorkOrderType(int? workOrderTypeId, Models.ClearConnection.WorkOrderType workOrderType)
        {
            OnWorkOrderTypeUpdated(workOrderType);

            var item = context.WorkOrderTypes
                              .Where(i => i.WORK_ORDER_TYPE_ID == workOrderTypeId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(workOrderType);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return workOrderType;
        }

        #endregion

        #region Process Type
        public async Task ExportProcessTypesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/clearrisk/processtypes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')") : $"export/clearrisk/processtypes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')", true);
        }

        public async Task ExportProcessTypesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/clearrisk/processtypes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')") : $"export/clearrisk/processtypes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')", true);
        }

        partial void OnProcessTypesRead(ref IQueryable<ProcessType> items);

        public async Task<IQueryable<ProcessType>> GetProcessTypes(Query query = null)
        {
            var items = context.ProcessTypes.AsQueryable();

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

            OnProcessTypesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnProcessTypeCreated(ProcessType item);

        public async Task<ProcessType> CreateProcessType(ProcessType processType)
        {
            OnProcessTypeCreated(processType);

            context.ProcessTypes.Add(processType);
            context.SaveChanges();

            return processType;
        }
        partial void OnUploadFileCreated(CompanyDocumentFile item);

        public async Task<CompanyDocumentFile> UpladCompanyDocumentFile(CompanyDocumentFile companyDocumentFile)
        {
            OnUploadFileCreated(companyDocumentFile);

            context.CompanyDocumentFiles.Add(companyDocumentFile);
            context.SaveChanges();

            return companyDocumentFile;
        }

        partial void OnCompanyDocumentFileGet(CompanyDocumentFile item);
        public async Task<CompanyDocumentFile> GetCompanyDocumentFileId(int? companydocumentId)
        {
            var items = context.CompanyDocumentFiles
                              .AsNoTracking()
                              .Where(i => i.DOCUMENTID == companydocumentId);

            var item = items.FirstOrDefault();

            OnCompanyDocumentFileGet(item);

            return await Task.FromResult(item);
        }

        partial void OnCompanyDocumentFileUpdated(CompanyDocumentFile item);
        public async Task<CompanyDocumentFile> UpdateCompanyDocumentFile(int? companyDocumentFileId, CompanyDocumentFile companyDocumentFile)
        {
            OnCompanyDocumentFileUpdated(companyDocumentFile);

            var item = context.CompanyDocumentFiles
                              .Where(i => i.DOCUMENTID == companyDocumentFileId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(companyDocumentFile);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return companyDocumentFile;
        }
        partial void OnCompanyDocumentFileDeleted(CompanyDocumentFile item);
        public async Task<CompanyDocumentFile> DeleteCompanyDocumentFile(int? companyDocumentFileId)
        {
            var item = context.CompanyDocumentFiles
                              .Where(i => i.DOCUMENTID == companyDocumentFileId)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnCompanyDocumentFileDeleted(item);

            context.CompanyDocumentFiles.Remove(item);

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
        partial void OnProcessTypeDeleted(ProcessType item);

        public async Task<ProcessType> DeleteProcessType(int? processTypeId)
        {
            var item = context.ProcessTypes
                              .Where(i => i.PROCESS_TYPE_ID == processTypeId)
                              .Include(i => i.WorkOrders)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnProcessTypeDeleted(item);

            context.ProcessTypes.Remove(item);

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

        partial void OnProcessTypeGet(ProcessType item);

        public async Task<ProcessType> GetProcessTypeByProcessTypeId(int? processTypeId)
        {
            var items = context.ProcessTypes
                              .AsNoTracking()
                              .Where(i => i.PROCESS_TYPE_ID == processTypeId);

            var item = items.FirstOrDefault();

            OnProcessTypeGet(item);

            return await Task.FromResult(item);
        }


        public async Task<ProcessType> CancelProcessTypeChanges(ProcessType item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnProcessTypeUpdated(ProcessType item);

        public async Task<ProcessType> UpdateProcessType(int? processTypeId, ProcessType processType)
        {
            OnProcessTypeUpdated(processType);

            var item = context.ProcessTypes
                              .Where(i => i.PROCESS_TYPE_ID == processTypeId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(processType);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return processType;
        }
        #endregion

        #region "Critically Master"
        public async Task ExportCriticalityMastersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/clearrisk/criticalitymasters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')") : $"export/clearrisk/criticalitymasters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')", true);
        }

        public async Task ExportCriticalityMastersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/clearrisk/criticalitymasters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')") : $"export/clearrisk/criticalitymasters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')", true);
        }

        partial void OnCriticalityMastersRead(ref IQueryable<CriticalityMaster> items);

        public async Task<IQueryable<CriticalityMaster>> GetCriticalityMasters(Query query = null)
        {
            var items = context.CriticalityMasters.AsQueryable();

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

            OnCriticalityMastersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCriticalityMasterCreated(CriticalityMaster item);

        public async Task<CriticalityMaster> CreateCriticalityMaster(CriticalityMaster criticalityMaster)
        {
            OnCriticalityMasterCreated(criticalityMaster);

            context.CriticalityMasters.Add(criticalityMaster);
            context.SaveChanges();

            return criticalityMaster;
        }

        partial void OnCriticalityMasterGet(CriticalityMaster item);

        public async Task<CriticalityMaster> GetCriticalityMasterByCriticalityId(int? criticalityId)
        {
            var items = context.CriticalityMasters
                              .AsNoTracking()
                              .Where(i => i.CRITICALITY_ID == criticalityId);

            var item = items.FirstOrDefault();

            OnCriticalityMasterGet(item);

            return await Task.FromResult(item);
        }

        public async Task<CriticalityMaster> CancelCriticalityMasterChanges(CriticalityMaster item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnCriticalityMasterUpdated(CriticalityMaster item);

        public async Task<CriticalityMaster> UpdateCriticalityMaster(int? criticalityId, CriticalityMaster criticalityMaster)
        {
            OnCriticalityMasterUpdated(criticalityMaster);

            var item = context.CriticalityMasters
                              .Where(i => i.CRITICALITY_ID == criticalityId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(criticalityMaster);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return criticalityMaster;
        }

        partial void OnCriticalityMasterDeleted(CriticalityMaster item);

        public async Task<CriticalityMaster> DeleteCriticalityMaster(int? criticalityId)
        {
            var item = context.CriticalityMasters
                              .Where(i => i.CRITICALITY_ID == criticalityId)
                              .Include(i => i.WorkOrders)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnCriticalityMasterDeleted(item);

            context.CriticalityMasters.Remove(item);

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
        #endregion

        #region Order Status
        public async Task ExportOrderStatusesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/clearrisk/orderstatuses/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')") : $"export/clearrisk/orderstatuses/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')", true);
        }

        public async Task ExportOrderStatusesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/clearrisk/orderstatuses/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')") : $"export/clearrisk/orderstatuses/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')", true);
        }

        partial void OnOrderStatusesRead(ref IQueryable<OrderStatus> items);

        public async Task<IQueryable<OrderStatus>> GetOrderStatuses(Query query = null)
        {
            var items = context.OrderStatuses.AsQueryable();

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

            OnOrderStatusesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnOrderStatusCreated(OrderStatus item);

        public async Task<OrderStatus> CreateOrderStatus(OrderStatus orderStatus)
        {
            OnOrderStatusCreated(orderStatus);

            context.OrderStatuses.Add(orderStatus);
            context.SaveChanges();

            return orderStatus;
        }

        partial void OnOrderStatusDeleted(OrderStatus item);

        public async Task<OrderStatus> DeleteOrderStatus(int? orderStatusId)
        {
            var item = context.OrderStatuses
                              .Where(i => i.ORDER_STATUS_ID == orderStatusId)
                              .Include(i => i.WorkOrders)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnOrderStatusDeleted(item);

            context.OrderStatuses.Remove(item);

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

        partial void OnOrderStatusGet(OrderStatus item);

        public async Task<OrderStatus> GetOrderStatusByOrderStatusId(int? orderStatusId)
        {
            var items = context.OrderStatuses
                              .AsNoTracking()
                              .Where(i => i.ORDER_STATUS_ID == orderStatusId);

            var item = items.FirstOrDefault();

            OnOrderStatusGet(item);

            return await Task.FromResult(item);
        }

        public async Task<OrderStatus> CancelOrderStatusChanges(OrderStatus item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnOrderStatusUpdated(OrderStatus item);

        public async Task<OrderStatus> UpdateOrderStatus(int? orderStatusId, OrderStatus orderStatus)
        {
            OnOrderStatusUpdated(orderStatus);

            var item = context.OrderStatuses
                              .Where(i => i.ORDER_STATUS_ID == orderStatusId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(orderStatus);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return orderStatus;
        }
        #endregion

        #region "Priority"
        public async Task ExportPriorityMastersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/clearrisk/prioritymasters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')") : $"export/clearrisk/prioritymasters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')", true);
        }

        public async Task ExportPriorityMastersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/clearrisk/prioritymasters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')") : $"export/clearrisk/prioritymasters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')", true);
        }

        partial void OnPriorityMastersRead(ref IQueryable<PriorityMaster> items);

        public async Task<IQueryable<PriorityMaster>> GetPriorityMasters(Query query = null)
        {
            var items = context.PriorityMasters.AsQueryable();

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

            OnPriorityMastersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnPriorityMasterCreated(PriorityMaster item);

        public async Task<PriorityMaster> CreatePriorityMaster(PriorityMaster priorityMaster)
        {
            OnPriorityMasterCreated(priorityMaster);

            context.PriorityMasters.Add(priorityMaster);
            context.SaveChanges();

            return priorityMaster;
        }

        partial void OnPriorityMasterDeleted(PriorityMaster item);

        public async Task<PriorityMaster> DeletePriorityMaster(int? priorityId)
        {
            var item = context.PriorityMasters
                              .Where(i => i.PRIORITY_ID == priorityId)
                              .Include(i => i.WorkOrders)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnPriorityMasterDeleted(item);

            context.PriorityMasters.Remove(item);

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

        partial void OnPriorityMasterGet(PriorityMaster item);

        public async Task<PriorityMaster> GetPriorityMasterByPriorityId(int? priorityId)
        {
            var items = context.PriorityMasters
                              .AsNoTracking()
                              .Where(i => i.PRIORITY_ID == priorityId);

            var item = items.FirstOrDefault();

            OnPriorityMasterGet(item);

            return await Task.FromResult(item);
        }

        public async Task<PriorityMaster> CancelPriorityMasterChanges(PriorityMaster item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnPriorityMasterUpdated(PriorityMaster item);

        public async Task<PriorityMaster> UpdatePriorityMaster(int? priorityId, PriorityMaster priorityMaster)
        {
            OnPriorityMasterUpdated(priorityMaster);

            var item = context.PriorityMasters
                              .Where(i => i.PRIORITY_ID == priorityId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(priorityMaster);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return priorityMaster;
        }
        #endregion

        #region Person Contact
        public async Task ExportPersonContactsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/clearrisk/personcontacts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')") : $"export/clearrisk/personcontacts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')", true);
        }

        public async Task ExportPersonContactsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/clearrisk/personcontacts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')") : $"export/clearrisk/personcontacts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')", true);
        }

        partial void OnPersonContactsRead(ref IQueryable<PersonContact> items);

        public async Task<IQueryable<PersonContact>> GetPersonContacts(Query query = null)
        {
            var items = context.PersonContacts.AsQueryable();

            items = items.Include(i => i.Company);

            items = items.Include(i => i.PersonalState);

            items = items.Include(i => i.PersonalCountry);

            items = items.Include(i => i.BusinessState);

            items = items.Include(i => i.BusinessCountry);

            items = items.Include(i => i.StatusMaster);

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

            OnPersonContactsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnPersonContactCreated(PersonContact item);

        public async Task<PersonContact> CreatePersonContact(PersonContact personContact)
        {
            OnPersonContactCreated(personContact);

            context.PersonContacts.Add(personContact);
            context.SaveChanges();

            return personContact;
        }

        partial void OnPersonContactDeleted(PersonContact item);

        public async Task<PersonContact> DeletePersonContact(int? personContactId)
        {
            var item = context.PersonContacts
                              .Where(i => i.PERSON_CONTACT_ID == personContactId)

                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnPersonContactDeleted(item);

            context.PersonContacts.Remove(item);

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

        partial void OnPersonContactGet(PersonContact item);

        public async Task<PersonContact> GetPersonContactByPersonContactId(int? personContactId)
        {
            var items = context.PersonContacts
                              .AsNoTracking()
                              .Where(i => i.PERSON_CONTACT_ID == personContactId);

            items = items.Include(i => i.Company);

            items = items.Include(i => i.PersonalState);

            items = items.Include(i => i.PersonalCountry);

            items = items.Include(i => i.BusinessState);

            items = items.Include(i => i.BusinessCountry);

            items = items.Include(i => i.StatusMaster);

            var item = items.FirstOrDefault();

            OnPersonContactGet(item);

            return await Task.FromResult(item);
        }

        public async Task<PersonContact> CancelPersonContactChanges(PersonContact item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnPersonContactUpdated(PersonContact item);

        public async Task<PersonContact> UpdatePersonContact(int? personContactId, PersonContact personContact)
        {
            OnPersonContactUpdated(personContact);

            var item = context.PersonContacts
                              .Where(i => i.PERSON_CONTACT_ID == personContactId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(personContact);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return personContact;
        }
        #endregion

        #region "Account Stats"
        public AccountStats GetAccountStats()
        {
            int registerCopany = context.People
                        .Where(p => p.COMPANYTYPE == 2)
                        .Count();

            int WorkOrderRasied = context.WorkOrders
                        .Where(p => p.STATUS_ID == 3)
                        .Count();

            int WorkOrderCompleted = context.WorkOrders
                       .Where(p => p.STATUS_ID == 11)
                       .Count();

            decimal CurrentBalance = context.People
                        .Where(p => p.COMPANYTYPE == 2)
                        .Sum(p => p.CURRENT_BALANCE);

            var stats = new AccountStats()
            {
                registerCompany = registerCopany,
                WorkOrderRasied = WorkOrderRasied,
                WorkOrderCompleted = WorkOrderCompleted,
                CurrentBalance = CurrentBalance
            };

            return stats;


        }

        public CompanyStats GetAccountStats(int companyId)
        {
            int registerCopany = context.People.Where(p => p.PARENT_PERSON_ID == companyId)
                        .Where(p => p.COMPANYTYPE == 3)
                        .Count();

            int Downloaded = context.WorkOrders.Where(w => w.COMPANY_ID == companyId)
                        .Where(p => p.STATUS_LEVEL_ID == 2)
                        .Count();

            int Failure = context.WorkOrders.Where(w => w.COMPANY_ID == companyId)
                       .Where(p => p.WARNING_LEVEL_ID == 2)
                       .Count();

            int Critical = context.WorkOrders.Where(w => w.COMPANY_ID == companyId)
                      .Where(p => p.WARNING_LEVEL_ID == 3)
                      .Count();

            decimal CurrentBalance = context.People
                        .Where(p => p.COMPANYTYPE == 2 && p.PERSON_ID == companyId)
                        .Sum(p => p.CURRENT_BALANCE);

            var stats = new CompanyStats()
            {
                registerEmployee = registerCopany,
                Downloaded = Downloaded,
                Failure = Failure,
                Critical = Critical,
                CurrentBalance = CurrentBalance
            };

            return stats;


        }

        public IEnumerable<RevenueByCompany> GetRevenueByCompany()
        {
            return context.People.Where(p => p.COMPANYTYPE == 2)
                                  .ToList()
                                 .GroupBy(p => p.COMPANY_NAME)
                                 .Select(group => new RevenueByCompany()
                                 {
                                     Company = group.Key,
                                     Revenue = group.Sum(opportunity => opportunity.CURRENT_BALANCE)
                                 });
        }

        public IEnumerable<MonthlyWorkOrder> GetCompanyOrderStatus(int companyId)
        {
            return context.WorkOrders.Include(a => a.WarningLevel).Where(p => p.COMPANY_ID == companyId && (p.DATE_RAISED >= DateTime.Now.AddDays(-30)
                                      && p.DATE_RAISED < DateTime.Now.AddDays(1)))
                                  .ToList()
                                 .GroupBy(p => p.WarningLevel.NAME)
                                 .Select(group => new MonthlyWorkOrder()
                                 {
                                     Status = group.Key,
                                     Count = group.Count()
                                 });
        }



        public IEnumerable<MonthlyAssesments> GetCompanyAssessmentStatus(int companyId)
        {
            return context.Assesments.Include(a => a.EntityStatus).Where(p => p.COMPANYID == companyId && (p.ASSESMENTDATE >= DateTime.Now.AddDays(-30)
                                      && p.ASSESMENTDATE < DateTime.Now.AddDays(1)))
                                  .ToList()
                                 .GroupBy(p => p.EntityStatus.NAME)
                                 .Select(group => new MonthlyAssesments()
                                 {
                                     Status = group.Key,
                                     Count = group.Count()
                                 });
        }

        public IEnumerable<WorkOrderByMonth> MonthyWorkOrderStatus()
        {
            return context.WorkOrders
                                 .Include(p => p.OrderStatus)
                                 .ToList()
                                 .GroupBy(opportunity => new DateTime(opportunity.DATE_RAISED.Year, opportunity.DATE_RAISED.Month, 1))
                                 .Select(group => new WorkOrderByMonth()
                                 {
                                     NoofOrder = group.Count(),
                                     Month = group.Key
                                 })
                                 .OrderBy(deals => deals.Month);
        }

        public IEnumerable<SurveyByMonth> MonthySurveyStats()
        {
            return context.SurveyReports
                                 .Include(p => p.Survey)
                                 .ToList()
                                 .GroupBy(opportunity => new DateTime(opportunity.SURVEY_DATE.Year, opportunity.SURVEY_DATE.Month, 1))
                                 .Select(group => new SurveyByMonth()
                                 {
                                     NoofSurvey = group.Count(),
                                     Month = group.Key
                                 })
                                 .OrderBy(deals => deals.Month);
        }

        public IEnumerable<SurveyByName> MonthySurveyConductStats()
        {
            return context.SurveyReports
                                 .Include(p => p.Survey)
                                 .ToList()
                                 .GroupBy(p => p.Survey.SURVEY_TITLE)
                                 .Select(group => new SurveyByName()
                                 {
                                     noofSurvey = group.Count(),
                                     SurveyTitle = group.Key
                                 })
                                 .OrderBy(deals => deals.SurveyTitle);
        }
        #endregion

        #region "Currency"
        public async Task ExportCurrenciesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/clearrisk/currencies/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')") : $"export/clearrisk/currencies/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')", true);
        }

        public async Task ExportCurrenciesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/clearrisk/currencies/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')") : $"export/clearrisk/currencies/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')", true);
        }

        partial void OnCurrenciesRead(ref IQueryable<Currency> items);

        public async Task<IQueryable<Currency>> GetCurrencies(Query query = null)
        {
            var items = context.Currencies.AsQueryable();

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

            OnCurrenciesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCurrencyCreated(Currency item);

        public async Task<Currency> CreateCurrency(Currency currency)
        {
            OnCurrencyCreated(currency);

            context.Currencies.Add(currency);
            context.SaveChanges();

            return currency;
        }


        partial void OnCurrencyDeleted(Currency item);

        public async Task<Currency> DeleteCurrency(int? currencyId)
        {
            var item = context.Currencies
                              .Where(i => i.CURRENCY_ID == currencyId)
                              .Include(i => i.CompanyAccountTransactions)
                              .Include(i => i.People)
                              .Include(i => i.CompanyTransactions)

                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnCurrencyDeleted(item);

            context.Currencies.Remove(item);

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

        partial void OnCurrencyGet(Currency item);

        public async Task<Currency> GetCurrencyByCurrencyId(int? currencyId)
        {
            var items = context.Currencies
                              .AsNoTracking()
                              .Where(i => i.CURRENCY_ID == currencyId);

            var item = items.FirstOrDefault();

            OnCurrencyGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Currency> CancelCurrencyChanges(Currency item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnCurrencyUpdated(Currency item);

        public async Task<Currency> UpdateCurrency(int? currencyId, Currency currency)
        {
            OnCurrencyUpdated(currency);

            var item = context.Currencies
                              .Where(i => i.CURRENCY_ID == currencyId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(currency);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return currency;
        }
        #endregion

        #region "Company Account Transactions"
        public async Task ExportCompanyAccountTransactionsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/clearrisk/companyaccounttransactions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')") : $"export/clearrisk/companyaccounttransactions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')", true);
        }

        public async Task ExportCompanyAccountTransactionsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/clearrisk/companyaccounttransactions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')") : $"export/clearrisk/companyaccounttransactions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')", true);
        }

        partial void OnCompanyAccountTransactionsRead(ref IQueryable<CompanyAccountTransaction> items);

        public async Task<IQueryable<CompanyAccountTransaction>> GetCompanyAccountTransactions(Query query = null)
        {
            var items = context.CompanyAccountTransactions.AsQueryable();

            items = items.Include(i => i.Person);

            items = items.Include(i => i.Currency);

            items = items.Include(i => i.Assesment);
            items = items.Include(i => i.Transaction);

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

            OnCompanyAccountTransactionsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCompanyAccountTransactionCreated(CompanyAccountTransaction item);

        public async Task<CompanyAccountTransaction> CreateCompanyAccountTransaction(CompanyAccountTransaction companyAccountTransaction)
        {
            OnCompanyAccountTransactionCreated(companyAccountTransaction);

            context.CompanyAccountTransactions.Add(companyAccountTransaction);
            context.SaveChanges();

            return companyAccountTransaction;
        }

        #endregion

        #region "Help & Reference"

        partial void OnHelpReferencesRead(ref IQueryable<HelpReference> items);

        public async Task<IQueryable<HelpReference>> GetHelpReferences(Query query = null)
        {
            var items = context.HelpReferences.AsQueryable();

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

            OnHelpReferencesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnHelpReferenceCreated(HelpReference item);

        public async Task<HelpReference> CreateHelpReference(HelpReference helpReference)
        {
            OnHelpReferenceCreated(helpReference);

            context.HelpReferences.Add(helpReference);
            context.SaveChanges();

            return helpReference;
        }


        partial void OnHelpReferenceDeleted(HelpReference item);

        public async Task<HelpReference> DeleteHelpReference(int? helpId)
        {
            var item = context.HelpReferences
                              .Where(i => i.HELP_ID == helpId)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnHelpReferenceDeleted(item);

            context.HelpReferences.Remove(item);

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

        partial void OnHelpReferenceGet(HelpReference item);

        public async Task<HelpReference> GetHelpReferenceByHelpId(int? helpId)
        {
            var items = context.HelpReferences
                              .AsNoTracking()
                              .Where(i => i.HELP_ID == helpId);

            var item = items.FirstOrDefault();

            OnHelpReferenceGet(item);

            return await Task.FromResult(item);
        }
        public async Task<HelpReference> GetHelpReferenceByHelpScreenId(int? screenId)
        {
            var items = context.HelpReferences
                              .AsNoTracking()
                              .Where(i => i.SCREEN_ID == screenId);

            var item = items.FirstOrDefault();

            OnHelpReferenceGet(item);

            return await Task.FromResult(item);
        }

        public async Task<HelpReference> CancelHelpReferenceChanges(HelpReference item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnHelpReferenceUpdated(HelpReference item);

        public async Task<HelpReference> UpdateHelpReference(int? helpId, HelpReference helpReference)
        {
            OnHelpReferenceUpdated(helpReference);

            var item = context.HelpReferences
                              .Where(i => i.HELP_ID == helpId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(helpReference);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return helpReference;
        }
        partial void OnAssesmentScheduleUpdated(AssesmentSchedule item);

        public async Task<AssesmentSchedule> UpdateAssesmentSchedule(int? assessment_schedule_id, AssesmentSchedule assesmentSchedule)
        {
            OnAssesmentScheduleUpdated(assesmentSchedule);

            var item = context.AssesmentSchedules
                              .Where(i => i.ASSESSMENT_SCHEDULE_ID == assessment_schedule_id)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(assesmentSchedule);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return assesmentSchedule;
        }

        #endregion

        #region How To Use

        partial void OnHowToUsesRead(ref IQueryable<HowToUse> items);

        public async Task<IQueryable<HowToUse>> GetHowToUses(Query query = null)
        {
            var items = context.HowToUses.AsQueryable();

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

            OnHowToUsesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnHowToUseCreated(HowToUse item);

        public async Task<HowToUse> CreateHowToUse(HowToUse howToUse)
        {
            OnHowToUseCreated(howToUse);

            context.HowToUses.Add(howToUse);
            context.SaveChanges();

            return howToUse;
        }

        partial void OnHowToUseDeleted(HowToUse item);
        public async Task<HowToUse> DeleteHowToUse(int? howToUseId)
        {
            var item = context.HowToUses
                              .Where(i => i.HowToUseId == howToUseId)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnHowToUseDeleted(item);

            context.HowToUses.Remove(item);

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

        partial void OnHowToUseUpdated(HowToUse item);

        public async Task<HowToUse> UpdateHowToUse(int? howToUseId, HowToUse howToUse)
        {
            OnHowToUseUpdated(howToUse);

            var item = context.HowToUses
                              .Where(i => i.HowToUseId == howToUseId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(howToUse);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return howToUse;
        }

        partial void OnHowToUseGet(HowToUse item);

        public async Task<HowToUse> GetHowToUseByHelpId(int? howToUseId)
        {
            var items = context.HowToUses
                              .AsNoTracking()
                              .Where(i => i.HowToUseId == howToUseId);

            var item = items.FirstOrDefault();

            OnHowToUseGet(item);

            return await Task.FromResult(item);
        }


        #endregion

        #region "Assessment Schedules"
        partial void OnAssesmentscheduleRead(ref IQueryable<Models.ClearConnection.AssesmentSchedule> items);

        public async Task<IQueryable<Models.ClearConnection.AssesmentSchedule>> GetAssesmentSchedules(Query query = null)
        {
            var items = context.AssesmentSchedules.AsQueryable();

            // items = items.Include(i => i.Assesment1);

            items = items.Include(i => i.ScheduleType);

            items = items.Include(i => i.Assesment);






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

            OnAssesmentscheduleRead(ref items);

            return await Task.FromResult(items);
        }
        #endregion

        #region "Survey Types"
        //public async Task ExportSurveyTypesToExcel(Query query = null, string fileName = null)
        //{
        //    navigationManager.NavigateTo(query != null ? query.ToUrl($"export/clearrisk/surveytypes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/clearrisk/surveytypes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        //}

        //public async Task ExportSurveyTypesToCSV(Query query = null, string fileName = null)
        //{
        //    navigationManager.NavigateTo(query != null ? query.ToUrl($"export/clearrisk/surveytypes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/clearrisk/surveytypes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        //}

        partial void OnSurveyTypesRead(ref IQueryable<SurveyType> items);

        public async Task<IQueryable<SurveyType>> GetSurveyTypes(Query query = null)
        {
            var items = context.SurveyTypes.AsQueryable();

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

            OnSurveyTypesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSurveyTypeCreated(SurveyType item);

        public async Task<SurveyType> CreateSurveyType(SurveyType surveyType)
        {
            OnSurveyTypeCreated(surveyType);

            context.SurveyTypes.Add(surveyType);
            context.SaveChanges();

            return surveyType;
        }

        partial void OnDesignationCreated(Desigation item);

        public async Task<Desigation> CreateDesignation(Desigation desigation)
        {
            OnDesignationCreated(desigation);

            context.Desigations.Add(desigation);
            context.SaveChanges();

            return desigation;
        }
        partial void OnDesignationDeleted(Desigation item);
        public async Task<Desigation> DeleteDesigation(int? designationId)
        {
            var item = context.Desigations
                              .Where(i => i.DESIGNATION_ID == designationId)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnDesignationDeleted(item);

            context.Desigations.Remove(item);

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
        partial void OnSurveyTypeDeleted(SurveyType item);

        public async Task<SurveyType> DeleteSurveyType(int? surveyTypeId)
        {
            var item = context.SurveyTypes
                              .Where(i => i.SURVEY_TYPE_ID == surveyTypeId)
                              .Include(i => i.Surveys)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnSurveyTypeDeleted(item);

            context.SurveyTypes.Remove(item);

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

        partial void OnSurveyTypeGet(SurveyType item);

        public async Task<SurveyType> GetSurveyTypeBySurveyTypeId(int? surveyTypeId)
        {
            var items = context.SurveyTypes
                              .AsNoTracking()
                              .Where(i => i.SURVEY_TYPE_ID == surveyTypeId);

            var item = items.FirstOrDefault();

            OnSurveyTypeGet(item);

            return await Task.FromResult(item);
        }
        partial void OnAssesmentScheduleGet(AssesmentSchedule item);

        public async Task<AssesmentSchedule> GetAssesmentScheduleId(int? assesmentScheduleID)
        {
            var items = context.AssesmentSchedules
                              .AsNoTracking()
                              .Where(i => i.ASSESSMENT_SCHEDULE_ID == assesmentScheduleID);

            var item = items.FirstOrDefault();

            OnAssesmentScheduleGet(item);

            return await Task.FromResult(item);
        }

        partial void OnDesignationGet(Desigation item);

        public async Task<Desigation> GetDesigationId(int? designationID)
        {
            var items = context.Desigations
                              .AsNoTracking()
                              .Where(i => i.DESIGNATION_ID == designationID);

            var item = items.FirstOrDefault();

            OnDesignationGet(item);

            return await Task.FromResult(item);
        }

        public async Task<SurveyType> CancelSurveyTypeChanges(SurveyType item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnSurveyTypeUpdated(SurveyType item);

        public async Task<SurveyType> UpdateSurveyType(int? surveyTypeId, SurveyType surveyType)
        {
            OnSurveyTypeUpdated(surveyType);

            var item = context.SurveyTypes
                              .Where(i => i.SURVEY_TYPE_ID == surveyTypeId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(surveyType);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return surveyType;
        }

        partial void OnDesignationUpdated(Desigation item);

        public async Task<Desigation> UpdateDesigation(int? desigationId, Desigation desigation)
        {
            OnDesignationUpdated(desigation);

            var item = context.Desigations
                              .Where(i => i.DESIGNATION_ID == desigationId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(desigation);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return desigation;
        }
        #endregion

        #region WorkSite Activity"
        partial void OnSiteActivitiesRead(ref IQueryable<Models.ClearConnection.SiteActivity> items);

        public async Task<IQueryable<Models.ClearConnection.SiteActivity>> GetSiteActivities(Query query = null)
        {
            var items = context.SiteActivities.AsQueryable();

            items = items.Include(i => i.Assesment);

            items = items.Include(i => i.Worker);

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

            OnSiteActivitiesRead(ref items);

            return await Task.FromResult(items);
        }
        #endregion

        #region SystemFeatures
        partial void OnGetSystemFeatures(ref IQueryable<SystemFeatures> items);

        public async Task<IQueryable<SystemFeatures>> GetSystemFeatures(Query query = null)
        {
            var items = context.SystemFeaturess.AsQueryable();

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

            OnGetSystemFeatures(ref items);

            return await Task.FromResult(items);
        }


        partial void OnCreateSystemFeatures(SystemFeatures item);

        public async Task<SystemFeatures> CreateSystemFeatures(SystemFeatures systemFeatures)
        {
            OnCreateSystemFeatures(systemFeatures);

            context.SystemFeaturess.Add(systemFeatures);
            context.SaveChanges();

            return systemFeatures;
        }


        partial void OnGetSystemFeaturesByFeature_ID(SystemFeatures item);

        public async Task<SystemFeatures> GetSystemFeaturesByFeature_ID(int? feature_ID)
        {
            var items = context.SystemFeaturess
                              .AsNoTracking()
                              .Where(i => i.Feature_ID == feature_ID);

            var item = items.FirstOrDefault();

            OnGetSystemFeaturesByFeature_ID(item);

            return await Task.FromResult(item);
        }


        partial void OnUpdateSystemFeatures(SystemFeatures item);

        public async Task<SystemFeatures> UpdateSystemFeatures(int? feature_ID, SystemFeatures systemFeatures)
        {
            OnUpdateSystemFeatures(systemFeatures);

            var item = context.SystemFeaturess
                              .Where(i => i.Feature_ID == feature_ID)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(systemFeatures);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return systemFeatures;
        }

        partial void OnDeleteSystemFeatures(SystemFeatures item);

        public async Task<SystemFeatures> DeleteSystemFeatures(int? feature_ID)
        {
            var item = context.SystemFeaturess
                              .Where(i => i.Feature_ID == feature_ID)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnDeleteSystemFeatures(item);

            context.SystemFeaturess.Remove(item);

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
        #endregion

        #region BlogData

        partial void OnGetBlogTable(ref IQueryable<BlogTable> items);

        public async Task<IQueryable<BlogTable>> GetBlogTable(Query query = null)
        {
            var items = context.BlogTables.AsQueryable();

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

            OnGetBlogTable(ref items);

            return await Task.FromResult(items);
        }

        partial void OnDeleteBlogTable(BlogTable item);

        public async Task<BlogTable> DeleteBlogTable(int? blog_Id)
        {
            var item = context.BlogTables
                              .Where(i => i.Blog_Id == blog_Id)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnDeleteBlogTable(item);

            context.BlogTables.Remove(item);

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

        partial void OnCreateBlogTable(BlogTable item);

        public async Task<BlogTable> CreateBlogTable(BlogTable blogTable)
        {
            OnCreateBlogTable(blogTable);

            context.BlogTables.Add(blogTable);
            context.SaveChanges();

            return blogTable;
        }

        partial void OnEditBlogTableByHelpId(BlogTable item);

        public async Task<BlogTable> EditBlogTableByHelpId(int? blog_Id)
        {
            var items = context.BlogTables
                              .AsNoTracking()
                              .Where(i => i.Blog_Id == blog_Id);

            var item = items.FirstOrDefault();

            OnEditBlogTableByHelpId(item);

            return await Task.FromResult(item);
        }

        partial void OnUpdateBlogTable(BlogTable item);

        public async Task<BlogTable> UpdateBlogTable(int? blog_Id, BlogTable blogTable)
        {
            OnUpdateBlogTable(blogTable);

            var item = context.BlogTables
                              .Where(i => i.Blog_Id == blog_Id)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(blogTable);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return blogTable;
        }

        partial void OnGetBlogTableById(BlogTable item);

        public async Task<BlogTable> GetBlogTableById(long? blog_Id)
        {

            try
            {

                var items = context.BlogTables
                                  .AsNoTracking()
                                  .Where(i => i.Blog_Id == blog_Id);

                //items = items.Include(i => i.BgTittle);

                //items = items.Include(i => i.BgShortDetails);

                //items = items.Include(i => i.BgLongDetails);

                //items = items.Include(i => i.BgImgPath);

                //items = items.Include(i => i.CreatedBy);

                //items = items.Include(i => i.CreatedDate);

                var item = items.FirstOrDefault();

                OnGetBlogTableById(item);

                return await Task.FromResult(item);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}
