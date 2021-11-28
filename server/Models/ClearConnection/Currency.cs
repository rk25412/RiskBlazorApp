using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
  [Table("CURRENCY", Schema = "dbo")]
  public partial class Currency
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CURRENCY_ID
    {
      get;
      set;
    }

    public ICollection<Person> People { get; set; }
    public ICollection<CompanyAccountTransaction> CompanyAccountTransactions { get; set; }
    public ICollection<CompanyTransaction> CompanyTransactions { get; set; }
    public string ISO_CODE
    {
      get;
      set;
    }
    public string CURSYMBOL
    {
      get;
      set;
    }
  }
}
