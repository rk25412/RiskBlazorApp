using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
    [Table("SITE_ACTIVITY", Schema = "dbo")]
    public class SiteActivity
    {
        public SiteActivity()
        {
            this.STATUS = "Active";
            this.CREATED_DATE = DateTime.Now;
            this.UPDATED_DATE = DateTime.Now;
            this.IS_DELETED = false;
            this.ESCALATION_LEVEL_ID = 1;
            this.WARNING_LEVEL_ID = 1;
            this.STATUS_LEVEL_ID = 1;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "SITE ACTIVITY ID is required")]
        [Display(Name = "SITE ACTIVITY ID")]
        public virtual int SITE_ACTIVITY_ID { get; set; }

        [Required(ErrorMessage = "ASSESMENT ID is required")]
        [Display(Name = "ASSESMENT ID")]
        public virtual int ASSESMENT_ID { get; set; }

        [Display(Name = "LATITUDE")]
        public virtual decimal? LATITUDE { get; set; }

        [Display(Name = "LONGITUDE")]
        public virtual decimal? LONGITUDE { get; set; }

        [Required(ErrorMessage = "START DATE is required")]
        [Display(Name = "START DATE")]
        public virtual DateTime START_DATE { get; set; }

        [Required(ErrorMessage = "END DATE is required")]
        [Display(Name = "END DATE")]
        public virtual DateTime END_DATE { get; set; }

        [MaxLength(10)]
        [StringLength(10)]
        [Required(ErrorMessage = "STATUS is required")]
        [Display(Name = "STATUS")]
        public virtual string STATUS { get; set; }

        [Required(ErrorMessage = "CREATED DATE is required")]
        [Display(Name = "CREATED DATE")]
        public virtual DateTime CREATED_DATE { get; set; }

        [Required(ErrorMessage = "CREATOR ID is required")]
        [Display(Name = "CREATOR ID")]
        public virtual int CREATOR_ID { get; set; }

        [Required(ErrorMessage = "UPDATED DATE is required")]
        [Display(Name = "UPDATED DATE")]
        public virtual DateTime UPDATED_DATE { get; set; }

        [Required(ErrorMessage = "UPDATER ID is required")]
        [Display(Name = "UPDATER ID")]
        public virtual int UPDATER_ID { get; set; }

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

        [Required(ErrorMessage = "EMPLOYEE ID is required")]
        [Display(Name = "EMPLOYEE ID")]
        public virtual int EMPLOYEE_ID { get; set; }

        [ForeignKey("ASSESMENT_ID")]
        public Assesment Assesment { get; set; }

        [ForeignKey("EMPLOYEE_ID")]
        public Person Worker { get; set; }

    }
}
