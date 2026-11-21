# Management.Tickets API

API REST em .NET 9 para gerenciamento de chamados (tickets): usuários com perfis, departamentos, categorias,
abertura e atendimento de tickets com chat, notificações e envio de e-mails assíncrono.

O frontend que consome esta API fica em um repositório separado: **Management.Tickets-FrontEnd** (Angular 21).

---

## Funcionalidades

| Área | O que existe hoje |
|---|---|
| Autenticação | Login com JWT, "esqueci minha senha" (link por e-mail, token válido por 30 min), redefinição de senha, troca obrigatória da senha temporária/expirada |
| Usuários (Admin) | Criar (gera senha temporária e envia e-mail de boas-vindas), listar com filtro e paginação, consultar, editar, desativar (soft delete) e reativar |
| Senhas | Política de 8 a 12 caracteres com maiúscula, minúscula, dígito e caractere especial; bloqueio das 5 últimas senhas; expiração em 45 dias; hash BCrypt |
| Departamentos e categorias | Criar e listar (paginação e busca por nome); "meus departamentos" do usuário logado |
| Tickets | Criar (o usuário precisa pertencer ao departamento escolhido), listar todos / atribuídos a mim / criados por mim / dos meus departamentos, consultar, assumir/atribuir, alterar status |
| Chat do ticket | Enviar e listar mensagens; não é possível enviar mensagem em ticket `Resolved` ou `Closed` |
| Notificações | Geradas de forma assíncrona quando uma mensagem é enviada no chat; listar as não lidas e marcar como lida |

Não existem hoje: edição ou exclusão de departamentos e categorias, troca de role ou de departamento de um usuário
existente, histórico de status do ticket e notificações para atribuição ou mudança de status.

---

## Tecnologias

| Item | Versão / pacote |
|---|---|
| Runtime | .NET 9 (`net9.0`) |
| Persistência | Entity Framework Core 9.0.14 + SQL Server |
| Autenticação | `Microsoft.AspNetCore.Authentication.JwtBearer` 9.0.14 (HS256) |
| Validação | FluentValidation 12.1.1 |
| Mapeamento | AutoMapper 16.1.0 |
| Hash de senha | BCrypt.Net-Next 4.1.0 |
| Mensageria | RabbitMQ.Client 7.2.2 |
| E-mail | Brevo (API HTTP `v3/smtp/email`) |
| Logs | Serilog.AspNetCore 10.0.0 + Serilog.Sinks.MSSqlServer 9.0.3 |
| Documentação da API | Swashbuckle.AspNetCore 9.0.6 |
| Testes | xUnit 2.9.2, FluentAssertions 8.9.0, Moq 4.20.72, Bogus 35.6.5 |

---

## Arquitetura

Clean Architecture com use cases. A direção das dependências entre os projetos é:

```
Tickets.WebAPI ──► Tickets.Application ──► Tickets.Domain
      │                    │
      │                    └──► Tickets.Exceptions
      ├──► Tickets.Infrastructure ──► Tickets.Application, Tickets.Domain
      └──► Tickets.Exceptions
```

| Projeto | Responsabilidade |
|---|---|
| `Tickets.Domain` | Entidades, enums e interfaces de repositório. Não depende de nenhum outro projeto nem de pacotes |
| `Tickets.Application` | Use cases (um por operação, método `Execute`), DTOs, validators, interfaces de serviços (`ICurrentUser`, `IJwtService`, `IEmailService`, `IMessagePublisher`...), eventos e handlers. DI em `AppDIExtension.cs` |
| `Tickets.Infrastructure` | EF Core (`DbContext`, configurações, migrations), repositórios e Unit of Work, JWT, BCrypt, RabbitMQ, outbox, Brevo e background services. DI em `InfraDIExtension.cs` |
| `Tickets.WebAPI` | Controllers, modelos HTTP (`Models/`), perfis do AutoMapper, `ExceptionFilter`, Serilog, Swagger, JWT e CORS |
| `Tickets.Exceptions` | Exceções de negócio com o status HTTP correspondente |

### Decisões técnicas

- **Use cases**: concentram a regra de negócio; os controllers apenas mapeiam o modelo HTTP para o DTO e delegam.
- **Repository + Unit of Work**: os repositórios não chamam `SaveChanges`; cada use case persiste com um único
  `Commit()`, o que torna a operação de negócio e a mensagem de outbox atômicas.
- **FluentValidation**: validators registrados por varredura do assembly. Existem para login, usuários e senhas;
  tickets, mensagens, departamentos e categorias não têm validator.
- **Tratamento de erros**: `ExceptionFilter` global converte `TicketsException` em resposta HTTP
  (`{ "success": false, "errors": ["..."] }`); exceções não previstas viram `500` com mensagem genérica.

### Estrutura de diretórios

```
Management.Tickets/
├── Management.Tickets.sln
├── Dockerfile                       build multi-stage (sdk:9.0 → aspnet:9.0), porta 8080
├── src/
│   ├── backend/
│   │   ├── Tickets.Domain/          Entities/, Enums/, Interfaces/Repositories/
│   │   ├── Tickets.Application/     UseCases/, DTOs/, Validators/, Interfaces/, Events/, Handlers/, Services/
│   │   ├── Tickets.Infrastructure/  Data/, Migrations/, Repositories/, Messaging/, Services/, Settings/
│   │   └── Tickets.WebAPI/          Controllers/, Models/, Mappings/, Filters/, Logging/, Configurations/
│   └── shared/
│       └── Tickets.Exceptions/
└── tests/
    ├── CommonTestUtilities/         builders de mocks (Moq) e requests (Bogus)
    ├── UseCases/                    testes de use cases
    └── Validators.Test/             testes de validators
```

---

## Domínio

| Entidade | Descrição |
|---|---|
| `User` | Nome, e-mail (único), ativo/inativo, quem criou |
| `Role` / `UserRole` | Perfis do usuário. Seed: `1 Admin`, `2 Technician`, `3 User` |
| `UserPassword` / `UserPasswordHistory` | Hash da senha atual com data de expiração e histórico de senhas |
| `PasswordResetToken` | Hash SHA-256 do token de redefinição, expiração e data de uso |
| `Department` / `UserDepartment` | Departamentos e vínculo usuário ↔ departamento |
| `Category` | Categorias de ticket |
| `Ticket` | Título, descrição, prioridade, status, criador, responsável, departamento e categoria |
| `TicketMessage` | Mensagens do chat do ticket |
| `Notification` | Notificação de um usuário sobre um ticket (lida/não lida) |
| `OutboxMessage` | Eventos pendentes de publicação no RabbitMQ |

Enums: `TicketStatus` (`Open=1`, `InProgress=2`, `Resolved=3`, `Closed=4`) e `TicketsPriority` (`Low=1`,
`Medium=2`, `High=3`, `Urgent=4`). Na entrada são enviados como número; nas respostas de ticket são devolvidos como
string (`"Open"`, `"High"`...).

Ciclo do ticket: criado como `Open`; ao ser atribuído passa a `InProgress`; depois disso o status pode ser alterado
livremente por Admin ou Technician (exceto pelo próprio criador do ticket). Não há validação de transições.

---

## Endpoints

Todas as rotas têm o prefixo `/api`. "Autenticado" significa apenas `[Authorize]`, sem restrição de role no
controller (algumas regras adicionais ficam dentro do use case).

| Método | Rota | Acesso | Descrição |
|---|---|---|---|
| POST | `/Auth/login` | anônimo | Login; retorna `accessToken`, `expiresAt`, `mustChangePassword` e dados do usuário |
| POST | `/Auth/forgot-password` | anônimo | Solicita o link de redefinição (resposta em texto) |
| POST | `/Auth/reset-password` | anônimo | Redefine a senha com o token recebido por e-mail |
| PUT | `/Auth/{id}/change-temporary-password` | autenticado (o próprio usuário) | Troca a senha temporária/expirada |
| POST | `/Users` | Admin | Cria usuário (role e departamento) |
| GET | `/Users` | Admin | Lista paginada (`page`, `pageSize`, busca, ativo/inativo) |
| GET | `/Users/{id}` | Admin | Consulta usuário |
| PUT | `/Users/{id}/update-user` | autenticado (Admin edita qualquer um; demais, só a si) | Edita nome e e-mail |
| DELETE | `/Users/{id}` | Admin | Desativa (soft delete) |
| PUT | `/Users/{id}` | Admin | Reativa |
| PUT | `/Users/{id}/update-password` | autenticado (o próprio usuário) | Troca voluntária de senha |
| POST | `/Departments` | autenticado | Cria departamento |
| GET | `/Departments` | autenticado | Lista paginada com busca por nome |
| GET | `/Departments/my-departments` | autenticado | Departamentos do usuário logado |
| POST | `/Categories` | autenticado | Cria categoria |
| GET | `/Categories` | autenticado | Lista paginada com busca por nome |
| POST | `/Tickets` | autenticado | Cria ticket |
| GET | `/Tickets` | autenticado | Lista todos os tickets |
| GET | `/Tickets/{id}` | autenticado | Consulta ticket |
| GET | `/Tickets/assigned` | Admin, Technician | Tickets atribuídos ao usuário logado |
| GET | `/Tickets/created-by-userid` | autenticado | Tickets criados pelo usuário logado |
| GET | `/Tickets/by-department` | autenticado | Tickets dos departamentos do usuário logado |
| PUT | `/Tickets/{id}/assign` | Admin, Technician | Atribui o ticket (`technicianId`) e muda o status para `InProgress` |
| PATCH | `/Tickets/{ticketId}/status` | autenticado (use case exige Admin/Technician e que não seja o criador) | Altera o status |
| POST | `/Tickets/{ticketId}/messages` | autenticado (criador, responsável ou Admin) | Envia mensagem no chat |
| GET | `/Tickets/{ticketId}/messages` | autenticado | Lista mensagens do ticket |
| GET | `/Notifications` | autenticado | Notificações não lidas do usuário |
| PATCH | `/Notifications/{id}/read` | autenticado (dono da notificação) | Marca como lida |

As listagens de tickets aceitam `page`, `pageSize` (máximo 50, exceto em `by-department`), `title`, `priority` e
`status`, e são ordenadas por data de criação decrescente.

A documentação interativa (Swagger UI) fica na **raiz** da aplicação (`/`), em qualquer ambiente; o documento
OpenAPI está em `/swagger/v1/swagger.json`. O Swagger aceita o token em "Authorize" no formato `Bearer {token}`.

---

## Autenticação e autorização

- JWT assinado com HS256, contendo as claims `NameIdentifier` (id), `Email` e uma claim `Role` por perfil. A
  validade é definida por `Jwt:ExpirationMinutes`. Emissor, audiência, assinatura e expiração são validados.
- O usuário logado chega aos use cases por `ICurrentUser` (`UserId`, `Email`, `Role`).
- A autorização é feita em duas camadas: `[Authorize(Roles = ...)]` no controller e regras dentro de alguns use cases.
- Usuários criados pelo Admin recebem uma senha temporária já expirada. O login retorna `mustChangePassword = true`
  quando a senha expirou; a troca obrigatória é conduzida pelo frontend.
- Não há refresh token nem revogação: desativar um usuário não invalida um token já emitido.

---

## Banco de dados e persistência

- SQL Server via EF Core. A aplicação usa a connection string `ConnectionStrings:DefaultConnection`.
- As migrations são aplicadas **automaticamente no startup** (`Database.Migrate()` em `Program.cs`); não é preciso
  rodar `dotnet ef database update` para executar a API.
- Seeds: apenas as três roles e um usuário técnico "System" (Id 1, sem senha e sem role).
- **Não existe seed do primeiro Admin.** Como só o Admin cria usuários, o primeiro administrador precisa ser criado
  diretamente no banco (registros em `Users`, `UserPasswords` com hash BCrypt, `UserRoles` com `RoleId = 1` e,
  para abrir tickets, `UserDepartments`).
- Datas são gravadas com `DateTime.Now` (hora local do servidor).
- A tabela `Logs` do Serilog é criada automaticamente no mesmo banco.

Para criar uma nova migration (nunca edite as existentes):

```bash
dotnet ef migrations add NomeDaMigration -p src/backend/Tickets.Infrastructure -s src/backend/Tickets.WebAPI
```

O design-time usa `TicketsDbContextFactory`: em `ASPNETCORE_ENVIRONMENT=Development` usa uma connection string fixa
no código, de uma máquina específica, e em `Production` lê a variável de ambiente `Azure__ConnectionString`. Em outra
máquina, ajuste a factory localmente ou defina o ambiente `Production` com essa variável.

---

## Mensageria e processamento assíncrono

E-mails e notificações usam o padrão **Outbox**:

```
Use case ──(mesma transação)──► OutboxMessages
OutboxProcessor (background, a cada 5 s, lotes de 10) ──► RabbitMQ (fila por tipo de evento)
RabbitMqConsumerService (background) ──► EventDispatcher ──► Handler
```

| Evento | Origem | Fila | Efeito |
|---|---|---|---|
| `CreateUserEmailEvent` | criação de usuário | `welcome-email` | e-mail de boas-vindas com a senha temporária |
| `PasswordRecoveryEmailEvent` | "esqueci minha senha" | `password-recovery-email` | e-mail com o link de redefinição |
| `TicketMessageCreatedEvent` | mensagem no chat | `ticket-notifications` | grava `Notification` para o criador e/ou o responsável |

- A API publica e consome as próprias mensagens no mesmo processo.
- A conexão com o RabbitMQ usa **TLS obrigatório** (fixo no código). Um RabbitMQ local sem TLS não funciona sem
  alterar o código.
- A entrega é *at-least-once*: não há deduplicação nos handlers, limite de tentativas nem dead-letter queue.

### E-mail (Brevo)

`BrevoEmailService` envia por `POST https://api.brevo.com/v3/smtp/email`. Os templates são HTML montados no código
(`UserEmailService`). O link do e-mail de redefinição usa `FrontendSettings:BaseUrl`; o link de login do e-mail de
boas-vindas é fixo para o domínio do front na Vercel.

---

## Logs e observabilidade

- Serilog grava no SQL Server (tabela `Logs`), nível mínimo `Information`, com as colunas extras `UserId`,
  `RequestPath`, `RequestMethod`, `ClientIp` e `RequestId` (preenchidas por `RequestLoggingMiddleware`), além de
  `UseSerilogRequestLogging`.
- A configuração do Serilog é feita em código em `Program.cs`; a seção `Serilog` do `appsettings.json` não é lida.
- Não há health checks nem métricas.

---

## Testes

```bash
dotnet test tests/Validators.Test
```

| Projeto | Conteúdo | Estado (verificado em 2026-11-20) |
|---|---|---|
| `tests/Validators.Test` | `CreateUserValidator` (6 testes) | compila e passa (6/6) |
| `tests/UseCases` | `CreateUserUseCase` | **não compila** (CS7036: o teste usa a assinatura antiga do construtor) |
| `tests/CommonTestUtilities` | builders de mocks e requests | — |

Por causa do projeto `UseCases`, `dotnet build Management.Tickets.sln` e `dotnet test` na solução falham, embora os
projetos de produção compilem. Os demais use cases (tickets, autenticação, departamentos, notificações...) e os
handlers não têm testes. Não há testes de integração, cobertura medida nem pipeline de CI.

---

## Como executar localmente

### Pré-requisitos

- .NET SDK 9
- SQL Server acessível
- RabbitMQ com TLS habilitado
- Conta no Brevo (chave de API e remetente)
- Opcional: `dotnet-ef` para criar migrations (`dotnet tool install --global dotnet-ef`)

### Configuração

Os arquivos `appsettings.json` e `appsettings.Development.json` trazem as chaves com os valores sensíveis em branco.
Forneça-os por variáveis de ambiente (o separador `:` vira `__`) e não versione segredos:

| Chave | Variável de ambiente | Descrição |
|---|---|---|
| `ConnectionStrings:DefaultConnection` | `ConnectionStrings__DefaultConnection` | Banco da aplicação e dos logs |
| `Jwt:SecretKey` | `Jwt__SecretKey` | Chave de assinatura do JWT |
| `Jwt:Issuer`, `Jwt:Audience`, `Jwt:ExpirationMinutes` | `Jwt__Issuer`... | Já preenchidas no `appsettings.json` |
| `BrevoSettings:ApiKey` | `BrevoSettings__ApiKey` | Chave da API do Brevo |
| `BrevoSettings:SenderEmail`, `BrevoSettings:SenderName` | `BrevoSettings__SenderEmail`... | Remetente (já preenchidos) |
| `RabbitMqSettings:Host`, `UserName`, `Password`, `VirtualHost` | `RabbitMqSettings__Host`... | Conexão com o RabbitMQ (`Port` já preenchida) |
| `FrontendSettings:BaseUrl` | `FrontendSettings__BaseUrl` | URL do front usada no link de redefinição de senha |

Exemplo (PowerShell, valores fictícios):

```powershell
$env:ConnectionStrings__DefaultConnection = "<sua-connection-string>"
$env:Jwt__SecretKey = "<chave-com-pelo-menos-32-caracteres>"
$env:BrevoSettings__ApiKey = "<sua-chave-brevo>"
$env:RabbitMqSettings__Host = "<host-rabbitmq>"
$env:RabbitMqSettings__UserName = "<usuario>"
$env:RabbitMqSettings__Password = "<senha>"
$env:RabbitMqSettings__VirtualHost = "<vhost>"
$env:FrontendSettings__BaseUrl = "http://localhost:4200"
```

O projeto não tem `UserSecretsId` configurado; para usar `dotnet user-secrets`, é preciso rodar
`dotnet user-secrets init` no `Tickets.WebAPI` (o que altera o `.csproj`).

### Execução

```bash
dotnet restore Management.Tickets.sln
dotnet run --project src/backend/Tickets.WebAPI --launch-profile https
```

- Perfil `https`: `https://localhost:7253` e `http://localhost:5072` (perfil `http`: só `http://localhost:5072`).
- Swagger UI: `https://localhost:7253/`.
- CORS liberado apenas para `http://localhost:4200` e para o domínio do front na Vercel (fixo em `Program.cs`).

### Docker

O `Dockerfile` na raiz publica o `Tickets.WebAPI` e expõe a porta 8080 (usado no deploy no Render):

```bash
docker build -t management-tickets-api .
docker run -p 8080:8080 -e ConnectionStrings__DefaultConnection="<...>" -e Jwt__SecretKey="<...>" management-tickets-api
```

---

## Estado atual e limitações

- **Autorização incompleta em alguns endpoints**: `GET /Tickets` lista todos os tickets para qualquer usuário
  autenticado; `GET /Tickets/{id}` e `GET /Tickets/{ticketId}/messages` não verificam se o usuário participa do
  ticket; criação de departamentos e categorias não exige Admin; qualquer Technician pode atribuir ou alterar o
  status de qualquer ticket.
- **Login com e-mail inexistente retorna 500**: a comparação com um hash fictício, feita para equalizar o tempo de
  resposta, lança exceção no BCrypt.
- **Criação de ticket sem validação**: título vazio ou categoria inexistente resultam em 500; a resposta retorna
  `id = 0`.
- A senha temporária do usuário criado fica em texto puro na tabela `OutboxMessages` e o conteúdo dos e-mails é
  escrito no console.
- Uma falha de banco durante o ciclo do `OutboxProcessor` encerra o serviço em background e, com isso, a aplicação.
- Não há rate limiting, lockout de login nem cabeçalhos de segurança (HSTS/CSP); o Swagger fica público também em
  produção.
- Testes quebrados e sem CI (ver [Testes](#testes)).

---

## Autor

**Igor Bueno**
[LinkedIn](https://www.linkedin.com/in/igorbuenov/) · Backend Developer (.NET | APIs | Segurança)

---

## Observação

Projeto desenvolvido com foco em evolução profissional e simulação de cenários reais de mercado.
