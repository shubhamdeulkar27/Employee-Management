using BusinessLayer.Interface;
using BusinessLayer.Services;
using CommonLayer;
using EmployeeManagement.Controllers;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Interface;
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
        //Reference Of Controller.
        EmployeeController controller;

        //Reference Of Interfaces Of Business And Repository Layer.
        private readonly IEmployeeManagementBL employeeManagementBL;
        private readonly IEmployeeManagementRL employeeManagementRL;

        //Reference Of Configuration.
        private readonly IConfiguration configuration;
        private readonly IDistributedCache distributedCache;

        /// <summary>
        /// Construtor For Setting RL, BL and IDistributedCache References.
        /// </summary>
        public EmployeeManagementTests()
        {
            employeeManagementRL = new EmployeeManagementRL(configuration);
            employeeManagementBL = new EmployeeManagementBL(employeeManagementRL);
            controller = new EmployeeController(employeeManagementBL, distributedCache, configuration);
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

        /// <summary>
        /// Test Case For Register API, will Return Bad Request For Invalid Data.
        /// </summary>
        [Fact]
        public void GivenInvalidDetailShouldReturnBadRequest()
        {
            User user = new User();
            user.Role = "";
            user.EmailId = "";
            user.UserName = "";
            user.Password = "";

            var response = controller.RegisterUser(user);
        }
    }
}
