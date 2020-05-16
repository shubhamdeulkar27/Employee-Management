using CommonLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    /// <summary>
    /// Interface for IUserBL Implementation.
    /// </summary>
    public interface IUserBL
    {
        /// <summary>
        /// Abstract Function For Implementation.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        bool RegisterUser(User user);
    }
}
