using System.Text.Json.Serialization;

namespace Bimodal.Test.Token
{
    public class JwtAuthResult
    {
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }

        [JsonPropertyName("expires")]
        public int ExpiresIn { get; set; }
    }
}
