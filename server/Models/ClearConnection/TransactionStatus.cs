using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
    [Table("TRANSACTION_STATUS", Schema = "dbo")]
    public partial class TransactionStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TRANSACTION_STATUS_ID
        {
            get;
            set;
        }

        public ICollection<CompanyTransaction> CompanyTransactions { get; set; }
        public string NAME
        {
            get;
            set;
        }
    }
}
