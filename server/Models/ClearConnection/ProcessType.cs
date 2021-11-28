using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
  [Table("PROCESS_TYPE", Schema = "dbo")]
  public partial class ProcessType
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PROCESS_TYPE_ID
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
