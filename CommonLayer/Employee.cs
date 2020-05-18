using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace CommonLayer
{
    /// <summary>
    /// POCO Class For Employee Detail Model.
    /// </summary>
    public class Employee
    {
        //Employee Detail Fields.
        public int Id { get; set; }

        [Required(ErrorMessage ="FirstName Required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName Required")]
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage ="EmailId Required")]
        public string EmailId { get; set; }

        [Required(ErrorMessage ="Mobile Number Required")]
        public long Mobile { get; set; }

        [Required(ErrorMessage ="Address Required")]
        public string Address { get; set; }

        public string DOB { get; set; }

        [Required(ErrorMessage ="Employment Required")]
        public string Employment { get; set; }

        /// <summary>
        /// Overriding ToString Method.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "FirstName : " + FirstName  + ", LastName : " + LastName 
                + ", EmailId : " + EmailId + ", Mobile : " + Mobile 
                +", Address : " + Address + ", DOB : " + DOB + ", Employment : " + Employment;
        }

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
            if (!(obj is Employee))
            {
                return false;
            }
            return this.FirstName == ((Employee)obj).FirstName && this.LastName == ((Employee)obj).LastName
                && this.EmailId == ((Employee)obj).EmailId && this.Mobile == ((Employee)obj).Mobile
                && this.Address == ((Employee)obj).Address && this.DOB == ((Employee)obj).DOB
                && this.Employment == ((Employee)obj).Employment;
        }

        /// <summary>
        /// Overriding GetHashCode Method.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
