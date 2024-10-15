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
                        Complement = a.Complement,
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
            try
            {
                var customer = await customerRegistryDBContext.Customers
                    .Include(c => c.Addresses)
                    .FirstOrDefaultAsync(x => x.Id == viewModel.Id);

                if (customer == null)
                {
                    return NotFound(); // Retorna um erro 404 se o cliente não for encontrado
                }

                // Atualiza os dados do cliente
                customer.Name = viewModel.Name;
                customer.Email = viewModel.Email;
                customer.Phone = viewModel.Phone;

                // Armazena os endereços existentes em um dicionário
                var existingAddresses = customer.Addresses.ToDictionary(a => a.Id);

                // Lista para armazenar novos endereços
                var newAddresses = new List<Address>();

                foreach (var addressModel in viewModel.Addresses)
                {
                    if (addressModel.Id == Guid.Empty) // Verifica se o ID é vazio
                    {
                        // Adiciona novos endereços
                        var newAddress = new Address
                        {
                            Id = Guid.NewGuid(), // Gera um novo GUID
                            Street = addressModel.Street,
                            City = addressModel.City,
                            State = addressModel.State,
                            CEP = addressModel.CEP,
                            Complement = addressModel.Complement,
                            Type = addressModel.Type
                        };

                        newAddresses.Add(newAddress); // Adiciona à lista de novos endereços
                    }
                    else
                    {
                        // Atualiza endereços existentes
                        if (existingAddresses.TryGetValue(addressModel.Id, out var existingAddress))
                        {
                            existingAddress.Street = addressModel.Street;
                            existingAddress.City = addressModel.City;
                            existingAddress.State = addressModel.State;
                            existingAddress.CEP = addressModel.CEP;
                            existingAddress.Complement = addressModel.Complement;
                            existingAddress.Type = addressModel.Type;
                        }
                        else
                        {
                            // Adiciona novo endereço se não estiver no dicionário
                            newAddresses.Add(new Address
                            {
                                Id = addressModel.Id,
                                Street = addressModel.Street,
                                City = addressModel.City,
                                State = addressModel.State,
                                CEP = addressModel.CEP,
                                Complement = addressModel.Complement,
                                Type = addressModel.Type
                            });
                        }
                    }
                }

                // Adiciona novos endereços à lista de endereços do cliente
                foreach (var newAddress in newAddresses)
                {
                    customer.Addresses.Add(newAddress);
                }

                // Remove endereços que não estão mais na lista de endereços do viewModel
                var addressesToRemove = customer.Addresses
                    .Where(a => !viewModel.Addresses.Any(am => am.Id == a.Id))
                    .ToList();

                foreach (var address in addressesToRemove)
                {
                    customer.Addresses.Remove(address);
                    customerRegistryDBContext.Addresses.Remove(address); // Remover também do banco
                }

                await customerRegistryDBContext.SaveChangesAsync();

                return RedirectToAction("Show");
            }
            catch (Exception ex)
            {
                // Logue a exceção para ajudar no diagnóstico
                Console.WriteLine($"Erro: {ex.Message}");
                // Retorna a mesma view para que o usuário possa tentar novamente
                return View(viewModel);
            }
        }






        //Deleta cliente
        [HttpPost]
        public async Task<IActionResult> Delete(EditCustomerViewModel viewModel)
        {
            var customer = await customerRegistryDBContext.Customers.FindAsync(viewModel.Id);

            if (customer != null)
            {
                customerRegistryDBContext.Customers.Remove(customer);
                await customerRegistryDBContext.SaveChangesAsync();

                return Ok();
            }
            return NotFound();
        }

        //Deleta Endereço
        [HttpPost]
        public async Task<IActionResult> DeleteAddress(Guid id)
        {
            var address = await customerRegistryDBContext.Addresses.FindAsync(id);

            if (address != null)
            {
                customerRegistryDBContext.Addresses.Remove(address);
                await customerRegistryDBContext.SaveChangesAsync();

                return Ok(); // Retorna uma resposta de sucesso
            }

            return NotFound(); // Retorna 404 se o endereço não for encontrado
        }
    }
}