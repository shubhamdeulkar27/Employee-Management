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
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage ="FirstName Can Only Have Characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName Required")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "LastName Can Only Have Characters")]
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage ="EmailId Required")]
        public string EmailId { get; set; }

        [Required(ErrorMessage ="Mobile Number Required")]
        [MinLength(10)]
        public string Mobile { get; set; }

        [Required(ErrorMessage ="Address Required")]
        public string Address { get; set; }

        public string BirthDate { get; set; }

        [Required(ErrorMessage ="Employment Required")]
        public string Employment { get; set; }
    }
}
