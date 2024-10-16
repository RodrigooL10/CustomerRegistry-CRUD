using CustomerRegistrySystem.Models.Domain;

namespace CustomerRegistrySystem.Models
{
    public class AddCustomerViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
       // public string CPF { get; set; }
        public List<AddAddressViewModel> Addresses { get; set; }
    }

    public class AddAddressViewModel
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

    }
}
