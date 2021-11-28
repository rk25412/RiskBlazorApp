using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
    [Table("WORK_ORDER", Schema = "dbo")]
    public partial class WorkOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WORK_ORDER_ID
        {
            get;
            set;
        }
        public ICollection<WorkOrder> BaseWorkOrders { get; set; }
        public ICollection<WorkerPerson> WorkerPeople { get; set; }
        public DateTime DATE_RAISED
        {
            get;
            set;
        }
        public DateTime DUE_DATE
        {
            get;
            set;
        }
        public string PURCHASE_ORDER_NUMBER
        {
            get;
            set;
        }
        public string WORK_ORDER_NUMBER
        {
            get;
            set;
        }
        public int STATUS_ID
        {
            get;
            set;
        }

        [ForeignKey("STATUS_ID")]
        public OrderStatus OrderStatus { get; set; }
        public int? PRIORITY_ID
        {
            get;
            set;
        }

        [ForeignKey("PRIORITY_ID")]
        public PriorityMaster PriorityMaster { get; set; }
        public int WORK_ORDER_TYPE
        {
            get;
            set;
        }

        [ForeignKey("WORK_ORDER_TYPE")]
        public WorkOrderType WorkOrderType { get; set; }
        public int? PARENT_WORK_ORDER_ID
        {
            get;
            set;
        }

        [ForeignKey("PARENT_WORK_ORDER_ID")]
        public WorkOrder BaseWorkOrder { get; set; }
        public string DESCRIPTION
        {
            get;
            set;
        }
        public int? REQUESTED_BY_ID
        {
            get;
            set;
        }

        [ForeignKey("REQUESTED_BY_ID")]
        public PersonContact RequestedBy { get; set; }
        public int? CLIENT_ID
        {
            get;
            set;
        }

        [ForeignKey("CLIENT_ID")]
        public Person Client { get; set; }
        public int? CONTRACTOR_ID
        {
            get;
            set;
        }
        [ForeignKey("CONTRACTOR_ID")]
        public Person Contractor { get; set; }
        public int? CONTRACTOR_CONTACT_ID
        {
            get;
            set;
        }

        [ForeignKey("CONTRACTOR_CONTACT_ID")]
        public PersonContact ContractorContact { get; set; }
        public int? WORK_LOCATION_ID
        {
            get;
            set;
        }

        [ForeignKey("WORK_LOCATION_ID")]
        public PersonSite WorkLocation { get; set; }
        public string ATTACHED_IMAGE_URL
        {
            get;
            set;
        }
        public int? ASSET_ID
        {
            get;
            set;
        }
        public int? ASSIGNED_TO_ID
        {
            get;
            set;
        }

        [ForeignKey("ASSIGNED_TO_ID")]
        public Person FacilitiesManager { get; set; }
        public DateTime? ASSIGNED_DATE
        {
            get;
            set;
        }
        public DateTime? START_DATE
        {
            get;
            set;
        }
        public DateTime? END_DATE
        {
            get;
            set;
        }
        public decimal? AUTHORIZED_COST
        {
            get;
            set;
        }
        public Int64? ESTIMETED_HOUR
        {
            get;
            set;
        }
        public string ALLOCATE_TO
        {
            get;
            set;
        }
        public string ROLE_ALLOCATION
        {
            get;
            set;
        }
        public string TRADE_REQUEST
        {
            get;
            set;
        }
        public string CLIENT_JOB_ALLOCATION_DETAILS
        {
            get;
            set;
        }
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
        public bool IS_DELETED
        {
            get;
            set;
        }
        public int? ESCALATION_LEVEL_ID
        {
            get;
            set;
        }
        public int? WARNING_LEVEL_ID
        {
            get;
            set;
        }
        public int? STATUS_LEVEL_ID
        {
            get;
            set;
        }
        public int? CLIENT_CONTACT_ID
        {
            get;
            set;
        }

        [ForeignKey("CLIENT_CONTACT_ID")]
        public PersonContact ClientContact { get; set; }
        public string CLIENT_WORK_ORDER_NUMBER
        {
            get;
            set;
        }
        public bool CLIENT_APPROVAL
        {
            get;
            set;
        }
        public DateTime? CLIENT_APPROVAL_DATE
        {
            get;
            set;
        }
        public int? APPROVED_BY_ID
        {
            get;
            set;
        }

        [ForeignKey("APPROVED_BY_ID")]
        public PersonContact ApproveBy { get; set; }
        public bool EXTENSION_REQUIRED
        {
            get;
            set;
        }
        public DateTime? EXTENSION_DATE_REQUESTED
        {
            get;
            set;
        }
        public string EXTENSION_REASON
        {
            get;
            set;
        }
        public bool EXTENSION_APPROVED
        {
            get;
            set;
        }
        public int? EXTENSION_APPROVED_BY
        {
            get;
            set;
        }
        public DateTime? NEW_DUE_DATE
        {
            get;
            set;
        }
        public TimeSpan? ADDITIONAL_HOURS
        {
            get;
            set;
        }
        public decimal ESTIMATEDWORKORDER_COST
        {
            get;
            set;
        }
        public decimal ACTUALWORKORDER_COST
        {
            get;
            set;
        }
        public string REPORTTOWORKORDER
        {
            get;
            set;
        }
        public int PROCESSTYPE_ID
        {
            get;
            set;
        }

        public IEnumerable<Assesment> Assessments { get; set; }

        [ForeignKey("PROCESSTYPE_ID")]
        public ProcessType ProcessType { get; set; }
        public int? REACTIVECRITICALITY_ID
        {
            get;
            set;
        }
        [ForeignKey("REACTIVECRITICALITY_ID")]
        public CriticalityMaster CriticalityMaster { get; set; }
        public DateTime? LANDLORDCOMMENTDATE
        {
            get;
            set;
        }
        public string LANDLORDCOMMENT
        {
            get;
            set;
        }
        public DateTime? PROPERTYMANAGERCOMMENTDATE
        {
            get;
            set;
        }
        public string PROPERTYMANAGERCOMMENT
        {
            get;
            set;
        }
        public string COMPANYWORKORDERNO
        {
            get;
            set;
        }
        public int? COMPANY_ID
        {
            get;
            set;
        }

        [ForeignKey("COMPANY_ID")]
        public Person ManagementCompany { get; set; }

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

        [ForeignKey("STATUS_LEVEL_ID")]
        public EntityStatus EntityStatus { get; set; }

        [ForeignKey("WARNING_LEVEL_ID")]
        public WarningLevel WarningLevel { get; set; }

        public bool ISINTERNAL { get; set; }

        public string RowClass => this.WARNING_LEVEL_ID > 1 ? "table-danger" : null;

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
        public string OrderStatusName
        {
            get
            {
                return OrderStatus?.NAME ?? string.Empty;
            }
        }

        [NotMapped]
        public string PriorityMasterNAME
        {
            get
            {
                return PriorityMaster?.NAME ?? string.Empty;
            }
        }

        [NotMapped]
        public string EntityStatusNAME
        {
            get
            {
                return EntityStatus?.NAME ?? string.Empty;
            }
        }

        [NotMapped]
        public string WarningLevelNAME
        {
            get
            {
                return WarningLevel?.NAME ?? string.Empty;
            }
        }

    }
}
