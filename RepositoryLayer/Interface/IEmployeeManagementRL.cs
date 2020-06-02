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
        bool RegisterUser(User data);

        /// <summary>
        /// Abstract Function For Login User.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool LoginUser(User data);

        /// <summary>
        /// Abstract Function to Register Employee Implementation.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool RegisterEmployee(Employee data);

        /// <summary>
        /// Abstract Function For Implementation Of GetEmployees.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        List<Employee> GetEmployees();

        /// <summary>
        /// Abstract Function For Implementation Of Get Specific Employee Details Function.
        /// </summary>
        /// <returns></returns>
        Employee GetEmployee(int Id);

        /// <summary>
        /// Abstract Functon For Implementation Of Update Employee Function.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool UpdateEmployee(int Id, Employee data);

        /// <summary>
        /// Abstract Function For Implementation Of Delete Employee Details Function.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        bool DeleteEmployee(int Id);
    }
}
