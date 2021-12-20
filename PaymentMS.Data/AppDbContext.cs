using Microsoft.EntityFrameworkCore;
using PaymentMS.Domain.Entities;

namespace PaymentMS.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            //Configuration.LazyLoadingEnabled = false; 
            //Configuration.ProxyCreationEnabled = false;
            //Database.Migrate();
        }
    }
}