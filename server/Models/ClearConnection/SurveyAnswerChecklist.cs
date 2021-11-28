using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
    [Table("SURVEY_ANSWER_CHECKLIST", Schema = "dbo")]
    public partial class SurveyAnswerChecklist
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SURVEY_ANSWER_CHECKLIST_ID
        {
            get;
            set;
        }

        public ICollection<SurveyAnswerValue> SurveyAnswerValues { get; set; }
        public DateTime? CREATED_DATE
        {
            get;
            set;
        }
        public int CREATOR_ID
        {
            get;
            set;
        }
        public DateTime? UPDATED_DATE
        {
            get;
            set;
        }
        public int UPDATER_ID
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
        public int SURVEY_REPORT_ID
        {
            get;
            set;
        }
        [ForeignKey("SURVEY_REPORT_ID")]
        public SurveyReport SurveyReport { get; set; }
        public int? SURVEY_QUESTION_ID
        {
            get;
            set;
        }

        [ForeignKey("SURVEY_QUESTION_ID")]
        public SurveyQuestion Question { get; set; }

        public int? PARENT_QUESTION_ID
        {
            get;
            set;
        }

        [ForeignKey("PARENT_QUESTION_ID")]
        public SurveyQuestion SurveyParentQuestion { get; set; }
        public int ESCALATION_LEVEL_ID
        {
            get;
            set;
        }
        public int WARNING_LEVEL_ID
        {
            get;
            set;
        }
        public int ENTITY_STATUS_ID
        {
            get;
            set;
        }
        public string COMMENTS
        {
            get;
            set;
        }

        public string SurveyorComments
        {
            get;
            set;
        }

        public string YESNO
        {
            get;
            set;
        }
    }
}
