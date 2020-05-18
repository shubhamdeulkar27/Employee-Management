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
        public int Id { get; set; }

        [Required(ErrorMessage ="FirstName Required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName Required")]
        public string LastName { get; set; }

        [EmailAddress]
        public string EmailId { get; set; }

        [Required(ErrorMessage ="Mobile Number Required")]
        public long Mobile { get; set; }

        [Required(ErrorMessage ="Address Required")]
        public string Address { get; set; }

        public string DOB { get; set; }

        [Required(ErrorMessage ="Employment Required")]
        public string Employment { get; set; }
    }
}
