# Sistema Distribuído 2026 — Facvest

Visão geral do repositório e como os três principais módulos se encaixam. Para detalhes técnicos, consulte os documentos de arquitetura em cada pasta.

| Módulo | Pasta | Documentação |
|--------|--------|----------------|
| App mobile | `Mobile/app/` | [ARCHITECTURE.md](Mobile/app/ARCHITECTURE.md) |
| Portal web (FitPortal) | `Client/portal/` | [ARCHITECTURE.md](Client/portal/ARCHITECTURE.md) |
| API backend | `Server/` | [ARQUITETURA.md](Server/ARQUITETURA.md) |

---

## Visão do sistema

O ecossistema combina **portal web** (profissional de educação física), **aplicativo mobile** (perfil de aluno) e **API REST** em JSON. Os clientes conversam com o backend via HTTP; o servidor concentra regras de negócio, autenticação JWT e persistência via Entity Framework Core.

---

## Server — API (`Server/`)

- **Stack:** C# / **.NET 10**, Web API REST.
- **Organização:** Clean Architecture com os projetos:
  - `SD_Server` — API principal (endpoints de domínio)
  - `SD_Server.Auth` — API de autenticação (emissão de JWT)
  - `SD_Server.Application` — Casos de uso / Handlers CQRS
  - `SD_Server.Domain` — Entidades, interfaces de repositório, enums
  - `SD_Server.Infra.Data` — EF Core, DbContext, Repositories, Migrations
  - `SD_Server.Infra` — Infraestrutura geral
  - `SD_Api_Base`, `SD_Api_Extensions`, `SD_SharedKernel` — Utilitários transversais
  - `6-tests/SD_Server.Auth.Tests` — Testes unitários (xUnit + Moq + FluentAssertions)
- **Padrões:** CQRS com **MediatR**, validação com **FluentValidation**, **SimpleInjector** integrado ao DI do ASP.NET Core.
- **Respostas:** tipo funcional `Result<TFailure, TSuccess>` no shared kernel; controllers base tratam sucesso e falha de forma uniforme.
- **Auth:** JWT (claims: id, email, nome, role; expiração de 30 min; issuer/audience `MatrixCompetency`); documentação interativa com **Scalar** (tema DeepSpace) com suporte a Bearer token.
- **Usuários:** tabela unificada `tb_Users` com `TypeUserEnum` (Admin, Professional, Student); senhas armazenadas com hash BCrypt.
- **Transações:** padrão Unit of Work (`IUnitOfWork`) garantindo atomicidade nas operações de criação, edição e exclusão de alunos (Student + User criados/atualizados/removidos na mesma transação).
- **Testes:** 26 testes unitários cobrindo o handler de autenticação e o validator de login — todos passando.

---

## Client — FitPortal (`Client/portal/`)

- **Stack:** **Angular 20**, **TypeScript**, **PrimeNG 20**, PrimeIcons, **Chart.js**, **RxJS**, **SCSS**.
- **Papel:** portal do **profissional** (gestão de alunos, avaliações, dashboard, perfil).
- **Arquitetura:** Clean Architecture em quatro camadas no front:
  - **Core** — guards, interceptor JWT, `AuthService`.
  - **Domain** — entidades, repositórios (contratos), use cases.
  - **Data** — models, datasources (hoje em grande parte **mock**), implementações de repositório.
  - **Presentation** — layout (sidebar/topbar), páginas com lazy loading; componentes não acessam Data diretamente.
- **Estado atual:** dados simulados até o backend estar disponível; troca de datasources para HTTP preserva Domain e Presentation intactos.

---

## Mobile — App Flutter (`Mobile/app/`)

- **Stack:** **Flutter** (SDK), estrutura por feature.
- **Arquitetura:** inspirada em Clean Architecture — **presentation** (telas, estilos), **domain** (entidades, interfaces de serviço), **data** ainda a implementar (repositórios, API).
- **Fluxo atual:** rotas `/login` e `/home`; navegação com limpeza de pilha entre login e home.
- **Estado atual:** autenticação ainda hardcoded; falta camada data, integração real com backend, expansão de testes e eventual gerenciamento de estado (Provider, Riverpod, BLoC, etc.).

---

## Alinhamento entre módulos

- **Conceito comum:** separação em camadas (domínio independente de UI e de origem dos dados).
- **Integração futura:** portal e mobile devem consumir a API (`Server`), substituindo mocks e credenciais fixas por HTTP + JWT, alinhado ao que o FitPortal já prevê no interceptor e no `AuthService`.

---

## Onde aprofundar

- Mobile: estrutura de pastas, rotas, tabelas de dependências e pendências — `Mobile/app/ARCHITECTURE.md`.
- Portal: fluxo de dados, rotas, convenções de nome e passo a passo para novas features — `Client/portal/ARCHITECTURE.md`.
- Servidor: diagramas, CQRS, JWT, CORS, Scalar e guia para novos endpoints — `Server/ARQUITETURA.md`.
