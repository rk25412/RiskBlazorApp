using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
    [Table("HAZARD_VALUE", Schema = "dbo")]
    public partial class HazardValue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HAZARD_ID
        {
            get;
            set;
        }
        public string NAME
        {
            get;
            set;
        }

        [Column("HAZARD_VALUE")]
        public int HAZARD_VALUE1
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
