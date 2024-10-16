using CustomerRegistrySystem.Domain;

namespace CustomerRegistrySystem.Application.ViewModels
{

    public class EditCustomerViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CPF { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Genders Gender { get; set; }
        public List<EditAddressViewModel> Addresses { get; set; }
    }

    public class EditAddressViewModel
    {
        public Guid Id { get; set; }
        public string Street { get; set; }
        public int Number { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string CEP { get; set; }
        public AddressType Type { get; set; }
        public string Complement { get; set; }
        public Guid CustomerId { get; set; }
    }
}
