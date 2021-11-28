using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
  [Table("SURVEY_TYPE", Schema = "dbo")]
  public partial class SurveyType
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SURVEY_TYPE_ID
    {
      get;
      set;
    }

    public ICollection<Survey> Surveys { get; set; }
    public string NAME
    {
      get;
      set;
    }
  }
}
