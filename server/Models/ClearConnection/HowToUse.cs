using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Clear.Risk.Models.ClearConnection
{
    [Table("HowToUse", Schema = "dbo")]
    public class HowToUse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int HowToUseId { get; set; }
        
        [MaxLength(150)]
        [StringLength(150)]
        [Display(Name ="Subject")]
        public virtual string Subject { get; set; }

        [MaxLength(150)]
        [StringLength(150)]
        [Display(Name = "Pdf Name")]
        public virtual string PdfName { get; set; }

        [MaxLength(500)]
        [StringLength(500)]
        public virtual string PdfPath { get; set; }

        [MaxLength(200)]
        [StringLength(200)]
        [Display(Name = "Video Name")]
        public virtual string VideoName { get; set; }

        [MaxLength(500)]
        [StringLength(500)]
        public virtual string VideoPath { get; set; }
    }
}
