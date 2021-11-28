using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
    [Table("PERSON_SITE", Schema = "dbo")]
    public partial class PersonSite
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PERSON_SITE_ID
        {
            get;
            set;
        }

        public ICollection<Assesment> Assesments { get; set; }
        public int? PERSON_ID
        {
            get;
            set;
        }
        public Person Person { get; set; }
        public string SITE_ADDRESS1
        {
            get;
            set;
        }
        public string SITE_ADDRESS2
        {
            get;
            set;
        }
        public string SITE_ADDRESS3
        {
            get;
            set;
        }
        public string CITY
        {
            get;
            set;
        }
        public int? STATE_ID
        {
            get;
            set;
        }
        public State State { get; set; }
        public string POST_CODE
        {
            get;
            set;
        }
        public int COUNTRY_ID
        {
            get;
            set;
        }
        public Country Country { get; set; }
        public decimal? LATITUDE
        {
            get;
            set;
        }
        public decimal? LONGITUDE
        {
            get;
            set;
        }
        public decimal? SITE_AREA
        {
            get;
            set;
        }
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
        public DateTime UPDATED_DATE
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

        public string SITE_NAME { get; set; }
        public string BUILDING_NAME { get; set; }

        public string FLOOR { get; set; }

        public string ROOMNO { get; set; }

        public bool IS_DEFAULT { get; set; } = false;

    }
}
