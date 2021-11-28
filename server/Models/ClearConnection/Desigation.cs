using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
    [Table("DESIGNATION", Schema = "dbo")]
    public partial class Desigation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DESIGNATION_ID
        {
            get;
            set;
        }        
        public string DESIGNATIONNAME
        {
            get;
            set;
        }
    }
}
