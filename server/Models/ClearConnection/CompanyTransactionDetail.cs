using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
  [Table("COMPANY_TRANSACTION_DETAIL", Schema = "dbo")]
  public partial class CompanyTransactionDetail
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TransactionDetailID
    {
      get;
      set;
    }
    public int TransactionID
    {
      get;
      set;
    }
    public CompanyTransaction CompanyTransaction { get; set; }
    public int PRODUCT_ID
    {
      get;
      set;
    }
    public Applicence Applicence { get; set; }
    public decimal PRICE
    {
      get;
      set;
    }
    public decimal TAX_AMT
    {
      get;
      set;
    }
    public decimal PRICE_TOTAL
    {
      get;
      set;
    }
    public DateTime? PlanValidFrom
    {
      get;
      set;
    }
    public DateTime? PlanValidUpTo
    {
      get;
      set;
    }
    public DateTime? CREATED_DATE
    {
      get;
      set;
    }
    public Int64? CREATOR_ID
    {
      get;
      set;
    }
    public DateTime? UPDATED_DATE
    {
      get;
      set;
    }
    public Int64? UPDATER_ID
    {
      get;
      set;
    }
    public DateTime? DELETED_DATE
    {
      get;
      set;
    }
    public Int64? DELETER_ID
    {
      get;
      set;
    }
    public bool? IS_DELETED
    {
      get;
      set;
    }
  }
}
