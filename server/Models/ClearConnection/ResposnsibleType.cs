using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
    [Table("RESPOSNSIBLE_TYPE", Schema = "dbo")]
    public partial class ResposnsibleType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RESPONSIBLE_ID
        {
            get;
            set;
        }

        public ICollection<SwmsTemplatestep> SwmsTemplatesteps { get; set; }
        public string NAME
        {
            get;
            set;
        }
        public int RESPONSIBLE_VALUE
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
