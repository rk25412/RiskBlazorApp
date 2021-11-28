using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
  [Table("SURVEY_ANSWER_DETAILS", Schema = "dbo")]
  public partial class SurveyAnswerDetail
  {
    [Key]
    public Int64 SURVEYA_DETAILS_ID
    {
      get;
      set;
    }
    public Int64? BUILIDING_ID
    {
      get;
      set;
    }
    public Int64? SURVEY_ID
    {
      get;
      set;
    }
    public Int64? USER_ID
    {
      get;
      set;
    }
    public int? QUESTION_ORDER
    {
      get;
      set;
    }
    public string QUESTION_TYPE
    {
      get;
      set;
    }
    public string QUESTION_TEXT
    {
      get;
      set;
    }
    public Int64? QUESTION_ID
    {
      get;
      set;
    }
    public int? ANSWER_ORDER_NO
    {
      get;
      set;
    }
    public string ANSWER_TEXT
    {
      get;
      set;
    }
    public DateTime? CREATED_DATE
    {
      get;
      set;
    }
    public string CHOICE_QUESTION_TEXT
    {
      get;
      set;
    }
  }
}
