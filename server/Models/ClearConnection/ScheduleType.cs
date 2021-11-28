using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
  [Table("SCHEDULE_TYPE", Schema = "dbo")]
  public partial class ScheduleType
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SCHEDULE_TYPE_ID
    {
      get;
      set;
    }

     
    public string NAME
    {
      get;
      set;
    }
  }
}
