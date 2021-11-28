using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
  [Table("SWMS_REFERENCEDLEGISLATION", Schema = "dbo")]
  public partial class SwmsReferencedlegislation
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int REFLID
    {
      get;
      set;
    }
    public int SWMSID
    {
      get;
      set;
    }
    public SwmsTemplate SwmsTemplate { get; set; }
    public int REFERENCE_LEGISLATION_ID
    {
      get;
      set;
    }
    public ReferencedLegislation ReferencedLegislation { get; set; }
    public bool IS_DELETED
    {
      get;
      set;
    }
  }
}
