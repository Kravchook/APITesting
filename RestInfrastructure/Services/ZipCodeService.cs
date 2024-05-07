using APITesting.RestInfrastructure.ApiClients;
using RestSharp;
using System.Net;

namespace APITesting.RestInfrastructure.Services
{
    public class ZipCodeService
    {
        readonly ApiReadRestClient _apiReadRestClientInstance = ApiReadRestClient.Instance();
        readonly ApiWriteRestClient _apiWriteRestClientInstance = ApiWriteRestClient.Instance();

        public List<string> GetZipCodes(HttpStatusCode expectedHttpStatusCode)
        {
            var request = _apiReadRestClientInstance.CreateRestRequest("http://localhost:49000/zip-codes", Method.Get);
            var response = _apiReadRestClientInstance.ExecuteRequest<List<string>>(request, expectedHttpStatusCode);

            return response.Data;
        }

        public List<string> PostZipCodes(List<string> zipCodes, HttpStatusCode expectedHttpStatusCode)
        {
            var request = _apiWriteRestClientInstance.CreateRestRequest("http://localhost:49000/zip-codes/expand", Method.Post);
            request.AddJsonBody(zipCodes);
            //    new
            //{
            //    s1="code1",
            //    s2="code2"
            //});

            var response = _apiWriteRestClientInstance.ExecuteRequest<List<string>>(request, expectedHttpStatusCode);

            return response.Data;
        }
    }
}
