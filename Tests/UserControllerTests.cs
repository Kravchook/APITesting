using APITask10.RestInfrastructure;
using APITask10.RestInfrastructure.Enums;
using APITask10.RestInfrastructure.Services;

namespace APITask10.Tests
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