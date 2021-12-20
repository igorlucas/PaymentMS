using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GetnetProvider.Commands.CommandResponses
{
    public class CreateCustomerResponse : CreateCustomerCommandBase
    {
        [JsonPropertyName("status")]
        [StringLength(20)]
        public string Status { get; set; }

        public CreateCustomerResponse() { }
    }
}