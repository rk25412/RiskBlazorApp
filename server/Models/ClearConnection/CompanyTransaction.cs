using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
  [Table("COMPANY_TRANSACTION", Schema = "dbo")]
  public partial class CompanyTransaction
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TRANSACTIONID
    {
      get;
      set;
    }

    public ICollection<CompanyTransactionDetail> CompanyTransactionDetails { get; set; }
    public decimal AMOUNTPAID
    {
      get;
      set;
    }
    public int PERSON_ID
    {
      get;
      set;
    }
    public string PAYMENTREFNO
    {
      get;
      set;
    }
    public string REMARKS
    {
      get;
      set;
    }
    public decimal TAXAMOUNT
    {
      get;
      set;
    }
    public decimal TOTALAMOUNT
    {
      get;
      set;
    }
    public DateTime TRANSACTIONDATE
    {
      get;
      set;
    }
    public string TRANSACTIONREFNO
    {
      get;
      set;
    }
    public int TRANSACTION_STATUS_ID
    {
      get;
      set;
    }
    public TransactionStatus TransactionStatus { get; set; }
    public int TRANSACTIONTYPE
    {
      get;
      set;
    }
    public DateTime? PAYMENTDATE
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
    public int CURRENCY_ID
    {
      get;
      set;
    }
    public Currency Currency { get; set; }

        [ForeignKey("PERSON_ID")]
        public Person Person { get; set; }

        [NotMapped]
        public string CardNumder { get; set; }

        [NotMapped]
        public int Month { get; set; }

        [NotMapped]
        public int Year { get; set; }

        [NotMapped]
        public string CVC { get; set; }

        public ICollection<CompanyAccountTransaction> AccountTransactions { get; set; }

    }
}
