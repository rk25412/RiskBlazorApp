using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
  [Table("PERSON_ROLE", Schema = "dbo")]
  public partial class PersonRole
  {
    [Key]
    public int PERSON_ID
    {
      get;
      set;
    }
    [Key]
    public int ROLE_ID
    {
      get;
      set;
    }

        [ForeignKey("PERSON_ID")]
        public Person User { get; set; }

        [ForeignKey("ROLE_ID")]
        public Systemrole Role { get; set; }
    }
}
