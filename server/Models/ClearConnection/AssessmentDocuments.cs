using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clear.Risk.Models.ClearConnection
{
    [Table("ASSESSMENT_DOCUMENTS", Schema = "dbo")]
    public class AssessmentDocument
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ASSESSMENT_DOCUMENT_ID { get; set; }


        public int ASSESSMENT_ID { get; set; }

        [ForeignKey("ASSESSMENT_ID")]
        public Assesment Assessment { get; set; }

        public int DOCUMENT_ID { get; set; }
        
        [ForeignKey("DOCUMENT_ID")]
        public CompanyDocumentFile CompanyDocuments { get; set; }

    }
}
