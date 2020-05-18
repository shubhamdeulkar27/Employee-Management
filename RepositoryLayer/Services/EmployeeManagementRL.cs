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
        public Message RegisterUser(User user)
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
                    Message message = new Message();
                    message.Status = "True";
                    message.ResponseMessage = "User Resgisterd";
                    message.Data = user.UserName;
                    return message;
                }
                else
                {
                    Message message = new Message();
                    message.Status = "False";
                    message.ResponseMessage = "User Not Resgisterd";
                    message.Data = user.UserName;
                    return message;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
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
        public Message LoginUser(User user)
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
                    Message message = new Message();
                    message.Status = "True";
                    message.ResponseMessage = "Login Successfull";
                    message.Data = user.UserName;
                    return message;
                }
                else
                {
                    Message message = new Message();
                    message.Status = "False";
                    message.ResponseMessage = "Login Attempt Failed";
                    message.Data = user.UserName;
                    return message; 
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// Function To Register Employee.
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public Message RegisterEmployee(Employee employee)
        {
            try
            {
                //establishing Connection.
                Connection();

                //Creating Sql Comman For Stored Procedure.
                SqlCommand command = new SqlCommand("spAddEmployee", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                command.Parameters.AddWithValue("@LastName", employee.LastName);
                command.Parameters.AddWithValue("@EmailId", employee.EmailId);
                command.Parameters.AddWithValue("@Mobile", employee.Mobile);
                command.Parameters.AddWithValue("@Address", employee.Address);
                command.Parameters.AddWithValue("@DOB", employee.DOB);
                command.Parameters.AddWithValue("@Employment", employee.Employment);

                //Oppening Connetion.
                connection.Open();

                //Executing Command.
                int i = command.ExecuteNonQuery();

                //closing Connection.
                connection.Close();

                if (i >= 1)
                {
                    Message message = new Message();
                    message.Status = "True";
                    message.ResponseMessage = "Emplyee Registered Successfully";
                    message.Data = employee.ToString();
                    return message;
                }
                else
                {
                    Message message = new Message();
                    message.Status = "False";
                    message.ResponseMessage = "Emplyee Registered Failed";
                    message.Data = employee.ToString();
                    return message;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

        }
    }
}
