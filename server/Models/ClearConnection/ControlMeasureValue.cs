using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
    [Table("CONTROL_MEASURE_VALUE", Schema = "dbo")]
    public partial class ControlMeasureValue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CONTROL_MEASURE_ID
        {
            get;
            set;
        }
        public string NAME
        {
            get;
            set;
        }
        public int CONTROL_VALUE
        {
            get;
            set;
        }
        public int ESCALATION_LEVEL_ID
        {
            get;
            set;
        }
        public int WARNING_LEVEL_ID
        {
            get;
            set;
        }
        public int STATUS_LEVEL_ID
        {
            get;
            set;
        }
        public int ENTITY_STATUS_ID
        {
            get;
            set;
        }
    }
}
