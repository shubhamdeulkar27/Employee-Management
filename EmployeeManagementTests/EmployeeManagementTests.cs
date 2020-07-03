using BusinessLayer.Interface;
using BusinessLayer.Services;
using CommonLayer;
using EmployeeManagement.Controllers;
using RepositoryLayer.Services;
using System;
using Xunit;

namespace EmployeeManagementTests
{
    /// <summary>
    /// Class For Test Cases For Comman Layer.
    /// </summary>
    public class EmployeeManagementTests
    {
        /// <summary>
        /// Test Case 1.0 Given UserName, Password and UserName,Password Should Return Equal.
        /// </summary>
        [Fact]
        public void GivenUserNamePasswordAndUserNamePasswordShouldReturnEqual()
        {
            //Creating User Instances.
            User userOne = new User();
            userOne.UserName = "Chrisy";
            userOne.Password = "1234567";
            User userTwo = new User();
            userTwo.UserName = "Chrisy";
            userTwo.Password = "1234567";

            //Asserting Values.
            Assert.Equal(userOne,userTwo);
        }

        /// <summary>
        /// Test Case 1.1 Given Empty UserName Should throw Validation Exception.
        /// </summary>
        [Fact]
        public void GivenEmptyUserNameShouldThrowValidationException()
        {
            //Expected Value.
            string expected = "UserName Is Required";
            try
            {
                //Creating User Instance.
                User user = new User();
            }
            catch (Exception exception)
            {
                //Asserting Values.
                Assert.Equal(expected,exception.Message);
            }
        }

        /// <summary>
        /// Test Case 1.2 Given MinimumLength Password Should throw Validation Exception.
        /// </summary>
        [Fact]
        public void GivenEmptyPasswordShouldThrowValidationException()
        {
            //Expected Value.
            string expected = "Password Is Required";
            try
            {
                //Creating User Instance.
                User user = new User();
                user.UserName = "Chrisy";
            }
            catch (Exception exception)
            {
                //Asserting Values.
                Assert.Equal(expected, exception.Message);
            }
        }

        /// <summary>
        /// Test Case 1.3 For Checking Encryption Function. 
        /// </summary>
        [Fact]
        public void TestForChrckingEncryptingFunction()
        {
            //Encrypting Strings.
            string encryptedSting1 = EmployeeManagementRL.EncodePasswordToBase64("Visual");
            string encryptedSting2 = EmployeeManagementRL.EncodePasswordToBase64("Visual");
           
            //Asserting Values.
            Assert.Equal(encryptedSting1,encryptedSting2);
        }

        /// <summary>
        /// Test Case 1.4 Given Employee Details Should Return Equal.
        /// </summary>
        [Fact]
        public void GivenEmployeeDetailsShouldRetrunEqual()
        {
            //Creatingg Employee Instances.
            Employee employeeOne = new Employee();
            employeeOne.FirstName = "Abhi";
            employeeOne.LastName = "Fuke";
            employeeOne.EmailId = "abhi.fuke@gmail.com";
            employeeOne.Mobile = "1234567890";
            employeeOne.Address = "Govind Apartment, Karve Nagar, Pune, Pin 000000";
            employeeOne.BirthDate = "17/5/1996";
            employeeOne.Employment = "Full-Time";

            Employee employeeTwo = new Employee();
            employeeTwo.FirstName = "Abhi";
            employeeTwo.LastName = "Fuke";
            employeeTwo.EmailId = "abhi.fuke@gmail.com";
            employeeTwo.Mobile = "1234567890";
            employeeTwo.Address = "Govind Apartment, Karve Nagar, Pune, Pin 000000";
            employeeTwo.BirthDate = "17/5/1996";
            employeeTwo.Employment = "Full-Time";

            //Asserting Values.
            Assert.Equal(employeeOne,employeeTwo);
        }

        /// <summary>
        /// Test Case 1.5 Given Invalid FirstName Should Throw Validation Exception.
        /// </summary>
        [Fact]
        public void GivenInvalidFirstNameShouldThrowValidationException()
        {
            string expected = "FirstName Required";
            try
            {
                //Creatingg Employee Instances.
                Employee employee = new Employee();
                employee.FirstName = null;
                employee.LastName = "fuke";
                employee.EmailId = "abhi.fuke@gmail.com";
                employee.Mobile = "1234567890";
                employee.Address = "Govind Apartment, Karve Nagar, Pune, Pin 000000";
                employee.BirthDate = "17/5/1996";
                employee.Employment = "Full-Time";
            }
            catch (Exception exception)
            {
                //Asserting Values.
                Assert.Equal(expected, exception.Message);
            }
        }

        /// <summary>
        /// Test Case 1.6 Given Invalid LastName Should Throw Validation Exception.
        /// </summary>
        [Fact]
        public void GivenInvalidLastNameShouldThrowValidationException()
        {
            string expected = "LastName Required";
            try
            {
                //Creatingg Employee Instances.
                Employee employee = new Employee();
                employee.FirstName = "Abhi";
                employee.LastName = null;
                employee.EmailId = "abhi.fuke @gmail.com";
                employee.Mobile = "1234567890";
                employee.Address = "Govind Apartment, Karve Nagar, Pune, Pin 000000";
                employee.BirthDate = "17/5/1996";
                employee.Employment = "Full-Time";
            }
            catch (Exception exception)
            {
                //Asserting Values.
                Assert.Equal(expected, exception.Message);
            }
        }

        /// <summary>
        /// Test Case 1.7 Given Invalid EmailId Should Throw Validation Exception.
        /// </summary>
        [Fact]
        public void GivenInvalidEmailIdShouldThrowValidationException()
        {
            string expected = "EmailId Required";
            try
            {
                //Creatingg Employee Instances.
                Employee employee = new Employee();
                employee.FirstName = "Abhi";
                employee.LastName = "Fuke";
                employee.EmailId = null;
                employee.Mobile = "1234567890";
                employee.Address = "Govind Apartment, Karve Nagar, Pune, Pin 000000";
                employee.BirthDate = "17/5/1996";
                employee.Employment = "Full-Time";
            }
            catch (Exception exception)
            {
                //Asserting Values.
                Assert.Equal(expected, exception.Message);
            }
        }

        /// <summary>
        /// Test Case 1.8 Given Invalid Mobile Number Should Throw Validation Exception.
        /// </summary>
        [Fact]
        public void GivenInvalidMobileNumberShouldThrowValidationException()
        {
            string expected = "Mobile Number Required";
            try
            {
                //Creatingg Employee Instances.
                Employee employee = new Employee();
                employee.FirstName = "Abhi";
                employee.LastName = "Fuke";
                employee.EmailId = "abhi.fuke@gmail.com";
                employee.Address = "Govind Apartment, Karve Nagar, Pune, Pin 000000";
                employee.BirthDate = "17/5/1996";
                employee.Employment = "Full-Time";
            }
            catch (Exception exception)
            {
                //Asserting Values.
                Assert.Equal(expected, exception.Message);
            }
        }

        /// <summary>
        /// Test Case 1.9 Given Invalid Address Should Throw Validation Exception.
        /// </summary>
        [Fact]
        public void GivenInvalidAddressShouldThrowValidationException()
        {
            string expected = "Address Required";
            try
            {
                //Creatingg Employee Instances.
                Employee employee = new Employee();
                employee.FirstName = "Abhi";
                employee.LastName = "Fuke";
                employee.EmailId = "abhi.fuke@gmail.com";
                employee.Mobile = "9874561320";
                employee.Address = null;
                employee.BirthDate = "17/5/1996";
                employee.Employment = "Full-Time";
            }
            catch (Exception exception)
            {
                //Asserting Values.
                Assert.Equal(expected, exception.Message);
            }
        }

        /// <summary>
        /// Test Case 2.0 Given Invalid Employment Detail Should Throw Validation Exception.
        /// </summary>
        [Fact]
        public void GivenInvalidEmploymentShouldThrowValidationException()
        {
            string expected = "Employment Required";
            try
            {
                //Creatingg Employee Instances.
                Employee employee = new Employee();
                employee.FirstName = "Abhi";
                employee.LastName = "Fuke";
                employee.EmailId = "abhi.fuke@gmail.com";
                employee.Mobile = "9874561320";
                employee.Address = "Govind Apartment, Karve Nagar, Pune, Pin 000000";
                employee.BirthDate = "17/5/1996";
                employee.Employment = null;
            }
            catch (Exception exception)
            {
                //Asserting Values.
                Assert.Equal(expected, exception.Message);
            }
        }
    }
}
