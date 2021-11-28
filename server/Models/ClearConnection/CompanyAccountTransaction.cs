using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
    [Table("COMPANY_ACCOUNT_TRANSACTION", Schema = "dbo")]
    public partial class CompanyAccountTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int COMPANY_TRANSACTION_ID
        {
            get;
            set;
        }
        public DateTime TRANSACTION_DATE
        {
            get;
            set;
        }
        public int? TRANSACTION_ID
        {
            get;
            set;
        }
        public decimal PAYMENT_AMOUNT
        {
            get;
            set;
        }
        public decimal DEPOSITE_AMOUNT
        {
            get;
            set;
        }
        public string DESCRIPTION
        {
            get;
            set;
        }
        public int COMPANY_ID
        {
            get;
            set;
        }
        public Person Person { get; set; }
        public string TRXTYPE
        {
            get;
            set;
        }
        public DateTime? CREATED_DATE
        {
            get;
            set;
        }
        public int? CREATOR_ID
        {
            get;
            set;
        }
        public DateTime? UPDATED_DATE
        {
            get;
            set;
        }
        public int? UPDATER_ID
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

        [ForeignKey("TRANSACTION_ID")]
        public CompanyTransaction Transaction { get; set; }

        public int? ASSESMENTID
        {
            get;
            set;
        }

        [ForeignKey("ASSESMENTID")]
        public Assesment Assesment { get; set; }

        [NotMapped]
        public decimal Balance { get; set; }
    }
}
