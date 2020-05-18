using CommonLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    /// <summary>
    /// Interface for IUserBL Implementation.
    /// </summary>
    public interface IEmployeeManagementBL
    {
        /// <summary>
        /// Abstract Function For Implementation.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Message RegisterUser(User user);

        /// <summary>
        /// Abstract Function For LginUser Implementation.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Message LoginUser(User data);
    }
}
