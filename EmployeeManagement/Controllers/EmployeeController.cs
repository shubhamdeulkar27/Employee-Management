using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BusinessLayer.Interface;
using CommonLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;


namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        //IConfiguration Reference For JWT Token.
        private IConfiguration configuration;

        //Bussiness Layer Refernces.
        private IEmployeeManagementBL employeeManagementBL;

        //IDistributed Reference For Redis Cache.
        private readonly IDistributedCache distributedCache;

        //Sendeer Class Object For MSMQ
        Sender senderObject = new Sender();

        /// <summary>
        /// Parameter Constructor For Setting EmployeeManagementBL Object.
        /// </summary>
        /// <param name="employeeManagementBL"></param>
        public EmployeeController(IEmployeeManagementBL employeeManagementBL, IDistributedCache distributedCache,IConfiguration configuration)
        {
            this.employeeManagementBL = employeeManagementBL;
            this.distributedCache = distributedCache;
            this.configuration = configuration;
        }

        /// <summary>
        /// Function To Register User.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        // POST api/employee
        [HttpPost("registeruser")]
        [Authorize(Roles = "Admin")]
        public IActionResult RegisterUser([FromBody]User data)
        {
            try
            {
                //If Field is Empty Will Throw Custom Exception and Return BadRequest.
                if (data.Role=="" || data.EmailId=="" || data.UserName=="" || data.Password=="") 
                {
                    return BadRequest(new { Success = false, message = CustomExceptions.ExceptionType.INVALID_FIELD_EXCEPTION.ToString() });
                }

                //If field Is set to null then throw custome exception and return BadRequest.
                if (data.Role == null|| data.EmailId == null|| data.UserName == null || data.Password == null)
                {
                    return BadRequest(new { Success = false, message = CustomExceptions.ExceptionType.NULL_FIELD_EXCEPTION.ToString()});
                }

                bool result = employeeManagementBL.RegisterUser(data);
                if (result == true)
                {
                    //Message For MSMQ.
                    string message = " Hello " + Convert.ToString(data.UserName) + 
                                     " Your \n" + "Registration Succesful" +
                                     "\n UserName: "+Convert.ToString(data.UserName)+
                                     "\n Role: " + Convert.ToString(data.Role) +
                                     "\n Email :" + Convert.ToString(data.EmailId);

                    //Sending Message To MSMQ.
                    senderObject.Send(message);

                    return Ok(new { Success = true, Message = "User Registration Successful", Data = data });
                }
                else
                {
                    return Conflict(new { Success = false, Message = "User Already Exists", Data = data });
                }
                
            }
            catch (Exception exception)
            {
                return BadRequest(new { Success = "False", message = exception.Message });
            }
        }

        /// <summary>
        /// Function For User Login.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult LoginUser([FromBody]User data)
        {
            try
            {
                //If field is empty then will throw custom exception and return BadRequest.
                if (data.UserName=="" || data.Password=="") 
                {
                    return BadRequest(new { Success = false, Message = CustomExceptions.ExceptionType.INVALID_FIELD_EXCEPTION.ToString() });
                }

                //If field is null then will throw custom Exception and return BadRequest.
                if (data.UserName == null || data.Password == null)
                {
                    return BadRequest(new { Success = false, Message = CustomExceptions.ExceptionType.NULL_FIELD_EXCEPTION.ToString() });
                }

                //Set Response For Unauthorized.
                IActionResult response = Unauthorized();

                User userData = employeeManagementBL.LoginUser(data);
                if (userData.Role !=null && userData.EmailId !=null)
                {
                    var tokenString = GenerateJsonWebToken(userData);
                    return Ok(new { Success = true, Message = "Login Successfull", Data = data.UserName , Token = tokenString});
                }
                else
                {
                    return this.NotFound(new { Success = false, Message = response });
                }
            }
            catch (Exception exception)
            {
                return BadRequest(new { Success = "False", message = exception.Message});
            }
        }

        /// <summary>
        /// Function For JsonToken Generation.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string GenerateJsonWebToken(User data)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, data.UserName),
                new Claim(JwtRegisteredClaimNames.Email,data.EmailId),
                new Claim(ClaimTypes.Role, data.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
                            configuration["Jwt:Audiance"],
                            claims,
                            expires: DateTime.Now.AddMinutes(120),
                            signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Function To Register Employee.
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public IActionResult RegisterEmployee([FromBody]Employee employee)
        {
            try
            {
                //Key For Redis Cache
                string cacheKey = "employees";

                //If Fields are empty then will throw custom exception and return BadRequest.
                if (employee.FirstName=="" || employee.LastName=="" || employee.EmailId=="" || employee.Mobile=="" ||employee.Address==""|| employee.Employment=="") 
                {
                    return BadRequest(new { Success = false, Message = CustomExceptions.ExceptionType.INVALID_FIELD_EXCEPTION.ToString() });
                }

                //If fields are set null then throw custom exception and return BadRequest.
                if (employee.FirstName == null || employee.LastName == null || employee.EmailId == "" || employee.Mobile == "" || employee.Address == "" || employee.Employment == "")
                {
                    return BadRequest(new { Success = false, Message = CustomExceptions.ExceptionType.NULL_FIELD_EXCEPTION.ToString() });
                }

                bool result = employeeManagementBL.RegisterEmployee(employee);
                if (result == true)
                {
                    //If new Employee Details are Added then cache will be cleared.
                    if(distributedCache.Get(cacheKey)!=null)
                    {
                        distributedCache.Remove(cacheKey);
                    }
                    return Ok(new { Success = "True", Message = "Employee Registration Successful", Data = employee });
                }
                else
                {
                    return Conflict(new { Success = "False", Message = "Employee Registration Failed", Data = employee });
                }
            }
            catch (Exception exception)
            {
                return BadRequest(new { Success=false, message=exception.Message});
            }
        }

        /// <summary>
        /// Function To Get All Employees.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public IActionResult GetEmployees()
        {
            try
            {
                //List for Employees.
                List<Employee> employees;

                //Variables For Cache.
                string cacheKey = "employees";
                string serializedList;

                //Getting Encoded Employee List From Redis Cache.
                var encodedList = distributedCache.Get(cacheKey);
                
                //If redis cache has the data then List will be fetched from redis else
                //it will fetch data from Database.
                if(encodedList !=null)
                {
                    serializedList = Encoding.UTF8.GetString(encodedList);
                    employees = JsonConvert.DeserializeObject<List<Employee>>(serializedList);
                }
                else
                {
                    employees = employeeManagementBL.GetEmployees();
                    serializedList = JsonConvert.SerializeObject(employees);
                    encodedList = Encoding.UTF8.GetBytes(serializedList);
                    var options = new DistributedCacheEntryOptions()
                                      .SetSlidingExpiration(TimeSpan.FromMinutes(20))
                                      .SetAbsoluteExpiration(DateTime.Now.AddHours(6));
                    distributedCache.Set(cacheKey, encodedList, options);
                }

                //Sending Resonses Depending On Employee List.
                if (employees!=null)
                {
                    return Ok(new { Success = "True", Message = "Employee List Fetched Successfully", Data = employees });
                }
                else
                {
                    return NotFound(new { Success = "False", Message = "Employee Details Not Exists", Data = employees });
                }
            }
            catch (Exception exception)
            {
                return BadRequest(new { Success = "False" , message = exception.Message});
            }
        }

        /// <summary>
        /// Function To Get Specified Employee Details.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [Authorize]
        public IActionResult GetEmployee([FromRoute]int Id)
        {
            try
            {
                //If Id Is Invalid then Throw Custom Exception and Return Bad Request.
                if (Id < 0)
                {
                    return BadRequest(new { Success = false, Message = CustomExceptions.ExceptionType.INVALID_FIELD_EXCEPTION.ToString() });
                }

                //Employee Reference For Storing Employee Details.
                Employee employee = employeeManagementBL.GetEmployee(Id);

                //Sending Response Depending Employee Details.
                if (employee.EmailId != null)
                {
                    return Ok(new { Success = true, Message = "Employee Details Fetched Successfully", Data = employee });
                }
                else
                {
                    return NotFound(new { Success = false, Message = "Employee Detail Fetching Failed" });
                }
            }
            catch (Exception exception)
            {
                return BadRequest(new { Success= false , message = exception.Message});
            }
        }

        /// <summary>
        /// Function To Update Employee Details.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPut("{Id}")]
        [Authorize(Roles ="Admin")]
        public IActionResult UpdateEmployee([FromRoute]int Id, [FromBody]Employee employee)
        {
            try
            {
                //Keys for Redis Cache.
                string cacheKeyForEmployees = "employees";

                //If Id is invalid then throw custom exception and return BadRequest.
                if (Id<=0)
                {
                    return BadRequest(new { Success = false, Message = CustomExceptions.ExceptionType.INVALID_FIELD_EXCEPTION.ToString() });
                }

                //If Fields are empty then will throw custom exception and return BadRequest.
                if (employee.FirstName == "" || employee.LastName == "" || employee.EmailId == "" || employee.Mobile == "" || employee.Address == "" || employee.Employment == "")
                {
                    return BadRequest(new { Success = false, Message = CustomExceptions.ExceptionType.INVALID_FIELD_EXCEPTION.ToString() });
                }

                //If fields are set null then throw custom exception and return BadRequest.
                if (employee.FirstName == null || employee.LastName == null || employee.EmailId == null || employee.Mobile == null || employee.Address == null || employee.Employment == null)
                {
                    return BadRequest(new { Success = false, Message = CustomExceptions.ExceptionType.NULL_FIELD_EXCEPTION.ToString() });
                }

                bool result = employeeManagementBL.UpdateEmployee(Id, employee);
                if (result == true)
                {
                    //Clearing Redis Cache.
                    distributedCache.Remove(cacheKeyForEmployees);
                    return Ok(new { Success = true, Message = "Employee Details Updated Successfuly", Data = employee });
                }
                else
                {
                    return NotFound(new { Success = false, Message = "Employee Details Updation Failed" });
                }
            }
            catch (Exception exception)
            {
                return BadRequest(new { Success = false , message = exception });
            }
        }
        
        /// <summary>
        /// Function To Delete Employee Details.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("{Id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteEmployee([FromRoute]int Id)
        {
            try
            {
                //Keys for Redis Cache.
                string cacheKeyForEmployees = "employees";

                //If Id is invalid then throw custom exception and return BadRequest.
                if (Id <= 0)
                {
                    return BadRequest(new { Success = false, Message = CustomExceptions.ExceptionType.INVALID_FIELD_EXCEPTION.ToString() });
                }

                bool result = employeeManagementBL.DeleteEmployee(Id);
                if (result == true)
                {
                    //Clearing Redis Cache.
                    distributedCache.Remove(cacheKeyForEmployees);
                    return Ok(new { Success = true, Message = "Employee Deleted Successfuly", Data = Id });
                }
                else
                {
                    return NotFound(new { Success = false, Message = "Employee Deletion Failed", Data = Id });
                }
            }
            catch (Exception exception)
            {
                return BadRequest(new { Success=false , message = exception.Message});
            }
        }
    }
}