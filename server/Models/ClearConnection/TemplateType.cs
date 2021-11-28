using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
    [Table("TEMPLATE_TYPE", Schema = "dbo")]
    public partial class TemplateType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TEMPLATE_TYPE_ID
        {
            get;
            set;
        }

        public ICollection<Templateattachment> Templateattachments { get; set; }
        public ICollection<SwmsTemplate> SwmsTemplates { get; set; }
        public string NAME
        {
            get;
            set;
        }
        public bool ISACTIVE
        {
            get;
            set;
        }
    }
}
