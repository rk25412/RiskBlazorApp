using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
  [Table("SWMS_HAZARDOUSMATERIAL", Schema = "dbo")]
  public partial class SwmsHazardousmaterial
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int HAZARDOUSMATERIALID
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
    public int HAZARD_MATERIAL_ID
    {
      get;
      set;
    }
    public HazardMaterialValue HazardMaterialValue { get; set; }
    public bool IS_DELETED
    {
      get;
      set;
    }
  }
}
