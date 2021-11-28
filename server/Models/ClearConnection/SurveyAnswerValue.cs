using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
  [Table("SURVEY_ANSWER_VALUE", Schema = "dbo")]
  public partial class SurveyAnswerValue
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SURVEYVALUE_ID
    {
      get;
      set;
    }
    public int? SURVEY_CHECKLIST_ID
    {
      get;
      set;
    }
       [ForeignKey("SURVEY_CHECKLIST_ID")] 
    public SurveyAnswerChecklist AnswerChecklist { get; set; }
    public int? SURVEY_ANSWER_ID
    {
      get;
      set;
    }
       
    public SurveyQuestionAnswer SurveyAnswer { get; set; }
    public string ANSWER_DESCRIPTION
    {
      get;
      set;
    }
  }
}
