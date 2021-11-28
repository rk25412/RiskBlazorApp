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
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Clear.Risk.Models;
using System.Collections;
using System.Globalization;

namespace Clear.Risk.Pages
{
    public partial class Home : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager UriHelper { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }

        [Inject]
        protected ClearConnectionService ClearConnection { get; set; }

        protected IEnumerable<WorkOrderByMonth> _monthOrder;
        protected IEnumerable<WorkOrderByMonth> MonthOrder
        {
            get
            {
                return _monthOrder;
            }
            set
            {
                if (!object.Equals(_Stats, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "MonthOrder", NewValue = value, OldValue = _monthOrder };
                    _monthOrder = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected IEnumerable<SurveyByName> _surveyDone;
        protected IEnumerable<SurveyByName> SurveyDone
        {
            get
            {
                return _surveyDone;
            }
            set
            {
                if (!object.Equals(_Stats, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "SurveyDone", NewValue = value, OldValue = _surveyDone };
                    _surveyDone = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }


        protected IEnumerable<SurveyByMonth> _surveyMonthly;
        protected IEnumerable<SurveyByMonth> SurveyMonthly
        {
            get
            {
                return _surveyMonthly;
            }
            set
            {
                if (!object.Equals(_surveyMonthly, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "SurveyMonthly", NewValue = value, OldValue = _surveyMonthly };
                    _surveyMonthly = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected AccountStats _Stats;
        protected CompanyStats _CompanyStats;
        protected IEnumerable<RevenueByCompany> _revenueByCompany;
        protected IEnumerable<RevenueByCompany> RevenueCompany
        {
            get
            {
                return _revenueByCompany;
            }
            set
            {
                if (!object.Equals(_revenueByCompany, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "RevenueCompany", NewValue = value, OldValue = _revenueByCompany };
                    _revenueByCompany = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected IEnumerable<MonthlyWorkOrder> _monthlyWorkOrder;

        protected IEnumerable<MonthlyWorkOrder> GetMonthlyWorkOrders
        {
            get
            {
                return _monthlyWorkOrder;
            }
            set
            {
                if (!object.Equals(_monthlyWorkOrder, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "GetMonthlyWorkOrders", NewValue = value, OldValue = _monthlyWorkOrder };
                    _monthlyWorkOrder = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected IEnumerable<MonthlyAssesments> _monthlyAssessment;

        protected IEnumerable<MonthlyAssesments> GetMonthlyAssessments
        {
            get
            {
                return _monthlyAssessment;
            }
            set
            {
                if (!object.Equals(_monthlyAssessment, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "GetMonthlyAssessments", NewValue = value, OldValue = _monthlyAssessment };
                    _monthlyAssessment = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        protected AccountStats Stats
        {
            get
            {
                return _Stats;
            }
            set
            {
                if (!object.Equals(_Stats, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "Stats", NewValue = value, OldValue = _Stats };
                    _Stats = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected CompanyStats GetCompanyStats
        {
            get
            {
                return _CompanyStats;
            }
            set
            {
                if (!object.Equals(_CompanyStats, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "GetCompanyStats", NewValue = value, OldValue = _CompanyStats };
                    _CompanyStats = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        protected bool isLoading { get; set; }
        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            if (!Security.IsAuthenticated())
            {
                UriHelper.NavigateTo("Login", true);
            }
            else
            {
                //int id = Security.getUserId();
                //Models.ClearConnection.Person person = await ClearConnection.GetPersonByPersonId(id);
                //if (person.COMPANYTYPE == 3)
                //{
                //    NotificationService.Notify(NotificationSeverity.Error, $"Invalid Logon", $"As an employee, you can only logon using the app.", 180000);
                //    UriHelper.NavigateTo("/Login");
                //    return;
                //}

                isLoading = true;
                StateHasChanged();
                await Task.Delay(1);
                await Load();
                isLoading = false;
                StateHasChanged();

            }

        }
        protected async System.Threading.Tasks.Task Load()
        {
            if (Security.IsInRole("System Administrator"))
            {
                Stats = ClearConnection.GetAccountStats();
                RevenueCompany = ClearConnection.GetRevenueByCompany();

                MonthOrder = ClearConnection.MonthyWorkOrderStatus();
                //SurveyDone = ClearConnection.MonthySurveyConductStats();
                //SurveyMonthly = ClearConnection.MonthySurveyStats();
            }
            else if (Security.IsInRole("Administrator"))
            {
                GetCompanyStats = ClearConnection.GetAccountStats(Security.getCompanyId());
                GetMonthlyWorkOrders = ClearConnection.GetCompanyOrderStatus(Security.getCompanyId());
                GetMonthlyAssessments = ClearConnection.GetCompanyAssessmentStatus(Security.getCompanyId());
            }

        }

        protected string FormatAsUSD(object value)
        {
            return ((double)value).ToString("C0", CultureInfo.CreateSpecificCulture("en-US"));
        }
    }
}
