﻿using RestSharp;
using System.Net;
using APITask10.RestInfrastructure.Authenticators;

namespace APITask10.RestInfrastructure.ApiClients
{
    public class ApiReadRestClient
    {
        private static ApiReadRestClient instance;
        private static readonly object _locker = new object();

        public RestClient restClient;
        private ApiReadRestClient()
        {
            var restOptions = new RestClientOptions("http://localhost:49000/oauth/token")
            {
                Authenticator = new ApiReadAuthenticator()
            };
            restClient = new RestClient(restOptions);
        }

        public static ApiReadRestClient Instance()
        {
            if (instance == null)
            {
                lock (_locker)
                {
                    if (instance == null)
                        instance = new ApiReadRestClient();
                }
            }

            return instance;
        }

        public virtual RestResponse<T> ExecuteRequest<T>(RestRequest request, HttpStatusCode expectedHttpStatusCode = HttpStatusCode.OK) where T : new()
        {
            var response = restClient.ExecuteAsync<T>(request).Result;
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
    }
}
