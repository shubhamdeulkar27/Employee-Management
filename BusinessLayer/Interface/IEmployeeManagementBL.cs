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
        ResponseMessage<User> RegisterUser(User user);

        /// <summary>
        /// Abstract Function For LginUser Implementation.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        ResponseMessage<User> LoginUser(User data);

        /// <summary>
        /// Abstract Function to Register Employee Implementation.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        ResponseMessage<Employee> RegisterEmployee(Employee data);

        /// <summary>
        /// Abstract Function For Implementation Of GetEmployees.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        ResponseMessage<List<Employee>> GetEmployees();

        /// <summary>
        /// Abstract Function For Implementation Of Get Specific Employee Details Function.
        /// </summary>
        /// <returns></returns>
        ResponseMessage<Employee> GetEmployee(int Id);

        /// <summary>
        /// Abstract Function For Implementation Of Update Employee Function.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        ResponseMessage<Employee> UpdateEmployee(int Id,Employee data);

        /// <summary>
        /// Abstract Function For Implementation Of Delete Employee Details Function.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        ResponseMessage<int> DeleteEmployee(int Id);
    }
}
