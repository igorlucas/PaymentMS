using PaymentMS.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GetnetProvider.Models
{
    public class GetnetAddress
    {
        [JsonPropertyName("street")]
        [StringLength(60)]
        public string Street { get; set; }

        [JsonPropertyName("number")]
        [StringLength(10)]
        public string Number { get; set; }

        [JsonPropertyName("complement")]
        [StringLength(60)]
        public string Complement { get; set; }

        [JsonPropertyName("district")]
        [StringLength(40)]
        public string District { get; set; }

        [JsonPropertyName("city")]
        [StringLength(40)]
        public string City { get; set; }

        [JsonPropertyName("state")]
        [StringLength(20)]
        public string State { get; set; }

        [JsonPropertyName("country")]
        [StringLength(20)]
        public string Country { get; set; }

        [JsonPropertyName("postal_code")]
        [StringLength(8)]
        public string PostalCode { get; set; }

        public GetnetAddress() { }

        public GetnetAddress(Address address)
        {
            Street = address.Street;
            Number = address.Number;
            Complement = address.Complement;
            District = address.District;
            City = address.City;
            State = address.State;
            Country = address.Country;
            PostalCode = address.PostalCode;
        }
    }
}