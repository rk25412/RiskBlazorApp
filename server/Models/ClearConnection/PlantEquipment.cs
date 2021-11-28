using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
  [Table("PLANT_EQUIPMENT", Schema = "dbo")]
  public partial class PlantEquipment
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PLANT_EQUIPMENT_ID
    {
      get;
      set;
    }

    public ICollection<SwmsPlantequipment> SwmsPlantequipments { get; set; }
    public string NAME
    {
      get;
      set;
    }
    public int EQUIPMENT_VALUE
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
