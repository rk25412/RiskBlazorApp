using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
    [Table("ASSESMENT_SITE", Schema = "dbo")]
    public class AssesmentSite
    {
        public AssesmentSite()
        {
            this.IS_DELETED = false;
            this.ESCALATION_LEVEL_ID = 1;
            this.WARNING_LEVEL_ID = 1;
            this.STATUS_LEVEL_ID = 1;
            this.ENTITY_STATUS_ID = 1;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "ASSESMENT SITE ID is required")]
        [Display(Name = "ASSESMENT SITE ID")]
        public virtual int ASSESMENT_SITE_ID { get; set; }

        [Required(ErrorMessage = "ASSESMENT ID is required")]
        [Display(Name = "ASSESMENT ID")]
        public virtual int ASSESMENT_ID { get; set; }

        [Display(Name = "LOCATION ID")]
        public virtual int? LOCATION_ID { get; set; }

        [Required(ErrorMessage = "EMPLOYEE ID is required")]
        [Display(Name = "EMPLOYEE ID")]
        public virtual int EMPLOYEE_ID { get; set; }

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

        [Required(ErrorMessage = "IS DELETED is required")]
        [Display(Name = "IS DELETED")]
        public virtual bool IS_DELETED { get; set; }

        [Required(ErrorMessage = "ESCALATION LEVEL ID is required")]
        [Display(Name = "ESCALATION LEVEL ID")]
        public virtual int ESCALATION_LEVEL_ID { get; set; }

        [Required(ErrorMessage = "WARNING LEVEL ID is required")]
        [Display(Name = "WARNING LEVEL ID")]
        public virtual int WARNING_LEVEL_ID { get; set; }

        [Required(ErrorMessage = "STATUS LEVEL ID is required")]
        [Display(Name = "STATUS LEVEL ID")]
        public virtual int STATUS_LEVEL_ID { get; set; }

        [Required(ErrorMessage = "ENTITY STATUS ID is required")]
        [Display(Name = "ENTITY STATUS ID")]
        public virtual int ENTITY_STATUS_ID { get; set; }

        [Required(ErrorMessage = "LATITUDE is required")]
        [Display(Name = "LATITUDE")]
        public virtual decimal LATITUDE { get; set; }

        [Required(ErrorMessage = "LONGITUDE is required")]
        [Display(Name = "LONGITUDE")]
        public virtual decimal LONGITUDE { get; set; }

        [ForeignKey("ASSESMENTID")]
        public Assesment Assesment { get; set; }

        [ForeignKey("ASSESMENT_ID")]
        public Person Employee { get; set; }

    }
}
