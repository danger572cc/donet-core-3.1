using System.Text.Json.Serialization;

namespace Bimodal.Test.Common
{
    public class LoginResultDTO
    {
        [JsonPropertyName("username")]
        public string UserName { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }
    }
}
