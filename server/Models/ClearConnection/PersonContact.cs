using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
  [Table("PERSON_CONTACT", Schema = "dbo")]
  public partial class PersonContact
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PERSON_CONTACT_ID
    {
      get;
      set;
    }

   
    public int PERSON_ID
    {
      get;
      set;
    }
        [ForeignKey("PERSON_ID")]
    public Person Company { get; set; }
    public DateTime CREATED_DATE
    {
      get;
      set;
    }
    public int CREATOR_ID
    {
      get;
      set;
    }
    public DateTime UPDATED_DATE
    {
      get;
      set;
    }
    public int UPDATER_ID
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
    public int GENDER
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
        [ForeignKey("PERSONAL_STATE_ID")]
        public State PersonalState { get; set; }
    public int PERSONAL_COUNTRY_ID
    {
      get;
      set;
    }

        [ForeignKey("PERSONAL_COUNTRY_ID")]
        public Country PersonalCountry { get; set; }
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
        [ForeignKey("BUSINESS_STATE_ID")]
        public State BusinessState { get; set; }
    public int? BUSINESS_COUNTRY_ID
    {
      get;
      set;
    }
        [ForeignKey("BUSINESS_COUNTRY_ID")]
        public Country BusinessCountry { get; set; }
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
    public bool ISPRIMARY
    {
      get;
      set;
    }
    public int CONTACT_STATUS_ID
    {
      get;
      set;
    }

        [ForeignKey("CONTACT_STATUS_ID")]
        public StatusMaster StatusMaster { get; set; }
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
        [ForeignKey("DESIGNATION_ID")]
        public Desigation Desigation { get; set; }
        [NotMapped]
        public bool IssameAddress { get; set; }

        public string PERSONAL_POSTCODE { get; set; }
        public string BUSINESS_POSTCODE { get; set; }

        public string FullName
        {
            get
            {
                return this.FIRST_NAME + ' ' + this.LAST_NAME;
            }
        }
    }

    public class Gender
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
