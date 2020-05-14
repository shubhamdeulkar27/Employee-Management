using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CommonLayer
{
    /// <summary>
    /// Class For User Data.
    /// </summary>
    public class User
    {
        [Required(ErrorMessage ="UserName Is Required")]
        public string UserName { get; set; }

        [Required (ErrorMessage ="Password Is Required")]
        public string Password { get; set; }
    }
}
