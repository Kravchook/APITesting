using APITask10.RestInfrastructure.DataModels;
using RestSharp;
using RestSharp.Authenticators;

namespace APITask10.RestInfrastructure.Authenticators
{
    internal class ApiWriteAuthenticator : AuthenticatorBase
    {
        readonly string _baseUrl;
        readonly string _clientId;
        readonly string _clientSecret;

        public ApiWriteAuthenticator() : base("")
        {
            _baseUrl = "http://localhost:49000";
            _clientId = "0oa157tvtugfFXEhU4x7";
            _clientSecret = "X7eBCXqlFC7x-mjxG5H91IRv_Bqe1oq7ZwXNA8aq";
        }

        public ApiWriteAuthenticator(string baseUrl, string clientId, string clientSecret) : base("")
        {
            _baseUrl = baseUrl;
            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        protected override async ValueTask<Parameter> GetAuthenticationParameter(string accessToken)
        {
            Token = string.IsNullOrEmpty(Token) ? await GetWriteToken() : Token;

            return new HeaderParameter(KnownHeaders.Authorization, Token);
        }

        private async Task<string> GetWriteToken()
        {
            var restOptions = new RestClientOptions(_baseUrl)
            {
                Authenticator = new HttpBasicAuthenticator(_clientId, _clientSecret)
            };

            using var client = new RestClient(restOptions);

            var request = new RestRequest("oauth/token", Method.Post);
            request.AddParameter("grant_type", "client_credentials");
            request.AddParameter("scope", "write");
            var response = await client.PostAsync<TokenResponse>(request);

            return $" {response!.TokenType} {response.AccessToken} ";
        }
    }
}
