using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
  [Table("SWMS_PPEREQUIRED", Schema = "dbo")]
  public partial class SwmsPperequired
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PPEID
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
    public int PPE_VALUE_ID
    {
      get;
      set;
    }
    public Ppevalue Ppevalue { get; set; }
    public bool IS_DELETED
    {
      get;
      set;
    }
  }
}
