using APITesting.RestInfrastructure;
using APITesting.RestInfrastructure.Enums;
using APITesting.RestInfrastructure.Services;

namespace Task10.Tests
{
    public class Tests
    {
        private UserService UserService = new UserService();

        [Test]
        public void Test1()
        {
            var users = UserService.GetUsers(Sex.Female.StringValue());



        }
    }
}