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
    }
}
