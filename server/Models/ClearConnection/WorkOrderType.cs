using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
  [Table("WORK_ORDER_TYPE", Schema = "dbo")]
  public partial class WorkOrderType
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int WORK_ORDER_TYPE_ID
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
