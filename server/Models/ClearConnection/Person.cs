using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
    [Table("PERSON", Schema = "dbo")]
    public partial class Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PERSON_ID
        {
            get;
            set;
        }

        public ICollection<Assesment> Assesments { get; set; }
        public ICollection<Assesment> ClientAssesments { get; set; }
        public ICollection<Person> People1 { get; set; }
        public ICollection<Template> Templates { get; set; }
        public ICollection<PersonSite> PersonSites { get; set; }
        public ICollection<AssesmentEmployee> AssesmentEmployees { get; set; }
        public ICollection<CompanyAccountTransaction> CompanyAccountTransactions { get; set; }
        public ICollection<SwmsTemplate> SwmsTemplates { get; set; }
        public ICollection<SwmsTemplate> SwmsTemplates1 { get; set; }
        public Person Person1 { get; set; }
        public DateTime CREATED_DATE
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
        public int? TITLE
        {
            get;
            set;
        }
        public string COMPANY_NAME
        {
            get;
            set;
        }
        public string FIRST_NAME
        {
            get;
            set;
        }
        public string MIDDLE_NAME
        {
            get;
            set;
        }
        public string LAST_NAME
        {
            get;
            set;
        }


        public Int64? GENDER
        {
            get;
            set;
        }
        public DateTime? DOB
        {
            get;
            set;
        }
        public string PERSONALADDRESS1
        {
            get;
            set;
        }
        public string PERSONALADDRESS2
        {
            get;
            set;
        }
        public string PERSONAL_CITY
        {
            get;
            set;
        }
        public int? PERSONAL_STATE_ID
        {
            get;
            set;
        }
        public State State { get; set; }
        public int? PERSONAL_COUNTRY_ID
        {
            get;
            set;
        }
        public Country Country { get; set; }
        public string PERSONAL_PHONE
        {
            get;
            set;
        }
        public string PERSONAL_MOBILE
        {
            get;
            set;
        }
        public string PERSONAL_EMAIL
        {
            get;
            set;
        }
        public string PERSONAL_FAX
        {
            get;
            set;
        }
        public string PERSONAL_WEB_ADD
        {
            get;
            set;
        }
        public string PERSONAL_POSTCODE
        {
            get;
            set;
        }
        public string BUSINESS_ADDRESS1
        {
            get;
            set;
        }
        public string BUSINESS_ADDRESS2
        {
            get;
            set;
        }
        public string BUSINESS_CITY
        {
            get;
            set;
        }
        public int? BUSINESS_STATE_ID
        {
            get;
            set;
        }
        public State State1 { get; set; }
        public int? BUSINESS_COUNTRY_ID
        {
            get;
            set;
        }
        public Country Country1 { get; set; }
        public string BUSINESS_PHONE
        {
            get;
            set;
        }
        public string BUSINESS_MOBILE
        {
            get;
            set;
        }
        public string BUSINESS_EMAIL
        {
            get;
            set;
        }
        public string BUSINESS_FAX
        {
            get;
            set;
        }
        public string BUSINESS_WEB_ADD
        {
            get;
            set;
        }
        public string BUSINESS_POSTCODE
        {
            get;
            set;
        }
        public int? ESCALATION_LEVEL
        {
            get;
            set;
        }
        [ForeignKey("ESCALATION_LEVEL")]
        public EscalationLevel EscalationLevel { get; set; }
        public int? WARNING_LEVEL
        {
            get;
            set;
        }
        [ForeignKey("WARNING_LEVEL")]
        public WarningLevel WarningLevel { get; set; }
        public int? STATUS_LEVEL
        {
            get;
            set;
        }
        public int? PARENT_PERSON_ID
        {
            get;
            set;
        }
        public int? COMPANYTYPE
        {
            get;
            set;
        }
        public PersonType PersonType { get; set; }
        public bool? OUTOFHOUR
        {
            get;
            set;
        }
        public string OUTOFHOURCONTACT
        {
            get;
            set;
        }
        public string OUTOFHOUREMAIL
        {
            get;
            set;
        }
        public TimeSpan? OUTOFSTART
        {
            get;
            set;
        }
        public TimeSpan? OUTOFEND
        {
            get;
            set;
        }
        //public string DESIGNATION
        //{
        //  get;
        //  set;
        //}
        public int? DESIGNATION_ID
        {
            get;
            set;
        }
        public int? PERSON_STATUS
        {
            get;
            set;
        }
        [ForeignKey("PERSON_STATUS")]
        public StatusMaster Status { get; set; }
        public string UPLOAD_PROFILE
        {
            get;
            set;
        }
        public int? APPLICENCEID
        {
            get;
            set;
        }
        public Applicence Applicence { get; set; }
        public DateTime? APPLICENCE_STARTDATE
        {
            get;
            set;
        }
        public DateTime? APPLICENCE_ENDDATE
        {
            get;
            set;
        }
        public string TERMANDCONDITION
        {
            get;
            set;
        }
        public decimal CURRENT_BALANCE
        {
            get;
            set;
        }
        public string PASSWORDHASH
        {
            get;
            set;
        }
        public bool? ISMANAGER { get; set; }

        [NotMapped]
        public string ConfirmPassword
        {
            get;
            set;
        }

        [NotMapped]
        public bool isSameAddress { get; set; }
        public int? CURRENCY_ID
        {
            get;
            set;
        }
        public Currency Currency { get; set; }


        [ForeignKey("DESIGNATION_ID")]
        public Desigation Desigation { get; set; }

        [NotMapped]
        public string VALIDATEPWD
        {
            get;
            set;
        }

        [InverseProperty("User")]
        public ICollection<PersonRole> PersonRoles { get; set; }

        public string FullName
        {
            get
            {
                return this.FIRST_NAME + " " + this.LAST_NAME;
            }
        }
        [NotMapped]
        public string CHECKISMANAGER
        {
            get
            {
                return Convert.ToBoolean(this.ISMANAGER) ? "YES" : "NO";
            }
        }
        public int? ENTITY_STATUS_ID
        { get; set; }

        [ForeignKey("ENTITY_STATUS_ID")]
        public EntityStatus EntityStatus { get; set; }

        public int? ASSIGNED_TO_ID { get; set; }

        [ForeignKey("ASSIGNED_TO_ID")]
        public Person Manager { get; set; }

        public ICollection<PersonContact> Contacts { get; set; }

        public int? WORK_SITE_ID
        {
            get;
            set;
        }
        [ForeignKey("WORK_SITE_ID")]
        public PersonSite PersonSite { get; set; }

        public bool ACTIVATED { get; set; }

        public long? ACTCODE { get; set; }

        [NotMapped]
        public string ManagerName
        {
            get
            {
                return Manager?.FullName ?? string.Empty;
            }
        }
    }
}
