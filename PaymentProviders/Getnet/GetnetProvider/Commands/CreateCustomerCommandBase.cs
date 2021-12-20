using GetnetProvider.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GetnetProvider.Commands
{
    public abstract class CreateCustomerCommandBase
    {
        [JsonPropertyName("seller_id")]
        [Required]
        [StringLength(36)]
        public string SellerId { get; set; }

        [JsonPropertyName("customer_id")]
        [StringLength(100)]
        public string CustomerId { get; set; }

        [JsonPropertyName("first_name")]
        [Required]
        [StringLength(40)]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        [Required]
        [StringLength(80)]
        public string LastName { get; set; }

        [JsonPropertyName("document_type")]
        [Required]
        public string DocumentType { get; set; }//CPF-CNPJ

        [JsonPropertyName("document_number")]
        [Required]
        [StringLength(15, MinimumLength = 11)]
        public string DocumentNumber { get; set; }

        [JsonPropertyName("birth_date")]
        [StringLength(10)]
        public string BirthDate { get; set; }

        [JsonPropertyName("phone_number")]
        [StringLength(15)]
        [Phone]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("celphone_number")]
        [StringLength(15)]
        [Phone]
        public string CelphoneNumber { get; set; }

        [JsonPropertyName("email")]
        [EmailAddress]
        public string Email { get; set; }

        [JsonPropertyName("observation")]
        public string Observation { get; set; }

        [JsonPropertyName("address")]
        public GetnetAddress Address { get; set; }
    }
}
