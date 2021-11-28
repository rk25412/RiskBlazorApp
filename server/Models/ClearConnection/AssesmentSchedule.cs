using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
    [Table("ASSESMENT_SCHEDULE", Schema = "dbo")]
    public class AssesmentSchedule
    {
        public AssesmentSchedule()
        {
            this.MON = false;
            this.TUE = false;
            this.WED = false;
            this.THUS = false;
            this.FRI = false;
            this.SAT = false;
            this.SUN = false;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "ASSESSMENT SCHEDULE ID is required")]
        [Display(Name = "ASSESSMENT SCHEDULE ID")]
        public virtual int ASSESSMENT_SCHEDULE_ID { get; set; }

        [Required(ErrorMessage = "ASSESMENTID is required")]
        [Display(Name = "ASSESMENTID")]
        public virtual int ASSESMENTID { get; set; }

        [Required(ErrorMessage = "SCHEDULE TYPE ID is required")]
        [Display(Name = "SCHEDULE TYPE ID")]
        public virtual int SCHEDULE_TYPE_ID { get; set; }

        [Display(Name = "SCHEDULE AT")]
        public virtual int? SCHEDULE_AT { get; set; }

        [Display(Name = "INTERVAL")]
        public virtual int? INTERVAL { get; set; }

        [Display(Name = "SCHEDULE TIME")]
        public virtual DateTime? SCHEDULE_TIME { get; set; }

        [Required(ErrorMessage = "MON is required")]
        [Display(Name = "MON")]
        public virtual bool MON { get; set; }

        [Required(ErrorMessage = "TUE is required")]
        [Display(Name = "TUE")]
        public virtual bool TUE { get; set; }

        [Required(ErrorMessage = "WED is required")]
        [Display(Name = "WED")]
        public virtual bool WED { get; set; }

        [Required(ErrorMessage = "THUS is required")]
        [Display(Name = "THUS")]
        public virtual bool THUS { get; set; }

        [Required(ErrorMessage = "FRI is required")]
        [Display(Name = "FRI")]
        public virtual bool FRI { get; set; }

        [Required(ErrorMessage = "SAT is required")]
        [Display(Name = "SAT")]
        public virtual bool SAT { get; set; }

        [Required(ErrorMessage = "SUN is required")]
        [Display(Name = "SUN")]
        public virtual bool SUN { get; set; }

        [ForeignKey("ASSESMENTID")]
        public Assesment Assesment { get; set; }

        [ForeignKey("SCHEDULE_TYPE_ID")]
        public ScheduleType ScheduleType { get; set; }
        

        [NotMapped]
        public int? EndHour { get; set; }

        [NotMapped]
        public int? StartHour { get; set; }
    }
}
