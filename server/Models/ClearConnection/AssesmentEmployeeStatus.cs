using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
  [Table("ASSESMENT_EMPLOYEE_STATUS", Schema = "dbo")]
  public partial class AssesmentEmployeeStatus
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ASSESMENT_STATUS_ID
    {
      get;
      set;
    }

    public ICollection<AssesmentEmployeeAttachement> AssesmentEmployeeAttachements { get; set; }
    public string NAME
    {
      get;
      set;
    }
  }
}
