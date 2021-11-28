using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
    [Table("ASSESMENT_ATTACHEMENT", Schema = "dbo")]
    public partial class AssesmentAttachement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ATTACHEMENTID
        {
            get;
            set;
        }
        public int ASSESMENTID
        {
            get;
            set;
        }

        [ForeignKey("ASSESMENTID")]
        public Assesment Assesment { get; set; }
        public DateTime ATTACHEMENTDATE
        {
            get;
            set;
        }
        public string DOCUMENTPDFURL
        {
            get;
            set;
        }
        public string DOCUMENTTEMPLATEURL
        {
            get;
            set;
        }
        public int SWMS_TEMPLATE_ID
        {
            get;
            set;
        }

        [ForeignKey("SWMS_TEMPLATE_ID")]
        public SwmsTemplate SwmsTemplate { get; set; }
        public ICollection<AssesmentEmployeeAttachement> Attachments { get; set; }
    }
}
