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
    public partial class AssesmentService
    {
        private readonly ClearConnectionContext context;
        private readonly NavigationManager navigationManager;

        public AssesmentService(ClearConnectionContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        partial void OnSurveysRead(ref IQueryable<Models.ClearConnection.Survey> items);

        public async Task<IQueryable<Models.ClearConnection.Survey>> GetSurveys(Query query = null)
        {
            var items = context.Surveys.AsQueryable();

            items = items.Include(i => i.SurveyType);

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

            OnSurveysRead(ref items);

            return await Task.FromResult(items);
        }


        public async Task ExportSiteActivitiesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/clearrisk/siteactivities/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')") : $"export/clearrisk/siteactivities/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')", true);
        }

        public async Task ExportSiteActivitiesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/clearrisk/siteactivities/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')") : $"export/clearrisk/siteactivities/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')", true);
        }

        partial void OnSiteActivitiesRead(ref IQueryable<SiteActivity> items);

        public async Task<IQueryable<SiteActivity>> GetSiteActivities(Query query = null)
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


        public async Task<IQueryable<SiteActivity>> GetEmployeeSiteActivities(Query query = null)
        {
            var items = context.SiteActivities.AsQueryable();
            items=items.Include(x => x.Assesment);

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

        partial void OnSiteActivityCreated(SiteActivity item);

        public async Task<SiteActivity> CreateSiteActivity(SiteActivity siteActivity)
        {
            OnSiteActivityCreated(siteActivity);

            context.SiteActivities.Add(siteActivity);
            context.SaveChanges();

            return siteActivity;
        }

        partial void OnSiteActivityUpdated(SiteActivity item);

        public async Task<SiteActivity> UpdateSiteActivity(int? siteActivityId,SiteActivity siteActivity)
        {
            OnSiteActivityUpdated(siteActivity);

            var item = context.SiteActivities
                              .Where(i => i.SITE_ACTIVITY_ID == siteActivityId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(siteActivity);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return siteActivity;
        }
    }
}
