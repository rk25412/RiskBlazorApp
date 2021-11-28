using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
    [Table("SURVEY_REPORT", Schema = "dbo")]
    public partial class SurveyReport
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SURVEY_REPORT_ID
        {
            get;
            set;
        }

        public ICollection<SurveyAnswerChecklist> SurveyAnswerChecklists { get; set; }
        public DateTime SURVEY_DATE
        {
            get;
            set;
        }
        public DateTime CREATED_DATE
        {
            get;
            set;
        }
        public int? CREATOR_ID
        {
            get;
            set;
        }
        public DateTime? UPDATED_DATE
        {
            get;
            set;
        }
        public int? UPDATER_ID
        {
            get;
            set;
        }
        public DateTime? DELETED_DATE
        {
            get;
            set;
        }
        public int? DELETER_ID
        {
            get;
            set;
        }
        public bool IS_DELETED
        {
            get;
            set;
        }
        public int SURVEY_ID
        {
            get;
            set;
        }

        [ForeignKey("SURVEY_ID")]
        public Survey Survey { get; set; }

        public int SURVEYOR_ID
        {
            get;
            set;
        }
        [ForeignKey("SURVEYOR_ID")]
        public Person Surveyor { get; set; }
        public int? ASSESMENT_ID
        {
            get;
            set;
        }

        [ForeignKey("ASSESMENT_ID")]
        public Assesment Assesment { get; set; }
        public int? WORK_ORDER_ID
        {
            get;
            set;
        }

        [ForeignKey("WORK_ORDER_ID")]
        public WorkOrder Order { get; set; }
        public int ESCALATION_LEVEL_ID
        {
            get;
            set;
        }
        [ForeignKey("ESCALATION_LEVEL_ID")]
        public EscalationLevel EscalationLevel { get; set; }
        public int WARNING_LEVEL_ID
        {
            get;
            set;
        }
        [ForeignKey("WARNING_LEVEL_ID")]
        public WarningLevel WarningLevel { get; set; }
        public int ENTITY_STATUS_ID
        {
            get;
            set;
        }
        [ForeignKey("ENTITY_STATUS_ID")]
        public EntityStatus EntityStatus { get; set; }
        public string COMMENTS
        {
            get;
            set;
        }
        public int? COMPANY_ID
        {
            get;
            set;
        }

        [ForeignKey("COMPANY_ID")]
        public Person Company { get; set; }
        public string STATUS
        {
            get;
            set;
        }

        public string RowClass => this.WARNING_LEVEL_ID > 1 ? "table-danger" : null;

    }
}
