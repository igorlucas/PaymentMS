using PaymentMS.Domain.Entities;

namespace PaymentMS.Domain.Contracts.Services
{
    public interface ICustomerService
    {
        Customer Create(Customer customer);
        Customer ReadById(Guid id);
        List<Customer> ReadAll();
        Customer Update(Customer customer);
        void Delete(Customer customer);
    }
}