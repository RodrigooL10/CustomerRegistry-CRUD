# Customer Registry System

## Descrição do Projeto:
O Customer Registry System é uma solução para gerenciamento de clientes e seus respectivos endereços. O sistema permite realizar operações de criação, leitura, atualização e deleção (CRUD) dos dados de clientes, incluindo informações detalhadas como nome, CPF, email, telefone e endereços. Ele utiliza uma API ASP.NET Core para o backend e um front-end interativo para facilitar a visualização e manipulação dos dados.

Criado por Rodrigo Lima.

[https://github.com/RodrigooL10]

## Funcionalidades:
- Adicionar, Editar e Deletar Clientes: Permite gerenciar informações básicas do cliente, incluindo nome, CPF, email, telefone e data de nascimento.
- Gerenciamento de Endereços: O usuário pode adicionar, editar e deletar endereços associados a cada cliente.
- Visualização de Dados: Exibe as informações de clientes e endereços de forma clara utilizando grids do DevExpress.
- Responsividade: A interface é responsiva e se adapta a diferentes tamanhos de tela.

## Tecnologias Utilizadas:
- ASP.NET Core
- Entity Framework Core
- SQL Server
- DevExtreme
- JavaScript/jQuery

## Configuração do Projeto:
### Pré-requisitos:
- .NET 6 ou superior
- SQL Server

### Passos:
1. Clone o repositório
```
 git clone https://github.com/RodrigooL10/CustomerRegistry-CRUD.git
```

2. Configure o banco de dados SQL Server:

   - Crie um banco de dados chamado **CustomerRegistryDB**.
   - Localize e abra o arquivo `appsettings.json` na pasta `CustomerRegistry-CRUD/CustomerRegistrySystem`.
   - Atualize as seguintes configurações com suas credenciais:
    ```
    {
       "ConnectionStrings": {
         "CustomerRegistryConnectionString": "Server=SEU_SERVIDOR;Database=CustomerRegistryDB;User Id=SEU_USUARIO;Password=SUAS_SENHAS;Trusted_Connection=True;TrustServerCertificate=True"
       }
     }
    ```
   - Substitua `SEU_SERVIDOR`, `SEU_USUARIO` e `SUAS_SENHAS` pelas informações correspondentes ao seu ambiente.


3. Navegue até a pasta do backend:
```
cd CustomerRegistry-CRUD/CustomerRegistrySystem
```

4. Execute o projeto:
```
dotnet run
```

O backend do projeto estará disponível em `http://localhost:5026`.

## Estrutura do Projeto:
```
  CustomerManagementSystem
├── Infraestructure
├── Data
├── Migrations
├── Domain
│   └── Customer.cs
├── Application
│   ├── ViewModels
│   │   ├── AddCustomerViewModel.cs
│   │   ├── EditCustomerViewModel.cs
│   │   └── ErrorViewModel.cs
└── Presentation
    ├── Controllers
    │   ├── CustomerController.cs
    │   └── HomeController.cs
    ├── Views
    │   ├── Customer
    │   │   ├── Add
    │   │   ├── AddAddress
    │   │   ├── Edit
    │   │   ├── EditAddress
    │   │   └── Show
    │   ├── Home
    │   │   └── index
    │   └── Shared
    │       ├── Layout
    │       ├── ValidationScriptsPartial
    │       └── Error
```
## Contribuindo:
Contribuições são bem-vindas! Sinta-se à vontade para submeter pull requests.

1. Realize um fork do repositório.
2. Crie uma nova branch para a sua funcionalidade `(git checkout -b feature/NewFeature)`.
3. Registre suas alterações com um commit `(git commit -m 'Adicionar uma nova funcionalidade')`.
4. Envie suas alterações para a branch criada `(git push origin feature/NewFeature)`.
5. Inicie um Pull Request.

## Contato:
Rodrigo Lima - orodrigo.lima17@gmail.com
