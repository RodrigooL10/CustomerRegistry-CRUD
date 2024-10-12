using CustomerRegistrySystem.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CustomerRegistrySystem.Data
{
    public class CustomerRegistryDBContext : DbContext
    {
        public CustomerRegistryDBContext(DbContextOptions options) : base(options) 
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }
    }
}
