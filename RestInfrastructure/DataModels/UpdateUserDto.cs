using System.Text.Json.Serialization;

namespace APITesting.RestInfrastructure.DataModels
{
    public record UpdateUserDto
    {
        [JsonPropertyName("userNewValues")]
        public UserDto UserNewValues { get; init; }
        [JsonPropertyName("userToChange")]
        public UserDto UserToChange { get; init; }
    }
}