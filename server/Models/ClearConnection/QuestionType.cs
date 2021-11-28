using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
  [Table("QUESTION_TYPE", Schema = "dbo")]
  public partial class QuestionType
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int QUESTION_TYPE_ID
    {
      get;
      set;
    }

    public ICollection<SurveyQuestion> SurveyQuestions { get; set; }
    public string NAME
    {
      get;
      set;
    }
  }
}
