using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using Clear.Risk.Models.ClearConnection;
using Microsoft.AspNetCore.Identity;
using Clear.Risk.Models;
using Hangfire;
using Clear.Risk.ViewModels;
//using Clear.Risk.Services;

namespace Clear.Risk.Pages.RiskAssesment
{
    public partial class EditAssesmentSchedule: ComponentBase
    {
        [Parameter]
        public dynamic ASSESSMENT_SCHEDULE_ID { get; set; }
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        [Inject]
        protected DialogService DialogService { get; set; }
        [Inject]
        protected SecurityService Security { get; set; }
        [Inject]
        protected ClearConnectionService ClearRisk { get; set; }

        [Inject]
        protected NavigationManager UriHelper { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }
        protected bool isLoading { get; set; }
        [Inject]
        protected RunScheduleAssesment runSchedules { get; set; }
        //protected RunSchedulesWithNewScheduler runSchedules { get; set; }


        Clear.Risk.Models.ClearConnection.AssesmentSchedule _assesmentSchedule;
        protected Clear.Risk.Models.ClearConnection.AssesmentSchedule assesmentScheduleResult
        {
            get
            {
                return _assesmentSchedule;
            }
            set
            {
                if (!object.Equals(_assesmentSchedule, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "assesmentScheduleResult", NewValue = value, OldValue = _assesmentSchedule };
                    _assesmentSchedule = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        IEnumerable<Clear.Risk.Models.ClearConnection.ScheduleType> _getScheduleTypesResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.ScheduleType> getScheduleTypesResult
        {
            get
            {
                return _getScheduleTypesResult;
            }
            set
            {
                if (!object.Equals(_getScheduleTypesResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getScheduleTypesResult", NewValue = value, OldValue = _getScheduleTypesResult };
                    _getScheduleTypesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        IEnumerable<ScheduleTime> _getScheduleTimeResult;
        protected IEnumerable<ScheduleTime> getScheduleTimeResult
        {
            get
            {
                return _getScheduleTimeResult;
            }
            set
            {
                if (!object.Equals(_getScheduleTimeResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getScheduleTimeResult", NewValue = value, OldValue = _getScheduleTimeResult };
                    _getScheduleTimeResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            if (!Security.IsAuthenticated())
            {
                UriHelper.NavigateTo("Login", true);
            }
            else
            {
                isLoading = true;
                StateHasChanged();
                await Task.Delay(1);
                await Load();
                isLoading = false;
                StateHasChanged();
            }
        }
        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            UriHelper.NavigateTo("edit-assesment-record" + "/" + assesmentScheduleResult.ASSESMENTID.ToString());
        }
        protected void ChangeSchedule(object value, string name)
        {
            // ScheduleType scheduleType = (ScheduleType)value;


        }
        Dictionary<DateTime, string> events = new Dictionary<DateTime, string>();
        protected void Change(DateTime? value)
        {

        }
        List<StaticDropDownModel> DdlTime = new List<StaticDropDownModel>();
        protected async System.Threading.Tasks.Task Load()
        {
            //assesment = new Assesment();
            var clearConnectionGetScheduleTypesResult = await ClearRisk.GetScheduleTypes(new Query());
            getScheduleTypesResult = clearConnectionGetScheduleTypesResult;
            getScheduleTimeResult = await ClearRisk.GetScheduleTimes(new Query());
            var scheduleResults = await ClearRisk.GetAssesmentScheduleId(int.Parse($"{ASSESSMENT_SCHEDULE_ID}"));
            assesmentScheduleResult = scheduleResults;
            DdlTime.AddRange(new StaticDropDownModel[] {
                    new StaticDropDownModel { Value = 1, Text = "1:00 AM"},
                    new StaticDropDownModel { Value = 2, Text = "2:00 AM"},
                    new StaticDropDownModel { Value = 3, Text = "3:00 AM"},
                    new StaticDropDownModel { Value = 4, Text = "4:00 AM"},
                    new StaticDropDownModel { Value = 5, Text = "5:00 AM"},
                    new StaticDropDownModel { Value = 6, Text = "6:00 AM"},
                    new StaticDropDownModel { Value = 7, Text = "7:00 AM"},
                    new StaticDropDownModel { Value = 8, Text = "8:00 AM"},
                    new StaticDropDownModel { Value = 9, Text = "9:00 AM"},
                    new StaticDropDownModel { Value = 10, Text = "10:00 AM"},
                    new StaticDropDownModel { Value = 11, Text = "11:00 AM"},
                    new StaticDropDownModel { Value = 12, Text = "12:00 PM"},
                    new StaticDropDownModel { Value = 13, Text = "1:00 PM"},
                    new StaticDropDownModel { Value = 14, Text = "2:00 PM"},
                    new StaticDropDownModel { Value = 15, Text = "3:00 PM"},
                    new StaticDropDownModel { Value = 16, Text = "4:00 PM"},
                    new StaticDropDownModel { Value = 17, Text = "5:00 PM"},
                    new StaticDropDownModel { Value = 18, Text = "6:00 PM"},
                    new StaticDropDownModel { Value = 19, Text = "7:00 PM"},
                    new StaticDropDownModel { Value = 20, Text = "8:00 PM"},
                    new StaticDropDownModel { Value = 21, Text = "9:00 PM"},
                    new StaticDropDownModel { Value = 22, Text = "10:00 PM"},
                    new StaticDropDownModel { Value = 23, Text = "11:00 PM"}
                    }
                );
        }
        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.AssesmentSchedule args)
        {
            try
            {
                var clearConnectionUpdateSurveyAnswerResult = await ClearRisk.UpdateAssesmentSchedule(int.Parse($"{ASSESSMENT_SCHEDULE_ID}"), assesmentScheduleResult);

                if(clearConnectionUpdateSurveyAnswerResult != null)
                {
                    string[] cronExp = { "0", "*", "*", "*", "*", }; // This expression is for firing the scheduler at every 00 of every hour
                    string exp = "";

                    if (args.SCHEDULE_TYPE_ID == 1)
                    {

                        //Removed
                        for (int i = 0; i < cronExp.Length; i++)
                        {
                            if (i == cronExp.Length - 1)
                                exp += cronExp[i];
                            else
                                exp += cronExp[i] + " ";
                        }
                    }
                    else if (args.SCHEDULE_TYPE_ID == 4 && args.INTERVAL != null)
                    {
                        cronExp[4] = string.Empty;

                        cronExp[1] = string.Empty;

                        if (args.MON)
                            cronExp[4] = "MON";

                        if (args.TUE)
                        {
                            if (string.IsNullOrEmpty(cronExp[4]))
                                cronExp[4] = "TUE";
                            else
                                cronExp[4] += ",TUE";
                        }

                        if (args.WED)
                        {
                            if (string.IsNullOrEmpty(cronExp[4]))
                                cronExp[4] = "WED";
                            else
                                cronExp[4] += ",WED";
                        }

                        if (args.THUS)
                        {
                            if (string.IsNullOrEmpty(cronExp[4]))
                                cronExp[4] = "THU";
                            else
                                cronExp[4] += ",THU";
                        }

                        if (args.FRI)
                        {
                            if (string.IsNullOrEmpty(cronExp[4]))
                                cronExp[4] = "FRI";
                            else
                                cronExp[4] += ",FRI";
                        }

                        if (args.SAT)
                        {
                            if (string.IsNullOrEmpty(cronExp[4]))
                                cronExp[4] = "SAT";
                            else
                                cronExp[4] += ",SAT";
                        }

                        if (args.SUN)
                        {
                            if (string.IsNullOrEmpty(cronExp[4]))
                                cronExp[4] = "SUN";
                            else
                                cronExp[4] += ",SUN";
                        }

                        cronExp[1] = $"{args.StartHour}-{args.EndHour}/{args.INTERVAL}";

                        for (int i = 0; i < cronExp.Length; i++)
                        {
                            if (i == cronExp.Length - 1)
                                exp += cronExp[i];
                            else
                                exp += cronExp[i] + " ";
                        }

                        //RecurringJob.AddOrUpdate($"Assessment-{result.ASSESMENTID} ", () => runSchedules.Invoke(result.ASSESMENTID), exp);
                    }
                    else if (args.SCHEDULE_TYPE_ID == 3)
                    {

                    }
                    else if (args.SCHEDULE_TYPE_ID == 2 && args.SCHEDULE_AT != null)
                    {
                        cronExp[4] = string.Empty;
                        cronExp[1] = string.Empty;
                        cronExp[0] = string.Empty;

                        TimeSpan span = DateTime.Parse(args.SCHEDULE_TIME.ToString()).TimeOfDay;
                        int hrs = span.Hours;
                        int mins = span.Minutes;

                        cronExp[0] = mins.ToString();
                        cronExp[1] = hrs.ToString();

                        if (args.MON)
                            cronExp[4] = "MON";

                        if (args.TUE)
                        {
                            if (string.IsNullOrEmpty(cronExp[4]))
                                cronExp[4] = "TUE";
                            else
                                cronExp[4] += ",TUE";
                        }

                        if (args.WED)
                        {
                            if (string.IsNullOrEmpty(cronExp[4]))
                                cronExp[4] = "WED";
                            else
                                cronExp[4] += ",WED";
                        }

                        if (args.THUS)
                        {
                            if (string.IsNullOrEmpty(cronExp[4]))
                                cronExp[4] = "THU";
                            else
                                cronExp[4] += ",THU";
                        }

                        if (args.FRI)
                        {
                            if (string.IsNullOrEmpty(cronExp[4]))
                                cronExp[4] = "FRI";
                            else
                                cronExp[4] += ",FRI";
                        }

                        if (args.SAT)
                        {
                            if (string.IsNullOrEmpty(cronExp[4]))
                                cronExp[4] = "SAT";
                            else
                                cronExp[4] += ",SAT";
                        }

                        if (args.SUN)
                        {
                            if (string.IsNullOrEmpty(cronExp[4]))
                                cronExp[4] = "SUN";
                            else
                                cronExp[4] += ",SUN";
                        }

                        for (int i = 0; i < cronExp.Length; i++)
                        {
                            if (i == cronExp.Length - 1)
                                exp += cronExp[i];
                            else
                                exp += cronExp[i] + " ";
                        }
                    }
                    RecurringJob.AddOrUpdate($"Assessment-{args.ASSESMENTID}", () => runSchedules.Invoke(args.ASSESMENTID, Security.getCompanyId()), exp, TimeZoneInfo.Local);
                }

                NotificationService.Notify(NotificationSeverity.Success, $"Success", $"AssesmentSchedule update sucesfully !",500000);
                DialogService.Close(assesmentScheduleResult);
            }
            catch (System.Exception clearConnectionUpdateSurveyAnswerException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to update AssesmentSchedule");
            }
        }

        protected void OnFromToHourChange()
        {
            if (assesmentScheduleResult.StartHour >= assesmentScheduleResult.EndHour)
                assesmentScheduleResult.EndHour = assesmentScheduleResult.StartHour + 1;

            if (assesmentScheduleResult.EndHour <= assesmentScheduleResult.StartHour)
                assesmentScheduleResult.EndHour = assesmentScheduleResult.StartHour + 1;
        }

    }
}
