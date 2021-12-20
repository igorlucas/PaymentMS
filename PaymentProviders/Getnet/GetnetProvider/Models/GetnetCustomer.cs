using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GetnetProvider.Models
{   
    public class GetnetCustomer
    {
        [JsonPropertyName("customer_id")]
        public string CustomerId { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("document_type")]
        public string DocumentType { get; set; }

        [JsonPropertyName("document_number")]
        public string DocumentNumber { get; set; }

        [JsonPropertyName("phone_number")]
        [Phone]
        public string Phone { get; set; }

        [JsonPropertyName("celphone_number")]
        [Phone]
        public string Celphone { get; set; }

        [JsonPropertyName("email")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
