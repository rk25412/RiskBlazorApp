using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
  [Table("PRIORITY_MASTER", Schema = "dbo")]
  public partial class PriorityMaster
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PRIORITY_ID
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
