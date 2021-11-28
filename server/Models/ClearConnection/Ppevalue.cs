using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
    [Table("PPEVALUES", Schema = "dbo")]
    public partial class Ppevalue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PPE_ID
        {
            get;
            set;
        }

        public ICollection<SwmsPperequired> SwmsPperequireds { get; set; }
        public int KEY_VALUE
        {
            get;
            set;
        }
        public string KEY_DISPLAY
        {
            get;
            set;
        }
        public bool ACTIVE
        {
            get;
            set;
        }
        public string ICONIMAGE
        {
            get;
            set;
        }
        public string ICONPATH
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
        public bool? IS_DELETED
        {
            get;
            set;
        }
        public int ESCALATION_LEVEL
        {
            get;
            set;
        }
        public int WARNING_LEVEL
        {
            get;
            set;
        }
        public int STATUS_LEVEL
        {
            get;
            set;
        }
    }
}
