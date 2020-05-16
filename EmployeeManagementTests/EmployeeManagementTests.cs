using CommonLayer;
using RepositoryLayer.Services;
using System;
using Xunit;

namespace EmployeeManagementTests
{
    /// <summary>
    /// Class For Test Cases For Comman Layer.
    /// </summary>
    public class EmployeeManagementTests
    {
        /// <summary>
        /// Test Case 1.0 Given UserName, Password and UserName,Password Should Return Equal.
        /// </summary>
        [Fact]
        public void GivenUserNamePasswordAndUserNamePasswordShouldReturnEqual()
        {
            //Creating User Instances.
            User user = new User();
            user.UserName = "Chrisy";
            user.Password = "1234567";

            //Expected Values.
            string expectedUserName = "Chrisy";
            string expectedPassword = "1234567";

            //Asserting Values.
            Assert.Equal(expectedUserName,user.UserName);
            Assert.Equal(expectedPassword, user.Password);
        }

        /// <summary>
        /// Test Case 1.1 Given Empty UserName Should throw Validation Exception.
        /// </summary>
        [Fact]
        public void GivenEmptyUserNameShouldThrowValidationException()
        {
            //Expected Value.
            string expected = "UserName Is Required";
            try
            {
                //Creating User Instance.
                User user = new User();
            }
            catch (Exception exception)
            {
                //Asserting Values.
                Assert.Equal(expected,exception.Message);
            }
        }

        /// <summary>
        /// Test Case 1.2 Given MinimumLength Password Should throw Validation Exception.
        /// </summary>
        [Fact]
        public void GivenEmptyPasswordShouldThrowValidationException()
        {
            //Expected Value.
            string expected = "Password Is Required";
            try
            {
                //Creating User Instance.
                User user = new User();
                user.UserName = "Chrisy";
            }
            catch (Exception exception)
            {
                //Asserting Values.
                Assert.Equal(expected, exception.Message);
            }
        }

        /// <summary>
        /// Test Case 1.3 For Checking Encryption Function. 
        /// </summary>
        [Fact]
        public void TestForChrckingEncryptingFunction()
        {
            string encryptedSting1 = UserRL.EncodePasswordToBase64("Visual");
            string encryptedSting2 = UserRL.EncodePasswordToBase64("Visual");
            Assert.Equal(encryptedSting1,encryptedSting2);
        }
    }
}
