using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
    [Table("SWMS_TEMPLATE", Schema = "dbo")]
    public partial class SwmsTemplate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SWMSID
        {
            get;
            set;
        }

        public ICollection<AssesmentAttachement> AssesmentAttachements { get; set; }
        public ICollection<SwmsTemplatestep> SwmsTemplatesteps { get; set; }
        public ICollection<SwmsReferencedlegislation> SwmsReferencedlegislations { get; set; }
        public ICollection<SwmsPperequired> SwmsPperequireds { get; set; }
        public ICollection<SwmsPlantequipment> SwmsPlantequipments { get; set; }
        public ICollection<SwmsLicencespermit> SwmsLicencespermits { get; set; }
        public ICollection<SwmsHazardousmaterial> SwmsHazardousmaterials { get; set; }
        public int? COMPANYID
        {
            get;
            set;
        }
        public Person Person { get; set; }
        public DateTime? CREATED_DATE
        {
            get;
            set;
        }
        public DateTime? UPDATED_DATE
        {
            get;
            set;
        }
        public int SWMSTYPE
        {
            get;
            set;
        }
        public TemplateType TemplateType { get; set; }
        public int TEMPLATE_ID
        {
            get;
            set;
        }
        public Template Template { get; set; }
        public string TEMPLATENAME
        {
            get;
            set;
        }
        public string VERSION
        {
            get;
            set;
        }
        public int STATUS
        {
            get;
            set;
        }
        public StatusMaster StatusMaster { get; set; }
        public bool? IS_DELETED
        {
            get;
            set;
        }
        public string CREATEDBY
        {
            get;
            set;
        }
        public string DOCPATH
        {
            get;
            set;
        }
        public string SWMSTEMPLATENUMBER
        {
            get;
            set;
        }
        public int TEMPLATEQUESTION
        {
            get;
            set;
        }
        public SwmsTemplateCategory SwmsTemplateCategory { get; set; }
        public int? COUNTRY_ID
        {
            get;
            set;
        }
        public Country Country { get; set; }
        public int? CREATOR_ID
        {
            get;
            set;
        }
        public int? UPDATER_ID
        {
            get;
            set;
        }
        public DateTime? DELETED_DATE
        {
            get;
            set;
        }
        public int? DELETER_ID
        {
            get;
            set;
        }
        public int ESCALATION_LEVEL_ID
        {
            get;
            set;
        }
        public EscalationLevel EscalationLevel { get; set; }
        public int WARNING_LEVEL_ID
        {
            get;
            set;
        }
        public WarningLevel WarningLevel { get; set; }
        public int STATUS_LEVEL_ID
        {
            get;
            set;
        }
        public StatusLevel StatusLevel { get; set; }
        public int? STATEID
        {
            get;
            set;
        }
        public State State { get; set; }
        public int? TRADECATEGORYID
        {
            get;
            set;
        }
        public TradeCategory TradeCategory { get; set; }
        public int? SITEID
        {
            get;
            set;
        }
        public int? FM_MANAGER_ID
        {
            get;
            set;
        }
        public Person Person1 { get; set; }
        public string AUTHORIZATION_STATUS
        {
            get;
            set;
        }
        public string RiskAssessmentDoc { get; set; }
        public int? REFERENCETEMPLATEID { get; set; }
        public bool IS_DRAFT { get; set; } = false;



        [NotMapped]
        public string CompanyName
        {
            get
            {
                return Person?.COMPANY_NAME ?? string.Empty;
            }
        }

        [NotMapped]
        public string TemplateTypeName
        {
            get
            {
                return TemplateType?.NAME ?? string.Empty;
            }
        }
    }
}
