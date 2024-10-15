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
            };

            await customerRegistryDBContext.Customers.AddAsync(customer);
            await customerRegistryDBContext.SaveChangesAsync();

            return RedirectToAction("AddAddress", new { customerId = customer.Id});
        }



        //Adicionar Endereço
        [HttpGet]
        public async Task<IActionResult> AddAddress(Guid customerId)
        {
            var model = new AddAddressViewModel
            {
               Id = customerId // Vincula o cliente ao endereço
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress(AddAddressViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Lógica para salvar o endereço
                    var address = new Address
                    {
                        Id = model.Id,
                        Street = model.Street,
                        City = model.City,
                        State = model.State,
                        CEP = model.CEP,
                        Complement = model.Complement,
                        Type = model.Type,
                        CustomerId = model.CustomerId // Certifique-se de que o CustomerId esteja sendo enviado e salvo
                    };

                    await customerRegistryDBContext.AddAsync(address); // Supondo que _context seja seu DbContext
                    await customerRegistryDBContext.SaveChangesAsync();

                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    // Logar a exceção
                    // Você pode usar um logger para registrar detalhes do erro
                    return Json(new { success = false, error = ex.Message });
                }
            }

            return Json(new { success = false });
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
                    .FirstOrDefaultAsync(x => x.Id == viewModel.Id);

                if (customer == null)
                {
                    return NotFound(); // Cliente não encontrado
                }

                // Atualiza os dados do cliente
                customer.Name = viewModel.Name;
                customer.Email = viewModel.Email;
                customer.Phone = viewModel.Phone;

                await customerRegistryDBContext.SaveChangesAsync();
                return RedirectToAction("Show");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
                return View(viewModel);
            }
        }





        //Editar Endereço
        [HttpGet]
        [Route("Customer/EditAddress/{addressId}")]
        public async Task<IActionResult> EditAddress(Guid addressId)
        {
            Console.WriteLine($"Received addressId: {addressId}");

            if (addressId == Guid.Empty)
            {
                // Isso indica que o addressId não está sendo passado corretamente
                return NotFound("Endereço não encontrado 1.");
            }

            var address = await customerRegistryDBContext.Addresses
                .FirstOrDefaultAsync(a => a.Id == addressId);

            if (address == null)
            {
                return NotFound("Endereço não encontrado 2.");
            }

            var model = new EditAddressViewModel
            {
                Id = address.Id,
                Street = address.Street,
                City = address.City,
                State = address.State,
                CEP = address.CEP,
                Complement = address.Complement,
                Type = address.Type
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditAddress(EditAddressViewModel model)
        {
            if (ModelState.IsValid)
            {
                var address = customerRegistryDBContext.Addresses
                                      .FirstOrDefault(a => a.Id == model.Id);

                if (address == null)
                {
                    return NotFound();
                }

                address.Street = model.Street;
                address.City = model.City;
                address.State = model.State;
                address.CEP = model.CEP;
                address.Complement = model.Complement;
                address.Type = model.Type;

                await customerRegistryDBContext.SaveChangesAsync();
                return RedirectToAction("Show");
            }

            return View(model);
        }





        //Deletar cliente
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


        [HttpPost]
        public async Task<IActionResult> DeleteAddress(Guid id)
        {
            var address = await customerRegistryDBContext.Addresses.FindAsync(id);

            if (address != null)
            {
                customerRegistryDBContext.Addresses.Remove(address);
                await customerRegistryDBContext.SaveChangesAsync();

                return Ok(); // Endereço removido com sucesso
            }

            return NotFound(); // Endereço não encontrado
        }

    }
}