using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
    [Table("SWMS_SECTION", Schema = "dbo")]
    public partial class SwmsSection
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SWMS_SECTION_ID
        {
            get;
            set;
        }
        public string NAME
        {
            get;
            set;
        }
        public int? SWMS_VALUE
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
