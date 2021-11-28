using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clear.Risk.Models.ClearConnection;
using Hangfire;

namespace Clear.Risk.Services
{
    public class ScheduleStopper
    {
        private readonly ClearConnectionService context;
        public ScheduleStopper(ClearConnectionService context)
        {
            this.context = context;
        }

        public async Task StopAssessment(string jobId, int assessmentId)
        {
            Assesment assesment = await context.GetAssesmentByAssesmentid(assessmentId);

            assesment.IsScheduleRunning = true;

            if (assesment.ENTITY_STATUS_ID == 5)
            {
                assesment.AssessmentActivity += $" Assessment Schedule Stopped on {DateTime.Now}.";
                assesment.ENTITY_STATUS_ID = 4;
            }

            var updateResult = context.UpdateAssesment(assessmentId, assesment);

            RecurringJob.RemoveIfExists(jobId);
        }

    }
}
