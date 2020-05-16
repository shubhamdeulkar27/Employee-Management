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
        public string UserName { get; set; }
        
        [Required (ErrorMessage ="Password Is Required")]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
