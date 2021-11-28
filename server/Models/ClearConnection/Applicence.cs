using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
    [Table("APPLICENCE", Schema = "dbo")]
    public partial class Applicence
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int APPLICENCEID
        {
            get;
            set;
        }

        public ICollection<Person> People { get; set; }
        public ICollection<CompanyTransactionDetail> CompanyTransactionDetails { get; set; }
        public string LICENCE_NAME
        {
            get;
            set;
        }
        public string VERSION
        {
            get;
            set;
        }
        public string DESCRIPTION
        {
            get;
            set;
        }
        public string HELP
        {
            get;
            set;
        }
        public string URL
        {
            get;
            set;
        }
        public bool IS_DEFAULT
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
        public decimal PRICE
        {
            get;
            set;
        }
        public decimal DISCOUNT
        {
            get;
            set;
        }
        public decimal NETPRICE
        {
            get;
            set;
        }
        public int? CURRENCY_ID
        {
            get;
            set;
        }

        [ForeignKey("CURRENCY_ID")]
        public Currency Currency { get; set; }

        public int? COUNTRY_ID { get; set; }

        [ForeignKey("COUNTRY_ID")]
        public Country Country { get; set; }

        public decimal DEFAULT_CREDIT { get; set; }

        public decimal MIN_BALANCE { get; set; }
    }
}
