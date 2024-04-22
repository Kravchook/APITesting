using System.Text.Json.Serialization;

namespace Task10.RestInfrastructure.DataModels
{
    public record UserDto
    {
        [JsonPropertyName("age")]
        public int Age { get; init; }
        [JsonPropertyName("name")]
        public string Name { get; init; }
        [JsonPropertyName("sex")]
        public string Sex { get; init; }
        [JsonPropertyName("zipCode")]
        public string ZipCode { get; init; }
    }
}
