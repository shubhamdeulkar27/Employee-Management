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
        private IUserBL userBL;

        public EmployeeController(IUserBL userBL)
        {
            this.userBL = userBL;
        }

        /// <summary>
        /// Function To Register User.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        // POST api/employee
        [HttpPost]
        public IActionResult RegisterUser(User data)
        {
            bool response = userBL.RegisterUser(data);
            return Ok(new { response });
        }

    }
}