using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
  [Table("PERSON_TYPE", Schema = "dbo")]
  public partial class PersonType
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PERSON_TYPE_ID
    {
      get;
      set;
    }

    public ICollection<Person> People { get; set; }
    public string TYPE_NAME
    {
      get;
      set;
    }
    public bool REGDEFAULT
    {
      get;
      set;
    }
    public bool EMPDEFAULT
    {
      get;
      set;
    }
  }
}
