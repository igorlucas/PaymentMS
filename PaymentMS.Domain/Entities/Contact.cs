using PaymentMS.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace PaymentMS.Domain.Entities
{
    public class Contact
    {
        public Guid Id { get; set; }
        public ContactType ContactType { get; set; }

        [Required]
        //[StringLength(15)]
        public string ContactValue { get; set; }

        public Contact() => Id = Guid.NewGuid();
        public Contact(ContactType contactType, string contactValue)
        {
            Id = Guid.NewGuid();
            ContactType = contactType;
            ContactValue = contactValue;
        }
    }
}