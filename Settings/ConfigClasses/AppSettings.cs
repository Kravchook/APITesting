namespace APITesting.Settings.ConfigClasses
{
    public class AppSettings
    {
        public string BaseUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }


        public AppSettings(string baseUrl, string clientId, string clientSecret)
        {
            BaseUrl = baseUrl;
            ClientId = clientId;
            ClientSecret = clientSecret;
        }
    }
}