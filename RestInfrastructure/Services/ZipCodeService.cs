using System.Net;
using APITesting.RestInfrastructure.ApiClients;
using APITesting.Settings.ConfigClasses;
using RestSharp;

namespace APITesting.RestInfrastructure.Services
{
    public class ZipCodeService
    {
        readonly ApiReadRestClient _apiReadRestClientInstance = ApiReadRestClient.Instance();
        readonly ApiWriteRestClient _apiWriteRestClientInstance = ApiWriteRestClient.Instance();

        public List<string> GetZipCodes(HttpStatusCode expectedHttpStatusCode)
        {
            var request = _apiReadRestClientInstance.CreateRestRequest($"{Configurations.AppSettings.BaseUrl}/zip-codes", Method.Get);
            var response = _apiReadRestClientInstance.ExecuteRequest<List<string>>(request, expectedHttpStatusCode);

            return response.Data;
        }

        public List<string> PostZipCodes(List<string> zipCodes, HttpStatusCode expectedHttpStatusCode)
        {
            var request = _apiWriteRestClientInstance.CreateRestRequest($"{Configurations.AppSettings.BaseUrl}/zip-codes/expand", Method.Post);
            request.AddJsonBody(zipCodes);
            
            var response = _apiWriteRestClientInstance.ExecuteRequest<List<string>>(request, expectedHttpStatusCode);

            return response.Data;
        }
    }
}
