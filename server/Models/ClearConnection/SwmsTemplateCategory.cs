using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
  [Table("SWMS_TEMPLATE_CATEGORY", Schema = "dbo")]
  public partial class SwmsTemplateCategory
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TEMPLATE_CATEGORY_ID
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
  }
}
