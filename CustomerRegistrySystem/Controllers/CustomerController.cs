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

        //Exibir tabela de Clientes e Endereços
        [HttpGet]
        public async Task<IActionResult> Show()
        {
            var customers = await customerRegistryDBContext.Customers
                .Include(c => c.Addresses)
                .ToListAsync();

            return View(customers);
        }


        // Adicionar Cliente
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
                    CPF = addCustomerRequest.CPF,
                    DateOfBirth = addCustomerRequest.DateOfBirth,
                    Gender = addCustomerRequest.Gender
                };

                await customerRegistryDBContext.Customers.AddAsync(customer);
                await customerRegistryDBContext.SaveChangesAsync();

                return RedirectToAction("AddAddress", new { customerId = customer.Id });

        }



        // Adicionar Endereço
        [HttpGet]
        public IActionResult AddAddress(Guid customerId)
        {
            var model = new AddAddressViewModel
            {
                Id = customerId
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
                    var address = new Address
                    {
                        Id = model.Id,
                        Street = model.Street,
                        Number = model.Number,
                        City = model.City,
                        State = model.State,
                        CEP = model.CEP,
                        Complement = model.Complement,
                        Type = model.Type,
                        CustomerId = model.CustomerId 
                    };

                    await customerRegistryDBContext.AddAsync(address); 
                    await customerRegistryDBContext.SaveChangesAsync();

                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, error = ex.Message });
                }
            }

            return Json(new { success = false });
        }




        // Editar Cliente
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var customer = await customerRegistryDBContext.Customers
                .Include(c => c.Addresses)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (customer != null)
            {
                var viewModel = new EditCustomerViewModel()
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Email = customer.Email,
                    Phone = customer.Phone,
                    CPF = customer.CPF,
                    DateOfBirth = customer.DateOfBirth,
                    Gender = customer.Gender,
                    Addresses = customer.Addresses.Select(a => new EditAddressViewModel
                    {
                        Id = a.Id,
                        Street = a.Street,
                        Number = a.Number,
                        City = a.City,
                        State = a.State,
                        CEP = a.CEP,
                        Complement = a.Complement,
                        Type = a.Type
                    }).ToList()
                };

                return View("Edit", viewModel);
            }

            return RedirectToAction("Show");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditCustomerViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var customer = await customerRegistryDBContext.Customers
                    .FirstOrDefaultAsync(x => x.Id == viewModel.Id);

                if (customer == null)
                {
                    return NotFound();
                }

                customer.Name = viewModel.Name;
                customer.Email = viewModel.Email;
                customer.Phone = viewModel.Phone;
                customer.CPF = viewModel.CPF;
                customer.DateOfBirth = viewModel.DateOfBirth;
                customer.Gender = viewModel.Gender;

                await customerRegistryDBContext.SaveChangesAsync();
                return RedirectToAction("Show");
            }

            return View(viewModel);
        }





        //Editar Endereço
        [HttpGet]
        [Route("Customer/EditAddress/{addressId}")]
        public async Task<IActionResult> EditAddress(Guid addressId)
        {
            var address = await customerRegistryDBContext.Addresses
                .FirstOrDefaultAsync(a => a.Id == addressId);

            if (address == null)
            {
                return NotFound("Endereço não encontrado.");
            }

            var model = new EditAddressViewModel
            {
                Id = address.Id,
                Street = address.Street,
                Number = address.Number,
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
                var address = await customerRegistryDBContext.Addresses
                                      .FirstOrDefaultAsync(a => a.Id == model.Id);

                if (address == null)
                {
                    return NotFound();
                }

                address.Street = model.Street;
                address.Number = model.Number;
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





        //Deletar Endereço
        [HttpPost]
        public async Task<IActionResult> DeleteAddress(Guid id)
        {
            var address = await customerRegistryDBContext.Addresses.FindAsync(id);

            if (address != null)
            {
                customerRegistryDBContext.Addresses.Remove(address);
                await customerRegistryDBContext.SaveChangesAsync();

                return Ok();
            }

            return NotFound();
        }
    }
}