using CustomerRegistrySystem.Infraestructure.Data;
using CustomerRegistrySystem.Domain;
using CustomerRegistrySystem.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerRegistrySystem.Presentation.Controllers
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
            //Recupera todos os clientes e seus endereços
            var customers = await customerRegistryDBContext.Customers
                .Include(c => c.Addresses)
                .ToListAsync();

            //Passa os clientes para a view
            return View(customers); 
        }


        // Adicionar Cliente
        [HttpGet]
        public IActionResult Add()
        {
            //Exibe o formulário para adicionar um cliente
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCustomerViewModel addCustomerRequest)
        {
            //Cria um novo objeto de cliente a partir do view model
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

            //Adiciona o cliente ao banco de dados
            await customerRegistryDBContext.Customers.AddAsync(customer);
            await customerRegistryDBContext.SaveChangesAsync();

            //Redireciona para a página de adicionar endereço
            return RedirectToAction("AddAddress", new { customerId = customer.Id });

        }



        // Adicionar Endereço
        [HttpGet]
        public IActionResult AddAddress(Guid customerId)
        {
            var model = new AddAddressViewModel
            {
                //Define o ID do cliente para o endereço
                Id = customerId
            };

            //Exibe o formulário para adicionar um endereço 
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress(AddAddressViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Cria um novo objeto de endereço a partir do view model
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

                    //Adiciona o endereço ao banco de dados
                    await customerRegistryDBContext.AddAsync(address);
                    await customerRegistryDBContext.SaveChangesAsync();

                    //Retorna resposta de sucesso
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    //Retorna resposta de erro
                    return Json(new { success = false, error = ex.Message });
                }
            }

            // Retorna falha se o estado do modelo for inválido
            return Json(new { success = false });
        }




        // Editar Cliente
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            //Recupera o cliente para edição
            var customer = await customerRegistryDBContext.Customers
                .FirstOrDefaultAsync(x => x.Id == id);

            if (customer != null)
            {
                //Preenche o view model com os dados do cliente
                var viewModel = new EditCustomerViewModel()
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Email = customer.Email,
                    Phone = customer.Phone,
                    CPF = customer.CPF,
                    DateOfBirth = customer.DateOfBirth,
                    Gender = customer.Gender
                };

                //Retorna o view de edição
                return View("Edit", viewModel);
            }

            //Redireciona se o cliente não for encontrado
            return RedirectToAction("Show");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditCustomerViewModel viewModel)
        {
            try 
            {
                //Encontra o cliente pelo ID
                var customer = await customerRegistryDBContext.Customers
                    .FirstOrDefaultAsync(x => x.Id == viewModel.Id);

                if (customer == null)
                {
                    //Retorna Não Encontrado se o cliente não existir
                    return NotFound();
                }

                //Atualiza as propriedades do cliente
                customer.Name = viewModel.Name;
                customer.Email = viewModel.Email;
                customer.Phone = viewModel.Phone;
                customer.CPF = viewModel.CPF;
                customer.DateOfBirth = viewModel.DateOfBirth;
                customer.Gender = viewModel.Gender;

                //Salva as alterações no banco de dados
                await customerRegistryDBContext.SaveChangesAsync();

                //Redireciona para lista de clientes
                return RedirectToAction("Show");
            }
            catch (Exception ex)
            {
                //Registra exceções
                Console.WriteLine($"Erro: {ex.Message}");

                //Retorna a view com o modelo em caso de erro
                return View(viewModel);
            }
        }





        //Editar Endereço
        [HttpGet]
        [Route("Customer/EditAddress/{addressId}")]
        public async Task<IActionResult> EditAddress(Guid addressId)
        {
            //Recupera o endereço para edição
            var address = await customerRegistryDBContext.Addresses
                .FirstOrDefaultAsync(a => a.Id == addressId);

            if (address == null)
            {
                //Retorna não encontrado se o endereço não existir
                return NotFound("Endereço não encontrado.");
            }

            //Preenche o view model com os dados do endereço
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

            //Retorna a view de edição do endereço
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditAddress(EditAddressViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Encontra o endereço pelo ID
                var address = await customerRegistryDBContext.Addresses
                                      .FirstOrDefaultAsync(a => a.Id == model.Id);

                if (address == null)
                {
                    //Retorna não encontrado se o endereço não existir
                    return NotFound();
                }

                //Atualiza as propriedades do endereço
                address.Street = model.Street;
                address.Number = model.Number;
                address.City = model.City;
                address.State = model.State;
                address.CEP = model.CEP;
                address.Complement = model.Complement;
                address.Type = model.Type;

                //Salva as alterações no banco de dados
                await customerRegistryDBContext.SaveChangesAsync();

                // Redireciona para a lista de clientes
                return RedirectToAction("Show");
            }

            // Retorna a view com o modelo em caso de estado inválido
            return View(model);
        }





        //Deletar cliente
        [HttpPost]
        public async Task<IActionResult> Delete(EditCustomerViewModel viewModel)
        {
            var customer = await customerRegistryDBContext.Customers.FindAsync(viewModel.Id);

            if (customer != null)
            {
                // Remove o cliente do banco de dados
                customerRegistryDBContext.Customers.Remove(customer);
                await customerRegistryDBContext.SaveChangesAsync();

                //Retorna resposta de sucesso
                return Ok(); 
            }

            //Retorna não encontrado se o cliente não existir
            return NotFound();
        }





        //Deletar Endereço
        [HttpPost]
        public async Task<IActionResult> DeleteAddress(Guid id)
        {
            var address = await customerRegistryDBContext.Addresses.FindAsync(id);

            if (address != null)
            {
                // Remove o endereço do banco de dados
                customerRegistryDBContext.Addresses.Remove(address);
                await customerRegistryDBContext.SaveChangesAsync();

                // Retorna resposta de sucesso
                return Ok();
            }

            // Retorna não encontrado se o endereço não existir
            return NotFound();
        }
    }
}