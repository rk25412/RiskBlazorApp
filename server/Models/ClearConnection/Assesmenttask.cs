using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
  [Table("ASSESMENTTASK", Schema = "dbo")]
  public partial class Assesmenttask
  {
    [Key]
    public Int64 ASSESMENTTASKID
    {
      get;
      set;
    }
    public Int64 ASSESMENTID
    {
      get;
      set;
    }
    public DateTime ASSIGNEDDATE
    {
      get;
      set;
    }
    public Int64 EMPLOYEEID
    {
      get;
      set;
    }
    public string SIGNATUREURL
    {
      get;
      set;
    }
    public DateTime? SIGNEDDATE
    {
      get;
      set;
    }
    public int STATUS
    {
      get;
      set;
    }
    public Byte[] SINGNATUREIMAGE
    {
      get;
      set;
    }
    public bool ISNAMEEXIST
    {
      get;
      set;
    }
    public int ISSIGNEDSTATUS
    {
      get;
      set;
    }
    public string USERNAME
    {
      get;
      set;
    }
    public bool ISCOMPLETE
    {
      get;
      set;
    }
  }
}
