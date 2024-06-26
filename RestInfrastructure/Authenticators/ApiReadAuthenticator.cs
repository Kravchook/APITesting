using APITesting.RestInfrastructure.DataModels;
using APITesting.Settings.ConfigClasses;
using RestSharp;
using RestSharp.Authenticators;

namespace APITesting.RestInfrastructure.Authenticators
{
    internal class ApiReadAuthenticator : AuthenticatorBase
    {
        readonly string _baseUrl;
        readonly string _clientId;
        readonly string _clientSecret;

        public ApiReadAuthenticator() : base("")
        {
            _baseUrl = Configurations.AppSettings.BaseUrl;
            _clientId = Configurations.AppSettings.ClientId;
            _clientSecret = Configurations.AppSettings.ClientSecret;
        }

        public ApiReadAuthenticator(string baseUrl, string clientId, string clientSecret) : base("")
        {
            _baseUrl = baseUrl;
            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        protected override async ValueTask<Parameter> GetAuthenticationParameter(string accessToken)
        {
            Token = string.IsNullOrEmpty(Token) ? await GetReadToken() : Token;

            return new HeaderParameter(KnownHeaders.Authorization, Token);
        }

        private async Task<string> GetReadToken()
        {
            var restOptions = new RestClientOptions(_baseUrl)
            {
                Authenticator = new HttpBasicAuthenticator(_clientId, _clientSecret)
            };

            using var client = new RestClient(restOptions);

            var request = new RestRequest("oauth/token", Method.Post);
            request.AddParameter("grant_type", "client_credentials");
            request.AddParameter("scope", "read");
            var response = await client.PostAsync<TokenResponse>(request);

            return $" {response!.TokenType} {response.AccessToken} ";
        }
    }
}
