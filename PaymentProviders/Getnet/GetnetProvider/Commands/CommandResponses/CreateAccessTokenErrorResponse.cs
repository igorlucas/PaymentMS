using System.Text.Json.Serialization;

namespace GetnetProvider.Commands.CommandResponses
{   
    internal class CreateAccessTokenErrorResponse
    {
        public string Error { get; set; }

        [JsonPropertyName("error_description")]
        public string ErrorDescription { get; set; }
    }
}