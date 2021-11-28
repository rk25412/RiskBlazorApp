using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
    [Table("CONSEQUENCE", Schema = "dbo")]
    public partial class Consequence
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CONSEQUENCE_ID
        {
            get;
            set;
        }

        public ICollection<SwmsTemplatestep> SwmsTemplatesteps { get; set; }
        public ICollection<SwmsTemplatestep> SwmsTemplatesteps1 { get; set; }
        public string NAME
        {
            get;
            set;
        }
        public int CONSEQUENCE_VALUE
        {
            get;
            set;
        }
        public int ESCALATION_LEVEL_ID
        {
            get;
            set;
        }
        public int WARNING_LEVEL_ID
        {
            get;
            set;
        }
        public int STATUS_LEVEL_ID
        {
            get;
            set;
        }
        public int ENTITY_STATUS_ID
        {
            get;
            set;
        }
        [NotMapped]
        public string NameWithConsequenceValue
        {
            get
            {
                return this.NAME + "(" + this.CONSEQUENCE_VALUE + ")";
            }
        }
    }
}

