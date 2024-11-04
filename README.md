# E-Commerce - PersonalBuyApi

## Integrantes do Grupo
- RM551288 Gustavo René Dias Boamorte
- RM98442 João da Costa Feitosa
- RM99495 Igor Miguel Silva
- RM98093 Pedro Felipe Barros
- RM551732 Kauê Matheus Santana

## Arquitetura da API

### Escolha da Arquitetura

Para este projeto de e-commerce, optamos por uma **arquitetura monolítica**. As razões para essa escolha são:

1. **Simplicidade e Coesão**: Uma abordagem monolítica permite um desenvolvimento mais coeso e simplificado, onde todos os componentes são implementados em uma única aplicação. Isso facilita o gerenciamento e a comunicação entre os componentes.

2. **Facilidade de Implementação**: Como a aplicação é pequena e a equipe é relativamente pequena, uma arquitetura monolítica reduz a complexidade inicial do projeto e permite um desenvolvimento mais rápido.

3. **Menor Sobrecarga Operacional**: Em um ambiente de produção, uma aplicação monolítica pode ser mais fácil de configurar e gerenciar, especialmente se não houver necessidade de escalabilidade horizontal complexa.

### Implementação da API

A API foi implementada com uma arquitetura monolítica, seguindo os princípios RESTful para a comunicação com o cliente. A aplicação é construída usando ASP.NET Core, com uma base de dados Oracle para persistência de dados.
Uma API para gerenciamento de clientes, produtos e carrinhos de compras, com foco em uma ML para envio de sugestões de compras em emails no sistema de ecommerce.

### Princípios SOLID
- Single Responsibility Principle (SRP): Cada classe e método tem uma única responsabilidade. Isso é aplicado às camadas Controller, Service e Repository, separando a lógica de negócios e a persistência de dados.
- Dependency Injection (DI): Implementado para desacoplar dependências e facilitar testes. Utilizamos a injeção de dependências no Startup.cs para gerenciar instâncias de serviços e repositórios.
- Interface Segregation Principle (ISP): As interfaces foram projetadas de forma coesa, evitando métodos desnecessários.

### Testes Unitários
Os testes unitários foram desenvolvidos para validar a lógica dos métodos principais nas camadas de Service e Repository. Utilizamos xUnit como framework de testes, assegurando que cada funcionalidade isolada opere conforme o esperado

- Validação de CRUD: Testes para criação, leitura, atualização e exclusão dos recursos de Usuário, Produto e Carrinho.
- Validação de cenário para os métodos da service.

#### Estrutura do Projeto
- **Controllers**: Responsáveis por gerenciar as requisições HTTP e retornar respostas adequadas.
- **Services**: Contêm a lógica de negócios e regras de aplicação.
- **Repositories**: Gerenciam a persistência de dados e a comunicação com o banco de dados.
- **DTOs**: Objetos de Transferência de Dados utilizados para enviar e receber dados entre o cliente e o servidor.

## Endpoints CRUD

A seguir estão os endpoints CRUD disponíveis para os recursos de **usuários**, **produtos**, **autenticação** e **carrinhos de compras**:

### Usuários

- **POST /client**
  - **Descrição**: Registra um novo cliente no sistema.
  - **Corpo da Requisição**:
    ```json
    {
      "name": "John Doe",
      "email": "john.doe@example.com",
      "address": "123 Elm Street",
      "password": "securepassword"
    }
    ```

- **GET /client**
  - **Descrição**: Obtém todos os clientes cadastrados.

- **GET /client/{id}**
  - **Descrição**: Obtém um cliente específico pelo ID.
  - **Parâmetros**: `id` - Identificador do cliente.

- **PUT /client/{id}**
  - **Descrição**: Atualiza os detalhes de um cliente existente.
  - **Corpo da Requisição**:
    ```json
    {
      "name": "John Doe Updated",
      "email": "john.doe@example.com",
      "address": "123 Elm Street Updated"
    }
    ```

- **DELETE /client/{id}**
  - **Descrição**: Exclui um cliente pelo ID.

### Produtos

- **POST /product**
  - **Descrição**: Cria um novo produto.
  - **Corpo da Requisição**:
    ```json
    {
      "name": "Laptop",
      "price": 1500.99,
      "description": "High-performance laptop"
    }
    ```

- **GET /product**
  - **Descrição**: Obtém todos os produtos disponíveis.

- **GET /product/{id}**
  - **Descrição**: Obtém um produto específico pelo ID.
  - **Parâmetros**: `id` - Identificador do produto.

- **PUT /product/{id}**
  - **Descrição**: Atualiza os detalhes de um produto existente.
  - **Corpo da Requisição**:
    ```json
    {
      "name": "Laptop Updated",
      "price": 1400.99,
      "description": "Updated description"
    }
    ```

- **DELETE /product/{id}**
  - **Descrição**: Remove um produto pelo ID.

### Autenticação (Auth)

- **POST /auth/login**
  - **Descrição**: Autentica um cliente e retorna uma mensagem de sucesso.
  - **Corpo da Requisição**:
    ```json
    {
      "email": "john.doe@example.com",
      "password": "securepassword"
    }
    ```

- **POST /auth/register**
  - **Descrição**: Registra um novo cliente no sistema.
  - **Corpo da Requisição**:
    ```json
    {
      "name": "John Doe",
      "email": "john.doe@example.com",
      "address": "123 Elm Street",
      "password": "securepassword"
    }
    ```

### Carrinho de Compras (Cart)

- **GET /cart/{clientId}**
  - **Descrição**: Obtém o carrinho de compras de um cliente pelo ID.
  - **Parâmetros**: `clientId` - Identificador do cliente.

- **POST /cart/{clientId}/create**
  - **Descrição**: Cria um carrinho de compras para um cliente.
  - **Parâmetros**: `clientId` - Identificador do cliente.

- **POST /cart/{clientId}/add**
  - **Descrição**: Adiciona um produto ao carrinho de compras de um cliente.
  - **Corpo da Requisição**:
    ```json
    {
      "productId": 1,
      "quantity": 2
    }
    ```

- **DELETE /cart/{clientId}/remove/{productId}**
  - **Descrição**: Remove um produto do carrinho de compras de um cliente.
  - **Parâmetros**: `clientId` - Identificador do cliente, `productId` - Identificador do produto.

- **PUT /cart/{clientId}/update/{productId}/{quantityChange}**
  - **Descrição**: Atualiza a quantidade de um produto no carrinho de compras de um cliente.
  - **Parâmetros**: `clientId` - Identificador do cliente, `productId` - Identificador do produto, `quantityChange` - Quantidade a ser ajustada (positiva ou negativa).
 
- **DELETE /cart/{clientId}/delete**
  - **Descrição**: Eu removo o carrinho do usuário.
  - **Parâmetros**: `clientId` - Identificador do cliente.

### Pagamento do carrinho (Payments) - Via Stripe

- **POST /Payment/{clientId}**
  - **Descrição**: Cria um link para você efetuar o pagamento do carrinho via Stripe(lembrar no momento de copiar e colar a url remover as aspas duplas).
  - **Parâmetros**: `clientId` - Identificador do cliente.

## API de Pagamento

- **POST /api/Stripe/Payment**
  - **Descrição**: Monto o pagamento para uma lista de produtos.
  - **Corpo da Requisição**:
    ```json
    [
      {
        "product": {
          "productId": 0,
          "name": "string",
          "price": 0,
          "description": "string"
        },
        "productId": 0,
        "quantity": 0
      }
    ]
    ```

## Design Pattern Utilizado

- Repository

### Padrão Singleton

Implementamos o padrão Singleton para o **gerenciador de configurações**. Esse padrão garante que uma única instância da classe de configurações seja criada e utilizada em toda a aplicação. Isso é feito para evitar múltiplas instâncias e garantir consistência no gerenciamento das configurações.

### Documentação da API

A documentação da API é configurada utilizando Swagger/OpenAPI. Você pode acessar a documentação detalhada em http://localhost:5267/swagger. A documentação inclui descrições claras dos endpoints, modelos de dados, e exemplos de requisições e respostas.

## Como Baixar e Executar o Projeto

Para baixar e executar o projeto, siga os passos abaixo:

### Clonar o Repositório

Utilize o comando `git clone` para clonar o repositório em sua máquina local:

```bash
git clone https://github.com/gustavorenedev/PersonalBuyChallengeAPI.git
```

### Entre na pasta PersonalBuyChallengeAPI ou pelo terminal:
```bash
cd ./PersonalBuyChallengeAPI
```

### Restaurar as Dependências
## Execute o comando para restaurar as dependências do projeto:

No terminal:
- dotnet restore

## Abra a o arquivo sln

## Configurar o Banco de Dados
Certifique-se de que você tem um banco de dados Oracle configurado. Atualize as configurações de conexão no arquivo appsettings.json de acordo com suas configurações locais.

## Migrar o Banco de Dados
Aplique as migrações do banco de dados no terminal:
- dotnet ef database update

## Executar o Projeto
Inicie a aplicação com dos dois projetos, PersonalBuyEcommerceAPI com o PersonalBuyPaymentAPI:
Indo na pasta de cada um via terminal pode utilizar o:
- dotnet run

ou, na solution configurar para iniciar as duas aplicações pelo Configure Startup Project
![image](https://github.com/user-attachments/assets/1977e53b-26e8-4520-9ca3-7f9b9d62b101)

## Acessar a API
A API estará disponível em http://localhost:5267/swagger. Acesse a documentação em http://localhost:5267/swagger.

Se você encontrar algum problema ou tiver dúvidas, consulte a documentação oficial do ASP.NET Core.
