# Sistema Distribuído 2026 — Facvest

Visão geral do repositório e como os três principais módulos se encaixam. Para detalhes técnicos, consulte os documentos de arquitetura em cada pasta.

| Módulo | Pasta | Documentação |
|--------|--------|----------------|
| App mobile | `Mobile/app/` | [ARCHITECTURE.md](Mobile/app/ARCHITECTURE.md) |
| Portal web (FitPortal) | `Client/portal/` | [ARCHITECTURE.md](Client/portal/ARCHITECTURE.md) |
| API backend | `Server/` | [ARQUITETURA.md](Server/ARQUITETURA.md) |

---

## Visão do sistema

O ecossistema combina **portal web** (profissional de educação física), **aplicativo mobile** (perfil de aluno, no contexto do FitPortal) e **API REST** em JSON. Os clientes conversam com o backend via HTTP; o servidor concentra regras, autenticação e persistência (quando implementada).

---

## Server — API (`Server/`)

- **Stack:** C# / **.NET 10**, Web API REST.
- **Organização:** Clean Architecture com vários projetos: `SD_Server` (API principal), `SD_Server.Auth` (login e JWT), `SD_Server.Application`, `SD_Server.Domain`, `SD_Server.Infra`, além de `SD_Api_Base`, `SD_Api_Extensions` e `SD_SharedKernel`.
- **Padrões:** CQRS com **MediatR**, validação com **FluentValidation**, **AutoMapper**, **SimpleInjector** integrado ao DI do ASP.NET Core.
- **Respostas:** tipo funcional `Result` / `Option` no shared kernel; controllers base tratam sucesso e falha de forma uniforme.
- **Auth:** JWT (claims, expiração, issuer/audience); documentação interativa com **Scalar**.
- **Estado atual:** login e partes do domínio ainda com lógica temporária ou mock; banco de dados e integrações completas estão em evolução conforme o próprio guia do servidor.

---

## Client — FitPortal (`Client/portal/`)

- **Stack:** **Angular 20**, **TypeScript**, **PrimeNG 20**, PrimeIcons, **Chart.js**, **RxJS**, **SCSS**.
- **Papel:** portal do **profissional** (alunos, avaliações, dashboard, perfil).
- **Arquitetura:** Clean Architecture em quatro camadas no front:
  - **Core** — guards, interceptor JWT, `AuthService`.
  - **Domain** — entidades, repositórios (contratos), use cases.
  - **Data** — models, datasources (hoje em grande parte **mock**), implementações de repositório.
  - **Presentation** — layout (sidebar/topbar), páginas com lazy loading; componentes não acessam Data diretamente.
- **Estado atual:** dados simulados até o backend estar disponível; troca de datasources para HTTP deve preservar Domain e Presentation.

---

## Mobile — App Flutter (`Mobile/app/`)

- **Stack:** **Flutter** (SDK), estrutura por feature.
- **Arquitetura:** inspirada em Clean Architecture — **presentation** (telas, estilos), **domain** (entidades, interfaces de serviço), **data** ainda a implementar (repositórios, API).
- **Fluxo atual:** rotas `/login` e `/home`; navegação com limpeza de pilha entre login e home.
- **Estado atual:** autenticação ainda hardcoded; falta camada data, integração real com backend, asset de fundo do login, expansão de testes e eventual gerenciamento de estado (Provider, Riverpod, BLoC, etc.).

---

## Alinhamento entre módulos

- **Conceito comum:** separação em camadas (domínio independente de UI e de origem dos dados).
- **Integração futura:** portal e mobile devem consumir a API (`Server`), substituindo mocks e credenciais fixas por HTTP + JWT, alinhado ao que o FitPortal já prevê no interceptor e no `AuthService`.

---

## Onde aprofundar

- Mobile: estrutura de pastas, rotas, tabelas de dependências e pendências — `Mobile/app/ARCHITECTURE.md`.
- Portal: fluxo de dados, rotas, convenções de nome e passo a passo para novas features — `Client/portal/ARCHITECTURE.md`.
- Servidor: diagramas, CQRS, JWT, CORS, Scalar e guia para novos endpoints — `Server/ARQUITETURA.md`.
