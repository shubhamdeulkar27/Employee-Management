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
                ResponseMessage<User> response = employeeManagementBL.RegisterUser(data);
                return Ok(new { response.Status,response.Message,response.Data.UserName });
            }
            catch (Exception)
            {
                return BadRequest();
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
                ResponseMessage<User> response = employeeManagementBL.LoginUser(data);
                return Ok(new { response.Status, response.Message, response.Data.UserName });
            }
            catch (Exception)
            {
                return BadRequest();
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
                ResponseMessage<Employee> response = employeeManagementBL.RegisterEmployee(employee);
                return Ok(new { response.Status,response.Message,response.Data });
            }
            catch (Exception exception)
            {
                return BadRequest(new { Success=false,message=exception.Message});
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
                ResponseMessage<List<Employee>> response = employeeManagementBL.GetEmployees();
                return Ok(new { response.Status,response.Message,response.Data });
            }
            catch (Exception)
            {
                return BadRequest();
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
                ResponseMessage<Employee> response = employeeManagementBL.GetEmployee(Id);
                return Ok(new { response.Status, response.Message, response.Data });
            }
            catch (Exception)
            {
                return BadRequest();
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
                ResponseMessage<Employee> response = employeeManagementBL.UpdateEmployee(Id, employee);
                return Ok(new { response.Status, response.Message, response.Data });
            }
            catch (Exception)
            {
                return BadRequest();
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
                ResponseMessage<int> response = employeeManagementBL.DeleteEmployee(Id);
                return Ok(new { response.Status, response.Message, response.Data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}