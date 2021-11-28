using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
    [Table("SMTPSETUP", Schema = "dbo")]
    public partial class Smtpsetup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SMTP_SETUP_ID
        {
            get;
            set;
        }
        public bool IS_DELETED
        {
            get;
            set;
        }
        public string SMTP_SERVER_STRING
        {
            get;
            set;
        }
        public string SMTP_USER_ACCOUNT
        {
            get;
            set;
        }
        public string SMTP_ACCOUNT_PASSWORD
        {
            get;
            set;
        }
        public string SMTP_MAIL_FROM
        {
            get;
            set;
        }
        public int SMTP_PORT
        {
            get;
            set;
        }
        public bool USE_SSL
        {
            get;
            set;
        }

    }
}
