using Clear.Risk.Models.ClearConnection;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clear.Risk.Services
{
    public class ScheduleStarter
    {
        private readonly RunScheduleAssesment _runSchedules;
        private readonly ClearConnectionService context;

        public ScheduleStarter(RunScheduleAssesment runSchedules, ClearConnectionService context)
        {
            _runSchedules = runSchedules;
            this.context = context;
        }

        public async Task startAssessment(string jobId, int assesmentId, int companyId, string CronExp)
        {
            Assesment assesment = await context.GetAssesmentByAssesmentid(assesmentId);

            assesment.IsScheduleRunning = true;
            assesment.AssessmentActivity += $" Assessment Schedule Started on {DateTime.Now}.";
            
            var updateResult = context.UpdateAssesment(assesmentId, assesment);

            RecurringJob.AddOrUpdate(jobId, () => _runSchedules.Invoke(assesmentId, companyId), CronExp, TimeZoneInfo.Local);
        }
    }
}
