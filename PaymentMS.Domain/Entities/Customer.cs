using PaymentMS.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace PaymentMS.Domain.Entities
{
    public class Customer
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public PersonType PersonType { get; set; }

        [Required]
        [StringLength(120)]
        public string FullName { get; set; }

        [Required]
        public DocumentType DocumentType { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 11)]
        public string DocumentValue { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [Required]
        [MinLength(1)]
        public IEnumerable<Contact> Contacts { get; set; }

        [Required]
        [MinLength(1)]
        public IEnumerable<Address> Addresses { get; set; }

        public Customer() => Id = Guid.NewGuid();
        public Customer(PersonType personType, string fullName, DocumentType documentType, string documentValue,
            DateTime? birthDate, Contact[] contacts, Address[] addresses)
        {
            Id = Guid.NewGuid();
            PersonType = personType;
            FullName = fullName;
            DocumentType = documentType;
            DocumentValue = documentValue;
            BirthDate = birthDate;
            Contacts = contacts;
            Addresses = addresses;
        }
    }
}