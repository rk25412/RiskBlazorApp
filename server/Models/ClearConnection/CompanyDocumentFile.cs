using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
    [Table("COMPANY_DOCUMENT_FILE", Schema = "dbo")]
    public partial class CompanyDocumentFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DOCUMENTID
        {
            get;
            set;
        }
        public string DOCUMENTNAME
        {
            get;
            set;
        }
        public string FILENAME
        {
            get;
            set;
        }
        public string VERSION_NUMBER
        {
            get;
            set;
        }
        public int COMPANY_ID
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
        public string DOCUMENT_URL
        {
            get;
            set;
        }
        [ForeignKey("COMPANY_ID")]
        public Person Person { get; set; }

        [ForeignKey("CREATOR_ID")]
        public Person CreatedBy { get; set; }


        [NotMapped]
        public string CreatedByName
        {
            get
            {
                return CreatedBy?.FullName ?? string.Empty;
            }
        }

        [NotMapped]
        public string DocumentNameWithVersion
        {
            get
            {
                return DOCUMENTNAME + "-v" + VERSION_NUMBER;
            }
        }
    }
}
