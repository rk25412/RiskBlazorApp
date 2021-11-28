using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
    [Table("ASSESMENT_EMPLOYEE_ATTACHEMENT", Schema = "dbo")]
    public partial class AssesmentEmployeeAttachement
    {

        public int ASSESMENT_EMPLOYEE_ID
        {
            get;
            set;
        }

        [ForeignKey("ASSESMENT_EMPLOYEE_ID")]
        public AssesmentEmployee AssignedEmployee { get; set; }


        public int ATTACHEMENTID
        {
            get;
            set;
        }
        [ForeignKey("ATTACHEMENTID")]
        public AssesmentAttachement SWMSTemplateAttachement { get; set; }


        public string DOCUMENTNAME
        {
            get;
            set;
        }
        public string DOCUMENT_URL
        {
            get;
            set;
        }
        public DateTime ASSIGNED_DATE
        {
            get;
            set;
        }
        public DateTime? ACCEPTED_DATE
        {
            get;
            set;
        }
        public DateTime? SINGNATURE_DATE
        {
            get;
            set;
        }

        public DateTime? DEVICESINGNATURE_DATE
        {
            get;
            set;
        }
        public int EMPLOYEE_STATUS
        {
            get;
            set;
        }

        public int SRNo { get; set; }

        public int WARNING_LEVEL_ID
        {
            get;
            set;
        }

        [ForeignKey("WARNING_LEVEL_ID")]
        public WarningLevel WarningLevel { get; set; }

        public virtual byte[] SINGNATUREIMAGE { get; set; }

        [ForeignKey("EMPLOYEE_STATUS")]
        public AssesmentEmployeeStatus AssesmentEmployeeStatus { get; set; }



        //---------------------------------------------------------------------------------------------------

        public string OBSERVER_NAME { get; set; }
        public string OBSERVER_SIGN_URL { get; set; }
        public DateTime? OBSERVER_SIGN_DATE { get; set; }
        public int? OBSERVER_SIGN_STATUS { get; set; }

    }
}
