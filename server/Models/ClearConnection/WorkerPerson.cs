using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
    [Table("WORKER_PERSON", Schema = "dbo")]
    public partial class WorkerPerson
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WORKER_PERSON_ID
        {
            get;
            set;
        }
        public int? PERSON_ID
        {
            get;
            set;
        }
        public int WORK_ORDER_ID
        {
            get;
            set;
        }

        [ForeignKey("WORK_ORDER_ID")]
        public WorkOrder WorkOrder { get; set; }
        public int? TRADE_CATEGORY_ID
        {
            get;
            set;
        }
        public double? TOTAL_HOURES
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
        public DateTime? DELETED_DATE
        {
            get;
            set;
        }
        public int? DELETER_ID
        {
            get;
            set;
        }
        public bool? IS_DELETED
        {
            get;
            set;
        }
        public int? ESCALATION_LEVEL_ID
        {
            get;
            set;
        }
        public int? WARNING_LEVEL_ID
        {
            get;
            set;
        }
        public int? STATUS_LEVEL_ID
        {
            get;
            set;
        }
        public decimal? WORKING_HOURES
        {
            get;
            set;
        }
    }
}
