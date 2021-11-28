using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clear.Risk.ViewModels
{
    public class LoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class InputModel
    {
        
        public string Email { get; set; }

        
        public string Password { get; set; }

       
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    
}
