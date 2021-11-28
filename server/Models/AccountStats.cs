using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clear.Risk.Models
{
    public class AccountStats
    {
        public int registerCompany { get; set; }
        public int WorkOrderRasied { get; set; }
        public int WorkOrderCompleted { get; set; }
        public decimal CurrentBalance { get; set; }
    }

    public class RevenueByCompany
    {
        public string Company { get; set; }
        public decimal Revenue { get; set; }
    }

    public class RevenueByEmployee
    {
        public string Employee { get; set; }
        public decimal Revenue { get; set; }
    }

    public class RevenueByMonth
    {
        public DateTime Month { get; set; }
        public decimal Revenue { get; set; }
    }

    public class WorkOrderByMonth
    {
        public DateTime Month { get; set; }
        public int NoofOrder { get; set; }
    }

    public class SurveyByMonth
    {
        public DateTime Month { get; set; }
        public int NoofSurvey { get; set; }
    }

    public class SurveyByName
    {
        public string SurveyTitle { get; set; }
        public int noofSurvey { get; set; }
    }


    public class CompanyStats
    {
        public int registerEmployee { get; set; }
        public int Downloaded { get; set; }
        public int Failure { get; set; }

        public int Critical { get; set; }
        public decimal CurrentBalance { get; set; }
    }

    public class MonthlyWorkOrder
    {
        public string Status { get; set; }
        public int Count { get; set; }
    }

    public class MonthlyAssesments
    {
        public string Status { get; set; }
        public int Count { get; set; }
    }
}
