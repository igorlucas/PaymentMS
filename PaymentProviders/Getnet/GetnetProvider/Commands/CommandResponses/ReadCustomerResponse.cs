using GetnetProvider.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GetnetProvider.Commands.CommandResponses
{
    public class ReadCustomerResponse : GetnetCustomer
    {
        [JsonPropertyName("seller_id")]
        public string SellerId { get; set; }

        [JsonPropertyName("birth_date")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [JsonPropertyName("observation")]
        public string Observation { get; set; }

        [JsonPropertyName("address")]
        public GetnetAddress Address { get; set; }

    }
}
