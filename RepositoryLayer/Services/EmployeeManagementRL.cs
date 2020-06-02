using CommonLayer;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.Http;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace RepositoryLayer.Services
{
    /// <summary>
    /// Class EmployeeManagementRL To Interact With Database. 
    /// </summary>
    public class EmployeeManagementRL : IEmployeeManagementRL
    {
        //References.
        private IConfiguration configuration;
        private SqlConnection connection = null;
        string connectionString = null;

        /// <summary>
        /// Parameter Constrcutor For Setting Configuration Object.
        /// </summary>
        /// <param name="configuration"></param>
        public EmployeeManagementRL(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Function To Establish Connection With Database.
        /// </summary>
        private void Connection()
        {
            try
            {
                connectionString = configuration.GetSection("ConnectionStrings").GetSection("ConnectionString").Value;
                connection = new SqlConnection(connectionString);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// Function To Register User (UserName And Password).
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool RegisterUser(User user)
        {
            try
            {
                //Encrypting Password.
                string encryptedPassword = EncodePasswordToBase64(user.Password);
                
                //Establishing Connection.
                Connection();

                //Creating Sql Comman For Stored Procedure.
                SqlCommand command = new SqlCommand("spRegisterUser", connection);
                command.CommandType = CommandType.StoredProcedure;

                //Setting Parameters.
                command.Parameters.AddWithValue("@Role", user.Role);
                command.Parameters.AddWithValue("@EmailId", user.EmailId);
                command.Parameters.AddWithValue("@UserName", user.UserName);
                command.Parameters.AddWithValue("@Password", encryptedPassword);

                //Oppening The Conection.
                connection.Open();

                //Executing Stored Procedure.
                int i = command.ExecuteNonQuery();

                //Clossing Connection.
                connection.Close();

                if (i >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function For Encrypting Password.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }

        /// <summary>
        /// Function To Login User.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool LoginUser(User user)
        {
            try
            {
                //Encrypting Password.
                string encryptedPassword = EncodePasswordToBase64(user.Password);
                
                //Establishing Connection.
                Connection();

                //Creating Sql Comman For Stored Procedure.
                SqlCommand command = new SqlCommand("spLoginUser", connection);
                command.CommandType = CommandType.StoredProcedure;
                
                //Setting Parameters.
                command.Parameters.AddWithValue("@UserName", user.UserName);
                command.Parameters.AddWithValue("@Password", encryptedPassword);
                
                //Oppening The Conection.
                connection.Open();

                //Executing Store Procedure.
                SqlDataReader reader = command.ExecuteReader();
                
                int status=0;
                
                //While Loop For Reading status result from SqlDataReader.
                while (reader.Read())
                {
                    status = reader.GetInt32(0);
                }

                //Clossing Connection.
                connection.Close();

                if (status >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
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
                //Establishing Connection.
                Connection();

                //Creating Sql Command For Stored Procedure.
                SqlCommand command = new SqlCommand("spAddEmployee", connection);
                command.CommandType = CommandType.StoredProcedure;
                
                //Setting Parameters.
                command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                command.Parameters.AddWithValue("@LastName", employee.LastName);
                command.Parameters.AddWithValue("@EmailId", employee.EmailId);
                command.Parameters.AddWithValue("@Mobile", employee.Mobile);
                command.Parameters.AddWithValue("@Address", employee.Address);
                command.Parameters.AddWithValue("@BirthDate", employee.BirthDate);
                command.Parameters.AddWithValue("@Employment", employee.Employment);

                //Oppening Connetion.
                connection.Open();

                //Executing Command.
                int i = command.ExecuteNonQuery();

                //closing Connection.
                connection.Close();

                if (i >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }    
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function To Get All Employees.
        /// </summary>
        /// <returns></returns>
        public List<Employee> GetEmployees()
        {
            try
            {
                //List For Storing Employee Details.
                List<Employee> employeeList = new List<Employee>();

                //Establishing Connection.
                Connection();

                //Creating Sql Command For Stored Procedure.
                SqlCommand command = new SqlCommand("spGetEmployees", connection);
                command.CommandType = CommandType.StoredProcedure;

                //Opening Connectiion.
                connection.Open();

                //Executing Stored Procedure.
                SqlDataReader reader = command.ExecuteReader();

                //While Loop For Reading Data From SqlDataReader To Employee Object.
                while (reader.Read())
                {
                    Employee employeeData = new Employee();
                    employeeData.Id = (int)reader["Id"];
                    employeeData.FirstName = reader["FirstName"].ToString();
                    employeeData.LastName = reader["LastName"].ToString();
                    employeeData.EmailId = reader["EmailId"].ToString();
                    employeeData.Mobile = reader["Mobile"].ToString();
                    employeeData.Address = reader["Address"].ToString();
                    employeeData.BirthDate = reader["BirthDate"].ToString();
                    employeeData.Employment = reader["Employment"].ToString();
                    employeeList.Add(employeeData);
                }

                //Closing Connection.
                connection.Close();

                if (employeeList.Count != 0)
                {
                    return employeeList;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function For Getting Specified Employee Details.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Employee GetEmployee(int Id)
        {
            try
            {
                //Establishing Connection.
                Connection();
                
                //Creating SqlCommand For Stored Procedure.
                SqlCommand command = new SqlCommand("spGetEmployee", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id",Id);

                //Oppening Conection.
                connection.Open();

                //Executing Stored Procedure.
                SqlDataReader reader = command.ExecuteReader();

                //Employee Instance For Setting Data.
                Employee employeeData = new Employee();

                //While Loop For Reading Data From SqlDataReader To Employee Object.
                while (reader.Read())
                {
                    //reading Data From SqlDataReader
                    employeeData.Id = (int)reader["Id"];
                    employeeData.FirstName = reader["FirstName"].ToString();
                    employeeData.LastName = reader["LastName"].ToString();
                    employeeData.EmailId = reader["EmailId"].ToString();
                    employeeData.Mobile = reader["Mobile"].ToString();
                    employeeData.Address = reader["Address"].ToString();
                    employeeData.BirthDate = reader["BirthDate"].ToString();
                    employeeData.Employment = reader["Employment"].ToString();
                }

                //Closing Connection.
                connection.Close();

                if (employeeData != null)
                {
                    return employeeData;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function To Update Employee Details.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
        public bool UpdateEmployee(int Id, Employee employee)
        {
            try
            {
                //Establishing Connection.
                Connection();

                //Creating Sqlcommand For Stored Procedure.
                SqlCommand command = new SqlCommand("spUpdateEmployee", connection);
                command.CommandType = CommandType.StoredProcedure;
                
                //Setting Parameters.
                command.Parameters.AddWithValue("@Id",Id);
                command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                command.Parameters.AddWithValue("@LastName", employee.LastName);
                command.Parameters.AddWithValue("@EmailId", employee.EmailId);
                command.Parameters.AddWithValue("@Mobile", employee.Mobile);
                command.Parameters.AddWithValue("@Address", employee.Address);
                command.Parameters.AddWithValue("@BirthDate", employee.BirthDate);
                command.Parameters.AddWithValue("@Employment", employee.Employment);

                //Oppening Connetion.
                connection.Open();

                //Executing Command.
                int i = command.ExecuteNonQuery();

                //closing Connection.
                connection.Close();

                if (i >= 1)
                {
                    return true; 
                }
                else
                {
                    return false;
                }
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
                //Establishing Connection.
                Connection();

                //Creating SqlCommand For Stored Procedure.
                SqlCommand command = new SqlCommand("spDeleteEmployee", connection);
                command.CommandType = CommandType.StoredProcedure;

                //Setting Parameters.
                command.Parameters.AddWithValue("@Id", Id);

                //Oppening Connection
                connection.Open();

                //Executing Command.
                int i = command.ExecuteNonQuery();

                //Closing Connection.
                connection.Close();

                if (i >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
