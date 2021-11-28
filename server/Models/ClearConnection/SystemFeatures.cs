using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Clear.Risk.Models.ClearConnection
{
    [Table("FeaturesTable", Schema = "dbo")]
    public class SystemFeatures
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public virtual int Feature_ID { get; set; }

        public virtual int Feature_ScreenID { get; set; }

        [MaxLength(300)]
        [StringLength(300)]
        public virtual string Features_ScreenName { get; set; }

        [MaxLength]
        public virtual string Html_Content { get; set; }
    }
}
