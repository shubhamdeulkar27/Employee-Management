using CommonLayer;
using RepositoryLayer.Interface;
using BusinessLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    /// <summary>
    /// Business Logic Layer Class.
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
        public bool RegisterUser(User user)
        {
            try
            {
                return this.employeeManagementRL.RegisterUser(user);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function For Login User.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public User LoginUser(User user)
        {
            try
            {
                return this.employeeManagementRL.LoginUser(user);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function To Register Employee.
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public bool RegisterEmployee(Employee employee)
        {
            try
            {
                return this.employeeManagementRL.RegisterEmployee(employee);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function to Get Employees.
        /// </summary>
        /// <returns></returns>
        public List<Employee> GetEmployees()
        {
            try
            {
                return this.employeeManagementRL.GetEmployees();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function for Getting Specified Employee Details.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Employee GetEmployee(int Id)
        {
            try
            {
                return this.employeeManagementRL.GetEmployee(Id);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function To Update Employee Details.
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public bool UpdateEmployee(int Id,Employee employee)
        {
            try
            {
                return this.employeeManagementRL.UpdateEmployee(Id,employee);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function To Delete Employee Details.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DeleteEmployee(int Id)
        {
            try
            {
                return this.employeeManagementRL.DeleteEmployee(Id);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
