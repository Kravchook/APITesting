using APITesting.RestInfrastructure;
using APITesting.RestInfrastructure.DataModels;
using APITesting.RestInfrastructure.Enums;
using APITesting.RestInfrastructure.Services;
using System.Net;

namespace APITesting.Tests
{
    public class UserControllerTests
    {
        private UserService UserService = new UserService();
        private ZipCodeService ZipCodeService = new ZipCodeService();

        [Test]
        [Description("Debug")]
        public void ShowUsersTest()
        {
            var users = UserService.GetUsers();

            Console.WriteLine("Created users:");
            foreach (var user in users)
            {
                Console.WriteLine(user.Name);
            }
        }

        [Test]
        [Description("Task30 - Scenario 1")]
        public void CreateUser_AllFieldsFilledTest()
        {
            UserDto user = new UserDto
            {
                Age = 20, 
                Name = "Bob Dilan", 
                Sex = Sex.Male.StringValue(), 
                ZipCode = "12345"
            };

            //BUG: A duplicate user is being added to the application in case if user to add has the same name and sex as existing user in the system
            //AND the available zip code also exists (can be if there are duplicates among zip codes)
            try
            {
                UserService.CreateUser(user, HttpStatusCode.Created);
            }
            finally
            {
                var users = GetUsers();
                Assert.That(users.Find(u => u.Name.Equals(user.Name)), Is.Not.Null, $"User {user.Name} was not created");

                var zipCodes = ZipCodeService.GetZipCodes(HttpStatusCode.Created);
                Assert.That(zipCodes, Is.Not.Contain(user.ZipCode), $"Zip code {user.ZipCode} was not removed from the available zip codes list");

                Console.WriteLine("Available Zip codes:");
                foreach (var code in zipCodes)
                {
                    Console.WriteLine(code);
                }
            }
        }

        [Test]
        [Description("Task30 - Scenario 2")]
        public void CreateUser_RequiredFieldsFilledTest()
        {
            UserDto user = new UserDto
            {
                Name = "Janna Dark",
                Sex = Sex.Female.StringValue()
            };

            UserService.CreateUser(user, HttpStatusCode.Created);

            var users = GetUsers();
            Assert.That(users.Find(u => u.Name.Equals(user.Name)), Is.Not.Null, $"User {user.Name} was not created");
        }

        [Test]
        [Description("Task30 - Scenario 3")]
        public void CreateUser_AllFieldsFilled_IncorrectZipCodeTest()
        {
            UserDto user = new UserDto
            {
                Age = 20,
                Name = "Tom Jones",
                Sex = Sex.Male.StringValue(),
                ZipCode = "code10"
            };

            UserService.CreateUser(user, HttpStatusCode.FailedDependency);

            var users = GetUsers();
            Assert.That(users.Find(u => u.Name.Equals(user.Name)), Is.Null, $"User {user.Name} was created");
        }

        [Test]
        [Description("Task30 - Scenario 4")]
        public void CreateUser_CreateDuplicateForUserTest()
        {
            UserDto user = new UserDto
            {
                Name = "Janna Dark",
                Sex = Sex.Female.StringValue()
            };

            try
            {
                //BUG: If create a duplicate for the user with only required fields filled (Name, Sex) without specifying ZipCode getting error:
                //StatusCode not as expected - Actual: Created (201), Expected: 400 (BadRequest) 
                UserService.CreateUser(user, HttpStatusCode.BadRequest);
            }
            finally
            {
                //As a result of the BUG: User is created and added to application
                var users = GetUsers();
                Assert.That(users.Find(u => u.Name.Equals(user.Name)), Is.Null, $"User {user.Name} was created");
            }
        }

        private List<UserDto> GetUsers()
        {
            var users = UserService.GetUsers();

            Console.WriteLine("Created users:");
            foreach (var user in users)
            {
                
                Console.WriteLine(user.Name);
            }

            return users;
        }
    }
}