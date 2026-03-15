# Arquitetura do Projeto SD_Server

> **Para quem é este documento?**
> Este documento foi escrito pensando em pessoas com pouca ou nenhuma experiência em C# e .NET. Todos os conceitos técnicos serão explicados de forma simples e progressiva.

---

## Sumário

1. [Visão Geral](#1-visão-geral)
2. [O que é Clean Architecture?](#2-o-que-é-clean-architecture)
3. [Estrutura de Projetos (Pastas)](#3-estrutura-de-projetos-pastas)
4. [Diagrama Geral da Arquitetura](#4-diagrama-geral-da-arquitetura)
5. [Cada Projeto em Detalhe](#5-cada-projeto-em-detalhe)
   - [SD_Server (API Principal)](#51-sd_server--api-principal)
   - [SD_Server.Auth (API de Autenticação)](#52-sd_serverauth--api-de-autenticação)
   - [SD_Server.Application (Camada de Aplicação)](#53-sd_serverapplication--camada-de-aplicação)
   - [SD_Server.Domain (Camada de Domínio)](#54-sd_serverdomain--camada-de-domínio)
   - [SD_Server.Infra (Camada de Infraestrutura)](#55-sd_serverinfra--camada-de-infraestrutura)
   - [SD_Api_Base (Base Compartilhada para APIs)](#56-sd_api_base--base-compartilhada-para-apis)
   - [SD_Api_Extensions (Extensões de Configuração)](#57-sd_api_extensions--extensões-de-configuração)
   - [SD_SharedKernel (Utilitários Compartilhados)](#58-sd_sharedkernel--utilitários-compartilhados)
6. [Como uma Requisição Percorre o Sistema](#6-como-uma-requisição-percorre-o-sistema)
7. [Padrão CQRS e MediatR](#7-padrão-cqrs-e-mediatr)
8. [Autenticação JWT](#8-autenticação-jwt)
9. [Tratamento de Erros](#9-tratamento-de-erros)
10. [O Tipo Result — Lidando com Sucesso e Falha](#10-o-tipo-result--lidando-com-sucesso-e-falha)
11. [Injeção de Dependência (SimpleInjector + .NET DI)](#11-injeção-de-dependência-simpleinjector--net-di)
12. [CORS — Quem pode chamar a API?](#12-cors--quem-pode-chamar-a-api)
13. [Documentação Interativa com Scalar](#13-documentação-interativa-com-scalar)
14. [Dependências (Bibliotecas Externas)](#14-dependências-bibliotecas-externas)
15. [Fluxo Completo de Login](#15-fluxo-completo-de-login)
16. [Como Adicionar uma Nova Funcionalidade](#16-como-adicionar-uma-nova-funcionalidade)

---

## 1. Visão Geral

O projeto **SD_Server** é um backend (servidor) construído em **C# com .NET 10**, organizado como uma **Web API REST**. Isso significa que ele recebe requisições HTTP (como as que um navegador ou aplicativo mobile faz) e responde com dados em formato JSON.

O projeto é dividido em **múltiplos subprojetos** (chamados de *assemblies* ou *projetos* no .NET), cada um com uma responsabilidade bem definida. Essa separação segue o padrão **Clean Architecture**.

```
┌─────────────────────────────────────────────────────────────┐
│                        CLIENTE                              │
│            (Navegador, App Mobile, Postman...)               │
└──────────────────────────┬──────────────────────────────────┘
                           │  HTTP Request (JSON)
                           ▼
┌─────────────────────────────────────────────────────────────┐
│                     SD_Server (API)                         │
│                  SD_Server.Auth (Auth)                      │
└──────────────────────────┬──────────────────────────────────┘
                           │
                    (Lógica de Negócio)
                           │
              ┌────────────┴────────────┐
              ▼                         ▼
   SD_Server.Application      SD_Server.Domain
              │
              ▼
   SD_Server.Infra (Banco de Dados, arquivos...)
```

---

## 2. O que é Clean Architecture?

Imagine uma cebola: ela tem camadas, e as camadas internas não sabem nada sobre as externas.

```mermaid
graph TD
    subgraph "Camada Externa (Apresentação)"
        A[SD_Server - Controllers/API]
        B[SD_Server.Auth - Controllers/API]
    end
    subgraph "Camada de Aplicação"
        C[SD_Server.Application - Casos de Uso]
    end
    subgraph "Camada de Domínio (Núcleo)"
        D[SD_Server.Domain - Regras de Negócio]
    end
    subgraph "Camada de Infraestrutura"
        E[SD_Server.Infra - Banco de Dados]
    end
    subgraph "Compartilhados"
        F[SD_SharedKernel]
        G[SD_Api_Base]
        H[SD_Api_Extensions]
    end

    A --> C
    B --> C
    C --> D
    C --> E
    D --> F
    A --> G
    B --> G
    A --> H
    B --> H
```

**Regra principal:** As camadas internas (Domain) **nunca** dependem das externas (Infra, API). Isso garante que a lógica de negócio seja independente de banco de dados, framework web, etc.

---

## 3. Estrutura de Projetos (Pastas)

```
Server/
├── SD_Server/                  ← API principal (ponto de entrada do servidor)
├── SD_Server.Auth/             ← API de autenticação (login, JWT)
├── SD_Server.Application/      ← Casos de uso / lógica de aplicação
├── SD_Server.Domain/           ← Regras de negócio, exceções de domínio
├── SD_Server.Infra/            ← Infraestrutura (banco de dados, configs de ambiente)
├── SD_Api_Base/                ← Classe base para controllers e pipelines
├── SD_Api_Extensions/          ← Extensões de configuração (DI, CORS, Mediator...)
├── SD_SharedKernel/            ← Tipos utilitários compartilhados (Result, Option...)
└── CORSExtensions/             ← Projeto legado/auxiliar de CORS (não utilizado ativamente)
```

> **O que é um "projeto" no .NET?**
> É como uma pasta especial que compila separadamente e gera uma biblioteca (`.dll`) ou executável (`.exe`). Projetos podem referenciar outros projetos, como se fossem importações.

---

## 4. Diagrama Geral da Arquitetura

```mermaid
flowchart LR
    Cliente["🌐 Cliente\n(Browser / App)"]

    subgraph APIs["APIs (Servidores HTTP)"]
        ApiMain["SD_Server\n(API Principal)"]
        ApiAuth["SD_Server.Auth\n(API de Auth)"]
    end

    subgraph Shared["Bibliotecas Compartilhadas"]
        ApiBase["SD_Api_Base\n(Base Controllers)"]
        ApiExt["SD_Api_Extensions\n(Configurações)"]
        SharedKernel["SD_SharedKernel\n(Result, Option, Unit)"]
    end

    subgraph Core["Núcleo da Aplicação"]
        App["SD_Server.Application\n(Handlers / Casos de Uso)"]
        Domain["SD_Server.Domain\n(Exceções de Negócio)"]
        Infra["SD_Server.Infra\n(Banco / Ambiente)"]
    end

    Cliente -->|"HTTP POST /Auth/Login"| ApiAuth
    Cliente -->|"HTTP GET /Users"| ApiMain

    ApiAuth --> ApiBase
    ApiMain --> ApiBase
    ApiAuth --> ApiExt
    ApiMain --> ApiExt

    ApiAuth --> App
    ApiMain --> App

    App --> Domain
    App --> Infra
    App --> SharedKernel
    Domain --> SharedKernel
    ApiBase --> SharedKernel
    ApiBase --> Domain
```

---

## 5. Cada Projeto em Detalhe

### 5.1 `SD_Server` — API Principal

**O que faz:** É o ponto de entrada principal do servidor. Recebe requisições HTTP e as direciona para os handlers corretos.

**Arquivos importantes:**

| Arquivo | O que faz |
|---|---|
| `Program.cs` | Inicializa o servidor (configura serviços, rotas, middlewares) |
| `appsettings.json` | Configurações gerais (logs, hosts permitidos) |
| `Controllers/Users/UserController.cs` | Controller de usuários (ainda em construção) |
| `BusinessException.cs` | Cópia local da exceção de negócio |
| `ErrorCodes.cs` | Cópia local dos códigos de erro |

> **O que é um Controller?**
> É uma classe que "escuta" uma rota HTTP. Por exemplo, `UserController` escuta requisições em `/User`. Cada método dentro dele responde a um verbo HTTP (GET, POST, PUT, DELETE).

```mermaid
sequenceDiagram
    participant C as Cliente
    participant Ctrl as Controller
    participant M as MediatR
    participant H as Handler

    C->>Ctrl: POST /Auth/Login { email, password }
    Ctrl->>M: mediator.Send(LoginCommand)
    M->>H: Handle(LoginCommand)
    H-->>M: Result<Exception, LoginDTO>
    M-->>Ctrl: Result<Exception, LoginDTO>
    Ctrl-->>C: 200 OK { token, expiration }
```

---

### 5.2 `SD_Server.Auth` — API de Autenticação

**O que faz:** É uma API separada responsável exclusivamente por autenticação. Gera tokens JWT para usuários que fazem login.

**Estrutura interna:**

```
SD_Server.Auth/
├── Controllers/
│   └── AuthController.cs       ← Recebe a requisição de login
├── Commands/
│   ├── LoginCommand.cs         ← Objeto que representa o pedido de login
│   └── loginCommandValidator.cs ← Valida email e senha antes de processar
├── Handlers/
│   └── AuthLogin.cs            ← Lógica real do login + geração de JWT
├── Domain/
│   └── TypeAcess.cs            ← Enum com tipos de acesso (admin, etc.)
├── DTO/
│   └── LoginDTO.cs             ← Objeto de resposta (token + expiração)
└── Extensions/
    └── MapScalarAuthExtensions.cs ← Configuração da documentação Scalar
```

> **O que é um DTO?**
> *Data Transfer Object* — é um objeto simples usado apenas para transportar dados entre camadas. Ele não tem lógica, só propriedades.

> **O que é um Command?**
> No padrão CQRS (explicado na seção 7), um *Command* representa uma **intenção de mudança**. `LoginCommand` representa "quero fazer login com este email e senha".

---

### 5.3 `SD_Server.Application` — Camada de Aplicação

**O que faz:** Contém os casos de uso da aplicação. Atualmente tem apenas a classe marcadora `ApplicationModule`, que serve como referência para o assembly (permite que outros projetos encontrem os handlers registrados aqui).

```csharp
// ApplicationModule.cs — classe vazia que serve como "âncora" para o assembly
public class ApplicationModule { }
```

> **Por que uma classe vazia?**
> No .NET, para escanear um assembly (conjunto de código compilado) em busca de handlers, validators, etc., precisamos de uma referência a um tipo desse assembly. `ApplicationModule` serve exatamente para isso.

---

### 5.4 `SD_Server.Domain` — Camada de Domínio

**O que faz:** Contém as regras de negócio e as exceções específicas do domínio da aplicação.

**Arquivos:**

| Arquivo | O que faz |
|---|---|
| `Exceptions/BusinessException.cs` | Exceção base para erros de negócio |
| `Exceptions/ErrorCodes.cs` | Enum com todos os códigos de erro HTTP |

```mermaid
classDiagram
    class BusinessException {
        +ErrorCodes ErrorCode
        +string Message
        +BusinessException(errorCode, message)
    }

    class ErrorCodes {
        <<enumeration>>
        BadRequest = 400
        Unauthorized = 401
        Forbidden = 403
        NotFound = 404
        AlreadyExists = 409
        NotAllowed = 405
        InvalidObject = 422
        Unhandled = 500
        ServiceUnavailable = 503
    }

    BusinessException --> ErrorCodes
```

> **O que é uma exceção de negócio?**
> Diferente de um erro técnico (ex: banco de dados fora do ar), uma exceção de negócio representa uma violação de regra do sistema. Ex: "Usuário não encontrado", "Senha incorreta". Ao lançar uma `BusinessException`, o sistema sabe que deve retornar um erro específico para o cliente.

---

### 5.5 `SD_Server.Infra` — Camada de Infraestrutura

**O que faz:** Responsável por tudo que é "externo" ao sistema: banco de dados, variáveis de ambiente, arquivos, etc.

**Arquivos:**

| Arquivo | O que faz |
|---|---|
| `Configurations/EnviromentConfiguration.cs` | Carrega e disponibiliza configurações do ambiente |
| `InfraDataModule.cs` | Classe marcadora do assembly de Infra |

> **Nota:** O banco de dados ainda não está implementado. Os comentários no código indicam onde será adicionado (ex: `//builder.Services.AddDbServices(...)`).

---

### 5.6 `SD_Api_Base` — Base Compartilhada para APIs

**O que faz:** Fornece a classe base `ApiControllerBase` que todos os controllers herdam. Centraliza o tratamento de respostas HTTP.

**Arquivos:**

| Arquivo | O que faz |
|---|---|
| `Base/ApiControllerBase.cs` | Classe base com métodos para tratar Result e retornar HTTP |
| `Behavious/ValidationPipeline.cs` | Pipeline de validação automática com FluentValidation |
| `Exceptions/ExceptionPayload.cs` | Objeto de resposta de erro enviado ao cliente |
| `Exceptions/SeedException.cs` | Exceção para erros de seed (dados iniciais) |

```mermaid
classDiagram
    class ControllerBase {
        <<ASP.NET Core>>
    }

    class ApiControllerBase {
        +HandleCommand(result) IActionResult
        +HandleQuery(result) IActionResult
        +HandleQueryable(result) IActionResult
        +HandleFailure(exception) IActionResult
    }

    class AuthController {
        +Login(command) IActionResult
    }

    class UserController {
        +Index() IActionResult
    }

    ControllerBase <|-- ApiControllerBase
    ApiControllerBase <|-- AuthController
    ControllerBase <|-- UserController
```

**Como `HandleCommand` funciona:**

```
Result<Exception, LoginDTO>
       │
       ├── IsFailure = true  → HandleFailure() → retorna 400/500 com ExceptionPayload
       │
       └── IsSuccess = true  → Ok(result.Success) → retorna 200 com os dados
```

---

### 5.7 `SD_Api_Extensions` — Extensões de Configuração

**O que faz:** Centraliza toda a configuração dos serviços da aplicação em métodos de extensão reutilizáveis.

```mermaid
graph TD
    Program["Program.cs\n(ponto de entrada)"]
    
    Program -->|"AddDefaultServices()"| DefaultExt["DefaultServiceExtensions\n.AddDefaultServices()"]
    
    DefaultExt -->|"AddMapper()"| MapperExt["ServiceCollectionExtensions\n.AddMapper() — AutoMapper"]
    DefaultExt -->|"AddSimpleInjector()"| SIExt["SimpleInjectorExtensions\n.AddSimpleInjector()"]
    DefaultExt -->|"AddMediator()"| MedExt["MediatorExtensions\n.AddMediator() — MediatR"]
    DefaultExt -->|"AddValidators()"| ValExt["FluentValidationExtensions\n.AddValidators()"]
    
    Program -->|"UseCORS()"| CORSExt["CORSExtensions\n.UseCORS()"]
```

**Arquivos:**

| Arquivo | O que faz |
|---|---|
| `DefaultServiceExtensions.cs` | Registra todos os serviços padrão de uma vez |
| `MediatorExtensions.cs` | Configura o MediatR e o pipeline de validação |
| `FluentValidationExtensions.cs` | Registra todos os validators automaticamente |
| `SimpleInjectorExtensions.cs` | Integra o SimpleInjector com o ASP.NET Core |
| `ServiceCollectionExtensions.cs` | Registra AutoMapper e arquivos estáticos |
| `CORSExtensions.cs` | Configura política de CORS |
| `SettingsExtensions.cs` | Carrega seções do `appsettings.json` |
| `Settings/AppSettings.cs` | Modelo para connection string |
| `Settings/CORSSettings.cs` | Modelo para configurações de CORS |

---

### 5.8 `SD_SharedKernel` — Utilitários Compartilhados

**O que faz:** Contém tipos genéricos e utilitários usados por todos os outros projetos.

```mermaid
classDiagram
    class Result~TFailure, TSuccess~ {
        +TFailure Failure
        +TSuccess Success
        +bool IsFailure
        +bool IsSuccess
        +Match(failure, success) TResult
        +implicit operator(TFailure)
        +implicit operator(TSuccess)
        +Of(TSuccess) Result
        +Of(TFailure) Result
    }

    class Option~T~ {
        +T Value
        +bool IsSome
        +bool IsNone
        +Match(some, none) TR
        +static None
    }

    class Unit {
        +static Sucessful
    }

    class NoneType {
        <<struct>>
    }

    class Helper {
        +static None NoneType
        +Some(value) Option~T~
        +Unit() Unit
        +ToFunc(action) Func
    }

    Helper --> Option
    Helper --> Unit
    Helper --> NoneType
    Option --> NoneType
```

---

## 6. Como uma Requisição Percorre o Sistema

Vamos usar o login como exemplo concreto:

```mermaid
flowchart TD
    A["🌐 Cliente envia:\nPOST /Auth/Login\n{ email, password }"]
    
    B["AuthController.Login()\nRecebe o LoginCommand"]
    
    C["MediatR.Send(LoginCommand)\nDespacha o comando"]
    
    D["ValidationPipeline\nValida email e senha\ncom loginCommandValidator"]
    
    E{Validação OK?}
    
    F["AuthLogin.Handler.Handle()\nLógica de autenticação"]
    
    G{Usuário válido?}
    
    H["GenerateToken()\nCria token JWT"]
    
    I["Retorna LoginDTO\n{ token, expiration }"]
    
    J["HandleCommand(result)\nRetorna 200 OK com LoginDTO"]
    
    K["HandleFailure(exception)\nRetorna 400 com ExceptionPayload"]
    
    A --> B
    B --> C
    C --> D
    D --> E
    E -->|"Não"| K
    E -->|"Sim"| F
    F --> G
    G -->|"Não"| K
    G -->|"Sim"| H
    H --> I
    I --> J
```

---

## 7. Padrão CQRS e MediatR

### O que é CQRS?

**CQRS** = *Command Query Responsibility Segregation* (Separação de Responsabilidade entre Comandos e Consultas).

A ideia é simples:
- **Command** = operações que **mudam** dados (login, criar usuário, deletar...)
- **Query** = operações que apenas **leem** dados (buscar usuário, listar produtos...)

### O que é MediatR?

É uma biblioteca que implementa o padrão **Mediator**: em vez do controller chamar diretamente o serviço, ele envia uma mensagem para o MediatR, que encontra e chama o handler correto.

```mermaid
sequenceDiagram
    participant Ctrl as Controller
    participant Med as MediatR
    participant Val as ValidationPipeline
    participant H as Handler

    Ctrl->>Med: mediator.Send(LoginCommand)
    Med->>Val: Intercepta (Pipeline Behavior)
    Val->>Val: Valida o comando
    alt Validação falhou
        Val-->>Med: ValidationException
        Med-->>Ctrl: Result.Failure
    else Validação OK
        Val->>H: Chama o Handler
        H-->>Val: Result<Exception, LoginDTO>
        Val-->>Med: Result<Exception, LoginDTO>
        Med-->>Ctrl: Result<Exception, LoginDTO>
    end
```

**Por que usar MediatR?**
- O controller não precisa conhecer o handler diretamente (baixo acoplamento)
- Fácil adicionar comportamentos transversais (logging, validação, cache) via pipelines
- Facilita testes unitários

### Como criar um novo Command + Handler

```
1. Crie o Command (ex: CreateUserCommand.cs)
   - Implemente IRequest<Result<Exception, SeuDTO>>
   - Adicione as propriedades necessárias

2. Crie o Validator (ex: CreateUserCommandValidator.cs)
   - Herde de AbstractValidator<CreateUserCommand>
   - Defina as regras com RuleFor(...)

3. Crie o Handler (ex: CreateUser.cs)
   - Crie uma classe Handler que implemente IRequestHandler<CreateUserCommand, Result<Exception, SeuDTO>>
   - Implemente o método Handle(...)

4. No Controller, injete IMediator e chame mediator.Send(command)
```

---

## 8. Autenticação JWT

### O que é JWT?

**JWT** = *JSON Web Token*. É um token (como um crachá digital) que o servidor gera após o login e o cliente usa em todas as requisições seguintes para provar que está autenticado.

```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwiZW1haWwiOiJhZG1pbkB0ZXN0LmNvbSJ9.assinatura
│                                      │                                                    │
└─── Header (algoritmo)                └─── Payload (dados do usuário)                     └─── Assinatura
```

### Como o sistema gera o JWT

```mermaid
flowchart LR
    A["Email + Senha\ndo usuário"]
    B["Valida credenciais"]
    C["Busca usuário no banco\n(a implementar)"]
    D["Cria lista de Claims\n(id, email, nome, role)"]
    E["Assina com chave secreta\n(variável de ambiente 'Secret')"]
    F["Retorna token JWT\n+ data de expiração"]

    A --> B --> C --> D --> E --> F
```

**Claims** são informações embutidas no token:

| Claim | Valor no código | Significado |
|---|---|---|
| `NameIdentifier` | `"1"` | ID do usuário |
| `Email` | email do login | Email do usuário |
| `Name` | `"Admin"` | Nome do usuário |
| `Role` | `"Admin"` | Papel/permissão do usuário |

**Configurações do token:**
- **Issuer/Audience:** `"MatrixCompetency"` (identifica quem emitiu o token)
- **Expiração:** 30 minutos
- **Algoritmo:** HMAC SHA-256

> **Importante:** A chave secreta (`Secret`) deve ser configurada como **variável de ambiente**, nunca no código ou no `appsettings.json`.

### TypeAcess — Tipos de Acesso

```csharp
// SD_Server.Auth/Domain/TypeAcess.cs
public enum TypeAcess : byte
{
    admin = 0
    // Aqui serão adicionados outros tipos: professor, aluno, etc.
}
```

---

## 9. Tratamento de Erros

O sistema tem uma hierarquia bem definida para tratar erros:

```mermaid
flowchart TD
    E["Exception\n(base do .NET)"]
    
    BE["BusinessException\n(regra de negócio violada)\nEx: usuário não encontrado"]
    
    VE["ValidationException\n(FluentValidation)\nEx: email inválido"]
    
    SE["SeedException\n(erro ao popular dados iniciais)"]
    
    EP["ExceptionPayload\n(objeto enviado ao cliente)\n{ errorCode, errorMessage, errors }"]
    
    E --> BE
    E --> VE
    E --> SE
    
    BE -->|"400 BadRequest"| EP
    VE -->|"400 BadRequest"| EP
    E -->|"500 InternalServerError"| EP
```

### Como o `ApiControllerBase` decide o status HTTP

```
HandleFailure(exception):
├── exception é BusinessException?
│   └── → 400 Bad Request + ErrorCode da exceção
├── exception é ValidationException (FluentValidation)?
│   └── → 400 Bad Request + lista de erros de validação
└── qualquer outra Exception
    └── → 500 Internal Server Error
```

### Exemplo de resposta de erro

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

---

## 10. O Tipo `Result` — Lidando com Sucesso e Falha

Em vez de usar `try/catch` em todo lugar, o projeto usa um tipo chamado `Result<TFailure, TSuccess>` que representa explicitamente que uma operação pode ter dois resultados: sucesso ou falha.

### Como funciona

```csharp
// Retornar sucesso:
return new LoginDTO { Token = "...", Expiration = "..." };
// (conversão implícita para Result<Exception, LoginDTO>)

// Retornar falha:
return new Exception("Usuário não encontrado");
// (conversão implícita para Result<Exception, LoginDTO>)

// Verificar o resultado:
if (result.IsFailure)
    return HandleFailure(result.Failure);  // trata o erro
else
    return Ok(result.Success);             // retorna os dados
```

### Analogia simples

Imagine que você pede uma pizza. O entregador pode trazer:
- **Sucesso:** a pizza chegou (`result.Success = pizza`)
- **Falha:** algo deu errado (`result.Failure = new Exception("Pizzaria fechada")`)

O `Result` é como a caixa que pode conter qualquer um dos dois.

### Tipo `Option<T>`

Similar ao `Result`, mas para valores que podem ou não existir (como um `null` seguro):

```csharp
Option<Usuario> usuario = buscarUsuario(id);

usuario.Match(
    some: u => Console.WriteLine($"Encontrado: {u.Nome}"),
    none: () => Console.WriteLine("Usuário não encontrado")
);
```

### Tipo `Unit`

Representa "sem valor" — usado quando uma operação não retorna nada significativo (equivalente ao `void`, mas compatível com o tipo `Result`).

---

## 11. Injeção de Dependência (SimpleInjector + .NET DI)

### O que é Injeção de Dependência?

Em vez de uma classe criar suas próprias dependências (`new ServicoX()`), elas são **injetadas** automaticamente pelo framework. Isso facilita testes e troca de implementações.

```mermaid
graph LR
    subgraph "Sem DI (ruim)"
        A1["AuthController"]
        A1 -->|"new AuthService()"| B1["AuthService"]
        B1 -->|"new DbContext()"| C1["DbContext"]
    end

    subgraph "Com DI (bom)"
        A2["AuthController\n(recebe IMediator)"]
        A2 -.->|"injetado"| B2["IMediator\n(MediatR)"]
        B2 -.->|"injetado"| C2["Handler\n(registrado)"]
    end
```

### O projeto usa dois containers de DI

1. **.NET DI nativo** (`IServiceCollection`) — padrão do ASP.NET Core
2. **SimpleInjector** — container adicional com recursos avançados

A integração entre os dois é feita em `SimpleInjectorExtensions.cs`.

### O que é registrado automaticamente

| Serviço | Como é registrado |
|---|---|
| Controllers | `AddControllers()` |
| AutoMapper | `AddMapper<TAssembly>()` — escaneia assemblies |
| MediatR + Handlers | `AddMediator()` — escaneia assemblies |
| FluentValidation Validators | `AddValidators()` — escaneia assemblies |
| SimpleInjector | `AddSimpleInjector()` |

---

## 12. CORS — Quem pode chamar a API?

**CORS** = *Cross-Origin Resource Sharing*. É uma política de segurança do navegador que impede que sites desconhecidos façam requisições à sua API.

### Configuração atual (SD_Server.Auth)

```csharp
// Program.cs — permite qualquer origem (modo desenvolvimento)
builder.WithOrigins("*");
builder.AllowAnyMethod();
builder.AllowAnyHeader();
```

> **Atenção:** `"*"` (qualquer origem) é aceitável em desenvolvimento, mas em produção deve ser restrito às origens conhecidas.

### Configuração padrão (CORSSettings.Default)

```csharp
Origins = [
    "https://localhost:7002",
    // ...outros ambientes de teste
];
Methods = ["*"];
Headers = ["*"];
```

---

## 13. Documentação Interativa com Scalar

O projeto usa **Scalar** (alternativa moderna ao Swagger) para gerar uma interface web interativa onde é possível testar os endpoints da API.

**Acesso:** Após rodar o projeto, acesse `https://localhost:{porta}/scalar`

**Configuração:**
```csharp
app.MapScalarApiReference(opt => opt
    .WithTitle("SD_Server_Auth")
    .WithTheme(ScalarTheme.DeepSpace)
    .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
);
```

---

## 14. Dependências (Bibliotecas Externas)

| Biblioteca | Para que serve |
|---|---|
| **MediatR** | Implementa o padrão Mediator (CQRS) |
| **FluentValidation** | Validação de objetos com regras fluentes |
| **AutoMapper** | Mapeamento automático entre objetos (ex: Domain → DTO) |
| **SimpleInjector** | Container de injeção de dependência |
| **Scalar.AspNetCore** | Documentação interativa da API |
| **Microsoft.AspNetCore.Authentication.JwtBearer** | Autenticação via tokens JWT |
| **System.IdentityModel.Tokens.Jwt** | Criação e validação de tokens JWT |

---

## 15. Fluxo Completo de Login

Este é o fluxo mais completo implementado atualmente no sistema:

```mermaid
sequenceDiagram
    actor U as Usuário
    participant C as AuthController
    participant M as MediatR
    participant VP as ValidationPipeline
    participant V as loginCommandValidator
    participant H as AuthLogin.Handler
    participant JWT as JwtSecurityTokenHandler

    U->>C: POST /Auth/Login\n{ email: "admin@test.com", password: "123456" }
    
    C->>M: mediator.Send(LoginCommand)
    
    M->>VP: Intercepta (Pipeline)
    VP->>V: validator.Validate(command)
    
    alt Email vazio ou < 6 chars
        V-->>VP: Erros de validação
        VP-->>M: ValidationException
        M-->>C: Result.Failure
        C-->>U: 400 Bad Request\n{ errorCode: 400, errorMessage: "..." }
    else Validação OK
        VP->>H: Handle(LoginCommand)
        
        alt Email não contém "admin"
            H-->>C: Result.Failure(Exception)
            C-->>U: 500 Internal Server Error
        else Email válido
            H->>JWT: CreateToken(claims)
            JWT-->>H: token string
            H-->>M: Result.Success(LoginDTO)
            M-->>C: Result.Success(LoginDTO)
            C-->>U: 200 OK\n{ token: "eyJ...", expiration: "..." }
        end
    end
```

> **Nota:** A lógica atual de autenticação é temporária (verifica apenas se o email contém "admin"). A implementação real buscará o usuário no banco de dados e comparará a senha.

---

## 16. Como Adicionar uma Nova Funcionalidade

Exemplo: adicionar um endpoint `GET /User/{id}` para buscar um usuário.

### Passo 1 — Criar a Query (em `SD_Server.Auth` ou em um novo projeto de feature)

```csharp
// GetUserQuery.cs
public class GetUserQuery : IRequest<Result<Exception, UserDTO>>
{
    public int Id { get; set; }
}
```

### Passo 2 — Criar o DTO de resposta

```csharp
// UserDTO.cs
public class UserDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}
```

### Passo 3 — Criar o Handler

```csharp
// GetUser.cs
public class GetUser
{
    public class Handler : IRequestHandler<GetUserQuery, Result<Exception, UserDTO>>
    {
        public async Task<Result<Exception, UserDTO>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            // Aqui virá a busca no banco de dados
            // Por enquanto, retorno de exemplo:
            return new UserDTO { Id = request.Id, Name = "Teste", Email = "teste@teste.com" };
        }
    }
}
```

### Passo 4 — Criar o Controller (ou adicionar ao existente)

```csharp
// UserController.cs
[ApiController]
[Route("[controller]")]
public class UserController(IMediator mediator, IMapper mapper) : ApiControllerBase(mapper)
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var response = await mediator.Send(new GetUserQuery { Id = id });
        return HandleCommand(response);
    }
}
```

### Resumo do fluxo de adição

```mermaid
flowchart LR
    A["1. Criar Query/Command\n(o pedido)"]
    B["2. Criar DTO\n(a resposta)"]
    C["3. Criar Handler\n(a lógica)"]
    D["4. Criar/Atualizar Controller\n(a rota HTTP)"]
    E["5. Testar no Scalar\n(documentação interativa)"]

    A --> B --> C --> D --> E
```

---

## Referências Rápidas

| Conceito | Onde está no código |
|---|---|
| Ponto de entrada da API Auth | `SD_Server.Auth/Program.cs` |
| Endpoint de login | `SD_Server.Auth/Controllers/AuthController.cs` |
| Lógica de login + JWT | `SD_Server.Auth/Handlers/AuthLogin.cs` |
| Validação de login | `SD_Server.Auth/Commands/loginCommandValidator.cs` |
| Tratamento de respostas HTTP | `SD_Api_Base/Base/ApiControllerBase.cs` |
| Pipeline de validação automática | `SD_Api_Base/Behavious/ValidationPipeline.cs` |
| Tipo Result (sucesso/falha) | `SD_SharedKernel/Helpers/Result.cs` |
| Tipo Option (valor ou nada) | `SD_SharedKernel/Helpers/Option.cs` |
| Códigos de erro HTTP | `SD_Server.Domain/Exceptions/ErrorCodes.cs` |
| Configuração de serviços | `SD_Api_Extensions/DefaultServiceExtensions.cs` |
| Configuração de CORS | `SD_Api_Extensions/CORSExtensions.cs` |
| Tipos de acesso (roles) | `SD_Server.Auth/Domain/TypeAcess.cs` |
