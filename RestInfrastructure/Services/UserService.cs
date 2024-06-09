using APITesting.RestInfrastructure.ApiClients;
using APITesting.RestInfrastructure.DataModels;
using APITesting.Settings.ConfigClasses;
using NLog;
using RestSharp;
using System.Net;

namespace APITesting.RestInfrastructure.Services
{
    public class UserService
    {
        readonly ApiReadRestClient _apiReadRestClientInstance = ApiReadRestClient.Instance();
        readonly ApiWriteRestClient _apiWriteRestClientInstance = ApiWriteRestClient.Instance();

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public List<UserDto> GetUsers(string sex = "", int olderThan = 0, int youngerThan = 0)
        {
            var request = _apiReadRestClientInstance.CreateRestRequest($"{Configurations.AppSettings.BaseUrl}/users", Method.Get);
            if (sex != string.Empty)
            {
                request.AddQueryParameter("sex", sex);
            }
            if (olderThan != 0)
            {
                request.AddQueryParameter("olderThan", olderThan);
            }
            if (youngerThan != 0)
            {
                request.AddQueryParameter("youngerThan", youngerThan);
            }

            logger.Info($"Request for Get Users --> Resource: '{request.Resource}' Method: {request.Method}");
            var parameters = request.Parameters.GetParameters(ParameterType.QueryString);
            foreach (var parameter in parameters)
            {
                logger.Info($"Filter by parameter: Name '{parameter.Name}' Value: {parameter.Value}");
            }

            var response = _apiReadRestClientInstance.ExecuteRequest<List<UserDto>>(request);

            return response.Data;
        }

        public List<string> CreateUser(UserDto user, HttpStatusCode expectedHttpStatusCode)
        {
            var request = _apiWriteRestClientInstance.CreateRestRequest($"{Configurations.AppSettings.BaseUrl}/users", Method.Post);
            request.AddJsonBody(user);

            logger.Info($"Request for Create User --> Resource: '{request.Resource}' Method: {request.Method}");
            logger.Info($"Age: {user.Age}, Name: {user.Name}, Sex: {user.Sex}, Zip code: {user.ZipCode}");

            var response = _apiWriteRestClientInstance.ExecuteRequest<List<string>>(request, expectedHttpStatusCode);

            return response.Data;
        }

        public List<string> UpdateUser(UpdateUserDto updateUserDto, Method method, HttpStatusCode expectedHttpStatusCode)
        {
            var request = _apiWriteRestClientInstance.CreateRestRequest($"{Configurations.AppSettings.BaseUrl}/users", method);
            request.AddJsonBody(updateUserDto);

            logger.Info($"Request for Update User --> Resource: '{request.Resource}' Method: {request.Method}");
            logger.Info($"New User: Age '{updateUserDto.UserNewValues.Age}', Name '{updateUserDto.UserNewValues.Name}'," +
                        $" Sex '{updateUserDto.UserNewValues.Sex}', Zip code '{updateUserDto.UserNewValues.ZipCode}'");
            logger.Info($"User to change: Age '{updateUserDto.UserToChange.Age}', Name '{updateUserDto.UserToChange.Name}'," +
                        $" Sex '{updateUserDto.UserToChange.Sex}', Zip code '{updateUserDto.UserToChange.ZipCode}'");

            var response = _apiWriteRestClientInstance.ExecuteRequest<List<string>>(request, expectedHttpStatusCode);

            return response.Data;
        }

        public List<string> DeleteUser(UserDto user, HttpStatusCode expectedHttpStatusCode)
        {
            var request = _apiWriteRestClientInstance.CreateRestRequest($"{Configurations.AppSettings.BaseUrl}/users", Method.Delete);
            request.AddJsonBody(user);

            logger.Info($"Request for Delete User --> Resource: '{request.Resource}' Method: {request.Method}");
            logger.Info($"Age: {user.Age}, Name: {user.Name}, Sex: {user.Sex}, Zip code: {user.ZipCode}");

            var response = _apiWriteRestClientInstance.ExecuteRequest<List<string>>(request, expectedHttpStatusCode);

            return response.Data;
        }

        public List<string> UploadUsers(string path, HttpStatusCode expectedHttpStatusCode)
        {
            var request = _apiWriteRestClientInstance.UploadFileRestRequest($"{Configurations.AppSettings.BaseUrl}/users/upload", Method.Post);
            request.AddFile("file", path);

            logger.Info($"Request for Upload Users from file --> Resource: '{request.Resource}' Method: {request.Method} Path: {path}");

            var response = _apiWriteRestClientInstance.ExecuteRequest<List<string>>(request, expectedHttpStatusCode);

            return response.Data;
        }
    }
}
