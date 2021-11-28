using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
  [Table("SWMS_TEMPLATESTEP", Schema = "dbo")]
  public partial class SwmsTemplatestep
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int STEPID
    {
      get;
      set;
    }
    public int SWMSID
    {
      get;
      set;
    }
    public SwmsTemplate SwmsTemplate { get; set; }
    public string TASKCATEGEORY
    {
      get;
      set;
    }
    public string HAZARD
    {
      get;
      set;
    }
    public string HEALTHIMPACT
    {
      get;
      set;
    }
    public string SWMSTYPE
    {
      get;
      set;
    }
    public int? RISK_LIKELYHOOD_ID
    {
      get;
      set;
    }
    public RiskLikelyhood RiskLikelyhood { get; set; }
    public int? CONSEQUENCE_ID
    {
      get;
      set;
    }
    public Consequence Consequence { get; set; }
    public int? RISK_CONTRL_SCORE
    {
      get;
      set;
    }
    public string CONTROLLINGHAZARDS
    {
      get;
      set;
    }
    public int? RISK_AFTER_LIKELYHOOD_ID
    {
      get;
      set;
    }
    public RiskLikelyhood RiskLikelyhood1 { get; set; }
    public int? AFTER_CONSEQUENCE_ID
    {
      get;
      set;
    }
    public Consequence Consequence1 { get; set; }
    public int? AFTER_RISK_CONTROL_SCORE
    {
      get;
      set;
    }
    public int? RESPOSNSIBLE_TYPE_ID
    {
      get;
      set;
    }
    public ResposnsibleType ResposnsibleType { get; set; }
    public int? STATUS
    {
      get;
      set;
    }
    public bool? ISDELETE
    {
      get;
      set;
    }
    public int? STEPNO
    {
      get;
      set;
    }
        [NotMapped]
        public string RiskLikelyHoodBeforeName { get; set; }
        [NotMapped]
        public string ConsequenceName { get; set; }

        [NotMapped]
        public string RiskLikelyHoodAfterName { get; set; }

        [NotMapped]
        public string ConsequenceAfterName { get; set; }
        [NotMapped]
        public string ResposnsibleTypeName { get; set; }

        [NotMapped]
        public bool IsAdd { get; set; }

        [NotMapped]
        public IEnumerable<string> SpecificHazards { get; set; }


        [NotMapped]
        public IEnumerable<string> HealthImpacts { get; set; }

        [NotMapped]
        public IEnumerable<string> Controllings { get; set; }
    }
}
