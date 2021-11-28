using Radzen;
using System;
using System.Web;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components;
using Clear.Risk.Data;

namespace Clear.Risk
{
    public partial class SurveyServices
    {
        private readonly ClearConnectionContext context;
        private readonly NavigationManager navigationManager;

        public SurveyServices(ClearConnectionContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        partial void OnQuestionTypesRead(ref IQueryable<Models.ClearConnection.QuestionType> items);

        public async Task<IQueryable<Models.ClearConnection.QuestionType>> GetQuestionTypes(Query query = null)
        {
            var items = context.QuestionTypes.AsQueryable();

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

            OnQuestionTypesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSurveysRead(ref IQueryable<Models.ClearConnection.Survey> items);

        public async Task<IQueryable<Models.ClearConnection.Survey>> GetSurveys(Query query = null)
        {
            var items = context.Surveys.AsQueryable();

            items = items.Include(i => i.SurveyType);
            items = items.Include(i => i.BasedSurvey);
            items = items.Include(i => i.Company);

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

        partial void OnSurveyQuestionCreated(Models.ClearConnection.SurveyQuestion item);

        public async Task<Models.ClearConnection.SurveyQuestion> CreateSurveyQuestion(Models.ClearConnection.SurveyQuestion surveyQuestion)
        {
            OnSurveyQuestionCreated(surveyQuestion);
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.SurveyQuestions.Add(surveyQuestion);
                    context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
           

            return surveyQuestion;
        }

        partial void OnSurveyQuestionMaxIDGet(int item);

        public async Task<int> GetQuestionMaxID(int SurveyId)
        {
            try
            {
                int maxID = 0;
                var items = context.SurveyQuestions.AsQueryable().Where(a => a.SURVEY_ID == SurveyId);


                maxID = items.Count() + 1;

                OnSurveyQuestionMaxIDGet(maxID);

                return await Task.FromResult(maxID);
            }
            catch (Exception ex)
            {
                return -1;
            }

        }

        partial void OnSurveyQuestionsRead(ref IQueryable<Models.ClearConnection.SurveyQuestion> items);

        public async Task<IQueryable<Models.ClearConnection.SurveyQuestion>> GetSurveyQuestions(Query query = null)
        {
            var items = context.SurveyQuestions.AsQueryable();

            items = items.Include(i => i.Survey);

            items = items.Include(i => i.QuestionType);

            items = items.Include(i => i.ParentQuestion);

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

            OnSurveyQuestionsRead(ref items);

            return await Task.FromResult(items);
        }

        public async Task<IQueryable<Models.ClearConnection.SurveyQuestion>> GetSurveyQuestionList(Query query = null)
        {
            var items = context.SurveyQuestions.AsQueryable();

            items = items.Include(i => i.Survey);

            items = items.Include(i => i.QuestionType);
            items = items.Include(i => i.WarningLevel);

            items = items.Include(i => i.ParentQuestion);

            items = items.Include(i => i.SurveyAnswers);

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

            OnSurveyQuestionsRead(ref items);

            return await Task.FromResult(items);
        }

        public async Task<IQueryable<Models.ClearConnection.SurveyQuestion>> GetLimitedSurveyQuestions(Query query = null)
        {
            var items = context.SurveyQuestions.AsQueryable();

            //items = items.Include(i => i.Survey);

           // items = items.Include(i => i.QuestionType);

           // items = items.Include(i => i.SurveyQuestion1);

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

            OnSurveyQuestionsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSurveyDeleted(Models.ClearConnection.Survey item);

        public async Task<Models.ClearConnection.Survey> DeleteSurvey(int? surveyId)
        {
            var item = context.Surveys
                              .Where(i => i.SURVEY_ID == surveyId)
                              .Include(i => i.Assesments)
                              .Include(i => i.SurveyQuestions)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnSurveyDeleted(item);

            context.Surveys.Remove(item);

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

        partial void OnSurveyQuestionDeleted(Models.ClearConnection.SurveyQuestion item);

        public async Task<Models.ClearConnection.SurveyQuestion> DeleteSurveyQuestion(int? surveyqQuestionId)
        {
            var item = context.SurveyQuestions
                              .Where(i => i.SURVEYQ_QUESTION_ID == surveyqQuestionId)
                              //.Include(i => i.SurveyQuestions1)
                              .Include(i => i.SurveyAnswers)
                              .Include(i => i.SurveyAnswerValues)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnSurveyQuestionDeleted(item);

            context.SurveyQuestions.Remove(item);

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

        partial void OnSurveyTypesRead(ref IQueryable<Models.ClearConnection.SurveyType> items);

        public async Task<IQueryable<Models.ClearConnection.SurveyType>> GetSurveyTypes(Query query = null)
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

        partial void OnSurveyCreated(Models.ClearConnection.Survey item);

        public async Task<Models.ClearConnection.Survey> CreateSurvey(Models.ClearConnection.Survey survey)
        {
            OnSurveyCreated(survey);

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.Surveys.Add(survey);
                    context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }

           

            return survey;
        }

        partial void OnSurveyGet(Models.ClearConnection.Survey item);

        public async Task<Models.ClearConnection.Survey> GetSurveyBySurveyId(int? surveyId)
        {
            var items = context.Surveys
                              .AsNoTracking()
                              .Where(i => i.SURVEY_ID == surveyId);

            items = items.Include(i => i.SurveyType);

            var item = items.FirstOrDefault();

            OnSurveyGet(item);

            return await Task.FromResult(item);
        }

        partial void OnSurveyUpdated(Models.ClearConnection.Survey item);

        public async Task<Models.ClearConnection.Survey> UpdateSurvey(int? surveyId, Models.ClearConnection.Survey survey)
        {
            OnSurveyUpdated(survey);

            var item = context.Surveys
                              .Where(i => i.SURVEY_ID == surveyId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(survey);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return survey;
        }

        partial void OnSurveyQuestionGet(Models.ClearConnection.SurveyQuestion item);

        public async Task<Models.ClearConnection.SurveyQuestion> GetSurveyQuestionBySurveyqQuestionId(int? surveyqQuestionId)
        {
            var items = context.SurveyQuestions
                              .AsNoTracking()
                              .Where(i => i.SURVEYQ_QUESTION_ID == surveyqQuestionId);

            items = items.Include(i => i.Survey);

            items = items.Include(i => i.QuestionType);

            items = items.Include(i => i.ParentQuestion);


            var item = items.FirstOrDefault();

            OnSurveyQuestionGet(item);

            return await Task.FromResult(item);
        }

        partial void OnSurveyQuestionUpdated(Models.ClearConnection.SurveyQuestion item);

        public async Task<Models.ClearConnection.SurveyQuestion> UpdateSurveyQuestion(int? surveyqQuestionId, Models.ClearConnection.SurveyQuestion surveyQuestion)
        {
            OnSurveyQuestionUpdated(surveyQuestion);

            var item = context.SurveyQuestions
                              .Where(i => i.SURVEYQ_QUESTION_ID == surveyqQuestionId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(surveyQuestion);
            entry.State = EntityState.Modified;
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                context.Entry(item).State = EntityState.Unchanged;
                throw ex;
            }

            return surveyQuestion;
        }

        partial void OnSurveyAnswersRead(ref IQueryable<Models.ClearConnection.SurveyQuestionAnswer> items);

        public async Task<IQueryable<Models.ClearConnection.SurveyQuestionAnswer>> GetSurveyAnswers(Query query = null)
        {
            var items = context.SurveyQuestionAnswers.AsQueryable();

            items = items.Include(i => i.Question).Include(a=>a.WarningLevel);

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

            OnSurveyAnswersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSurveyAnswerDeleted(Models.ClearConnection.SurveyQuestionAnswer item);

        public async Task<Models.ClearConnection.SurveyQuestionAnswer> DeleteSurveyAnswer(int? surveyAnswerId)
        {
            var item = context.SurveyQuestionAnswers
                              .Where(i => i.SURVEY_ANSWER_ID == surveyAnswerId)
                              //.Include(i => i.SurveyAnswerValues)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnSurveyAnswerDeleted(item);

            context.SurveyQuestionAnswers.Remove(item);

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

        partial void OnSurveyAnswerCreated(Models.ClearConnection.SurveyQuestionAnswer item);

        public async Task<Models.ClearConnection.SurveyQuestionAnswer> CreateSurveyAnswer(Models.ClearConnection.SurveyQuestionAnswer surveyAnswer)
        {
            OnSurveyAnswerCreated(surveyAnswer);

            context.SurveyQuestionAnswers.Add(surveyAnswer);
            context.SaveChanges();

            return surveyAnswer;
        }

        partial void OnSurveyAnswerGet(Models.ClearConnection.SurveyQuestionAnswer item);

        public async Task<Models.ClearConnection.SurveyQuestionAnswer> GetSurveyAnswerBySurveyAnswerId(int? surveyAnswerId)
        {
            var items = context.SurveyQuestionAnswers
                              .AsNoTracking()
                              .Where(i => i.SURVEY_ANSWER_ID == surveyAnswerId);

            items = items.Include(i => i.Question);

            var item = items.FirstOrDefault();

            OnSurveyAnswerGet(item);

            return await Task.FromResult(item);
        }

        partial void OnSurveyAnswerUpdated(Models.ClearConnection.SurveyQuestionAnswer item);

        public async Task<Models.ClearConnection.SurveyQuestionAnswer> UpdateSurveyAnswer(int? surveyAnswerId, Models.ClearConnection.SurveyQuestionAnswer surveyAnswer)
        {
            OnSurveyAnswerUpdated(surveyAnswer);

            var item = context.SurveyQuestionAnswers
                              .Where(i => i.SURVEY_ANSWER_ID == surveyAnswerId)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(surveyAnswer);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return surveyAnswer;
        }

        partial void OnSurveyReportsRead(ref IQueryable<Models.ClearConnection.SurveyReport> items);

        public async Task<IQueryable<Models.ClearConnection.SurveyReport>> GetSurveyReports(Query query = null)
        {
            var items = context.SurveyReports.AsQueryable();

            items = items.Include(i => i.Survey);

            items = items.Include(i => i.Surveyor);

            items = items.Include(i => i.Assesment);

            items = items.Include(i => i.EscalationLevel);

            items = items.Include(i => i.WarningLevel);

            items = items.Include(i => i.EntityStatus);

            items = items.Include(i => i.Company);

            items = items.Include(i => i.Order);

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

            OnSurveyReportsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSurveyReportCreated(Models.ClearConnection.SurveyReport item);

        public async Task<Models.ClearConnection.SurveyReport> CreateSurveyReport(Models.ClearConnection.SurveyReport surveyReport)
        {
            OnSurveyReportCreated(surveyReport);
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.SurveyReports.Add(surveyReport);
                    context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
              

            return surveyReport;
        }

        partial void OnSurveyAnswerChecklistsRead(ref IQueryable<Models.ClearConnection.SurveyAnswerChecklist> items);

        public async Task<IQueryable<Models.ClearConnection.SurveyAnswerChecklist>> GetSurveyAnswerChecklists(Query query = null)
        {
            var items = context.SurveyAnswerChecklists.AsQueryable();

            items = items.Include(i => i.SurveyReport);

            items = items.Include(i => i.Question) ;

            items = items.Include(i => i.SurveyParentQuestion);

            items = items.Include(i => i.SurveyAnswerValues).ThenInclude(b=>b.SurveyAnswer);

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

        partial void OnSurveyAnswerChecklistCreated(Models.ClearConnection.SurveyAnswerChecklist item);

        public async Task<Models.ClearConnection.SurveyAnswerChecklist> CreateSurveyAnswerChecklist(Models.ClearConnection.SurveyAnswerChecklist surveyAnswerChecklist)
        {
            OnSurveyAnswerChecklistCreated(surveyAnswerChecklist);

            context.SurveyAnswerChecklists.Add(surveyAnswerChecklist);
            context.SaveChanges();

            return surveyAnswerChecklist;
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

        partial void OnSurveyAnswerValueCreated(Models.ClearConnection.SurveyAnswerValue item);

        public async Task<Models.ClearConnection.SurveyAnswerValue> CreateSurveyAnswerValue(Models.ClearConnection.SurveyAnswerValue surveyAnswerValue)
        {
            OnSurveyAnswerValueCreated(surveyAnswerValue);

            context.SurveyAnswerValues.Add(surveyAnswerValue);
            context.SaveChanges();

            return surveyAnswerValue;
        }

        partial void OnSurveyReportGet(Models.ClearConnection.SurveyReport item);

        public async Task<Models.ClearConnection.SurveyReport> GetSurveyReportBySurveyReportId(int? surveyReportId)
        {
            var items = context.SurveyReports
                              .AsNoTracking()
                              .Where(i => i.SURVEY_REPORT_ID == surveyReportId);

            items = items.Include(i => i.Survey);

            items = items.Include(i => i.Surveyor);

            items = items.Include(i => i.Assesment);

            items = items.Include(i => i.Assesment).ThenInclude(i => i.WarningLevel);

            items = items.Include(i => i.Assesment).ThenInclude(i => i.EntityStatus);

            items = items.Include(i => i.EscalationLevel);

            items = items.Include(i => i.WarningLevel);

            items = items.Include(i => i.EntityStatus);

            items = items.Include(i => i.Company);

            var item = items.FirstOrDefault();

            OnSurveyReportGet(item);

            return await Task.FromResult(item);
        }

    }
}
