using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
    [Table("RISK_LIKELYHOOD", Schema = "dbo")]
    public partial class RiskLikelyhood
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RISK_VALUE_ID
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
        public int RISK_VALUE
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


        public string NameWithRiskValue
        {
            get
            {
                return this.NAME + "(" + this.RISK_VALUE + ")";
            }
        }
    }
}
