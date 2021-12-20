using GetnetProvider.Models;
using PaymentMS.Domain.Entities;
using PaymentMS.Domain.Entities.Enums;

namespace GetnetProvider.Commands.CommandRequests
{
    public class CreateCustomerRequest : CreateCustomerCommandBase
    {
        public CreateCustomerRequest(string sellerId, string observation, Customer customer)
        {
            var names = customer.FullName.Split(" ");
            var firstName = names[0];
            var lastName = String.Join(" ", names.Where((source, index) => index != 0));
            var phone = customer.Contacts.FirstOrDefault(c => c.ContactType == ContactType.Phone);
            var cellPhone = customer.Contacts.FirstOrDefault(c => c.ContactType == ContactType.CellPhone);
            var email = customer.Contacts.FirstOrDefault(c => c.ContactType == ContactType.Email);
            var address = customer.Addresses.FirstOrDefault();

            SellerId = sellerId;
            CustomerId = customer.Id.ToString().ToUpper();
            FirstName = firstName;
            LastName = lastName;
            DocumentType = customer.DocumentType.ToString();
            DocumentNumber = customer.DocumentValue;
            BirthDate = customer.BirthDate.Value.ToString("dd/MM/yyyy");
            PhoneNumber = phone != null ? phone.ContactValue : null;
            CelphoneNumber = cellPhone != null ? cellPhone.ContactValue : null;
            Email = email != null ? email.ContactValue : null;
            Observation = observation;
            Address = new GetnetAddress(address);
        }

        public CreateCustomerRequest(string sellerId, string customerId, string firstName, string lastName, string documentType, string documentNumber, DateTime birthDate, string phoneNumber, string celphoneNumber, string email, string observation, Address address)
        {
            SellerId = sellerId;
            CustomerId = customerId;
            FirstName = firstName;
            LastName = lastName;
            DocumentType = documentType;
            DocumentNumber = documentNumber;
            BirthDate = birthDate.ToString("dd/MM/yyyy");
            PhoneNumber = phoneNumber;
            CelphoneNumber = celphoneNumber;
            Email = email;
            Observation = observation;
            Address = new GetnetAddress(address);
        }
    }
}