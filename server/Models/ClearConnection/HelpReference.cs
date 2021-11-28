using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
    [Table("HELP_REFERENCE", Schema = "dbo")]
    public class HelpReference
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public virtual int HELP_ID { get; set; }

        [Display(Name = "CREATED DATE")]
        public virtual DateTime? CREATED_DATE { get; set; }

        [Display(Name = "CREATOR ID")]
        public virtual int? CREATOR_ID { get; set; }

        [Display(Name = "UPDATED DATE")]
        public virtual DateTime? UPDATED_DATE { get; set; }

        [Display(Name = "UPDATER ID")]
        public virtual int? UPDATER_ID { get; set; }

        [Display(Name = "DELETED DATE")]
        public virtual DateTime? DELETED_DATE { get; set; }

        [Display(Name = "DELETER ID")]
        public virtual int? DELETER_ID { get; set; }

        [Display(Name = "IS DELETED")]
        public virtual bool? IS_DELETED { get; set; }

        [MaxLength(150)]
        [StringLength(150)]
        [Display(Name = "SCREEN NAME")]
        public virtual string SCREEN_NAME { get; set; }

        [Display(Name = "SCREEN ID")]
        public virtual int? SCREEN_ID { get; set; }

        [MaxLength]
        [Display(Name = "HTML CONTENT")]
        public virtual string HTML_CONTENT { get; set; }

        [Display(Name = "LAST UPDATED")]
        public virtual DateTime? LAST_UPDATED { get; set; }

        [Display(Name = "VERSION NUMBER")]
        public virtual int? VERSION_NUMBER { get; set; }

    }
}
