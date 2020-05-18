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
        private IEmployeeManagementRL userRL;
        
        /// <summary>
        /// Parameter Constructor For Setting IUser Reference.
        /// </summary>
        /// <param name="information"></param>
        public EmployeeManagementBL(IEmployeeManagementRL userRL)
        {
            this.userRL = userRL;
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
                return this.userRL.RegisterUser(user);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public Message LoginUser(User user)
        {
            try
            {
                return this.userRL.LoginUser(user);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
    }
}
