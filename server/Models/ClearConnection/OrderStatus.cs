using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
  [Table("ORDER_STATUS", Schema = "dbo")]
  public partial class OrderStatus
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ORDER_STATUS_ID
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
