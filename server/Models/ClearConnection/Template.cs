using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
    [Table("TEMPLATE", Schema = "dbo")]
    public partial class Template
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID
        {
            get;
            set;
        }

        public ICollection<Templateattachment> Templateattachments { get; set; }
        public ICollection<SwmsTemplate> SwmsTemplates { get; set; }
        public string DOCUMENTURL
        {
            get;
            set;
        }
        public string REFERENCENUMBER
        {
            get;
            set;
        }
        public int? RISKCATEGORYID
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
        public string TEMPLATENAME
        {
            get;
            set;
        }
        public int TRADECATEGORYID
        {
            get;
            set;
        }
        public TradeCategory TradeCategory { get; set; }
        public string VERSONNUMBER
        {
            get;
            set;
        }
        public int? COMPANYID
        {
            get;
            set;
        }
        public Person Person { get; set; }
        public int TYPEFORID
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
        public DateTime? CREATED_DATE
        {
            get;
            set;
        }
        public int? CREATOR_ID
        {
            get;
            set;
        }
        public DateTime? UPDATED_DATE
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
        public bool? IS_DELETED
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
    }
}
