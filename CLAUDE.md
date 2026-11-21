# Management.Tickets (backend)

API REST em .NET 9 para gerenciamento de chamados. Clean Architecture com use cases.

## Comandos
```bash
dotnet build Management.Tickets.sln
dotnet test                                   # xUnit, projetos em tests/
dotnet run --project src/backend/Tickets.WebAPI --launch-profile https   # https://localhost:7253, /swagger
# Nova migration (existe TicketsDbContextFactory para design-time):
dotnet ef migrations add NomeDaMigration -p src/backend/Tickets.Infrastructure -s src/backend/Tickets.WebAPI
```
As migrations são aplicadas automaticamente no startup (`Database.Migrate()` em `Program.cs`).
Dependências externas: SQL Server, RabbitMQ, Brevo (e-mail).

## Estrutura (`src/`)
- `backend/Tickets.Domain`: entidades (`Entities/`, herdam `BaseEntity`), enums e interfaces de repositório
  (`Interfaces/Repositories`). Sem dependências de outras camadas.
- `backend/Tickets.Application`: use cases, DTOs, validators (FluentValidation), interfaces de serviços
  (`ICurrentUser`, `IJwtService`, `IEmailService`...), eventos e handlers. Registro de DI em `AppDIExtension.cs`.
- `backend/Tickets.Infrastructure`: EF Core (`Data/`, `Migrations/`), repositórios, JWT, hash de senha (BCrypt),
  mensageria RabbitMQ, outbox, e-mail (Brevo). Registro de DI em `InfraDIExtension.cs`.
- `backend/Tickets.WebAPI`: controllers, `Models/` (request/response HTTP), `Mappings/` (AutoMapper),
  `Filters/ExceptionFilter`, logging (Serilog em SQL Server), Swagger, JWT.
- `shared/Tickets.Exceptions`: exceções de negócio.
- `tests/`: `UseCases`, `Validators.Test`, `CommonTestUtilities` (builders de fakes/requests).

## Como adicionar um use case (padrão do projeto)
1. Pasta `Application/UseCases/<Area>/<NomeDoUseCase>/` com `I<Nome>UseCase.cs` e `<Nome>UseCase.cs`
   (método público `Execute(...)`).
2. DTOs em `Application/DTOs/<Area>/`. Validator (se houver entrada do usuário) em `Application/Validators/<Area>/`;
   validators são registrados por assembly scan.
3. Registrar em `AppDIExtension.cs` (`services.AddScoped<I...UseCase, ...UseCase>()`).
4. Se precisar de acesso a dados: interface em `Domain/Interfaces/Repositories`, implementação em
   `Infrastructure/Repositories`, registro em `InfraDIExtension.cs`.
5. Controller em `WebAPI/Controllers` injeta a **interface** do use case; request/response HTTP em
   `WebAPI/Models/<Area>/Request|Response`; mapeamentos em `WebAPI/Mappings/<Area>`. Controllers ficam finos.
6. Teste em `tests/UseCases/<Area>/<Nome>/`, usando os builders de `CommonTestUtilities`.

## Convenções
- Regra de negócio fica no use case, nunca no controller. Persistência: `repository.AddAsync(...)` +
  `await _unitOfWork.Commit()`.
- Erros: lance as exceções de `Tickets.Exceptions` (`NotFoundException`, `BusinessRuleException`,
  `ErrorOnValidationException`, `ForbiddenException`, `UnauthorizedException`, `AuthValidationException`).
  O `ExceptionFilter` converte em resposta HTTP; não monte status code dentro de use case.
- Usuário logado: `ICurrentUser` (`UserId`, `Role`). Autorização por role em duas camadas:
  `[Authorize(Roles = "Admin, Technician")]` no controller e checagem de regra dentro do use case.
- E-mails e notificações assíncronas: evento em `Application/Events`, handler em `Application/Handlers`,
  gravado via outbox e processado por `OutboxProcessor` → RabbitMQ → consumer. Leia `Infrastructure/Messaging/`
  antes de criar um evento novo.
- Mensagens de erro ao usuário em português.
- Testes: xUnit + FluentAssertions, blocos `// Arr`, `// Act`, `// Assert`, nomes como
  `Should_..._When_...` / `Throw_Exception_When_...`. Fakes e requests via builders (`CommonTestUtilities`).

## Cuidados
- Nunca editar migrations já existentes: crie uma nova.
- Não imprimir nem copiar valores de `appsettings*.json` (connection string, chave JWT, Brevo). Para segredos
  locais, prefira user-secrets ou variáveis de ambiente.
- O CORS e as origens permitidas estão fixos em `Program.cs`.
- Nomes com typo existem e são referenciados em vários pontos (`GetTicketsByDepartament`, `ICreateCategorytUseCase`):
  não renomear como efeito colateral de outra tarefa.
- Os use cases usam `DateTime.Now` (não UTC); manter consistência ou tratar como tarefa separada.
- O README ainda lista como pendente o que já existe (tickets, RabbitMQ, middleware de exceção). Não use o
  roadmap do README como fonte de verdade.

## Lacunas conhecidas
- Testes cobrem só criação de usuário (use case e validator). Tickets, auth e demais use cases não têm testes.
- `CreateTicketUseCase` e outros use cases de tickets não têm validator.
