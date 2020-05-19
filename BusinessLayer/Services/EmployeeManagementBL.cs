using CommonLayer;
using RepositoryLayer.Interface;
using BusinessLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    /// <summary>
    /// Business Logic Layer Class Fr User.
    /// </summary>
    public class EmployeeManagementBL : IEmployeeManagementBL
    {
        // Reference.
        private IEmployeeManagementRL employeeManagementRL;
        
        /// <summary>
        /// Parameter Constructor For Setting IUser Reference.
        /// </summary>
        /// <param name="information"></param>
        public EmployeeManagementBL(IEmployeeManagementRL userRL)
        {
            this.employeeManagementRL = userRL;
        }

        /// <summary>
        /// Function To Register User.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Message RegisterUser(User user)
        {
            try
            {
                return this.employeeManagementRL.RegisterUser(user);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// Function For Login User.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Message LoginUser(User user)
        {
            try
            {
                return this.employeeManagementRL.LoginUser(user);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// Function To Register Employee.
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public Message RegisterEmployee(Employee employee)
        {
            try
            {
                return this.employeeManagementRL.RegisterEmployee(employee);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// Function to Get Employees.
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public List<Employee> GetEmployees()
        {
            try
            {
                return this.employeeManagementRL.GetEmployees();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
    }
}
