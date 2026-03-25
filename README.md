# 🎫 Management.Tickets

API backend para gerenciamento de chamados (tickets), desenvolvida com foco em boas práticas de arquitetura, segurança e escalabilidade, simulando um ambiente real de produção.

---

## 🚀 Objetivo

Este projeto foi desenvolvido com o objetivo de:

* Evoluir habilidades em backend com .NET
* Aplicar conceitos de arquitetura limpa (Clean Architecture)
* Simular um sistema real utilizado em empresas
* Praticar boas práticas de segurança e organização de código

---

## 🏗️ Arquitetura

O projeto segue os princípios de **Clean Architecture (Onion Architecture)**, com separação clara de responsabilidades:

* **Presentation (WebAPI)** → Controllers, autenticação, configuração
* **Application** → Casos de uso (UseCases), validações e regras de aplicação
* **Domain** → Entidades e contratos (interfaces)
* **Infrastructure** → Acesso a dados, serviços externos e implementações

---

## 🧩 Tecnologias e padrões utilizados

### 🔧 Tecnologias

* .NET / ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* JWT Authentication
* Serilog (logging estruturado)
* AutoMapper
* FluentValidation

### 🧠 Padrões e conceitos

* Clean Architecture
* Repository Pattern
* Unit of Work
* Dependency Injection
* DTOs (Data Transfer Objects)
* Separation of Concerns

---

## 🔐 Segurança

O projeto implementa boas práticas de segurança:

* Autenticação baseada em JWT
* Autorização baseada em Roles (Admin, Technician, User)
* Hash de senha com abstração via `IPasswordService`
* Proteção contra ataques de timing em autenticação
* Validação de entrada com FluentValidation

---

## 📂 Estrutura do projeto

```
src/backend/
├── Tickets.WebAPI        # Controllers, configuração da API, autenticação
├── Tickets.Application   # UseCases, DTOs, validações
├── Tickets.Domain        # Entidades e interfaces
├── Tickets.Infrastructure # EF Core, repositórios, serviços e migrations
```

---

## ⚙️ Funcionalidades principais

* ✅ Cadastro de usuários
* ✅ Autenticação com JWT
* ✅ Controle de acesso por roles
* ✅ Validação de dados
* ✅ Logging estruturado
* 🔄 (Em evolução) gerenciamento de tickets

---

## 🧠 Casos de uso (UseCases)

A lógica da aplicação é centralizada em UseCases, mantendo os controllers enxutos.

Exemplos:

* `CreateUserUseCase`

  * Validação de dados
  * Criação de usuário e senha
  * Associação de roles
  * Persistência via Unit of Work

* `AuthenticateUserUseCase`

  * Validação de credenciais
  * Proteção contra timing attacks
  * Geração de token JWT

---

## 📊 Logging e Observabilidade

O projeto utiliza **Serilog** com persistência em banco de dados:

* UserId
* RequestPath
* RequestMethod
* ClientIp
* RequestId

Logs são enriquecidos via middleware para rastreabilidade completa.

---

## 🗄️ Banco de dados

* SQL Server com Entity Framework Core
* Migrations versionadas
* Seed inicial de roles:

  * Admin
  * Technician
  * User

---

## ▶️ Como executar o projeto

1. Clonar o repositório:

```
git clone https://github.com/seu-usuario/management.tickets.git
```

2. Configurar a string de conexão no `appsettings.json`

3. Executar as migrations:

```
dotnet ef database update
```

4. Rodar a aplicação:

```
dotnet run
```

5. Acessar o Swagger:

```
https://localhost:{porta}/swagger
```

---

## 🛣️ Roadmap (evoluções futuras)

* [ ] Implementação completa de tickets
* [ ] Mensageria com RabbitMQ
* [ ] Testes automatizados (unitários e integração)
* [ ] Middleware global de exceção
* [ ] Cache e otimizações de performance

---

## 🎯 Diferenciais do projeto

* Arquitetura próxima de sistemas reais
* Forte preocupação com segurança
* Uso de padrões de mercado
* Logging estruturado para observabilidade
* Separação clara de responsabilidades

---

## 👨‍💻 Autor

**Igor Bueno**  
🔗 [LinkedIn](https://www.linkedin.com/in/igorbuenov/)  
💻 Backend Developer (.NET | APIs | Segurança)

---

## 📌 Observação

Este projeto foi desenvolvido com fins de estudo e evolução profissional, buscando simular desafios reais enfrentados no mercado.
