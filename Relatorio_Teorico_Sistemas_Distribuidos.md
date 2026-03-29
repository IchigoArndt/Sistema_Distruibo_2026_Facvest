# Relatório Teórico — Sistemas Distribuídos
## Frameworks e Tecnologias Utilizadas no Projeto FitPortal

**Disciplina:** Sistemas Distribuídos  
**Instituição:** Facvest  
**Ano:** 2026  
**Projeto:** Sistema Distribuído 2026 — FitPortal  

---

## Sumário

1. [Introdução](#1-introdução)
2. [Visão Geral do Sistema Distribuído](#2-visão-geral-do-sistema-distribuído)
3. [Arquitetura Geral do Sistema](#3-arquitetura-geral-do-sistema)
4. [Módulo Backend — API REST com .NET 10](#4-módulo-backend--api-rest-com-net-10)
   - 4.1 [Framework ASP.NET Core](#41-framework-aspnet-core)
   - 4.2 [Clean Architecture no Backend](#42-clean-architecture-no-backend)
   - 4.3 [Padrão CQRS com MediatR](#43-padrão-cqrs-com-mediatr)
   - 4.4 [Autenticação e Autorização com JWT](#44-autenticação-e-autorização-com-jwt)
   - 4.5 [Validação com FluentValidation](#45-validação-com-fluentvalidation)
   - 4.6 [Injeção de Dependência](#46-injeção-de-dependência)
   - 4.7 [Persistência de Dados com Entity Framework Core](#47-persistência-de-dados-com-entity-framework-core)
   - 4.8 [Observabilidade com Serilog](#48-observabilidade-com-serilog)
   - 4.9 [Documentação Interativa com Scalar](#49-documentação-interativa-com-scalar)
   - 4.10 [CORS — Controle de Acesso entre Origens](#410-cors--controle-de-acesso-entre-origens)
5. [Módulo Frontend Web — Angular 20](#5-módulo-frontend-web--angular-20)
   - 5.1 [Framework Angular](#51-framework-angular)
   - 5.2 [Clean Architecture no Frontend](#52-clean-architecture-no-frontend)
   - 5.3 [Programação Reativa com RxJS](#53-programação-reativa-com-rxjs)
   - 5.4 [Componentes UI com PrimeNG](#54-componentes-ui-com-primeng)
   - 5.5 [Visualização de Dados com Chart.js](#55-visualização-de-dados-com-chartjs)
   - 5.6 [Interceptor HTTP e Autenticação no Frontend](#56-interceptor-http-e-autenticação-no-frontend)
6. [Módulo Mobile — Flutter](#6-módulo-mobile--flutter)
   - 6.1 [Framework Flutter e Dart](#61-framework-flutter-e-dart)
   - 6.2 [Arquitetura por Feature no Mobile](#62-arquitetura-por-feature-no-mobile)
   - 6.3 [Navegação no Flutter](#63-navegação-no-flutter)
7. [Comunicação Distribuída entre os Módulos](#7-comunicação-distribuída-entre-os-módulos)
   - 7.1 [Protocolo HTTP/REST](#71-protocolo-httprest)
   - 7.2 [Formato de Dados JSON](#72-formato-de-dados-json)
   - 7.3 [Estratégia de Autenticação Distribuída com JWT](#73-estratégia-de-autenticação-distribuída-com-jwt)
8. [Padrões de Projeto Aplicados](#8-padrões-de-projeto-aplicados)
9. [Princípios de Sistemas Distribuídos Identificados no Projeto](#9-princípios-de-sistemas-distribuídos-identificados-no-projeto)
10. [Estado Atual e Evolução do Sistema](#10-estado-atual-e-evolução-do-sistema)
11. [Considerações Finais](#11-considerações-finais)
12. [Referências](#12-referências)

---

## 1. Introdução

O presente relatório descreve e analisa, sob uma perspectiva teórica, os frameworks, padrões arquiteturais e tecnologias empregados no projeto **FitPortal — Sistema Distribuído 2026**, desenvolvido como trabalho prático para a disciplina de Sistemas Distribuídos da Facvest.

O sistema é composto por três módulos independentes que se comunicam por meio de uma rede: um **backend** (servidor de API REST), um **portal web** (cliente para profissionais de educação física) e um **aplicativo mobile** (cliente para alunos). Essa composição caracteriza um sistema distribuído, onde os componentes de software são executados em processos separados, potencialmente em máquinas distintas, e se coordenam por troca de mensagens via rede.

A análise a seguir aborda cada tecnologia e padrão utilizado, explicando sua fundamentação teórica e sua aplicação concreta no projeto.

---

## 2. Visão Geral do Sistema Distribuído

Um **sistema distribuído** é definido por Tanenbaum e Van Steen (2017) como "uma coleção de elementos computacionais autônomos que se apresenta a seus usuários como um sistema único e coerente". O FitPortal satisfaz essa definição ao reunir três processos autônomos que colaboram para oferecer uma experiência unificada.

```
┌─────────────────────────────────────────────────────────────────┐
│                     SISTEMA FITPORTAL                           │
│                                                                 │
│  ┌──────────────┐    HTTP/REST    ┌──────────────────────────┐  │
│  │  Portal Web  │ ◄────────────► │   API Backend (.NET 10)  │  │
│  │  (Angular)   │                │   SD_Server + SD_Auth     │  │
│  └──────────────┘                └──────────────┬───────────┘  │
│                                                 │               │
│  ┌──────────────┐    HTTP/REST                  │ SQL/MongoDB   │
│  │  App Mobile  │ ◄────────────►               │               │
│  │  (Flutter)   │                ┌──────────────▼───────────┐  │
│  └──────────────┘                │      Banco de Dados      │  │
│                                  └──────────────────────────┘  │
└─────────────────────────────────────────────────────────────────┘
```

**Características de sistemas distribuídos presentes no projeto:**

| Característica | Como se manifesta no FitPortal |
|---|---|
| **Concorrência** | Múltiplos clientes (web e mobile) acessam o servidor simultaneamente |
| **Ausência de relógio global** | Cada módulo tem seu ciclo de vida independente |
| **Falhas independentes** | O portal pode falhar sem derrubar o app mobile |
| **Heterogeneidade** | Tecnologias distintas (C#, TypeScript, Dart) comunicando-se por protocolo comum |
| **Transparência de acesso** | Os clientes acessam recursos remotos como se fossem locais (via HTTP) |
| **Escalabilidade** | A separação em serviços permite escalar o backend independentemente |

---

## 3. Arquitetura Geral do Sistema

O sistema adota uma **arquitetura cliente-servidor em múltiplas camadas**, com dois tipos de clientes (web e mobile) e um servidor centralizado que expõe dois endpoints HTTP distintos:

- **SD_Server** — API principal para operações de domínio (gestão de alunos, avaliações etc.)
- **SD_Server.Auth** — API dedicada à autenticação (emissão de tokens JWT)

Essa separação de responsabilidades entre as APIs segue o princípio de **separação de preocupações** (*Separation of Concerns*), um dos fundamentos da engenharia de software moderna.

```
Clientes                   Servidor                    Dados
─────────                  ────────                    ─────
Angular (Web)  ──HTTP──►  SD_Server.Auth  ──────────►  (Banco de Dados)
Flutter (Mobile) ──HTTP──►  SD_Server     ──EF Core──►  SQL Server/MongoDB
```

A comunicação entre todos os módulos é feita exclusivamente via **HTTP/REST com JSON**, o que garante interoperabilidade entre plataformas heterogêneas — um requisito fundamental em sistemas distribuídos.

---

## 4. Módulo Backend — API REST com .NET 10

### 4.1 Framework ASP.NET Core

O backend é construído sobre o **ASP.NET Core**, framework open-source da Microsoft para desenvolvimento de aplicações web e APIs em C#. A versão utilizada no projeto é o **.NET 10**, a versão mais recente da plataforma.

**Características relevantes do ASP.NET Core para sistemas distribuídos:**

- **Modelo de pipeline de middleware:** as requisições HTTP passam por uma cadeia de middlewares configuráveis (autenticação, CORS, roteamento, autorização), permitindo composição modular do comportamento do servidor.
- **Injeção de dependência nativa:** o framework possui um container de DI embutido, facilitando o desacoplamento entre componentes.
- **Suporte a múltiplos protocolos:** além de HTTP/REST, suporta WebSockets, gRPC e SignalR, o que facilita a evolução do sistema.
- **Alta performance:** o ASP.NET Core é um dos frameworks web de maior desempenho disponíveis, com suporte a processamento assíncrono nativo via `async/await`.

O ponto de entrada de cada API é o arquivo `Program.cs`, onde são registrados todos os serviços e configurados os middlewares:

```csharp
// SD_Server/Program.cs — configuração do pipeline
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbServices(builder.Configuration);
builder.Services.AddDefaultServices<Program>(container);
// ...
var app = builder.Build();
app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();
app.MapControllers();
app.Run();
```

### 4.2 Clean Architecture no Backend

O projeto backend é organizado segundo os princípios da **Clean Architecture**, proposta por Robert C. Martin (Uncle Bob). Essa arquitetura organiza o código em camadas concêntricas, onde as dependências sempre apontam para dentro (em direção ao núcleo do domínio).

**Camadas e projetos correspondentes:**

| Camada | Projeto .NET | Responsabilidade |
|---|---|---|
| **Apresentação** | `SD_Server`, `SD_Server.Auth` | Controllers HTTP, roteamento, serialização |
| **Aplicação** | `SD_Server.Application` | Casos de uso, handlers CQRS, orquestração |
| **Domínio** | `SD_Server.Domain` | Entidades, regras de negócio, interfaces de repositório |
| **Infraestrutura** | `SD_Server.Infra`, `SD_Server.Infra.Data` | Banco de dados, EF Core, migrações |
| **Compartilhados** | `SD_Api_Base`, `SD_Api_Extensions`, `SD_SharedKernel` | Utilitários transversais |

**Regra de dependência:**

```
SD_Server (API) → SD_Server.Application → SD_Server.Domain ← SD_Server.Infra
```

O domínio (`SD_Server.Domain`) não depende de nenhuma outra camada, garantindo que as regras de negócio sejam independentes de banco de dados, framework web ou qualquer detalhe de implementação.

**Entidade de domínio Student:**

```csharp
// SD_Server.Domain/Features/Students/Student.cs
public class Student : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
    public string CellPhone { get; set; }
    public StatusEnum Status { get; set; } = StatusEnum.Active;
    public DateTime LastReview { get; set; }
}
```

### 4.3 Padrão CQRS com MediatR

O projeto implementa o padrão **CQRS** (*Command Query Responsibility Segregation*), que separa as operações de leitura (Queries) das operações de escrita (Commands). Esse padrão é especialmente relevante em sistemas distribuídos, pois permite otimizar independentemente os caminhos de leitura e escrita.

**Biblioteca utilizada:** [MediatR](https://github.com/jbogard/MediatR) — implementação do padrão Mediator em .NET.

O padrão Mediator, descrito por Gamma et al. no livro *Design Patterns* (1994), define um objeto que encapsula como um conjunto de objetos interage, promovendo baixo acoplamento ao evitar que os objetos se refiram uns aos outros explicitamente.

**Fluxo de uma requisição com CQRS/MediatR:**

```
Controller → mediator.Send(Command) → ValidationPipeline → Handler → Result
```

**Exemplo — Comando de Login:**

```csharp
// LoginCommand representa a intenção de autenticar
public class LoginCommand : IRequest<Result<Exception, LoginDTO>>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

// Handler processa o comando
public class Handler : IRequestHandler<LoginCommand, Result<Exception, LoginDTO>>
{
    public async Task<Result<Exception, LoginDTO>> Handle(
        LoginCommand request, CancellationToken cancellationToken)
    {
        // lógica de autenticação + geração de JWT
    }
}
```

**Pipeline Behavior:** o MediatR suporta comportamentos transversais (*cross-cutting concerns*) via `IPipelineBehavior<TRequest, TResponse>`. O projeto utiliza o `ValidationPipeline` para interceptar todos os comandos e executar validação automática antes de chegar ao handler.

### 4.4 Autenticação e Autorização com JWT

O sistema implementa autenticação baseada em **JWT** (*JSON Web Token*), definido pela RFC 7519. O JWT é um padrão aberto para transmissão segura de informações entre partes como um objeto JSON assinado digitalmente.

**Estrutura de um JWT:**

```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9   ← Header (algoritmo + tipo)
.eyJzdWIiOiIxIiwiZW1haWwiOiIuLi4ifQ==  ← Payload (claims do usuário)
.HMAC_SHA256_SIGNATURE                  ← Assinatura digital
```

**Configuração do token no projeto:**

| Parâmetro | Valor |
|---|---|
| Algoritmo de assinatura | HMAC SHA-256 |
| Issuer / Audience | `"FITPortal"` |
| Expiração | 30 minutos |
| Chave secreta | Variável de ambiente `Secret` |

**Claims embutidas no token:**

| Claim | Significado |
|---|---|
| `NameIdentifier` | ID do usuário |
| `Email` | E-mail do usuário |
| `Name` | Nome do usuário |
| `Role` | Papel/permissão (ex: Admin) |

O uso de variável de ambiente para a chave secreta (`Environment.GetEnvironmentVariable("Secret")`) é uma boa prática de segurança, evitando que segredos sejam expostos no código-fonte ou em arquivos de configuração versionados.

**Tipos de acesso definidos:**

```csharp
// SD_Server.Auth/Domain/TypeAcess.cs
public enum TypeAcess : byte
{
    admin = 0
    // futuro: professor, aluno, etc.
}
```

### 4.5 Validação com FluentValidation

O projeto utiliza a biblioteca **FluentValidation** para definir regras de validação de forma declarativa e fluente, separando a lógica de validação dos modelos de dados.

```csharp
// loginCommandValidator.cs
public class loginCommandValidator : AbstractValidator<LoginCommand>
{
    public loginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Por favor digite o seu e-mail")
            .MinimumLength(6).WithMessage("E-mail deve ter ao menos 6 caracteres");
    }
}
```

A integração com o MediatR via `ValidationPipeline` garante que toda requisição seja validada automaticamente antes de chegar ao handler, seguindo o princípio *fail fast* — erros são detectados o mais cedo possível no pipeline.

**Resposta de erro padronizada:**

```json
{
  "errorCode": 400,
  "errorMessage": "Por favor digite o seu e-mail",
  "errors": [
    {
      "propertyName": "Email",
      "errorMessage": "Por favor digite o seu e-mail"
    }
  ]
}
```

### 4.6 Injeção de Dependência

O projeto utiliza dois containers de injeção de dependência em conjunto:

1. **DI nativo do ASP.NET Core** (`IServiceCollection`) — para serviços do framework (controllers, EF Core, autenticação).
2. **SimpleInjector** — container adicional com recursos avançados de verificação e diagnóstico.

A **Injeção de Dependência** (DI) é um padrão que implementa o princípio de Inversão de Dependência (o "D" do SOLID). Em vez de uma classe instanciar suas próprias dependências, elas são fornecidas externamente, promovendo baixo acoplamento e facilidade de testes.

**Registro automático via varredura de assemblies:**

```csharp
// Todos os handlers, validators e mappers são registrados automaticamente
builder.Services.AddDefaultServices<Program>(container);
// Equivale a:
// - AddMapper() → AutoMapper escaneia assemblies
// - AddMediator() → MediatR registra todos os IRequestHandler<>
// - AddValidators() → FluentValidation registra todos os AbstractValidator<>
// - AddSimpleInjector() → integra SimpleInjector com o DI nativo
```

### 4.7 Persistência de Dados com Entity Framework Core

O projeto utiliza o **Entity Framework Core** (EF Core) como ORM (*Object-Relational Mapper*) para acesso ao banco de dados. O EF Core abstrai o banco de dados relacional, permitindo que o código trabalhe com objetos C# em vez de SQL diretamente.

**DbContext configurado:**

```csharp
// SdServerDbContext.cs
public class SdServerDbContext : DbContext
{
    public DbSet<Student> Students { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(SdServerDbContext).Assembly);
    }
}
```

**Migrações automáticas:** o projeto executa `db.Database.Migrate()` na inicialização, aplicando automaticamente quaisquer migrações pendentes ao banco de dados. Isso garante que o esquema do banco esteja sempre sincronizado com o modelo de domínio.

**Suporte a MongoDB para logs:** o `Program.cs` verifica a variável de ambiente `Connection_Mongo` e, se configurada, direciona os logs do Serilog para uma coleção MongoDB, demonstrando suporte a persistência poliglota.

### 4.8 Observabilidade com Serilog

O projeto implementa **logging estruturado** com a biblioteca **Serilog**, que permite registrar eventos como objetos estruturados (em vez de strings simples), facilitando a busca e análise em sistemas de monitoramento.

```csharp
builder.Host.UseSerilog((_, _, configuration) =>
{
    configuration
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .WriteTo.Console(outputTemplate: "...")
        .WriteTo.MongoDB(mongo, "Logs"); // opcional, via env var
});
```

**Sinks configurados:**
- **Console** — saída padrão para desenvolvimento e contêineres
- **MongoDB** — armazenamento persistente de logs (quando configurado via variável de ambiente)

A observabilidade é um dos pilares dos sistemas distribuídos modernos, pois permite rastrear o comportamento do sistema em produção e diagnosticar falhas.

### 4.9 Documentação Interativa com Scalar

O projeto utiliza **Scalar** como interface de documentação interativa da API, em substituição ao Swagger/OpenAPI tradicional. O Scalar gera automaticamente uma interface web onde é possível visualizar e testar todos os endpoints.

```csharp
app.MapScalarApiReference(opt => opt
    .WithTitle("SD_Server_Api")
    .WithTheme(ScalarTheme.DeepSpace)
    .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient));
```

Acesso: `https://localhost:{porta}/scalar`

### 4.10 CORS — Controle de Acesso entre Origens

**CORS** (*Cross-Origin Resource Sharing*) é um mecanismo de segurança implementado pelos navegadores que restringe requisições HTTP entre origens diferentes. Em um sistema distribuído com frontend e backend em domínios distintos, a configuração correta de CORS é obrigatória.

O projeto configura CORS em ambas as APIs:

```csharp
// Configuração atual (desenvolvimento) — permite qualquer origem
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, builder =>
    {
        builder.AllowAnyOrigin();
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
    });
});
```

> **Nota de segurança:** a configuração `AllowAnyOrigin()` é adequada para desenvolvimento, mas em produção deve ser restrita às origens conhecidas (domínios do portal e do app mobile).

---

## 5. Módulo Frontend Web — Angular 20

### 5.1 Framework Angular

O portal web é desenvolvido com **Angular**, framework frontend mantido pelo Google, na versão **20**. Angular é um framework completo (*opinionated*) que fornece soluções integradas para roteamento, formulários, comunicação HTTP, injeção de dependência e gerenciamento de componentes.

**Características do Angular relevantes para o projeto:**

| Recurso | Descrição |
|---|---|
| **TypeScript** | Linguagem principal, com tipagem estática que reduz erros em tempo de desenvolvimento |
| **Componentes** | Unidades encapsuladas de UI com template, lógica e estilos |
| **Roteamento** | Sistema de navegação SPA (*Single Page Application*) com lazy loading |
| **DI nativo** | Container de injeção de dependência integrado ao framework |
| **RxJS** | Programação reativa para fluxos de dados assíncronos |
| **HttpClient** | Cliente HTTP com suporte a interceptors |

### 5.2 Clean Architecture no Frontend

O portal aplica os princípios da **Clean Architecture** também no frontend, organizando o código em quatro camadas:

```
portal/src/app/
├── core/          ← Infraestrutura Angular (guards, interceptors, auth service)
├── domain/        ← Entidades, repositórios (contratos), use cases
├── data/          ← Models, datasources, implementações de repositório
└── presentation/  ← Componentes Angular, páginas, layout
```

**Regra de dependência no frontend:**

```
Presentation → Domain ← Data
Core → Domain
Presentation NÃO acessa Data diretamente
```

Essa organização garante que a interface visual (Presentation) nunca dependa diretamente da fonte de dados (Data), comunicando-se sempre por meio dos contratos definidos no Domain.

**Entidade de domínio Aluno:**

```typescript
// domain/entities/aluno.entity.ts
export interface Aluno {
  id: number;
  nome: string;
  email: string;
  telefone: string;
  status: 'Ativo' | 'Inativo';
  ultimaAvaliacao?: string;
}
```

**Repositório como contrato (interface):**

```typescript
// domain/repositories/aluno.repository.ts
export abstract class AlunoRepository {
  abstract getAll(): Observable<Aluno[]>;
  abstract getById(id: number): Observable<Aluno>;
  abstract create(aluno: Omit<Aluno, 'id'>): Observable<Aluno>;
  abstract update(id: number, ...): Observable<Aluno>;
  abstract delete(id: number): Observable<void>;
}
```

**Use Case orquestrando o repositório:**

```typescript
// domain/usecases/aluno/get-alunos.usecase.ts
@Injectable({ providedIn: 'root' })
export class GetAlunosUseCase {
  constructor(private readonly alunoRepository: AlunoRepository) {}

  execute(): Observable<Aluno[]> {
    return this.alunoRepository.getAll();
  }
}
```

**Estratégia de datasource intercambiável:** atualmente os datasources retornam dados mockados (`of([...mockData])`). Quando o backend estiver disponível, basta substituir o datasource por uma implementação HTTP, sem alterar Domain ou Presentation.

### 5.3 Programação Reativa com RxJS

O projeto utiliza **RxJS** (*Reactive Extensions for JavaScript*) para gerenciar fluxos de dados assíncronos. RxJS implementa o padrão **Observer** e o paradigma de **programação reativa**, onde os dados fluem como streams que podem ser transformados, filtrados e combinados.

**Conceitos RxJS utilizados:**

| Conceito | Uso no projeto |
|---|---|
| `Observable<T>` | Tipo de retorno de todos os métodos de repositório e use case |
| `of(...)` | Cria um Observable a partir de valores estáticos (mock) |
| `pipe(map(...))` | Transforma os dados do stream (ex: AlunoModel → Aluno) |
| `subscribe(...)` | Componentes se inscrevem para receber os dados |

```typescript
// Componente consumindo um use case via RxJS
ngOnInit(): void {
  this.getAlunosUseCase.execute().subscribe(alunos => {
    this.alunos = alunos;
  });
}
```

### 5.4 Componentes UI com PrimeNG

O portal utiliza **PrimeNG** (versão 20) como biblioteca de componentes de interface. PrimeNG fornece componentes ricos e acessíveis como tabelas, formulários, diálogos, menus e muito mais, acelerando o desenvolvimento da interface.

**Componentes PrimeNG utilizados:**

- `p-table` — tabelas de dados com paginação, ordenação e filtros
- `p-button` — botões estilizados
- `p-dialog` — modais/diálogos
- `p-inputtext` — campos de texto
- `p-tag` — badges de status
- `p-sidebar` — menu lateral

O tema visual utilizado é o **Aura Theme** do PrimeNG, configurado globalmente no `app.config.ts`.

### 5.5 Visualização de Dados com Chart.js

O dashboard do portal utiliza **Chart.js** para renderizar gráficos interativos. Chart.js é uma biblioteca JavaScript de código aberto que permite criar gráficos de linha, barra, pizza e outros tipos.

No contexto do FitPortal, o gráfico de linha no dashboard exibe métricas de progresso dos alunos ao longo do tempo.

### 5.6 Interceptor HTTP e Autenticação no Frontend

O Angular possui um mecanismo de **interceptors** que permite interceptar e modificar todas as requisições HTTP antes de enviá-las ao servidor. O projeto utiliza o `auth.interceptor.ts` para adicionar automaticamente o token JWT em todas as requisições:

```typescript
// core/interceptors/auth.interceptor.ts
// Antes de enviar qualquer requisição HTTP:
// Authorization: Bearer eyJhbGciOiJIUzI1NiIsIn...
```

O **auth.guard.ts** protege as rotas internas, redirecionando para `/login` caso o usuário não esteja autenticado:

```typescript
export const authGuard: CanActivateFn = () => {
  if (authService.isLoggedIn()) return true;
  return router.createUrlTree(['/login']);
};
```

**Lazy Loading de rotas:** as páginas internas são carregadas sob demanda, reduzindo o tempo de carregamento inicial da aplicação:

```typescript
{
  path: 'alunos',
  loadComponent: () => import('./presentation/pages/alunos/alunos')
                         .then(m => m.AlunosComponent)
}
```

---

## 6. Módulo Mobile — Flutter

### 6.1 Framework Flutter e Dart

O aplicativo mobile é desenvolvido com **Flutter**, framework open-source do Google para desenvolvimento de aplicações nativas multiplataforma a partir de uma única base de código. Flutter utiliza a linguagem **Dart**.

**Características do Flutter relevantes para o projeto:**

| Característica | Descrição |
|---|---|
| **Multiplataforma** | Um único código gera apps para Android, iOS, Web, Windows, Linux e macOS |
| **Widgets** | Tudo no Flutter é um widget — unidade básica de composição de UI |
| **Hot Reload** | Alterações no código refletem instantaneamente no app em execução |
| **Dart** | Linguagem orientada a objetos com tipagem forte e suporte a `async/await` |
| **Material Design** | Suporte nativo ao sistema de design do Google |

**Versão do SDK Dart:** `^3.10.3`

### 6.2 Arquitetura por Feature no Mobile

O app mobile adota uma arquitetura inspirada no **Clean Architecture**, organizada por features (funcionalidades):

```
lib/
└── core/
    └── features/
        ├── login/
        │   ├── domain/
        │   │   ├── entities/UserAuthentication.dart
        │   │   └── services/IUserAuthenticationService.dart
        │   └── presentation/
        │       ├── login_presenter.dart
        │       └── styles/
        └── home/
            └── presentation/
                └── home_presenter.dart
```

**Camadas por feature:**

| Camada | Responsabilidade |
|---|---|
| `domain/entities` | Modelos de dados do negócio (ex: `UserAuthentication`) |
| `domain/services` | Interfaces/contratos para serviços (ex: `IUserAuthenticationService`) |
| `presentation` | Widgets (telas) e estilos visuais |
| `data` (a implementar) | Repositórios concretos e chamadas HTTP |

A entidade `UserAuthentication` encapsula as credenciais do usuário, e a interface `IUserAuthenticationService` define o contrato para o serviço de autenticação — seguindo o princípio de Inversão de Dependência.

### 6.3 Navegação no Flutter

O Flutter utiliza o conceito de **Navigator** para gerenciar a pilha de telas. O projeto usa `Navigator.pushNamedAndRemoveUntil` para limpar a pilha ao transitar entre login e home, impedindo que o usuário volte ao login após autenticar-se:

```dart
// main.dart — configuração de rotas nomeadas
MaterialApp(
  initialRoute: '/login',
  routes: {
    '/login': (context) => const LoginPage(),
    '/home': (context) => const HomePage(),
  },
)
```

**Fluxo de navegação:**

```
/login ──(autenticação bem-sucedida)──► /home
/home  ──(logout)──────────────────────► /login
```

---

## 7. Comunicação Distribuída entre os Módulos

### 7.1 Protocolo HTTP/REST

A comunicação entre todos os módulos é feita via **HTTP/REST** (*Representational State Transfer*). REST é um estilo arquitetural definido por Roy Fielding em sua dissertação de doutorado (2000), baseado nos seguintes princípios:

| Princípio REST | Aplicação no FitPortal |
|---|---|
| **Interface uniforme** | Endpoints padronizados com verbos HTTP (GET, POST, PUT, DELETE) |
| **Stateless** | Cada requisição contém todas as informações necessárias (JWT no header) |
| **Cliente-Servidor** | Separação clara entre frontend (cliente) e backend (servidor) |
| **Cacheável** | Respostas podem ser marcadas como cacheáveis |
| **Sistema em camadas** | Clientes não precisam saber se comunicam com o servidor final ou um proxy |

**Endpoints documentados:**

| Método | Rota | Descrição |
|---|---|---|
| `POST` | `/Auth/Login` | Autentica usuário e retorna token JWT |
| `GET` | `/User` | Lista usuários (em construção) |
| `GET/POST/PUT/DELETE` | `/Student` | CRUD de alunos (em construção) |

### 7.2 Formato de Dados JSON

Todas as trocas de dados entre os módulos utilizam **JSON** (*JavaScript Object Notation*), formato leve e legível por humanos, amplamente adotado em APIs REST.

**Exemplo de resposta de login bem-sucedido:**

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiration": "1800000"
}
```

**Exemplo de resposta de erro:**

```json
{
  "errorCode": 400,
  "errorMessage": "Por favor digite o seu e-mail",
  "errors": [
    {
      "propertyName": "Email",
      "errorMessage": "Por favor digite o seu e-mail"
    }
  ]
}
```

### 7.3 Estratégia de Autenticação Distribuída com JWT

Em sistemas distribuídos, a autenticação é um desafio especial: como garantir que um usuário autenticado em um serviço seja reconhecido por outros serviços sem compartilhar estado?

O JWT resolve esse problema de forma elegante: o token é **autocontido** — contém todas as informações necessárias para verificar a identidade do usuário sem consultar um banco de dados. Qualquer serviço que conheça a chave secreta pode validar o token.

**Fluxo de autenticação distribuída:**

```
1. Cliente → POST /Auth/Login {email, password}
2. SD_Server.Auth valida credenciais
3. SD_Server.Auth gera JWT com claims (id, email, role)
4. Cliente armazena JWT (localStorage no web, secure storage no mobile)
5. Cliente → GET /Student (Authorization: Bearer JWT)
6. SD_Server valida JWT e extrai claims
7. SD_Server retorna dados autorizados
```

---

## 8. Padrões de Projeto Aplicados

O projeto aplica diversos padrões de projeto clássicos e modernos:

| Padrão | Onde é aplicado | Benefício |
|---|---|---|
| **Clean Architecture** | Backend (.NET), Frontend (Angular), Mobile (Flutter) | Separação de responsabilidades, testabilidade, manutenibilidade |
| **CQRS** | Backend — MediatR | Separação de leitura e escrita, escalabilidade independente |
| **Mediator** | Backend — MediatR | Baixo acoplamento entre controllers e handlers |
| **Repository** | Backend (IRepositoryBase), Frontend (AlunoRepository) | Abstração do acesso a dados |
| **Result Type** | Backend — SD_SharedKernel | Tratamento explícito de sucesso/falha sem exceções |
| **Option Type** | Backend — SD_SharedKernel | Representação segura de valores opcionais |
| **Observer** | Frontend — RxJS Observables | Fluxos de dados reativos e assíncronos |
| **Interceptor** | Frontend — auth.interceptor.ts | Comportamento transversal em requisições HTTP |
| **Guard** | Frontend — auth.guard.ts | Proteção declarativa de rotas |
| **Pipeline Behavior** | Backend — ValidationPipeline | Comportamento transversal no pipeline de comandos |
| **Factory Method** | Backend — DI containers | Criação de objetos desacoplada da lógica de uso |

**O tipo `Result<TFailure, TSuccess>`** merece destaque especial. Inspirado em linguagens funcionais (como Haskell e F#), ele representa explicitamente que uma operação pode ter dois resultados:

```csharp
// SD_SharedKernel/Helpers/Result.cs
public struct Result<TFailure, TSuccess>
{
    public TFailure Failure { get; }
    public TSuccess Success { get; }
    public bool IsFailure { get; }
    public bool IsSuccess => !IsFailure;

    public TResult Match<TResult>(
        Func<TFailure, TResult> failure,
        Func<TSuccess, TResult> success)
        => IsFailure ? failure(Failure) : success(Success);
}
```

Esse padrão elimina o uso excessivo de `try/catch` e torna o fluxo de erros explícito e rastreável.

---

## 9. Princípios de Sistemas Distribuídos Identificados no Projeto

### 9.1 Transparência

O sistema busca oferecer **transparência de acesso** — os clientes (Angular e Flutter) acessam recursos remotos da mesma forma que acessariam recursos locais. O `AlunoDataSource` no Angular, por exemplo, pode retornar dados mockados ou dados HTTP com a mesma interface.

### 9.2 Escalabilidade

A separação em múltiplos serviços (`SD_Server` e `SD_Server.Auth`) permite escalar cada serviço independentemente conforme a demanda. O serviço de autenticação, por exemplo, pode ser replicado em múltiplas instâncias sem afetar a API principal.

### 9.3 Tolerância a Falhas

O tipo `Result<TFailure, TSuccess>` e a hierarquia de exceções (`BusinessException`, `ValidationException`) garantem que falhas sejam tratadas de forma controlada, sem propagar exceções não tratadas para o cliente.

### 9.4 Heterogeneidade

O sistema demonstra heterogeneidade tecnológica: três linguagens diferentes (C#, TypeScript, Dart), três frameworks distintos (.NET, Angular, Flutter), comunicando-se por um protocolo universal (HTTP/JSON). Essa heterogeneidade é uma característica fundamental de sistemas distribuídos reais.

### 9.5 Statelessness

Seguindo os princípios REST, o servidor não mantém estado de sessão entre requisições. Toda a informação necessária para autenticar e autorizar uma requisição está contida no JWT enviado pelo cliente.

### 9.6 Separação de Serviços

A existência de duas APIs distintas (`SD_Server` e `SD_Server.Auth`) é um passo em direção à arquitetura de **microsserviços**, onde cada serviço tem uma responsabilidade bem definida e pode ser desenvolvido, implantado e escalado independentemente.

---

## 10. Estado Atual e Evolução do Sistema

O projeto encontra-se em desenvolvimento ativo. A tabela abaixo resume o estado atual de cada módulo:

| Módulo | Funcionalidade | Estado |
|---|---|---|
| **Backend Auth** | Login com JWT | Implementado (lógica mock) |
| **Backend Auth** | Validação de credenciais no banco | Pendente |
| **Backend API** | CRUD de Alunos | Estrutura criada, lógica pendente |
| **Backend API** | Banco de dados (EF Core + Migrations) | Estrutura criada, migrations geradas |
| **Backend API** | Logs no MongoDB | Configurado via env var |
| **Frontend Web** | Portal com layout completo | Implementado |
| **Frontend Web** | Dados mockados | Implementado |
| **Frontend Web** | Integração HTTP com backend | Pendente (datasources prontos para troca) |
| **Mobile** | Telas de Login e Home | Implementado |
| **Mobile** | Autenticação hardcoded | Implementado (temporário) |
| **Mobile** | Integração HTTP com backend | Pendente |
| **Mobile** | Gerenciamento de estado | Pendente (Provider/Riverpod/BLoC) |

**Estratégia de evolução:** a arquitetura foi projetada para facilitar a transição de dados mockados para dados reais. No frontend Angular, basta substituir os `DataSources` por implementações HTTP — o Domain e a Presentation permanecem inalterados. No mobile Flutter, a camada `data` (a implementar) conterá os repositórios concretos que farão as chamadas HTTP.

---

## 11. Considerações Finais

O projeto FitPortal demonstra a aplicação prática de conceitos fundamentais de sistemas distribuídos em um contexto acadêmico realista. A escolha de tecnologias modernas e amplamente adotadas pela indústria — .NET 10, Angular 20 e Flutter — combinada com padrões arquiteturais sólidos (Clean Architecture, CQRS, Repository Pattern) resulta em um sistema bem estruturado e preparado para crescer.

**Pontos fortes da arquitetura:**

1. **Separação clara de responsabilidades** em todos os módulos, facilitando manutenção e evolução independente.
2. **Protocolo de comunicação universal** (HTTP/REST + JSON), garantindo interoperabilidade entre tecnologias heterogêneas.
3. **Autenticação stateless com JWT**, adequada para sistemas distribuídos sem compartilhamento de sessão.
4. **Padrões de projeto bem estabelecidos** (CQRS, Repository, Result Type) que tornam o código previsível e testável.
5. **Estratégia de dados intercambiáveis** no frontend, permitindo desenvolvimento desacoplado do backend.

**Desafios e próximos passos:**

1. Implementar a camada de dados real no mobile (Flutter) com chamadas HTTP.
2. Completar a lógica de autenticação no backend com busca real no banco de dados.
3. Implementar os endpoints de CRUD de alunos e avaliações.
4. Adicionar gerenciamento de estado no mobile (Provider, Riverpod ou BLoC).
5. Configurar CORS restritivo para produção.
6. Implementar refresh token para sessões de longa duração.

O sistema, mesmo em desenvolvimento, já demonstra os principais desafios e soluções de sistemas distribuídos: comunicação entre processos heterogêneos, autenticação distribuída, separação de responsabilidades e estratégias para lidar com falhas de forma controlada.

---

## 12. Referências

- **TANENBAUM, Andrew S.; VAN STEEN, Maarten.** *Distributed Systems: Principles and Paradigms*. 3. ed. Pearson, 2017.

- **MARTIN, Robert C.** *Clean Architecture: A Craftsman's Guide to Software Structure and Design*. Prentice Hall, 2017.

- **FIELDING, Roy Thomas.** *Architectural Styles and the Design of Network-based Software Architectures*. Dissertação de doutorado, University of California, Irvine, 2000.

- **GAMMA, Erich et al.** *Design Patterns: Elements of Reusable Object-Oriented Software*. Addison-Wesley, 1994.

- **JONES, Eric.** RFC 7519 — JSON Web Token (JWT). IETF, 2015. Disponível em: https://datatracker.ietf.org/doc/html/rfc7519

- **Microsoft.** *ASP.NET Core documentation*. Disponível em: https://learn.microsoft.com/aspnet/core

- **Google.** *Flutter documentation*. Disponível em: https://docs.flutter.dev

- **Google.** *Angular documentation*. Disponível em: https://angular.dev

- **BOGARD, Jimmy.** *MediatR — Simple mediator implementation in .NET*. Disponível em: https://github.com/jbogard/MediatR

- **SKINNER, Jeremy.** *FluentValidation documentation*. Disponível em: https://docs.fluentvalidation.net

- **REACTIVEX.** *RxJS — Reactive Extensions Library for JavaScript*. Disponível em: https://rxjs.dev

---

*Relatório gerado em 29 de março de 2026 — Disciplina de Sistemas Distribuídos — Facvest*
