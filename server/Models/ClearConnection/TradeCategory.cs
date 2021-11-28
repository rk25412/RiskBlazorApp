using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
    [Table("TRADE_CATEGORY", Schema = "dbo")]
    public partial class TradeCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TRADE_CATEGORY_ID
        {
            get;
            set;
        }

        public ICollection<Assesment> Assesments { get; set; }
        public ICollection<Template> Templates { get; set; }
        public ICollection<SwmsTemplate> SwmsTemplates { get; set; }
        public ICollection<TradeCategory> TradeCategories1 { get; set; }
        public string TRADE_NAME
        {
            get;
            set;
        }
        public int? PARENT_ID
        {
            get;
            set;
        }
        public TradeCategory TradeCategory1 { get; set; }
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
        public Int64? ESCALATION_LEVEL
        {
            get;
            set;
        }
        public Int64? WARNING_LEVEL
        {
            get;
            set;
        }
        public Int64? STATUS_LEVEL
        {
            get;
            set;
        }
        public string DESCRIPTION
        {
            get;
            set;
        }
        public decimal HOURLY_COST
        {
            get;
            set;
        }
    }
}
