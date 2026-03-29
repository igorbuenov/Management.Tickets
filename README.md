# 🎫 Management.Tickets API

Backend para gerenciamento de chamados (tickets), desenvolvido com foco em **arquitetura limpa, segurança e boas práticas de engenharia de software**, simulando um ambiente real de produção.

---

## 🚀 Visão geral

Este projeto foi criado para aplicar conceitos utilizados no mercado, com foco em:

* Separação clara de responsabilidades
* Escalabilidade e manutenção
* Segurança na aplicação
* Código organizado e testável

---

## 🎯 Principais características

* 🧱 Clean Architecture (Onion Architecture)
* 🔐 Autenticação com JWT
* 👥 Autorização baseada em roles (Admin, Technician, User)
* 🧠 UseCases para centralizar regras de negócio
* 📦 Repository Pattern + Unit of Work
* ✅ Validação com FluentValidation
* 🔄 Mapeamento com AutoMapper
* 📊 Logging estruturado com Serilog
* 🛡️ Boas práticas de segurança (hash de senha, proteção contra timing attacks)

---

## 🏗️ Arquitetura

O projeto segue os princípios da Clean Architecture:

```
Presentation (WebAPI)
    ↓
Application (UseCases, DTOs, Validators)
    ↓
Domain (Entities, Interfaces)
    ↓
Infrastructure (EF Core, Repositories, Services)
```

Essa estrutura garante:

* baixo acoplamento
* alta testabilidade
* facilidade de evolução

---

## 🧠 Decisões técnicas

* **UseCases**: centralizam a lógica da aplicação, mantendo controllers simples
* **Repository Pattern**: desacopla o acesso ao banco de dados
* **Unit of Work**: garante consistência nas operações
* **JWT Authentication**: autenticação stateless
* **FluentValidation**: validação robusta e desacoplada
* **Serilog**: logging estruturado com rastreabilidade de requisições

---

## 🔐 Segurança

O projeto foi desenvolvido com foco em segurança:

* Hash de senha via abstração (`IPasswordService`)
* Armazenamento seguro de credenciais
* Autenticação com JWT
* Controle de acesso baseado em roles
* Proteção contra ataques de timing

---

## 📊 Logging e observabilidade

Logs estruturados com informações como:

* UserId
* RequestId
* Path e método HTTP
* IP do cliente

Permite rastrear facilmente o comportamento da aplicação.

---

## ⚙️ Funcionalidades

* ✅ Cadastro de usuários
* ✅ Autenticação
* ✅ Controle de acesso
* 🔄 Sistema de tickets (em evolução)

---

## ▶️ Como executar

```bash
git clone https://github.com/igorbuenov/Management.Tickets
cd Management.Tickets
```

Configurar `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "sua-connection-string"
}
```

Executar migrations:

```bash
dotnet ef database update
```

Rodar aplicação:

```bash
dotnet run
```

Acessar Swagger:

```
https://localhost:{porta}/swagger
```

---

## 🛣️ Roadmap

* [ ] Implementação completa de tickets
* [ ] Testes automatizados
* [ ] Mensageria (RabbitMQ)
* [ ] Middleware global de exceção
* [ ] Cache e otimizações

---

## 👨‍💻 Autor

**Igor Bueno**
🔗 [LinkedIn](https://www.linkedin.com/in/igorbuenov/)
💻 Backend Developer (.NET | APIs | Segurança)

---

## 📌 Observação

Projeto desenvolvido com foco em evolução profissional e simulação de cenários reais de mercado.
