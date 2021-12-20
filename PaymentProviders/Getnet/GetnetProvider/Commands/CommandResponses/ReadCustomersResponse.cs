using GetnetProvider.Models;
using System.Text.Json.Serialization;

namespace GetnetProvider.Commands.CommandResponses
{
    public class ReadCustomersResponse  
    {
        [JsonPropertyName("customers")]
        public IEnumerable<GetnetCustomer> Customers { get; set; }

        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }
    }
}
