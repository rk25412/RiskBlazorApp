using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
  [Table("HAZARD MATERIAL_VALUE", Schema = "dbo")]
  public partial class HazardMaterialValue
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int HAZARD_MATERIAL_ID
    {
      get;
      set;
    }

    public ICollection<SwmsHazardousmaterial> SwmsHazardousmaterials { get; set; }
    public string NAME
    {
      get;
      set;
    }
    public int MATERIAL_VALUE
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
