using APITesting.RestInfrastructure;
using APITesting.RestInfrastructure.DataModels;
using APITesting.RestInfrastructure.Enums;
using APITesting.RestInfrastructure.Services;
using System.Net;
using RestSharp;
using Allure.NUnit.Attributes;
using Allure.NUnit;
using NLog;

namespace APITesting.Tests
{
    [AllureNUnit]
    [AllureDisplayIgnored]
    [AllureSubSuite("User Controller Tests")]
    public class UserControllerTests
    {
        private UserService UserService = new UserService();
        private ZipCodeService ZipCodeService = new ZipCodeService();

        private static Logger logger = LogManager.GetCurrentClassLogger();

        [Test]
        [Description("Task40 - Scenario 1")]
        public void ShowUsersTest()
        {
            GetUsers();
        }

        [Test]
        [Description("Task40 - Scenario 2")]
        public void ShowUsersOlderThanTest()
        {
            GetUsers(olderThan: 19);
        }

        [Test]
        [Description("Task40 - Scenario 3")]
        public void ShowUsersYoungerThanTest()
        {
            GetUsers(youngerThan: 19);
        }

        [Test]
        [Description("Task40 - Scenario 4")]
        public void ShowUsersOfSpecificGenderTest()
        {
            GetUsers(sex: Sex.Female.StringValue());
        }

        [Test]
        [Description("Task30 - Scenario 1")]
        [AllureIssue("A duplicate user is being added to the application in case if user to add has the same name and sex as existing user in the system")]
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
                Age = 52,
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
        [AllureIssue("If create a duplicate for the user with only required fields filled (Name, Sex) without specifying ZipCode getting error")]
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

        [Test]
        [Description("Task50 - Scenario 1")]
        public void UpdateUser_AllFieldsFilledTest_MethodPut()
        {
            UserDto userNewValues = new UserDto
            {
                Age = 35,
                Name = "Bob Milanich",
                Sex = Sex.Male.StringValue(),
                ZipCode = "12345"
            };

            UserDto userToChange = new UserDto
            {
                Age = 20,
                Name = "Bob Dilan",
                Sex = Sex.Male.StringValue(),
                ZipCode = "12345"
            };

            UpdateUserDto updateUserDto = new UpdateUserDto
            {
                UserNewValues = userNewValues,
                UserToChange = userToChange
            };

            try
            {
                UserService.UpdateUser(updateUserDto, Method.Put, HttpStatusCode.OK);
            }
            finally
            {
                var users = GetUsers();
                Assert.That(users.Find(u => 
                    u.Name.Equals(userNewValues.Name) && u.Sex.Equals(userNewValues.Sex) && u.Age.Equals(userNewValues.Age) 
                    && u.ZipCode.Equals(userNewValues.ZipCode)), Is.Not.Null, $"User {userToChange.Name} was not updated");
            }
        }

        [Test]
        [Description("Task50 - Scenario 1")]
        public void UpdateUser_AllFieldsFilledTest_MethodPatch()
        {
            UserDto userNewValues = new UserDto
            {
                Age = 20,
                Name = "Bob Dilan",
                Sex = Sex.Male.StringValue(),
                ZipCode = "12345"
            };

            UserDto userToChange = new UserDto
            {
                Age = 35,
                Name = "Bob Milanich",
                Sex = Sex.Male.StringValue(),
                ZipCode = "12345"
            };

            UpdateUserDto updateUserDto = new UpdateUserDto
            {
                UserNewValues = userNewValues,
                UserToChange = userToChange
            };

            try
            {
                UserService.UpdateUser(updateUserDto, Method.Patch, HttpStatusCode.OK);
            }
            finally
            {
                var users = GetUsers();
                Assert.That(users.Find(u =>
                    u.Name.Equals(userNewValues.Name) && u.Sex.Equals(userNewValues.Sex) && u.Age.Equals(userNewValues.Age)
                    && u.ZipCode.Equals(userNewValues.ZipCode)), Is.Not.Null, $"User {userToChange.Name} was not updated");
            }
        }

        [Test]
        [Description("Task50 - Scenario 2")]
        [AllureIssue("The userToChange is being deleted from the application in case of incorrect new zip code")]
        public void UpdateUser_IncorrectZipCodeTest_MethodPut()
        {
            UserDto userNewValues = new UserDto
            {
                Age = 35,
                Name = "Bob Milanich",
                Sex = Sex.Male.StringValue(),
                ZipCode = "code10"
            };

            UserDto userToChange = new UserDto
            {
                Age = 20,
                Name = "Bob Dilan",
                Sex = Sex.Male.StringValue(),
                ZipCode = "12345"
            };

            UpdateUserDto updateUserDto = new UpdateUserDto
            {
                UserNewValues = userNewValues,
                UserToChange = userToChange
            };

            try
            {
                UserService.UpdateUser(updateUserDto, Method.Put, HttpStatusCode.FailedDependency);
            }
            finally
            {
                // BUG: The userToChange is being deleted ?! from the application in case of incorrect new zip code
                var users = GetUsers();
                Assert.That(users.Find(u =>
                    u.Name.Equals(userNewValues.Name) && u.Sex.Equals(userNewValues.Sex) && u.Age.Equals(userNewValues.Age)
                    && u.ZipCode.Equals(userNewValues.ZipCode)), Is.Null, $"User {userToChange.Name} was updated");
            }
        }

        [Test]
        [Description("Task50 - Scenario 2")]
        [AllureIssue("The userToChange is being deleted from the application in case of incorrect new zip code")]
        public void UpdateUser_IncorrectZipCodeTest_MethodPatch()
        {
            UserDto userNewValues = new UserDto
            {
                Age = 21,
                Name = "Janna Darkova",
                Sex = Sex.Female.StringValue(),
                ZipCode = "code11"
            };

            UserDto userToChange = new UserDto
            {
                Name = "Janna Dark",
                Sex = Sex.Female.StringValue()
            };

            UpdateUserDto updateUserDto = new UpdateUserDto
            {
                UserNewValues = userNewValues,
                UserToChange = userToChange
            };

            try
            {
                UserService.UpdateUser(updateUserDto, Method.Put, HttpStatusCode.FailedDependency);
            }
            finally
            {
                // BUG: The userToChange is being deleted ?! from the application in case of incorrect new zip code
                var users = GetUsers();
                Assert.That(users.Find(u =>
                    u.Name.Equals(userNewValues.Name) && u.Sex.Equals(userNewValues.Sex) && u.Age.Equals(userNewValues.Age)
                    && u.ZipCode.Equals(userNewValues.ZipCode)), Is.Null, $"User {userToChange.Name} was updated");
            }
        }

        [Test]
        [Description("Task50 - Scenario 3")]
        [AllureIssue("The userToChange is being deleted from the application in case of new required values are missing")]
        public void UpdateUser_RequiredFieldsAreMissingTest_MethodPut()
        {
            UserDto userNewValues = new UserDto
            {
                Age = 35,
                ZipCode = "code2"
            };

            UserDto userToChange = new UserDto
            {
                Age = 20,
                Name = "Bob Dilan",
                Sex = Sex.Male.StringValue(),
                ZipCode = "code2"
            };

            UpdateUserDto updateUserDto = new UpdateUserDto
            {
                UserNewValues = userNewValues,
                UserToChange = userToChange
            };

            try
            {
                UserService.UpdateUser(updateUserDto, Method.Put, HttpStatusCode.Conflict);
            }
            finally
            {
                // BUG: The userToChange is being deleted ?! from the application in case of new required values are missing
                var users = GetUsers();
                Assert.That(users.Find(u =>
                    u.Name.Equals(userNewValues.Name) && u.Sex.Equals(userNewValues.Sex) && u.Age.Equals(userNewValues.Age)
                    && u.ZipCode.Equals(userNewValues.ZipCode)), Is.Null, $"User {userToChange.Name} was updated");
            }
        }

        [Test]
        [Description("Task50 - Scenario 3")]
        [AllureIssue("The userToChange is being deleted from the application in case of new required values are missing")]
        public void UpdateUser_RequiredFieldsAreMissingTest_MethodPatch()
        {
            UserDto userNewValues = new UserDto
            {
                Age = 13
            };

            UserDto userToChange = new UserDto
            {
                Age = 0,
                Name = "Janna Dark",
                Sex = Sex.Female.StringValue()
            };

            UpdateUserDto updateUserDto = new UpdateUserDto
            {
                UserNewValues = userNewValues,
                UserToChange = userToChange
            };

            try
            {
                UserService.UpdateUser(updateUserDto, Method.Patch, HttpStatusCode.Conflict);
            }
            finally
            {
                // BUG: The userToChange is being deleted ?! from the application in case of new required values are missing
                var users = GetUsers();
                Assert.That(users.Find(u =>
                    u.Name.Equals(userNewValues.Name) && u.Sex.Equals(userNewValues.Sex) && u.Age.Equals(userNewValues.Age)
                    && u.ZipCode.Equals(userNewValues.ZipCode)), Is.Null, $"User {userToChange.Name} was updated");
            }
        }

        [Test]
        [Description("Task60 - Scenario 1")]
        [AllureIssue("Zip code is not returned to the list of available zip codes")]
        public void DeleteUser_AllFieldsFilledTest()
        {
            UserDto userToDelete = new UserDto
            {
                Age = 20,
                Name = "Bob Dilan",
                Sex = Sex.Male.StringValue(),
                ZipCode = "12345"
            };
            
            try
            {
                UserService.DeleteUser(userToDelete, HttpStatusCode.NoContent);
            }
            finally
            {
                var users = GetUsers();
                Assert.That(users.Find(u =>
                    u.Name.Equals(userToDelete.Name) && u.Sex.Equals(userToDelete.Sex) && u.Age.Equals(userToDelete.Age)
                    && u.ZipCode.Equals(userToDelete.ZipCode)), Is.Null, $"User {userToDelete.Name} was not deleted");

                //BUG: Zip code is NOT returned to the list of available zip codes 
                var zipCodes = ZipCodeService.GetZipCodes(HttpStatusCode.Created);
                Assert.That(zipCodes, Contains.Item(userToDelete.ZipCode), 
                    $"Zip code {userToDelete.ZipCode} was not added to the available zip codes list");

                Console.WriteLine("Available Zip codes:");
                foreach (var code in zipCodes)
                {
                    Console.WriteLine(code);
                }
            }
        }

        [Test]
        [Description("Task60 - Scenario 2")]
        [AllureIssue("In case if existing user has a Zip code specified - that user is not being deleted and zip code is not returned to the list of available zip codes")]
        public void DeleteUser_OnlyRequiredFieldsFilledTest()
        {
            UserDto userToDelete = new UserDto
            {
                //Name = "Bob Dilan",
                //Sex = Sex.Male.StringValue()
                Name = "Janna Dark",
                Sex = Sex.Female.StringValue()
            };

            try
            {
                UserService.DeleteUser(userToDelete, HttpStatusCode.NoContent);
            }
            finally
            {
                var users = GetUsers();
                Assert.That(users.Find(u => u.Name.Equals(userToDelete.Name) && u.Sex.Equals(userToDelete.Sex)),
                    Is.Null, $"User {userToDelete.Name} was not deleted");

                //BUG: In case if existing user has a Zip code specified - that user is NOT being deleted and zip code is not returned to the list
                //of available zip codes 
                //NOTE: In case if existing user does not have a Zip code specified - OK, that user is being deleted
                var zipCodes = ZipCodeService.GetZipCodes(HttpStatusCode.Created);
                Assert.That(zipCodes, Contains.Item(userToDelete.ZipCode),
                    $"Zip code {userToDelete.ZipCode} was not added to the available zip codes list");

                Console.WriteLine("Available Zip codes:");
                foreach (var code in zipCodes)
                {
                    Console.WriteLine(code);
                }
            }
        }

        [Test]
        [Description("Task60 - Scenario 3")]
        public void DeleteUser_AnyOfRequiredFieldsIsMissingTest()
        {
            UserDto userToDelete = new UserDto
            {
                Name = "Bob Dilan"
            };

            try
            {
                UserService.DeleteUser(userToDelete, HttpStatusCode.Conflict);
            }
            finally
            {
                var users = GetUsers();
                Assert.That(users.Find(u => u.Name.Equals(userToDelete.Name)),
                    Is.Not.Null, $"User {userToDelete.Name} was deleted");
            }
        }

        [Test]
        [Description("Task70 - Scenario 1")]
        [AllureIssue("If run the test for the second time it should fail as specified zip codes become unavailable")]
        public void UploadUsers_FileContainsUsersTest()
        {
            var path = Path.Combine(TestContext.CurrentContext.TestDirectory, "Settings\\Users\\users.json");

            var users = GetUsers();
            var userNames = users.Select(u => u.Name);

            try
            {
                //BUG: If run the test for the second time it should fail as specified zip codes become unavailable (this case actually comes from scenario 2)
                UserService.UploadUsers(path, HttpStatusCode.Created);
            }
            finally
            {
                var newUsers = GetUsers();
                var newUserNames = newUsers.Select(u => u.Name);

                Assert.That(newUserNames, Is.Not.EquivalentTo(userNames), $"Users are not replaced with users from file");
            }
        }

        [Test]
        [Description("Task70 - Scenario 2")]
        [AllureIssue("Users with incorrect (unavailable) zip code can be uploaded from file and replace existing users")]
        public void UploadUsers_UserHasIncorrectZipCodeTest()
        {
            var path = Path.Combine(TestContext.CurrentContext.TestDirectory, "Settings\\Users\\users.json");

            var users = GetUsers();
            var userNames = users.Select(u => u.Name);

            try
            {
                UserService.UploadUsers(path, HttpStatusCode.FailedDependency);
            }
            finally
            {
                var newUsers = GetUsers();
                var newUserNames = newUsers.Select(u => u.Name);

                //BUG: Users with incorrect (unavailable) zip code can be uploaded from file and replace existing users
                Assert.That(newUserNames, Is.EquivalentTo(userNames), $"New users are uploaded from file");
            }
        }

        [Test]
        [Description("Task70 - Scenario 3")]
        [AllureIssue("Get incorrect response code in case if any of required fields are missing in the file")]
        public void UploadUsers_UserHasMissedRequiredFieldTest()
        {
            var path = Path.Combine(TestContext.CurrentContext.TestDirectory, "Settings\\Users\\users.json");

            var users = GetUsers();
            var userNames = users.Select(u => u.Name);

            try
            {
                //BUG: Got 400 response code (Bad Request) in case if any of required fields are missing in the file
                UserService.UploadUsers(path, HttpStatusCode.Conflict);
            }
            finally
            {
                var newUsers = GetUsers();
                var newUserNames = newUsers.Select(u => u.Name);

                Assert.That(newUserNames, Is.EquivalentTo(userNames), $"New users are uploaded from file");
            }
        }

        private List<UserDto> GetUsers(string sex = "", int olderThan = 0, int youngerThan = 0)
        {
            var users = UserService.GetUsers(sex, olderThan, youngerThan);

            logger.Info("Created users:");
            
            foreach (var user in users)
            {
                logger.Info($"{user.Age}, {user.Name}, {user.Sex}, {user.ZipCode}");
            }

            return users;
        }
    }
}