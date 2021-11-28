using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
    [Table("ASSESMENT", Schema = "dbo")]
    public partial class Assesment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ASSESMENTID { get; set; }

        //public ICollection<Assesment> Assesments1 { get; set; }
        public ICollection<AssesmentAttachement> AssesmentAttachements { get; set; }
        public ICollection<AssesmentEmployee> AssesmentEmployees { get; set; }
        //public Assesment Assesment1 { get; set; }
        public int? CLIENTID
        {
            get;
            set;
        }
        [ForeignKey("CLIENTID")]
        public Person Client { get; set; }
        public int COMPANYID
        {
            get;
            set;
        }

        [ForeignKey("COMPANYID")]
        public Person Company { get; set; }
        public DateTime? DOCUMENTDOWNLOAD
        {
            get;
            set;
        }
        public DateTime? DOCUMENT_UPLOAD_DATE
        {
            get;
            set;
        }
        public string ORDERLOCATION
        {
            get;
            set;
        }
        public string PERMITNUMBER
        {
            get;
            set;
        }
        public string PLACEOFWORKADDRESS
        {
            get;
            set;
        }
        public string PURCHASEORDER
        {
            get;
            set;
        }
        public string REFERENCENUMBER
        {
            get;
            set;
        }
        public string RISKASSESSMENTNO
        {
            get;
            set;
        }
        public int? TRADECATEGORYID
        {
            get;
            set;
        }
        public TradeCategory TradeCategory { get; set; }
        public int? TYPEOFASSESSMENTID
        {
            get;
            set;
        }
        [ForeignKey("TYPEOFASSESSMENTID")]
        public TemplateType TemplateType { get; set; }
        public DateTime WORKENDDATE
        {
            get;
            set;
        }
        public string WORKORDERNUMBER
        {
            get;
            set;
        }
        public DateTime WORKSTARTDATE
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
        public DateTime ASSESMENTDATE
        {
            get;
            set;
        }
        public bool ISCOMPLETED
        {
            get;
            set;
        }
        public string SCOPEOFWORK
        {
            get;
            set;
        }
        public string CONTRACTORSITEMANAGER
        {
            get;
            set;
        }
        public string CONTRACTORSITEMNGRMNO
        {
            get;
            set;
        }
        public string PROJECTNAME
        {
            get;
            set;
        }
        public string PRINCIPALCONTRACTOR
        {
            get;
            set;
        }
        public string WORKINGCONTRACTOR
        {
            get;
            set;
        }
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

        [ForeignKey("ESCALATION_LEVEL_ID")]
        public EscalationLevel EscalationLevel { get; set; }
        public int WARNING_LEVEL_ID
        {
            get;
            set;
        }

        [ForeignKey("WARNING_LEVEL_ID")]
        public WarningLevel WarningLevel { get; set; }


        public int STATUS_LEVEL_ID
        {
            get;
            set;
        }

        [ForeignKey("STATUS_LEVEL_ID")]
        public StatusLevel StatusLevel { get; set; }
        public bool IS_DELETED
        {
            get;
            set;
        }
        public int ENTITY_STATUS_ID
        {
            get;
            set;
        }
        [ForeignKey("ENTITY_STATUS_ID")]
        public EntityStatus EntityStatus { get; set; }
        public int WORK_SITE_ID
        {
            get;
            set;
        }
        public PersonSite PersonSite { get; set; }
        public bool ISCOVIDSURVEY
        {
            get;
            set;
        }
        public int? COVID_SURVEY_ID
        {
            get;
            set;
        }
        public Survey Survey { get; set; }
        public int? INDUSTRY_ID
        {
            get;
            set;
        }
        public IndustryType IndustryType { get; set; }
        public bool ISSCHEDULE
        {
            get;
            set;
        }
        [NotMapped]
        public int? SCHEDULE_TYPE_ID
        {
            get;
            set;
        }


        [NotMapped]
        public DateTime? SCHEDULE_TIME
        {
            get;
            set;
        }
        [NotMapped]
        public bool MON
        {
            get;
            set;
        }
        [NotMapped]
        public bool TUE
        {
            get;
            set;
        }
        [NotMapped]
        public bool WED
        {
            get;
            set;
        }
        [NotMapped]
        public bool THUS
        {
            get;
            set;
        }
        [NotMapped]
        public bool FRI
        {
            get;
            set;
        }
        [NotMapped]
        public bool SAT
        {
            get;
            set;
        }
        [NotMapped]
        public bool SUN
        {
            get;
            set;
        }
        [NotMapped]
        public bool HOLIDAY
        {
            get;
            set;
        }

        [NotMapped]
        public int? HourInterval { get; set; }

        [NotMapped]
        public int? MinuteInterval { get; set; }

        [NotMapped]
        public IEnumerable<int> ScheduleAt { get; set; }

        [NotMapped]
        public IEnumerable<int> SWMSTemplateNames { get; set; }

        [NotMapped]
        public IEnumerable<int> EmployeeNames { get; set; }

        public int? CLIENT_CONTACT_ID { get; set; }

        public int? CONTRACTOR_CONTACT_ID { get; set; }

        public int? CONTRACTOR_ID { get; set; }

        [ForeignKey("CONTRACTOR_ID")]
        public Person Contractor { get; set; }

        [ForeignKey("CONTRACTOR_CONTACT_ID")]
        public PersonContact ContractorContact { get; set; }

        [ForeignKey("CLIENT_CONTACT_ID")]
        public PersonContact ClientContact { get; set; }

        //public int? DOCUMENTID { get; set; }

        //[ForeignKey("DOCUMENTID")]
        //public CompanyDocumentFile CompanyDocumentFile { get; set; }

        public IEnumerable<AssessmentDocument> Documents { get; set; }

        [NotMapped]
        public string SiteName { get; set; }
        [NotMapped]
        public string BuildingName { get; set; }
        [NotMapped]
        public string Floor { get; set; }
        [NotMapped]
        public string RoomNo { get; set; }

        [NotMapped]
        public string Address1 { get; set; }

        [NotMapped]
        public string Address2 { get; set; }

        [NotMapped]
        public string City { get; set; }

        [NotMapped]
        public string PostCode { get; set; }

        [NotMapped]
        public int? StateId { get; set; }
        [NotMapped]
        public int CountryId { get; set; }

        [NotMapped]
        [ForeignKey("StateId")]
        public State State { get; set; }

        [NotMapped]
        [ForeignKey("CountryId")]
        public Country Country { get; set; }

        [NotMapped]
        public double? Lat { get; set; }

        [NotMapped]
        public double? Lon { get; set; }

        public int? WORK_ORDER_ID { get; set; }

        [ForeignKey("WORK_ORDER_ID")]
        public WorkOrder WorkOrder { get; set; }

        public ICollection<CompanyAccountTransaction> Transactions { get; set; }

        public ICollection<AssesmentSchedule> ScheduleAssesments { get; set; }

        public string RowClass => this.WARNING_LEVEL_ID > 1 ? "table-danger" : null;

        public int? TEMPLATE_ID { get; set; }

        public int? StartHour { get; set; }
        public int? EndHour { get; set; }




        [ForeignKey("TEMPLATE_ID")]
        public Template Template { get; set; }

        public bool ISINTERNAL { get; set; }

        [NotMapped]
        public string Internal
        {
            get
            {
                if (this.ISINTERNAL)
                    return "Yes";
                else
                    return "No";
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

        [NotMapped]
        public string EntityStatusName
        {
            get
            {
                return EntityStatus?.NAME ?? string.Empty;
            }
        }

        [NotMapped]
        public string WarningLevelName
        {
            get
            {
                return WarningLevel?.NAME ?? string.Empty;
            }
        }

        public bool? IsScheduleRunning { get; set; }
        public string AssessmentActivity { get; set; }
        public int? ParentAssessmentId { get; set; }
    }
}
