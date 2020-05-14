using CommonLayer;
using System;
using Xunit;

namespace EmployeeManagementTests
{
    /// <summary>
    /// Class For Test Cases For Comman Layer.
    /// </summary>
    public class CommonLayerTests
    {
        /// <summary>
        /// Test Case 1.0 Given UserName, Password and UserName,Password Should Return Equal.
        /// </summary>
        [Fact]
        public void GivenUserNamePasswordAndUserNamePasswordShouldReturnEqual()
        {
            //Creating User Instances.
            User user = new User();
            user.UserName = "Chris";
            user.Password = "Thor";

            //Expected Values.
            string expectedUserName = "Chris";
            string expectedPassword = "Thor";

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
                user.UserName = "Chris";
            }
            catch (Exception exception)
            {
                //Asserting Values.
                Assert.Equal(expected, exception.Message);
            }
        }

    }
}
