using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
  [Table("INDUSTRY_TYPE", Schema = "dbo")]
  public partial class IndustryType
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int INDUSTRY_TYPE_ID
    {
      get;
      set;
    }

    public ICollection<Assesment> Assesments { get; set; }
    public string NAME
    {
      get;
      set;
    }
  }
}
