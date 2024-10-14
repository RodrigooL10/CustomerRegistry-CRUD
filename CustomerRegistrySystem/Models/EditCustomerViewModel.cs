using CustomerRegistrySystem.Models.Domain;

namespace CustomerRegistrySystem.Models
{

    public class EditCustomerViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        // Lista para suportar múltiplos endereços
        public List<EditAddressViewModel> Addresses { get; set; }
    }

    public class EditAddressViewModel
    {
        public Guid Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string CEP { get; set; }
        public string Complement { get; set; }
        public AddressType Type { get; set; }
    }
}
