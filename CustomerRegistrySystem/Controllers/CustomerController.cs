using CustomerRegistrySystem.Data;
using CustomerRegistrySystem.Models;
using CustomerRegistrySystem.Models.Domain;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CustomerRegistrySystem.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CustomerRegistryDBContext customerRegistryDBContext;

        public CustomerController(CustomerRegistryDBContext customerRegistryDBContext)
        {
            this.customerRegistryDBContext = customerRegistryDBContext;
        }

        [HttpGet]
        public async Task<IActionResult> Show()
        {
            var customers = await customerRegistryDBContext.Customers
                .Include(c => c.Addresses)
                .ToListAsync();

            return View(customers);
        }


        //Adicionar Cliente
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCustomerViewModel addCustomerRequest)
        {
            var customer = new Customer()
            {
                Id = Guid.NewGuid(),
                Name = addCustomerRequest.Name,
                Email = addCustomerRequest.Email,
                Phone = addCustomerRequest.Phone,
                
                //adicionando Endereços
                Addresses = addCustomerRequest.Addresses.Select(address => new Address
                {
                    Id = Guid.NewGuid(),
                    Street = address.Street,
                    City = address.City,
                    State = address.State,
                    CEP = address.CEP,
                    Complement = address.Complement,
                    Type = address.Type,
                }).ToList()
            };

            await customerRegistryDBContext.Customers.AddAsync(customer);
            await customerRegistryDBContext.SaveChangesAsync();

            return RedirectToAction("Show");
        }




        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            // Busca o cliente pelo ID
            var customer = await customerRegistryDBContext.Customers
                .Include(c => c.Addresses) // Incluindo os endereços, se necessário
                .FirstOrDefaultAsync(x => x.Id == id);

            if (customer != null)
            {
                // Criação do ViewModel para atualização
                var viewModel = new EditCustomerViewModel()
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Email = customer.Email,
                    Phone = customer.Phone,
                    Addresses = customer.Addresses.Select(a => new EditAddressViewModel
                    {
                        Id = a.Id,
                        Street = a.Street,
                        City = a.City,
                        State = a.State,
                        CEP = a.CEP,
                        Type = a.Type
                    }).ToList() // Preenchendo a lista de endereços
                };

                return View("Edit", viewModel); // Retorna a view de edição com o ViewModel
            }

            return RedirectToAction("Show");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditCustomerViewModel viewModel)
        {
            // Busca o cliente pelo ID
            var customer = await customerRegistryDBContext.Customers
                .Include(c => c.Addresses) // Inclui endereços para atualização
                .FirstOrDefaultAsync(x => x.Id == viewModel.Id);

            if (customer != null)
            {
                // Atualiza os dados do cliente
                customer.Name = viewModel.Name;
                customer.Email = viewModel.Email;
                customer.Phone = viewModel.Phone;

                // Atualiza os endereços
                foreach (var addressModel in viewModel.Addresses)
                {
                    var address = customer.Addresses.FirstOrDefault(a => a.Id == addressModel.Id);
                    if (address != null)
                    {
                        address.Street = addressModel.Street;
                        address.City = addressModel.City;
                        address.State = addressModel.State;
                        address.CEP = addressModel.CEP;
                        address.Type = addressModel.Type;
                    }
                }

                await customerRegistryDBContext.SaveChangesAsync(); // Salva as alterações no banco de dados

                return RedirectToAction("Show"); // Redireciona para a lista de clientes
            }

            return RedirectToAction("Show"); // Redireciona em caso de erro
        }


    }
}
