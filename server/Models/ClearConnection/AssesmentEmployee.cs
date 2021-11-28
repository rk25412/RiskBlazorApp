using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
    [Table("ASSESMENT_EMPLOYEE", Schema = "dbo")]
    public partial class AssesmentEmployee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ASSESMENT_EMPLOYEE_ID
        {
            get;
            set;
        }
        public int? ASSESMENT_ID
        {
            get;
            set;
        }

        [ForeignKey("ASSESMENT_ID")]
        public Assesment Assesment { get; set; }
        public int EMPLOYEE_ID
        {
            get;
            set;
        }

        [ForeignKey("EMPLOYEE_ID")]
        public Person Employee { get; set; }


        public int WARNING_LEVEL_ID
        {
            get;
            set;
        }

        [ForeignKey("WARNING_LEVEL_ID")]
        public WarningLevel WarningLevel { get; set; }

        public ICollection<AssesmentEmployeeAttachement> AssignedEmployees { get; set; }

        public bool IS_ACTIVE { get; set; } = true;

        //-------------------------------------------------------------
        public string SignatureImageUrl { get; set; }
        public DateTime? Sign_Date { get; set; }
        public DateTime? Server_Sign_Date { get; set; }
        public int? SignedStatus { get; set; }
        public int? VersionNo { get; set; }
        public string FileName { get; set; }

        //--------------------------------------------------------------

        [NotMapped]
        public bool isSigned
        {
            get
            {
                return SignedStatus != null ? true : false;
            }
        }
    }
}
