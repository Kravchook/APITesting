using APITesting.RestInfrastructure;
using APITesting.RestInfrastructure.Enums;
using APITesting.RestInfrastructure.Services;

namespace APITesting.Tests
{
    public class UserControllerTests
    {
        public UserService UserService = new UserService();

        [Test]
        public void GetUsers()
        {
            var users = UserService.GetUsers(Sex.Female.StringValue());
        }

    }
}