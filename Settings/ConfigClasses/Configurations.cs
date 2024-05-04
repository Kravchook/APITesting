using APITesting.Settings.UtilHelpers;

namespace APITesting.Settings.ConfigClasses
{
    public static class Configurations
    {
        private static readonly string[] FilePath = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "appsettings*.json");

        public static readonly AppSettings AppSettings = JsonHelper.DeserializeJson<AppSettings>(FilePath);
    }
}