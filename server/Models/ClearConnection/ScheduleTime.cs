using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
    [Table("SCHEDULE_TIME", Schema = "dbo")]

    public class ScheduleTime
    {
        [Key]
        [Required(ErrorMessage = "SCHEDULE TIME ID is required")]
        [Display(Name = "SCHEDULE TIME ID")]
        public virtual int SCHEDULE_TIME_ID { get; set; }

        [MaxLength(50)]
        [StringLength(50)]
        [Required(ErrorMessage = "SCHEDULE TIME is required")]
        [Display(Name = "SCHEDULE TIME")]
        public virtual string SCHEDULE_TIME { get; set; }

    }
}
