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
        public IActionResult RegisterUser(User data)
        {
            Message response = employeeManagementBL.RegisterUser(data);
            return Ok(new { response });
        }
        
        /// <summary>
        /// Function For User Login.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public IActionResult LoginUser(User data)
        {
            Message response = employeeManagementBL.LoginUser(data);
            return Ok(new { response });
        }

        /// <summary>
        /// Function To Register Employee.
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost("registeremployee")]
        public IActionResult RegisterEmployee(Employee employee)
        {
            Message response = employeeManagementBL.RegisterEmployee(employee);
            return Ok(new { response });
        }
    }
}