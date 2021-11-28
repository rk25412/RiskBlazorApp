using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
  [Table("SWMS_LICENCESPERMITS", Schema = "dbo")]
  public partial class SwmsLicencespermit
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int LPID
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
    public int LICENCE_PERMIT_ID
    {
      get;
      set;
    }
    public LicencePermit LicencePermit { get; set; }
    public bool IS_DELETED
    {
      get;
      set;
    }
  }
}
