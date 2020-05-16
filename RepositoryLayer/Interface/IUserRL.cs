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
    public interface IUserRL
    {
        /// <summary>
        /// Abstract bool Function For User Registration Implementation.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool RegisterUser(User data);
    }
}
