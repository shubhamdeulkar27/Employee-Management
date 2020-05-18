using CommonLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Http;

namespace RepositoryLayer.Interface
{
    /// <summary>
    /// Interface For IUserRL Impelementation.
    /// </summary>
    public interface IEmployeeManagementRL
    {
        /// <summary>
        /// Abstract bool Function For User Registration Implementation.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Message RegisterUser(User data);

        /// <summary>
        /// Abstract Function For Login User.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Message LoginUser(User data);
    }
}
