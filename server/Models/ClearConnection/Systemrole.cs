using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Clear.Risk.Models.ClearConnection
{
  [Table("SYSTEMROLE", Schema = "dbo")]
  public partial class Systemrole
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ROLE_ID
    {
      get;
      set;
    }
    public DateTime? CREATED_DATE
    {
      get;
      set;
    }
    public Int64? CREATOR_ID
    {
      get;
      set;
    }
    public DateTime? UPDATED_DATE
    {
      get;
      set;
    }
    public Int64? UPDATER_ID
    {
      get;
      set;
    }
    public DateTime? DELETED_DATE
    {
      get;
      set;
    }
    public Int64? DELETER_ID
    {
      get;
      set;
    }
    public bool? IS_DELETED
    {
      get;
      set;
    }
    public string ROLE_NAME
    {
      get;
      set;
    }
    public string ROLE_DESC
    {
      get;
      set;
    }
    public int? ROLE_LEVEL
    {
      get;
      set;
    }

        [InverseProperty("Role")]
        public ICollection<PersonRole> PersonRoles { get; set; }
    }
}
