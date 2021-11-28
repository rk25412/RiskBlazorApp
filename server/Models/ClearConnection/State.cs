using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
  [Table("STATES", Schema = "dbo")]
  public partial class State
  {
    [Key]
    public int ID
    {
      get;
      set;
    }

    public ICollection<Person> People { get; set; }
    public ICollection<Person> People1 { get; set; }
    public ICollection<Template> Templates { get; set; }
    public ICollection<PersonSite> PersonSites { get; set; }
    public ICollection<SwmsTemplate> SwmsTemplates { get; set; }
    public string STATENAME
    {
      get;
      set;
    }
    public int? COUNTRYID
    {
      get;
      set;
    }
    public Country Country { get; set; }
  }
}
