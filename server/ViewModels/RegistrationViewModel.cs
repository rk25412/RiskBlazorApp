using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clear.Risk.ViewModels
{
    public class RegistrationViewModel
    {
        public string CompanyName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int Country { get; set; }
        public int State { get; set; }
        public string City { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string PostCode { get; set; }
        public string Fax { get; set; }
    }
}

