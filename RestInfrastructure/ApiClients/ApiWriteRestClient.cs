using System.Net;
using APITesting.RestInfrastructure.Authenticators;
using APITesting.Settings.ConfigClasses;
using NLog;
using RestSharp;

namespace APITesting.RestInfrastructure.ApiClients
{
    public class ApiWriteRestClient
    {
        private static ApiWriteRestClient instance;
        private static readonly object _locker = new object();

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public RestClient RestClient;

        private ApiWriteRestClient()
        {
            var restOptions = new RestClientOptions($"{Configurations.AppSettings.BaseUrl}/oauth/token")
            {
                Authenticator = new ApiWriteAuthenticator()
            };
            RestClient = new RestClient(restOptions);
        }

        public static ApiWriteRestClient Instance()
        {
            if (instance == null)
            {
                lock (_locker)
                {
                    if (instance == null)
                        instance = new ApiWriteRestClient();
                }
            }

            return instance;
        }

        public virtual RestResponse<T> ExecuteRequest<T>(RestRequest request, HttpStatusCode expectedHttpStatusCode = HttpStatusCode.OK) where T : new()
        {
            var response = RestClient.ExecuteAsync<T>(request).Result;
            logger.Info($"Response content:{response.Content}");

            Assert.That(response.StatusCode, Is.EqualTo(expectedHttpStatusCode), "StatusCode not as expected");

            return response;
        }

        public RestRequest CreateRestRequest(string resource, Method method)
        {
            var restRequest = new RestRequest(resource, method);

            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddHeader("Accept", "*/*");
            restRequest.AddHeader("Accept-Encoding", "gzip, deflate, br");

            return restRequest;
        }

        public RestRequest UploadFileRestRequest(string resource, Method method)
        {
            var restRequest = new RestRequest(resource, method);
            
            return restRequest;
        }
    }
}
