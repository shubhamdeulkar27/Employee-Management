using BusinessLayer.Interface;
using BusinessLayer.Services;
using CommonLayer;
using EmployeeManagement.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RepositoryLayer.Interface;
using RepositoryLayer.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;
namespace NUnitTestProject
{
    public class Tests
    {
        //Controller Reference.
        EmployeeController controller;

        //References BL and RL.
        private readonly IEmployeeManagementRL employeeManagementRL;
        private readonly IEmployeeManagementBL employeeManagementBL;

        //Reference Of Configuration.
        private readonly IConfiguration configuration;
        private readonly IDistributedCache distributedCache;

        //Model Instances.
        User user = new User();
        Employee employee = new Employee();

        //Variable.
        int ValidId = 3029;

        /// <summary>
        /// Constructor For Setting Required References.
        /// </summary>
        public Tests()
        {
            var myConfiguration = new Dictionary<string, string>
            {
                {"Jwt:Key", "ThisismySecretKey"},
                {"Jwt:Issuer", "https://localhost:44315/"},
                {"Jwt:Audiance", "https://localhost:44315/"},
                {"Logging:LogLevel:Default","Warning" },
                {"AllowedHosts","*" },
                {"ConnectionStrings:ConnectionString","Server=localhost\\SQLEXPRESS;Database=EmployeeManagement;Trusted_Connection=True;" }
            };
            this.configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();
            employeeManagementRL = new EmployeeManagementRL(configuration);
            employeeManagementBL = new EmployeeManagementBL(employeeManagementRL);
            distributedCache = new RedisCache(new RedisCacheOptions
            {
                Configuration = "localhost:6379"
            });
            controller = new EmployeeController(employeeManagementBL, distributedCache, configuration);
            
        }

        /// <summary>
        /// Test Case For User Register API.
        /// Empty String Value Should Return Bad Request.
        /// </summary>
        [Test]
        public void EmptyStringFieldsShouldReturnBadRequest()
        {
            //Setting Data.
            user.Role = "";
            user.EmailId = "";
            user.UserName = "";
            user.Password = "";
            var response = controller.RegisterUser(user) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var dataResponseSuccess = dataResponse["Success"].ToObject<bool>();
            var dataResponseMessage = dataResponse["message"].ToString();

            //Expected Response.
            bool Success = false;
            string Message = "INVALID_FIELD_EXCEPTION";

            //Asserting Values.
            Assert.IsInstanceOf<BadRequestObjectResult>(response);
            Assert.AreEqual(Success, dataResponseSuccess);
            Assert.AreEqual(Message, dataResponseMessage);
        }

        /// <summary>
        /// Test Case For User Register API.
        /// Null Field Should Return Bad Request.
        /// </summary>
        [Test]
        public void NullFieldsShouldReturnBadRequest()
        {
            //Setting Data Fields.
            user.Role = null;
            user.EmailId = null;
            user.UserName = null;
            user.Password = null;
            var response = controller.RegisterUser(user) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var dataResponseSuccess = dataResponse["Success"].ToObject<bool>();
            var dataResponseMessage = dataResponse["message"].ToString();

            //Expected Response.
            bool Success = false;
            string message = "NULL_FIELD_EXCEPTION";

            //Asserting Values.
            Assert.IsInstanceOf<BadRequestObjectResult>(response);
            Assert.AreEqual(Success, dataResponseSuccess);
            Assert.AreEqual(message, dataResponseMessage);
        }

        /// <summary>
        /// Test Case For User Register API.
        /// If User Detail Already Exists Then Return Conflict 409. 
        /// </summary>
        [Test]
        public void IfDetailsExistsShouldReturnConflict()
        {
            //Setting Data Fields.
            user.Role = "Admin";
            user.EmailId = "app.admin@gmail.com";
            user.UserName = "admin@27";
            user.Password = "Admin@27";
            var response = controller.RegisterUser(user) as ConflictObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var dataResponseSuccess = dataResponse["Success"].ToObject<bool>();
            var dataResponseMessage = dataResponse["Message"].ToString();
            var dataResponseData = dataResponse["Data"].ToObject<User>();

            //Expected Response.
            bool Success = false;
            string Message = "User Already Exists";

            //Asserting Values.
            Assert.IsInstanceOf<ConflictObjectResult>(response);
            Assert.AreEqual(Success, dataResponseSuccess);
            Assert.AreEqual(Message, dataResponseMessage);
            Assert.IsInstanceOf<User>(dataResponseData);
        }

        /// <summary>
        /// Test Case For User Register API.
        /// If Valid Data is Provided Then It Will Return OOk Object Result.
        /// </summary>
        [Test]
        public void ValidDataShouldReturnOk()
        {
            //Setting Data Fields.
            user.Role = "Tester";
            user.EmailId = "app.tester@gmail.com";
            user.UserName = "tester@27";
            user.Password = "Tester@27";
            var response = controller.RegisterUser(user) as OkObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var dataResponseSuccess = dataResponse["Success"].ToObject<bool>();
            var dataResponseMessage = dataResponse["Message"].ToString();
            var dataResponseData = dataResponse["Data"].ToObject<User>();


            //Expected Response.
            string Data = "{ Success = True, Message = User Registration Successful, Data = "+user+" }";
            bool Success = true;
            string Message = "User Registration Successful";

            //Asserting Values.
            Assert.IsInstanceOf<OkObjectResult>(response);
            Assert.AreEqual(Success,dataResponseSuccess);
            Assert.AreEqual(Message,dataResponseMessage);
            Assert.IsInstanceOf<User>(dataResponseData);
        }

        /// <summary>
        /// Test Cases For User Login API.
        /// Fields are set as empty string should return Bad Request.
        /// </summary>
        [Test]
        public void EmptyCredentialsShouldReturnBadRequest() 
        {
            //Setting Data Fields.
            user.UserName = "";
            user.Password = "";
            var response = controller.LoginUser(user) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var dataResponseSuccess = dataResponse["Success"].ToObject<bool>();
            var dataResponseMessage = dataResponse["Message"].ToString();
            
            //Expeted Response.
            bool Success = false;
            string Message = "INVALID_FIELD_EXCEPTION";
 
            //Asserting Values.
            Assert.IsInstanceOf<BadRequestObjectResult>(response);
            Assert.AreEqual(Success, dataResponseSuccess);
            Assert.AreEqual(Message, dataResponseMessage);
        }

        /// <summary>
        /// Test Cases For User Login API.
        /// Fields are set as null should return Bad Request.
        /// </summary>
        [Test]
        public void NullCredentialsShouldReturnBadRequest()
        {
            //Setting Data Fields.
            user.UserName = null;
            user.Password = null;
            var response = controller.LoginUser(user) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var dataResponseSuccess = dataResponse["Success"].ToObject<bool>();
            var dataResponseMessage = dataResponse["Message"].ToString();

            //Expected Response.
            bool Success = false;
            string Message = "NULL_FIELD_EXCEPTION";

            //Asserting Values.
            Assert.IsInstanceOf<BadRequestObjectResult>(response);
            Assert.AreEqual(Success, dataResponseSuccess);
            Assert.AreEqual(Message, dataResponseMessage);
        }

        /// <summary>
        /// Test Cases For User Login API.
        /// Invalid Credentials Should Return NotFound.
        /// </summary>
        [Test]
        public void InvalidCredentialsShouldReturnNotFound()
        {
            //Setting Data Fields.
            user.UserName = "chris@20";
            user.Password = "Chris@20";
            var response = controller.LoginUser(user) as NotFoundObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var dataResponseSuccess = dataResponse["Success"].ToObject<bool>();
            var dataResponseMessage = dataResponse["Message"]["StatusCode"].ToString();

            //Expected Values.
            bool Success = false;
            string Message = "401";

            //Asserting Values.
            Assert.IsInstanceOf<NotFoundObjectResult>(response);
            Assert.AreEqual(Success,dataResponseSuccess);
            Assert.AreEqual(Message, dataResponseMessage);
        }

        /// <summary>
        /// Test Cases For User Login API.
        /// Valid Credentials Should Return Ok.
        /// </summary>
        [Test]
        public void GivenValidCredentialsShouldReturnOk()
        {
            //Setting Data Fields.
            user.UserName = "admin@27";
            user.Password = "Admin@27";
            var response = controller.LoginUser(user) as OkObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var dataResponseSuccess = dataResponse["Success"].ToObject<bool>();
            var dataResponseMessage = dataResponse["Message"].ToString();
            var dataResponseData = dataResponse["Data"].ToString();

            //Expected Values.
            bool Success = true;
            string Message = "Login Successfull";
            string Data = user.UserName.ToString();

            //Asserting Values.
            Assert.IsInstanceOf<OkObjectResult>(response);
            Assert.AreEqual(Success,dataResponseSuccess);
            Assert.AreEqual(Message, dataResponseMessage);
            Assert.AreEqual(Data,dataResponseData);
        }

        /// <summary>
        /// Test Case For Get Employees API.
        /// </summary>
        [Test]
        public void TestForGetEmployees()
        {
            //Calling Get Employees.
            var response = controller.GetEmployees() as OkObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var dataResponseSuccess = dataResponse["Success"].ToObject<bool>();
            var dataResponseMessage = dataResponse["Message"].ToString();
            
            //Expected Values.
            bool Success = true;
            string Message = "Employee List Fetched Successfully";
            
            //Asserting Values.
            Assert.IsInstanceOf<OkObjectResult>(response);
            Assert.AreEqual(Success,dataResponseSuccess);
            Assert.AreEqual(Message, dataResponseMessage);
        }

        /// <summary>
        /// Register Employee API Test Cases.
        /// Empty String Fields Should Return Bad Request.
        /// </summary>
        [Test]
        public void EmptyStringFieldsShoudReturnBadRequests()
        {
            //Setting Values.
            employee.FirstName = "";
            employee.LastName = "";
            employee.EmailId = "";
            employee.Mobile = "";
            employee.Address = "";
            employee.Employment = "";
            var response = controller.RegisterEmployee(employee) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var dataResponseSuccess = dataResponse["Success"].ToObject<bool>();
            var dataResponseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            bool Success = false;
            string Message = "INVALID_FIELD_EXCEPTION";

            //Asserting Values.
            Assert.IsInstanceOf<BadRequestObjectResult>(response);
            Assert.AreEqual(Success, dataResponseSuccess);
            Assert.AreEqual(Message, dataResponseMessage);
        }

        /// <summary>
        /// Register Employee API Test Cases.
        /// Null Fields Should Return Bad Request.
        /// </summary>
        [Test]
        public void NullFieldsShoudReturnBadRequests()
        {
            //Setting Values.
            employee.FirstName = null;
            employee.LastName = null;
            employee.EmailId = null;
            employee.Mobile = null;
            employee.Address = null;
            employee.Employment = null;
            var response = controller.RegisterEmployee(employee) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var dataResponseSuccess = dataResponse["Success"].ToObject<bool>();
            var dataResponseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            bool Success = false;
            string Message = "NULL_FIELD_EXCEPTION";

            //Asserting Values.
            Assert.IsInstanceOf<BadRequestObjectResult>(response);
            Assert.AreEqual(Success, dataResponseSuccess);
            Assert.AreEqual(Message, dataResponseMessage);
        }

        /// <summary>
        /// Register Employee API Test Cases.
        /// If Employee Exists Should Return Conflict.
        /// </summary>
        [Test]
        public void IfEmployeeExistsShouldReturnConflict()
        {
            //Setting Values.
            employee.FirstName = "Shubham";
            employee.LastName = "Deulkar";
            employee.EmailId = "shubhamdeulkar27@gmail.com";
            employee.Mobile = "9011907526";
            employee.Address = "Vashigao, NaviMumbai, Maharashtra.";
            employee.BirthDate = "27/12/1995";
            employee.Employment = "Full-Time";
            var response = controller.RegisterEmployee(employee) as ConflictObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var dataResponseSuccess = dataResponse["Success"].ToObject<bool>();
            var dataResponseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            bool Success = false;
            string Message = "Employee Registration Failed";

            //Asserting Values.
            Assert.IsInstanceOf<ConflictObjectResult>(response);
            Assert.AreEqual(Success,dataResponseSuccess);
            Assert.AreEqual(Message, dataResponseMessage);
        }

        /// <summary>
        /// Register Employee API Test Cases.
        /// Valid Employee Details Should Return Ok.
        /// </summary>
        [Test]
        public void ValidEmployeeDetailsShouldReturnOk()
        {
            //Setting Values.
            employee.FirstName = "Suraj";
            employee.LastName = "Thorat";
            employee.EmailId = "suraj.thorat@gmail.com";
            employee.Mobile = "7811797526";
            employee.Address = "NaviMumbai, Maharashtra.";
            employee.BirthDate = "12/02/1985";
            employee.Employment = "Full-Time";
            var response = controller.RegisterEmployee(employee) as OkObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var dataResponseSuccess = dataResponse["Success"].ToObject<bool>();
            var dataResponseMessage = dataResponse["Message"].ToString();
            var dataResponseData = dataResponse["Data"].ToObject<Employee>();

            //Expected Values.
            bool Success = true;
            string Message = "Employee Registration Successful";

            //Asserting Values.
            Assert.IsInstanceOf<OkObjectResult>(response);
            Assert.AreEqual(Success, dataResponseSuccess);
            Assert.AreEqual(Message, dataResponseMessage);
            Assert.IsInstanceOf<Employee>(dataResponseData);
        }

        /// <summary>
        /// Test Case For Get Employee By Id.
        /// If Id Is Invalid Should Return Bad Request.
        /// </summary>
        [Test]
        public void IfIdIsInvalidShouldReturnBadRequest()
        {
            //Setting Values.
            int Id = -1;
            var response = controller.GetEmployee(Id) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var dataResponseSuccess = dataResponse["Success"].ToObject<bool>();
            var dataResponseMessage = dataResponse["Message"].ToString();

            //Expected Value.
            bool Success = false;
            string Message = "INVALID_FIELD_EXCEPTION";

            //Asserting Values.
            Assert.IsInstanceOf<BadRequestObjectResult>(response);
            Assert.AreEqual(Success, dataResponseSuccess);
            Assert.AreEqual(Message, dataResponseMessage);
        }

        /// <summary>
        /// Test Case For Get Employee By Id.
        /// If Id does not exists then Return Not Found.
        /// </summary>
        [Test]
        public void IfIdNotExistsShouldReturnNotFound()
        {
            //Setting Values.
            int Id = 208;
            var response = controller.GetEmployee(Id) as NotFoundObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var dataResponseSuccess = dataResponse["Success"].ToObject<bool>();
            var dataResponseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            bool Success = false;
            string Message = "Employee Detail Fetching Failed";

            //Asserting Values.
            Assert.IsInstanceOf<NotFoundObjectResult>(response);
            Assert.AreEqual(Success, dataResponseSuccess);
            Assert.AreEqual(Message, dataResponseMessage);
        }

        /// <summary>
        /// Test Case For Get Employee By Id.
        /// If Id Matches then Return Ok.
        /// </summary>
        [Test]
        public void IfIdMatchesShouldReturnOk()
        {
            //Setting Values.
            int Id = 1;
            var response = controller.GetEmployee(Id) as OkObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var dataResponseSuccess = dataResponse["Success"].ToObject<bool>();
            var dataResponseMessage = dataResponse["Message"].ToString();
            var dataResponseData = dataResponse["Data"].ToObject<Employee>();

            //Expected Values.
            string Message = "Employee Details Fetched Successfully";

            //Asserting Values.
            Assert.IsInstanceOf<OkObjectResult>(response);
            Assert.IsTrue(dataResponseSuccess);
            Assert.AreEqual(Message, dataResponseMessage);
            Assert.IsInstanceOf<Employee>(dataResponseData);
        }

        /// <summary>
        /// Test Case For Update Employee API.
        /// If Id Is Invalid Should Return BadRequest.
        /// </summary>
        [Test]
        public void GivenInvalidIdShouldReturnBadRequest()
        {
            //Setting Data.
            int Id = -25;
            employee.FirstName = "Suraj";
            employee.LastName = "Thorat";
            employee.EmailId = "suraj.thorat@gmail.com";
            employee.Mobile = "7811797526";
            employee.Address = "NaviMumbai, Maharashtra.";
            employee.BirthDate = "12/02/1985";
            employee.Employment = "Full-Time";
            var response = controller.UpdateEmployee(Id, employee) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var dataResponseSuccess = dataResponse["Success"].ToObject<bool>();
            var dataResponseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            string Message = "INVALID_FIELD_EXCEPTION";

            //Asserting Values.
            Assert.IsInstanceOf<BadRequestObjectResult>(response);
            Assert.IsFalse(dataResponseSuccess);
            Assert.AreEqual(Message, dataResponseMessage);
        }

        /// <summary>
        /// Test Case For Update Employee API.
        /// If Id does not exists Should Return Not Found.
        /// </summary>
        [Test]
        public void IdDoesNotExistsShouldReturnNotFound()
        {
            //Setting Data.
            int Id = 208;
            employee.FirstName = "Suraj";
            employee.LastName = "Thorat";
            employee.EmailId = "suraj.thorat@gmail.com";
            employee.Mobile = "7811797526";
            employee.Address = "NaviMumbai, Maharashtra.";
            employee.BirthDate = "12/02/1985";
            employee.Employment = "Full-Time";
            var response = controller.UpdateEmployee(Id, employee) as NotFoundObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var dataResponseSuccess = dataResponse["Success"].ToObject<bool>();
            var dataResponseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            string Message = "Employee Details Updation Failed";

            //Asserting Values.
            Assert.IsInstanceOf<NotFoundObjectResult>(response);
            Assert.IsFalse(dataResponseSuccess);
            Assert.AreEqual(Message, dataResponseMessage);
        }

        /// <summary>
        /// Test Case For Update Employee API.
        /// If Employee Detail Fields Are Empty Strings Should Return BadRequest.
        /// </summary>
        [Test]
        public void IfFieldsAreEmptyStringShouldReturnBadRequest()
        {
            //Setting Data.
            int Id = ValidId;
            employee.FirstName = "";
            employee.LastName = "";
            employee.EmailId = "";
            employee.Mobile = "";
            employee.Address = "";
            employee.BirthDate = "";
            employee.Employment = "";
            var response = controller.UpdateEmployee(Id, employee) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var dataResponseSuccess = dataResponse["Success"].ToObject<bool>();
            var dataResponseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            string Message = "INVALID_FIELD_EXCEPTION";

            //Asserting Values.
            Assert.IsInstanceOf<BadRequestObjectResult>(response);
            Assert.IsFalse(dataResponseSuccess);
            Assert.AreEqual(Message, dataResponseMessage);
        }

        /// <summary>
        /// Test Case For Update Employee API.
        /// If Employee Detail Fields Null Should Return BadRequest.
        /// </summary>
        [Test]
        public void IfFieldsAreNullShouldReturnBadRequest()
        {
            //Setting Data.
            int Id = ValidId;
            employee.FirstName = null;
            employee.LastName = null;
            employee.EmailId = null;
            employee.Mobile = null;
            employee.Address = null;
            employee.BirthDate = null;
            employee.Employment = null;
            var response = controller.UpdateEmployee(Id, employee) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var dataResponseSuccess = dataResponse["Success"].ToObject<bool>();
            var dataResponseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            string Message = "NULL_FIELD_EXCEPTION";

            //Asserting Values.
            Assert.IsInstanceOf<BadRequestObjectResult>(response);
            Assert.IsFalse(dataResponseSuccess);
            Assert.AreEqual(Message, dataResponseMessage);
        }

        /// <summary>
        /// Test Case For Update Employee API.
        ///  Valid Id and Data Should Return Ok.
        /// </summary>
        [Test]
        public void ValidIdandDataShouldReturnOk()
        {
            //Setting Data.
            int Id = ValidId;
            employee.FirstName = "Suraj";
            employee.LastName = "Thorat";
            employee.EmailId = "suraj.thorat@gmail.com";
            employee.Mobile = "7811797526";
            employee.Address = "NaviMumbai, Maharashtra.";
            employee.BirthDate = "12/02/1985";
            employee.Employment = "Full-Time";
            var response = controller.UpdateEmployee(Id, employee) as OkObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var dataResponseSuccess = dataResponse["Success"].ToObject<bool>();
            var dataResponseMessage = dataResponse["Message"].ToString();
            var dataResponseData = dataResponse["Data"].ToObject<Employee>();

            //Expected Values.
            string Message = "Employee Details Updated Successfuly";

            //Asserting Values.
            Assert.IsInstanceOf<OkObjectResult>(response);
            Assert.IsTrue(dataResponseSuccess);
            Assert.AreEqual(Message, dataResponseMessage);
            Assert.IsInstanceOf<Employee>(dataResponseData);
        }

        /// <summary>
        /// Test Cases For Delete API.
        /// If Id Is Not Valid SHould Return Bad Request.
        /// </summary>
        [Test]
        public void IfEmployeeIdIsInvalidShouldReturnBadRequest()
        {
            //Setting Data.
            int Id = -8;
            var response = controller.DeleteEmployee(Id) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var dataResponseSuccess = dataResponse["Success"].ToObject<bool>();
            var dataResponseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            string Message = "INVALID_FIELD_EXCEPTION";

            //Asserting Values.
            Assert.IsInstanceOf<BadRequestObjectResult>(response);
            Assert.IsFalse(dataResponseSuccess);
            Assert.AreEqual(Message, dataResponseMessage);
        }

        /// <summary>
        /// Test Cases For Delete API.
        /// If Id Does Not Exists Should Return Not Found.
        /// </summary>
        [Test]
        public void IfIdDoesNotExistsShouldReturnNotFound()
        {
            //Setting Data.
            int Id = 208;
            var response = controller.DeleteEmployee(Id) as NotFoundObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var dataResposneSuccess = dataResponse["Success"].ToObject<bool>();
            var dataResponseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            string Message = "Employee Deletion Failed";

            //Asserting Values.
            Assert.IsInstanceOf<NotFoundObjectResult>(response);
            Assert.IsFalse(dataResposneSuccess);
            Assert.AreEqual(Message, dataResponseMessage);
        }

        /// <summary>
        /// Test Cases For Delete API.
        /// Given Valid Id Should Return Ok.
        /// </summary>
        [Test]
        public void GivenValidIdShouldReturnOk()
        {
            //Setting Data.
            int Id = ValidId;
            var response = controller.DeleteEmployee(Id) as OkObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var dataResponseSuccess = dataResponse["Success"].ToObject<bool>();
            var dataResponseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            string Message = "Employee Deleted Successfuly";

            //Asserting Values.
            Assert.IsInstanceOf<OkObjectResult>(response);
            Assert.IsTrue(dataResponseSuccess);
            Assert.AreEqual(Message, dataResponseMessage);
        }
    }
}