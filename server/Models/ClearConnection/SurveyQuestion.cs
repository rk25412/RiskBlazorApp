using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
    [Table("SURVEY_QUESTION", Schema = "dbo")]
    public partial class SurveyQuestion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SURVEYQ_QUESTION_ID
        {
            get;
            set;
        }

        // public ICollection<SurveyQuestion> SurveyQuestions1 { get; set; }
        public ICollection<SurveyQuestionAnswer> SurveyAnswers { get; set; }

        public ICollection<SurveyQuestionAnswer> SurveyGoToAnswers { get; set; }
        public ICollection<SurveyAnswerValue> SurveyAnswerValues { get; set; }

        public ICollection<SurveyAnswerChecklist> SurveyAnswerChecklists { get; set; }
        public int SURVEY_ID
        {
            get;
            set;
        }

        [ForeignKey("SURVEY_ID")]
        public Survey Survey { get; set; }
        public DateTime? CREATED_DATE
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
        public bool? IS_DELETED
        {
            get;
            set;
        }
        public int? SURVEYQ_ORDER
        {
            get;
            set;
        }
        public int QUESTION_TYPE_ID
        {
            get;
            set;
        }

        [ForeignKey("QUESTION_TYPE_ID")]
        public QuestionType QuestionType { get; set; }
        public string QUESTION_TITLE
        {
            get;
            set;
        }
        public string QUESTION_DESC
        {
            get;
            set;
        }
        public int? SCORE
        {
            get;
            set;
        }
        public int? PARENT_Q_ID
        {
            get;
            set;
        }

        [ForeignKey("PARENT_Q_ID")]
        public SurveyQuestion ParentQuestion { get; set; }
        public string EXTERNAL_REF
        {
            get;
            set;
        }

        public int WARNING_LEVEL_ID { get; set; }

        [ForeignKey("WARNING_LEVEL_ID")]
        public WarningLevel WarningLevel { get; set; }
    }
}
