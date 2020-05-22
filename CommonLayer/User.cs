using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CommonLayer
{
    /// <summary>
    /// Getting And Setting Data Fields With User Data.
    /// </summary>
    public class User
    {
        [Required(ErrorMessage ="UserName Is Required")]
        [MinLength(6)]
        [RegularExpression("^([a-zA-Z0-9]{1,15})$", ErrorMessage ="Invalid UserName")]
        public string UserName { get; set; }
        
        [Required (ErrorMessage ="Password Is Required")]
        [MinLength(6)]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,30}$", ErrorMessage ="Invalid Password")]
        public string Password { get; set; }
    }
}
