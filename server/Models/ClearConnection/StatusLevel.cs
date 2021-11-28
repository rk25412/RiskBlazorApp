using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
  [Table("STATUS_LEVEL", Schema = "dbo")]
  public partial class StatusLevel
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int STATUS_LEVEL_ID
    {
      get;
      set;
    }

    public ICollection<Template> Templates { get; set; }
    public ICollection<SwmsTemplate> SwmsTemplates { get; set; }
    public string NAME
    {
      get;
      set;
    }
    public string ICON_PATH
    {
      get;
      set;
    }
  }
}
