using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clear.Risk.ViewModels
{
    public class WorkActivity
    {
        public int AssesmentSiteId { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? UpdateLatitude { get; set; }
        public decimal? UpdateLongitude { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public virtual int EmployeeID { get; set; }
        public string WorkOrderNo { get; set; }
        public bool IsComplete { get; set; }
        public int? TaskId { get; set; }
        public string WorkOrderStatus { get; set; }
    }

    public class AssesmentSiteActivity
    {
        public int AssesmentSiteId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }

        public virtual int EmployeeID { get; set; }
        public string WorkOrderNo { get; set; }


    }

    public class SignedDto
    {
        public int taskId { get; set; }
        public string EmpID { get; set; }
        public string imageUrl { get; set; }

        //unauthorised worker has signed a document
        public bool IsNameExist { get; set; }
        public string Username { get; set; }
        public int IsSignedStatus { get; set; }
        public int attachementid { get; set; }

        public DateTime SingNatureDate { get; set; }
    }
    /*-------------------------------------------------*/
    public class SignInstructionDto
    {
        public int VersionNo { get; set; }
        public string FileName { get; set; }
        //public int EmpId { get; set; }
        public int AssesmentId { get; set; }
        public string imageUrl { get; set; }
        public int IsSignedStatus { get; set; }
        public DateTime SingnatureDate { get; set; }
    }


    public class SignObserverDto
    {
        public int EmpID { get; set; }
        public string name { get; set; }
        public string ImageUrl { get; set; }
        public int IsSignedStatus { get; set; }
        public int attachementid { get; set; }
        public DateTime SignatureDate { get; set; }
    }
    /*-------------------------------------------------*/

    public class ResponseDto
    {
        public string Result { get; set; }
    }

    public class SurveyReportViewModel
    {
        public int SurveyId { get; set; }
        public int AssesmentId { get; set; }

        public int? WorkOrerId { get; set; }

        public int? CompanyId { get; set; }

        public string Comments { get; set; }

        public IList<QuestionViewModel> Questions { get; set; }
    }

    public class QuestionViewModel
    {
        public int QuestionId { get; set; }
        public int? ParentQuestionId { get; set; }

        public string FreeText { get; set; }

        public IEnumerable<int> Answers { get; set; }

        public bool? YesNo { get; set; }

        public string AnswerComments { get; set; }
    }

    public class StripToken
    {
        public string TokenId { get; set; }
        public string ErroMessage { get; set; }
    }


    public class PayentDetails
    {
        public virtual string TransactionId { get; set; }
        public virtual string TransactionStatus { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual bool Paid { get; set; }
        public virtual string ReceiptNumber { get; set; }
    }
}
