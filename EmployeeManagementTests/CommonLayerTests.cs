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
    }
}
