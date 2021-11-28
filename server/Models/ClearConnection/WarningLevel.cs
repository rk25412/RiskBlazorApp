using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
    [Table("WARNING_LEVEL", Schema = "dbo")]
    public partial class WarningLevel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WARNING_LEVEL_ID
        {
            get;
            set;
        }

        public ICollection<Template> Templates { get; set; }
        public ICollection<SwmsTemplate> SwmsTemplates { get; set; }
        public string NAME
        {
            get;
            set;
        }
        public string ICON_PATH
        {
            get;
            set;
        }
    }
}
