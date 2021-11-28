using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
  [Table("REFERENCED_LEGISLATION", Schema = "dbo")]
  public partial class ReferencedLegislation
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int LEGISLATION_ID
    {
      get;
      set;
    }

    public ICollection<SwmsReferencedlegislation> SwmsReferencedlegislations { get; set; }
    public string NAME
    {
      get;
      set;
    }
    public int LEGISLATION_VALUE
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
    public int STATUS_LEVEL_ID
    {
      get;
      set;
    }
    public int ENTITY_STATUS_ID
    {
      get;
      set;
    }
  }
}
