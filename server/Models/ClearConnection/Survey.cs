using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
    [Table("SURVEY", Schema = "dbo")]
    public partial class Survey
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SURVEY_ID
        {
            get;
            set;
        }

        public ICollection<Assesment> Assesments { get; set; }
        public ICollection<SurveyQuestion> SurveyQuestions { get; set; }
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
        public DateTime UPDATED_DATE
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
        public int SURVEY_TYPE_ID
        {
            get;
            set;
        }
        public SurveyType SurveyType { get; set; }
        public string SURVEY_TITLE
        {
            get;
            set;
        }
        public string SURVEY_DESC
        {
            get;
            set;
        }
        public int? LAST_QUESTION_ID
        {
            get;
            set;
        }
        public int TOTAL_SCORE
        {
            get;
            set;
        }
        public int YES_NO_SCORE
        {
            get;
            set;
        }
        public int CHOICE_SCORE
        {
            get;
            set;
        }
        public int TEXT_SCORE
        {
            get;
            set;
        }
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
        public string EXTERNAL_REF
        {
            get;
            set;
        }

        public int? COMPANY_ID { get; set; }

        [ForeignKey("COMPANY_ID")]
        public Person Company { get; set; }

        public bool ISBASED { get; set; }

        public int? BASE_SURVEY_ID { get; set; }

        [ForeignKey("BASE_SURVEY_ID")]
        public Survey BasedSurvey { get; set; }

        [NotMapped]
        public string SurveyTypeName
        {
            get
            {
                return SurveyType?.NAME ?? string.Empty;
            }
        }


        [NotMapped]
        public int TScore
        {
            get
            {
                return (YES_NO_SCORE + CHOICE_SCORE + TEXT_SCORE);
            }
        }
    }
}
