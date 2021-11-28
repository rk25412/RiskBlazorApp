using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
  [Table("CRITICALITY_MASTER", Schema = "dbo")]
  public partial class CriticalityMaster
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CRITICALITY_ID
    {
      get;
      set;
    }

    public ICollection<WorkOrder> WorkOrders { get; set; }
    public string NAME
    {
      get;
      set;
    }
  }
}
