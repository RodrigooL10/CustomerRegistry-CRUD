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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configura a chave primária para Customer
            modelBuilder.Entity<Customer>()
                .HasKey(c => c.Id);

            // Configura a chave primária para Address
            modelBuilder.Entity<Address>()
                .HasKey(a => a.Id);

            // Configura o relacionamento entre Customer e Address
            modelBuilder.Entity<Address>()
                .HasOne(a => a.Customer)
                .WithMany(c => c.Addresses)
                .HasForeignKey(a => a.CustomerId);
        }
    }
}
