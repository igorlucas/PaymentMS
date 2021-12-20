using GetnetProvider.Models;
using GetnetProvider.Services;
using Microsoft.EntityFrameworkCore;
using PaymentMS.Data;
using PaymentMS.Domain.Contracts.Services;
using PaymentMS.Domain.Entities;

namespace PaymentMS.API.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext _db;
        private readonly IGetnetCustomerService _getnetCustomerService;
        private readonly GetnetSettings _getnetSettings;

        public CustomerService(AppDbContext db)
        {
            _db = db;
        }

        public Customer Create(Customer customer)
        {
            try
            {
                _db.Customers.Add(customer);
                _db.SaveChanges();
                return customer;    
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<Customer> ReadAll()
            => _db.Customers.Include(c => c.Addresses).Include(c => c.Contacts).ToList();

        public Customer ReadById(Guid id)
            => _db.Customers.Include(c => c.Addresses).Include(c => c.Contacts).FirstOrDefault(c => c.Id == id);

        public Customer Update(Customer customer)
        {
            try
            {
                _db.Customers.Update(customer);
                _db.SaveChanges();
                return customer;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(Customer customer)
        {
            try
            {
                _db.Customers.Remove(customer);
                _db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}