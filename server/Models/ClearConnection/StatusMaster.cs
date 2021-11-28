using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
  [Table("STATUS_MASTER", Schema = "dbo")]
  public partial class StatusMaster
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int STATUS_ID
    {
      get;
      set;
    }

    public ICollection<Assesment> Assesments { get; set; }
    public ICollection<Template> Templates { get; set; }
    public ICollection<SwmsTemplate> SwmsTemplates { get; set; }
    public string NAME
    {
      get;
      set;
    }
  }
}
