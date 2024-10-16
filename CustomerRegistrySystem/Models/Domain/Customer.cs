using System.Net;

namespace CustomerRegistrySystem.Models.Domain
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
     //   public string CPF { get; set; }
        public ICollection<Address> Addresses { get; set; }

    }


    public class Address
    {
        public Guid Id { get; set; }
        public string Street { get; set; }
       // public string Number { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string CEP { get; set; }
        public AddressType Type { get; set; }
        public string Complement { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
    }

    public enum AddressType
    {
        Fiscal,
        Cobrança,
        Entrega
    }
}
