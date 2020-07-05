using System;
using System.Collections.Generic;
using System.Text;
using BusinessLayer.Interface;
using CommonLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        //Bussiness Layer Refernces.
        private IEmployeeManagementBL employeeManagementBL;

        //IDistributed Reference For Redis Cache.
        private readonly IDistributedCache distributedCache;

        /// <summary>
        /// Parameter Constructor For Setting EmployeeManagementBL Object.
        /// </summary>
        /// <param name="employeeManagementBL"></param>
        public EmployeeController(IEmployeeManagementBL employeeManagementBL, IDistributedCache distributedCache)
        {
            this.employeeManagementBL = employeeManagementBL;
            this.distributedCache = distributedCache;
        }

        /// <summary>
        /// Function To Register User.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        // POST api/employee
        [HttpPost("registeruser")]
        public IActionResult RegisterUser([FromBody]User data)
        {
            try
            {
                //If Field is Empty Will Throw Custom Exception and Return BadRequest.
                if (data.Role=="" || data.EmailId=="" || data.UserName=="" || data.Password=="") 
                {
                    return BadRequest(new { Success = false, message = CustomExceptions.ExceptionType.INVALID_FIELD_EXCEPTION });
                }

                //If field Is set to null then throw custome exception and return BadRequest.
                if (data.Role == null|| data.EmailId == null|| data.UserName == null || data.Password == null)
                {
                    return BadRequest(new { Success = false, message = CustomExceptions.ExceptionType.NULL_FIELD_EXCEPTION});
                }

                bool result = employeeManagementBL.RegisterUser(data);
                if (result == true)
                {
                    return Ok(new { Success = "True", Message = "User Registration Successful", Data = data });
                }
                else
                {
                    return Ok(new { Success = "True", Message = "User Already Exists", Data = data });
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
        [HttpPost("login")]
        public IActionResult LoginUser([FromBody]User data)
        {
            try
            {
                //If field is empty then will throw custom exception and return BadRequest.
                if (data.UserName=="" || data.Password=="") 
                {
                    return BadRequest(new { Success = false, Message = CustomExceptions.ExceptionType.INVALID_FIELD_EXCEPTION });
                }

                //If field is null then will throw custom Exception and return BadRequest.
                if (data.UserName == null || data.Password == null)
                {
                    return BadRequest(new { Success = false, Message = CustomExceptions.ExceptionType.NULL_FIELD_EXCEPTION });
                }

                bool result = employeeManagementBL.LoginUser(data);
                if (result == true)
                {
                    return Ok(new { Success = "True", Message = "User Login Successful", Data = data.UserName });
                }
                else
                {
                    return Ok(new { Success = "False", Message = "User Login Failed", Data = data.UserName });
                }
            }
            catch (Exception exception)
            {
                return BadRequest(new { Success = "False", message = exception.Message});
            }
        }

        /// <summary>
        /// Function To Register Employee.
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RegisterEmployee([FromBody]Employee employee)
        {
            try
            {
                //If Fields are empty then will throw custom exception and return BadRequest.
                if (employee.FirstName=="" || employee.LastName=="" || employee.EmailId=="" || employee.Mobile=="" ||employee.Address==""|| employee.Employment=="") 
                {
                    return BadRequest(new { Success = false, Message = CustomExceptions.ExceptionType.INVALID_FIELD_EXCEPTION });
                }

                //If fields are set null then throw custom exception and return BadRequest.
                if (employee.FirstName == null || employee.LastName == null || employee.EmailId == "" || employee.Mobile == "" || employee.Address == "" || employee.Employment == "")
                {
                    return BadRequest(new { Success = false, Message = CustomExceptions.ExceptionType.INVALID_FIELD_EXCEPTION });
                }

                bool result = employeeManagementBL.RegisterEmployee(employee);
                if (result == true)
                {
                    return Ok(new { Success = "True", Message = "Employee Registration Successful", Data = employee });
                }
                else
                {
                    return Ok(new { Success = "False", Message = "Employee Registration Failed", Data = employee });
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
                                      .SetSlidingExpiration(TimeSpan.FromMinutes(5))
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
                    return Ok(new { Success = "True", Message = "Employee Details Not Exists", Data = employees });
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
        [HttpGet("{id}")]
        public IActionResult GetEmployee([FromRoute]int Id)
        {
            try
            {
                //If Id Is Invalid then Throw Custom Exception and Return Bad Request.
                if (Id < 0)
                {
                    return BadRequest(new { Success = false, Message = CustomExceptions.ExceptionType.INVALID_FIELD_EXCEPTION });
                }

                //Employee Reference For Storing Employee Details.
                Employee employee;
                
                //Variables For Redis Cache.
                string cacheKey = Id.ToString();
                string serializedEmployee;

                //Getting Employee Details From Redis Cache.
                var encodedEmployee = distributedCache.Get(cacheKey);

                //If Redis has employee detail then it will fetch from Redis else it will fetch from Database.
                if(encodedEmployee!=null)
                {
                    serializedEmployee = Encoding.UTF8.GetString(encodedEmployee);
                    employee = JsonConvert.DeserializeObject<Employee>(serializedEmployee);
                }
                else
                {
                    employee = employeeManagementBL.GetEmployee(Id);
                    serializedEmployee = JsonConvert.SerializeObject(employee);
                    encodedEmployee = Encoding.UTF8.GetBytes(serializedEmployee);
                    var options = new DistributedCacheEntryOptions()
                                     .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                                     .SetAbsoluteExpiration(DateTime.Now.AddHours(6));
                    distributedCache.Set(cacheKey, encodedEmployee, options);
                }

                //Sending Response Depending Employee Details.
                if (employee != null)
                {
                    return Ok(new { Success = "True", Message = "Employee Detail Fetched Successfully", Data = employee });
                }
                else
                {
                    return Ok(new { Success = "False", Message = "Employee Detail Fetching Failed", Data = employee });
                }
            }
            catch (Exception exception)
            {
                return BadRequest(new { Success="False" , message = exception.Message});
            }
        }

        /// <summary>
        /// Function To Update Employee Details.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPut("{Id}")]
        public IActionResult UpdateEmployee([FromRoute]int Id, [FromBody]Employee employee)
        {
            try
            {
                //If Id is invalid then throw custom exception and return BadRequest.
                if(Id<0)
                {
                    return BadRequest(new { Success = false, Message = CustomExceptions.ExceptionType.INVALID_FIELD_EXCEPTION });
                }

                //If Fields are empty then will throw custom exception and return BadRequest.
                if (employee.FirstName == "" || employee.LastName == "" || employee.EmailId == "" || employee.Mobile == "" || employee.Address == "" || employee.Employment == "")
                {
                    return BadRequest(new { Success = false, Message = CustomExceptions.ExceptionType.INVALID_FIELD_EXCEPTION });
                }

                //If fields are set null then throw custom exception and return BadRequest.
                if (employee.FirstName == null || employee.LastName == null || employee.EmailId == "" || employee.Mobile == "" || employee.Address == "" || employee.Employment == "")
                {
                    return BadRequest(new { Success = false, Message = CustomExceptions.ExceptionType.INVALID_FIELD_EXCEPTION });
                }

                bool result = employeeManagementBL.UpdateEmployee(Id, employee);
                if (result == true)
                {
                    return Ok(new { Success = "True", Message = "Employee Details Updated Successfuly", Data = employee });
                }
                else
                {
                    return Ok(new { Success = "False", Message = "Employee Details Updation Failed", Data = employee });
                }
            }
            catch (Exception exception)
            {
                return BadRequest(new { Success = "False" , message = exception });
            }
        }
        
        /// <summary>
        /// Function To Delete Employee Details.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("{Id}")]
        public IActionResult DeleteEmployee([FromRoute]int Id)
        {
            try
            {
                //If Id is invalid then throw custom exception and return BadRequest.
                if (Id < 0)
                {
                    return BadRequest(new { Success = false, Message = CustomExceptions.ExceptionType.INVALID_FIELD_EXCEPTION });
                }

                bool result = employeeManagementBL.DeleteEmployee(Id);
                if (result == true)
                {
                    return Ok(new { Success = "True", Message = "Employee Deleted Successfuly", Data = Id });
                }
                else
                {
                    return Ok(new { Success = "False", Message = "Employee Deletion Failed", Data = Id });
                }
            }
            catch (Exception exception)
            {
                return BadRequest(new { Success="False" , message = exception.Message});
            }
        }
    }
}