using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using CommonLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Interface;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        //Bussiness Layer Refernces.
        private IEmployeeManagementBL employeeManagementBL;

        /// <summary>
        /// Parameter Constructor For Setting EmployeeManagementBL Object.
        /// </summary>
        /// <param name="employeeManagementBL"></param>
        public EmployeeController(IEmployeeManagementBL employeeManagementBL)
        {
            this.employeeManagementBL = employeeManagementBL;
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
                bool result = employeeManagementBL.RegisterUser(data);
                if (result == true)
                {
                    return Ok(new { Success = "True", Message = "User Registration Successful", Data = data });
                }
                else
                {
                    return Ok(new { Success = "False", Message = "User Registration Failed", Data = data });
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
                List<Employee> employees = employeeManagementBL.GetEmployees();
                if (employees!=null)
                {
                    return Ok(new { Success = "True", Message = "Employee List Fetched Successfully", Data = employees });
                }
                else
                {
                    return Ok(new { Success = "False", Message = "Employee List Fetching Failed", Data = employees });
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
                Employee employee = employeeManagementBL.GetEmployee(Id);
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