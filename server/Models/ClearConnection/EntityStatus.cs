using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
    [Table("ENTITY_STATUS", Schema = "dbo")]
    public partial class EntityStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ENTITY_STATUS_ID
        {
            get;
            set;
        }
        public string NAME
        {
            get;
            set;
        }
    }
}
