using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clear.Risk.Models.ClearConnection
{
  [Table("COUNTRY", Schema = "dbo")]
  public partial class Country
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
    public ICollection<State> States { get; set; }
    public string COUNTRYNAME
    {
      get;
      set;
    }
    public string SHORTNAME
    {
      get;
      set;
    }
  }
}
