using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
  [Table("TEMPLATEATTACHMENT", Schema = "dbo")]
  public partial class Templateattachment
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID
    {
      get;
      set;
    }
    public string ATTACHEMENTNAME
    {
      get;
      set;
    }
    public string DOCUMENTURL
    {
      get;
      set;
    }
    public bool ISDELETED
    {
      get;
      set;
    }
    public int STATUS
    {
      get;
      set;
    }
    public int TEMPLATEID
    {
      get;
      set;
    }
    public Template Template { get; set; }
    public int TEMPLATETYPEID
    {
      get;
      set;
    }
    public TemplateType TemplateType { get; set; }
  }
}
