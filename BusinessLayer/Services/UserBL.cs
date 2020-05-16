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
    public class UserBL : IUserBL
    {
        // Reference.
        private IUserRL information;
        
        /// <summary>
        /// Parameter Constructor For Setting IUser Reference.
        /// </summary>
        /// <param name="information"></param>
        public UserBL(IUserRL information)
        {
            this.information = information;
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
                return this.information.RegisterUser(user);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
    }
}
