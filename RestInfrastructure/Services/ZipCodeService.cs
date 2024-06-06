using System.Net;
using APITesting.RestInfrastructure.ApiClients;
using APITesting.Settings.ConfigClasses;
using NLog;
using RestSharp;

namespace APITesting.RestInfrastructure.Services
{
    public class ZipCodeService
    {
        readonly ApiReadRestClient _apiReadRestClientInstance = ApiReadRestClient.Instance();
        readonly ApiWriteRestClient _apiWriteRestClientInstance = ApiWriteRestClient.Instance();

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public List<string> GetZipCodes(HttpStatusCode expectedHttpStatusCode)
        {
            var request = _apiReadRestClientInstance.CreateRestRequest($"{Configurations.AppSettings.BaseUrl}/zip-codes", Method.Get);
            logger.Info($"Request for Get Zip Codes --> Resource: '{request.Resource}' Method: {request.Method}");

            var response = _apiReadRestClientInstance.ExecuteRequest<List<string>>(request, expectedHttpStatusCode);

            return response.Data;
        }

        public List<string> PostZipCodes(List<string> zipCodes, HttpStatusCode expectedHttpStatusCode)
        {
            var request = _apiWriteRestClientInstance.CreateRestRequest($"{Configurations.AppSettings.BaseUrl}/zip-codes/expand", Method.Post);
            request.AddJsonBody(zipCodes);

            logger.Info($"Request for Post Zip Codes --> Resource: '{request.Resource}' Method: {request.Method}");

            var response = _apiWriteRestClientInstance.ExecuteRequest<List<string>>(request, expectedHttpStatusCode);

            return response.Data;
        }
    }
}
