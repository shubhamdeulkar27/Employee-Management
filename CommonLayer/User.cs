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

        /// <summary>
        /// Overriding Equals Method.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (!(obj is User))
            {
                return false;
            }

            return this.UserName == ((User)obj).UserName && this.Password == ((User)obj).Password;
        }

        /// <summary>
        /// Overriding GetHashCode Methood.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
