using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
    [Table("BlogTable", Schema = "dbo")]

    public class BlogTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual long Blog_Id { get; set; }
        public virtual string BgTittle { get; set; }
        public virtual string BgShortDetails { get; set; }

        [MaxLength]
        public virtual string BgLongDetails { get; set; }

        [MaxLength]
        public virtual string BgImgPath { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime CreatedDate { get; set; }
    }
}
