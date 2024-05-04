using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;

namespace APITesting.Settings.UtilHelpers
{
    public static class JsonHelper
    {
        private static StreamReader _streamReader;

        public static T DeserializeJson<T>(IEnumerable<string> files)
        {
            var json = ParseJsonFilesToString(files);

            return JsonConvert.DeserializeObject<T>(json);
        }

        private static string ParseJsonFilesToString(IEnumerable<string> files)
        {
            string json = "";
            foreach (var file in files)
            {
                using (
                    _streamReader = new StreamReader(file))
                {
                    json += _streamReader.ReadToEnd();
                }
            }

            return json.Replace("}{", ",");
        }

        public static List<T> FromJson<T>(string json) =>
            JsonConvert.DeserializeObject<List<T>>(json, Settings);

        public static readonly JsonSerializerSettings Settings = new()
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            }
        };
    }
}