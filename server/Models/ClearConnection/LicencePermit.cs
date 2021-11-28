using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
  [Table("LICENCE_PERMIT", Schema = "dbo")]
  public partial class LicencePermit
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PERMIT_ID
    {
      get;
      set;
    }

    public ICollection<SwmsLicencespermit> SwmsLicencespermits { get; set; }
    public string NAME
    {
      get;
      set;
    }
    public int PERMIT_VALUE
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
