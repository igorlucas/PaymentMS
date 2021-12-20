using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GetnetProvider.Models
{
    internal class AccessToken
    {
        [StringLength(36)]
        [JsonPropertyName("access_token")]
        public string Token { get; set; }

        [JsonPropertyName("token_type")]
        public string Type { get; set; }
    }
}