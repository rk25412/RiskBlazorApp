using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Identity;

namespace Clear.Risk.Models
{
    public partial class ApplicationUser : IdentityUser
    {
        [NotMapped]
        public IEnumerable<string> RoleNames { get; set; }

        [IgnoreDataMember]
        public override string PasswordHash { get; set; }

        [IgnoreDataMember, NotMapped]
        public string Password { get; set; }

        [IgnoreDataMember, NotMapped]
        public string ConfirmPassword { get; set; }

        [IgnoreDataMember, NotMapped]
        public string Name
        {
            get
            {
                return UserName;
            }
            set
            {
                UserName = value;
            }
        }

        [NotMapped]
        public string CompanyName
        {
            get;
            set;
        }
        [NotMapped]
        public int? CountryID
        {
            get;
            set;
        }
        [NotMapped]
        public int? StateID
        {
            get;
            set;
        }
        [NotMapped]
        public int? CompanyTypeID
        {
            get;
            set;
        }
        [NotMapped]
        public decimal CurrentBalance
        {
            get;
            set;
        }
        [NotMapped]
        public int? CompanyId
        {
            get;
            set;
        }
        [NotMapped]
        public int? PERSONAL_COUNTRY_ID { get; set; }
        [NotMapped]
        public int? PERSONAL_STATE_ID { get; set; }
        [NotMapped]
        public string FIRST_NAME { get; set; }
        [NotMapped]
        public string MIDDLE_NAME { get; set; }
        [NotMapped]
        public string LAST_NAME { get; set; }
        [NotMapped]
        public string COMPANY_NAME { get; set; }
        [NotMapped]
        public string PERSONALADDRESS1 { get; set; }
        [NotMapped]
        public string PERSONALADDRESS2 { get; set; }
        [NotMapped]
        public string PERSONAL_CITY { get; set; }
        [NotMapped]
        public string PERSONAL_MOBILE { get; set; }
        [NotMapped]
        public string PERSONAL_PHONE { get; set; }
        [NotMapped]
        public string BUSINESS_ADDRESS1 { get; set; }
        [NotMapped]
        public string BUSINESS_ADDRESS2 { get; set; }
        [NotMapped]
        public string BUSINESS_CITY { get; set; }
        [NotMapped]
        public int? BUSINESS_COUNTRY_ID { get; set; }
        [NotMapped]
        public string BUSINESS_EMAIL { get; set; }
        [NotMapped]
        public string BUSINESS_MOBILE { get; set; }
        [NotMapped]
        public string BUSINESS_PHONE { get; set; }
        [NotMapped]
        public int? BUSINESS_STATE_ID { get; set; }
        [NotMapped]
        public string BUSINESS_WEB_ADD { get; set; }
        [NotMapped]
        public int APPLICENCEID { get; set; }
        [NotMapped]
        public int PERSON_STATUS { get; set; }
        [NotMapped]
        public decimal CURRENT_BALANCE { get; set; }
        [NotMapped]
        public int COMPANYTYPE { get; set; }
        [NotMapped]
        public int? DEFAULT_CURRENCY { get; set; }

        [NotMapped]
        public int PARENT_PERSON_ID { get; set; }

        [NotMapped]
        public string PERSONAL_EMAIL { get; set; }

        [NotMapped]
        public string PERSONAL_FAX { get; set; }

        [NotMapped]
        public string PERSONAL_WEB_ADD { get; set; }

        [NotMapped]
        public string BUSINESS_FAX { get; set; }

        [NotMapped]
        public string PERSONAL_POSTCODE { get; set; }
        [NotMapped]
        public string BUSINESS_POSTCODE { get; set; }
        [NotMapped]
        public bool isSameAddress { get; set; }

        [NotMapped]
        public string PersonalState { get; set; }
        [NotMapped]
        public string BusinessState { get; set; }
        [NotMapped]
        public string PersonalCountry { get; set; }
        [NotMapped]
        public string BusinessCountry { get; set; }
    }
}
