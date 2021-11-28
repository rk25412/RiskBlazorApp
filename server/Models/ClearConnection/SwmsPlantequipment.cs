using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
  [Table("SWMS_PLANTEQUIPMENT", Schema = "dbo")]
  public partial class SwmsPlantequipment
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PEID
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
    public int PLANT_EQUIPMENT_ID
    {
      get;
      set;
    }
    public PlantEquipment PlantEquipment { get; set; }
    public bool IS_DELETED
    {
      get;
      set;
    }
  }
}
