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
    public class UserRL : IUserRL
    {
        //References.
        private IConfiguration configuration;
        private SqlConnection connection = null;
        string connectionString = null;

        /// <summary>
        /// Parameter Constrcutor For Setting Configuration Object.
        /// </summary>
        /// <param name="configuration"></param>
        public UserRL(IConfiguration configuration)
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
                command.Parameters.AddWithValue("@UserName", user.UserName);
                command.Parameters.AddWithValue("@Password", encryptedPassword);

                //Oppening The Conection.
                connection.Open();

                //Executing Store Procedure.
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

    }
}
