using APITesting.RestInfrastructure.ApiClients;
using APITesting.RestInfrastructure.DataModels;
using APITesting.Settings.ConfigClasses;
using RestSharp;
using System.Net;

namespace APITesting.RestInfrastructure.Services
{
    public class UserService
    {
        readonly ApiReadRestClient _apiReadRestClientInstance = ApiReadRestClient.Instance();
        readonly ApiWriteRestClient _apiWriteRestClientInstance = ApiWriteRestClient.Instance();

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

            var response = _apiReadRestClientInstance.ExecuteRequest<List<UserDto>>(request);

            return response.Data;
        }

        public List<string> CreateUser(UserDto user, HttpStatusCode expectedHttpStatusCode)
        {
            var request = _apiWriteRestClientInstance.CreateRestRequest($"{Configurations.AppSettings.BaseUrl}/users", Method.Post);
            request.AddJsonBody(user);

            var response = _apiWriteRestClientInstance.ExecuteRequest<List<string>>(request, expectedHttpStatusCode);

            return response.Data;
        }

        public List<string> UpdateUser(UpdateUserDto updateUserDto, Method method, HttpStatusCode expectedHttpStatusCode)
        {
            var request = _apiWriteRestClientInstance.CreateRestRequest($"{Configurations.AppSettings.BaseUrl}/users", method);
            request.AddJsonBody(updateUserDto);

            var response = _apiWriteRestClientInstance.ExecuteRequest<List<string>>(request, expectedHttpStatusCode);

            return response.Data;
        }

        public List<string> DeleteUser(UserDto user, HttpStatusCode expectedHttpStatusCode)
        {
            var request = _apiWriteRestClientInstance.CreateRestRequest($"{Configurations.AppSettings.BaseUrl}/users", Method.Delete);
            request.AddJsonBody(user);

            var response = _apiWriteRestClientInstance.ExecuteRequest<List<string>>(request, expectedHttpStatusCode);

            return response.Data;
        }

        public List<string> UploadUsers(string path, HttpStatusCode expectedHttpStatusCode)
        {
            var request = _apiWriteRestClientInstance.UploadFileRestRequest($"{Configurations.AppSettings.BaseUrl}/users/upload", Method.Post);
            request.AddFile("file", path);

            var response = _apiWriteRestClientInstance.ExecuteRequest<List<string>>(request, expectedHttpStatusCode);

            return response.Data;
        }
    }
}
