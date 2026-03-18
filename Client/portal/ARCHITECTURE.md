# Arquitetura do Projeto — FitPortal

> **Portal do Profissional de Educação Física**  
> Disciplina: Sistemas Distribuídos — Facvest 2026  
> Este documento é voltado para colaboradores com **pouca ou nenhuma experiência** com as tecnologias utilizadas. Leia com calma e na ordem apresentada.

---

## Sumário

1. [Visão Geral do Projeto](#1-visão-geral-do-projeto)
2. [Tecnologias Utilizadas](#2-tecnologias-utilizadas)
3. [O que é Arquitetura Limpa?](#3-o-que-é-arquitetura-limpa)
4. [Estrutura de Pastas](#4-estrutura-de-pastas)
5. [As Quatro Camadas em Detalhe](#5-as-quatro-camadas-em-detalhe)
   - [5.1 Core](#51-core--infraestrutura-angular)
   - [5.2 Domain](#52-domain--regras-de-negócio)
   - [5.3 Data](#53-data--acesso-a-dados)
   - [5.4 Presentation](#54-presentation--interface-visual)
6. [Fluxo de Dados Completo](#6-fluxo-de-dados-completo)
7. [Rotas e Navegação](#7-rotas-e-navegação)
8. [Autenticação](#8-autenticação)
9. [Como Adicionar uma Nova Funcionalidade](#9-como-adicionar-uma-nova-funcionalidade)
10. [Convenções de Nomenclatura](#10-convenções-de-nomenclatura)
11. [Como Executar o Projeto](#11-como-executar-o-projeto)
12. [Dependências](#12-dependências)

---

## 1. Visão Geral do Projeto

O **FitPortal** é o portal web destinado ao **profissional de educação física**. Ele faz parte de um sistema maior composto por:

- **Portal** (este projeto) — acesso pelo navegador, para o profissional gerenciar seus alunos e avaliações.
- **App Mobile** (projeto futuro) — acesso pelo celular, para o aluno acompanhar seu progresso.

```mermaid
graph TD
    subgraph Sistema FitPortal
        Portal["🖥️ Portal Web\n(Professor)"]
        App["📱 App Mobile\n(Aluno)"]
        API["⚙️ API Backend\n(Servidor)"]
        DB[("🗄️ Banco de Dados")]
    end

    Portal -- "HTTP/REST" --> API
    App -- "HTTP/REST" --> API
    API -- "SQL/NoSQL" --> DB
```

> **Nota:** Por enquanto, o Portal usa **dados mockados** (simulados em memória) enquanto o backend não está pronto. Quando o backend for desenvolvido, basta trocar os `DataSources` — o restante do código não muda.

---

## 2. Tecnologias Utilizadas

| Tecnologia | O que é | Para que serve no projeto |
|---|---|---|
| **TypeScript** | JavaScript com tipos | Linguagem principal. Evita erros ao declarar o tipo dos dados. |
| **Angular 20** | Framework frontend | Estrutura base da aplicação. Gerencia telas, rotas e dados. |
| **PrimeNG 20** | Biblioteca de componentes UI | Botões, tabelas, diálogos, formulários prontos e estilizados. |
| **PrimeIcons** | Biblioteca de ícones | Ícones vetoriais usados na interface. |
| **Chart.js** | Biblioteca de gráficos | Gráfico de linha no Dashboard. |
| **RxJS** | Biblioteca de programação reativa | Gerencia fluxos de dados assíncronos (ex: chamadas HTTP). |
| **SCSS** | CSS com superpoderes | Estilização dos componentes com variáveis e aninhamento. |

### Conceito importante: Observable (RxJS)

No Angular, em vez de usar `Promise` para lidar com dados assíncronos, utilizamos **Observables** do RxJS. Pense num Observable como uma "torneira de dados":

```typescript
// Exemplo: o método execute() retorna um Observable de Aluno[]
this.getAlunosUseCase.execute().subscribe(alunos => {
  // "alunos" chega aqui quando os dados estiverem prontos
  this.alunos = alunos;
});
```

---

## 3. O que é Arquitetura Limpa?

A **Arquitetura Limpa** (Clean Architecture) é uma forma de organizar o código em **camadas separadas**, onde cada camada tem uma única responsabilidade. A regra principal é:

> **As camadas internas não sabem da existência das camadas externas.**

```mermaid
graph TD
    subgraph "Camadas (de dentro para fora)"
        D["🟡 Domain\n(Regras de negócio)\nNão depende de nada"]
        DA["🟠 Data\n(Acesso a dados)\nDepende do Domain"]
        C["🔵 Core\n(Infraestrutura Angular)\nDepende do Domain"]
        P["🟢 Presentation\n(Interface visual)\nDepende do Domain"]
    end

    DA -->|"implementa contratos do"| D
    C -->|"usa entidades do"| D
    P -->|"chama use cases do"| D
    P -.->|"NÃO acessa diretamente"| DA
```

### Por que isso é importante?

| Sem Arquitetura Limpa | Com Arquitetura Limpa |
|---|---|
| Tela chama banco de dados diretamente | Tela chama Use Case → Use Case usa Repository |
| Mudar o banco quebra as telas | Mudar o banco só muda o DataSource |
| Difícil de testar | Fácil de testar cada parte isolada |
| Código bagunçado com o tempo | Código organizado e previsível |

---

## 4. Estrutura de Pastas

```
portal/
├── src/
│   ├── app/
│   │   │
│   │   ├── core/                        ← 🔵 Camada Core
│   │   │   ├── guards/
│   │   │   │   └── auth.guard.ts        ← Protege rotas que exigem login
│   │   │   ├── interceptors/
│   │   │   │   └── auth.interceptor.ts  ← Adiciona token JWT nas requisições
│   │   │   └── services/
│   │   │       └── auth.service.ts      ← Lógica de login/logout
│   │   │
│   │   ├── domain/                      ← 🟡 Camada Domain
│   │   │   ├── entities/
│   │   │   │   ├── aluno.entity.ts      ← Define como é um Aluno
│   │   │   │   ├── avaliacao.entity.ts  ← Define como é uma Avaliação
│   │   │   │   └── profissional.entity.ts
│   │   │   ├── repositories/
│   │   │   │   ├── aluno.repository.ts      ← Contrato: "o que posso fazer com Alunos"
│   │   │   │   └── avaliacao.repository.ts  ← Contrato: "o que posso fazer com Avaliações"
│   │   │   └── usecases/
│   │   │       ├── aluno/
│   │   │       │   ├── get-alunos.usecase.ts     ← Caso de uso: buscar todos os alunos
│   │   │       │   └── create-aluno.usecase.ts   ← Caso de uso: criar um aluno
│   │   │       └── avaliacao/
│   │   │           ├── get-avaliacoes.usecase.ts
│   │   │           └── create-avaliacao.usecase.ts
│   │   │
│   │   ├── data/                        ← 🟠 Camada Data
│   │   │   ├── models/
│   │   │   │   ├── aluno.model.ts       ← Formato do dado vindo da API + mapeador
│   │   │   │   └── avaliacao.model.ts
│   │   │   ├── datasources/
│   │   │   │   ├── aluno.datasource.ts      ← Busca dados (mock ou HTTP)
│   │   │   │   └── avaliacao.datasource.ts
│   │   │   └── repositories/
│   │   │       ├── aluno.repository.impl.ts     ← Implementação concreta do contrato
│   │   │       └── avaliacao.repository.impl.ts
│   │   │
│   │   ├── presentation/                ← 🟢 Camada Presentation
│   │   │   ├── layout/
│   │   │   │   └── main-layout/         ← Sidebar + Topbar (moldura de todas as páginas)
│   │   │   ├── pages/
│   │   │   │   ├── login/               ← Tela de login
│   │   │   │   ├── dashboard/           ← Painel principal
│   │   │   │   ├── alunos/              ← Listagem de alunos
│   │   │   │   ├── avaliacoes/          ← Listagem de avaliações
│   │   │   │   └── perfil/              ← Perfil do profissional
│   │   │   └── shared/
│   │   │       └── components/          ← Componentes reutilizáveis (a implementar)
│   │   │
│   │   ├── app.config.ts    ← Configurações globais (providers, PrimeNG, DI)
│   │   ├── app.routes.ts    ← Mapeamento de URLs para componentes
│   │   └── app.ts           ← Componente raiz (ponto de entrada)
│   │
│   ├── styles.scss          ← Estilos globais
│   └── index.html           ← HTML base da SPA
│
├── package.json             ← Dependências do projeto
└── angular.json             ← Configurações do Angular CLI
```

---

## 5. As Quatro Camadas em Detalhe

### 5.1 Core — Infraestrutura Angular

A camada Core contém tudo que é **específico do Angular** e é transversal ao projeto inteiro.

```mermaid
graph LR
    subgraph Core
        AS["auth.service.ts\nGerencia sessão"]
        AG["auth.guard.ts\nProtege rotas"]
        AI["auth.interceptor.ts\nAdiciona token HTTP"]
    end

    AG -->|"usa"| AS
    AI -->|"lê token de"| LS["localStorage"]
```

#### `auth.service.ts` — Serviço de Autenticação

Responsável por controlar se o usuário está logado ou não.

```typescript
// Métodos disponíveis:
authService.login(email, senha)   // faz o login
authService.logout()              // faz o logout e redireciona para /login
authService.isLoggedIn()          // retorna true/false
```

#### `auth.guard.ts` — Protetor de Rotas

Executado automaticamente pelo Angular antes de abrir qualquer página protegida. Se o usuário não estiver logado, redireciona para `/login`.

```typescript
// O Angular chama esse guard automaticamente antes de acessar qualquer rota
// que tenha: canActivate: [authGuard]
export const authGuard: CanActivateFn = () => {
  if (authService.isLoggedIn()) return true;      // deixa passar
  return router.createUrlTree(['/login']);         // bloqueia e redireciona
};
```

#### `auth.interceptor.ts` — Interceptador HTTP

Toda vez que o código fizer uma requisição HTTP para o backend, este interceptador adiciona automaticamente o token JWT no cabeçalho. O componente não precisa se preocupar com isso.

```typescript
// Antes de enviar qualquer requisição HTTP:
Authorization: Bearer eyJhbGciOiJIUzI1NiIsIn...
```

---

### 5.2 Domain — Regras de Negócio

Esta é a camada mais importante. Ela define **o que o sistema faz**, sem depender de Angular, banco de dados ou qualquer framework. É **TypeScript puro**.

```mermaid
graph TD
    subgraph Domain
        subgraph Entities
            AE["aluno.entity.ts"]
            AVE["avaliacao.entity.ts"]
            PE["profissional.entity.ts"]
        end
        subgraph Repositories ["Repositories (Contratos)"]
            AR["AlunoRepository\n(abstract class)"]
            AVR["AvaliacaoRepository\n(abstract class)"]
        end
        subgraph UseCases ["Use Cases"]
            GA["GetAlunosUseCase"]
            CA["CreateAlunoUseCase"]
            GAV["GetAvaliacoesUseCase"]
            CAV["CreateAvaliacaoUseCase"]
        end
    end

    GA -->|"depende de"| AR
    CA -->|"depende de"| AR
    GAV -->|"depende de"| AVR
    CAV -->|"depende de"| AVR
    AR -->|"usa tipo"| AE
    AVR -->|"usa tipo"| AVE
```

#### Entities — O que são os dados

As entities definem a **forma** dos dados do negócio. São interfaces TypeScript simples.

```typescript
// aluno.entity.ts — Define como um Aluno existe no sistema
export interface Aluno {
  id: number;
  nome: string;
  email: string;
  telefone: string;
  status: 'Ativo' | 'Inativo';   // só pode ser um desses dois valores
  ultimaAvaliacao?: string;       // o "?" significa que é opcional
}
```

#### Repositories — Os Contratos

Um Repository é um **contrato** (classe abstrata) que define o que pode ser feito com cada entidade. Ele **não diz como** fazer, apenas **o que** deve ser feito. A implementação real fica na camada Data.

```typescript
// aluno.repository.ts — "Eu prometo que quem me implementar vai ter esses métodos"
export abstract class AlunoRepository {
  abstract getAll(): Observable<Aluno[]>;                    // buscar todos
  abstract getById(id: number): Observable<Aluno>;           // buscar por ID
  abstract create(aluno: Omit<Aluno, 'id'>): Observable<Aluno>; // criar
  abstract update(id: number, ...): Observable<Aluno>;       // atualizar
  abstract delete(id: number): Observable<void>;             // deletar
}
```

> **Analogia:** O Repository é como um **cardápio de restaurante**. Ele lista o que está disponível, mas não explica como a cozinha prepara cada prato.

#### Use Cases — O que o sistema permite fazer

Cada Use Case representa **uma ação específica** que o usuário pode executar. Eles orquestram as chamadas ao Repository.

```typescript
// get-alunos.usecase.ts
@Injectable({ providedIn: 'root' })
export class GetAlunosUseCase {
  constructor(private readonly alunoRepository: AlunoRepository) {}

  execute(): Observable<Aluno[]> {
    return this.alunoRepository.getAll();  // delega para o repositório
  }
}
```

> **Por que ter Use Cases se eles só chamam o repositório?**  
> Para casos simples parece redundante, mas em casos reais, um Use Case pode: validar dados antes de salvar, combinar dados de múltiplos repositórios, registrar logs, emitir eventos, etc.

---

### 5.3 Data — Acesso a Dados

Esta camada é responsável por **buscar e salvar dados**, seja em memória (mock), em uma API HTTP ou em outro lugar. Ela implementa os contratos definidos no Domain.

```mermaid
graph TD
    subgraph Data
        subgraph Models
            AM["aluno.model.ts\nAlunoModel + toAlunoEntity()"]
            AVM["avaliacao.model.ts\nAvaliacaoModel + toAvaliacaoEntity()"]
        end
        subgraph DataSources
            ADS["AlunoDataSource\n(mock / futuro: HTTP)"]
            AVDS["AvaliacaoDataSource\n(mock / futuro: HTTP)"]
        end
        subgraph RepositoryImpls ["Repository Implementations"]
            ARI["AlunoRepositoryImpl\nextends AlunoRepository"]
            AVRI["AvaliacaoRepositoryImpl\nextends AvaliacaoRepository"]
        end
    end

    ARI -->|"usa"| ADS
    ARI -->|"mapeia com"| AM
    AVRI -->|"usa"| AVDS
    AVRI -->|"mapeia com"| AVM
```

#### Models — O formato da API

O Model representa como o dado chega da API (geralmente com nomes em snake_case). Ele também contém funções de mapeamento para converter para a Entity do Domain.

```typescript
// aluno.model.ts

// Assim o dado vem da API (snake_case é padrão em backends)
export interface AlunoModel {
  id: number;
  nome: string;
  ultima_avaliacao?: string;   // API usa underline
}

// Função que converte AlunoModel → Aluno (entity do Domain)
export function toAlunoEntity(model: AlunoModel): Aluno {
  return {
    id: model.id,
    nome: model.nome,
    ultimaAvaliacao: model.ultima_avaliacao  // camelCase no frontend
  };
}
```

#### DataSources — De onde os dados vêm

O DataSource é quem de fato **faz a chamada**. Hoje usa dados mockados (`of([...])` = retorna dados fixos como Observable). No futuro, basta substituir por `this.http.get<AlunoModel[]>('/api/alunos')`.

```typescript
// Hoje (mock):
getAll(): Observable<AlunoModel[]> {
  return of([...this.mockData]);
}

// Futuro (HTTP real):
getAll(): Observable<AlunoModel[]> {
  return this.http.get<AlunoModel[]>(`${environment.apiUrl}/alunos`);
}
```

#### Repository Implementations — A implementação concreta

Conecta o DataSource ao contrato do Domain, fazendo a conversão dos dados.

```typescript
// aluno.repository.impl.ts
export class AlunoRepositoryImpl extends AlunoRepository {
  constructor(private readonly dataSource: AlunoDataSource) {
    super();
  }

  getAll(): Observable<Aluno[]> {
    return this.dataSource.getAll().pipe(
      map(models => models.map(toAlunoEntity))  // converte cada model em entity
    );
  }
}
```

#### Como o Angular sabe qual implementação usar?

No `app.config.ts`, registramos um mapeamento:

```typescript
// "Quando alguém pedir AlunoRepository, entregue AlunoRepositoryImpl"
{ provide: AlunoRepository, useClass: AlunoRepositoryImpl }
```

Isso é **Injeção de Dependência (DI)** — o Angular resolve automaticamente qual classe instanciar.

---

### 5.4 Presentation — Interface Visual

A camada de Presentation contém todos os componentes Angular que o usuário vê e interage. Ela **nunca acessa a camada Data diretamente** — sempre passa pelos Use Cases do Domain.

```mermaid
graph TD
    subgraph Presentation
        subgraph Layout
            ML["MainLayoutComponent\nSidebar + Topbar + RouterOutlet"]
        end
        subgraph Pages
            LP["LoginComponent"]
            DP["DashboardComponent"]
            AP["AlunosComponent"]
            AVP["AvaliacoesComponent"]
            PP["PerfilComponent"]
        end
        subgraph Shared
            SC["shared/components/\n(componentes reutilizáveis)"]
        end
    end

    ML -->|"contém (router-outlet)"| DP
    ML -->|"contém (router-outlet)"| AP
    ML -->|"contém (router-outlet)"| AVP
    ML -->|"contém (router-outlet)"| PP
```

#### Cada componente Angular tem 3 arquivos

| Arquivo | Responsabilidade |
|---|---|
| `nome.ts` | Lógica do componente (TypeScript) |
| `nome.html` | Template visual (HTML + sintaxe Angular) |
| `nome.scss` | Estilos específicos do componente (SCSS) |

#### Exemplo: Como o AlunosComponent funciona

```typescript
// alunos.ts — só conhece o Use Case, não sabe de DataSource nem HTTP
export class AlunosComponent implements OnInit {
  alunos: Aluno[] = [];

  constructor(private readonly getAlunosUseCase: GetAlunosUseCase) {}

  ngOnInit(): void {
    // ngOnInit é chamado automaticamente quando o componente abre
    this.getAlunosUseCase.execute().subscribe(alunos => {
      this.alunos = alunos;  // popula a lista que o HTML vai exibir
    });
  }
}
```

```html
<!-- alunos.html — exibe os dados usando diretivas Angular -->
@for (aluno of filteredAlunos; track aluno.id) {
  <tr>
    <td>{{ aluno.nome }}</td>
    <td>{{ aluno.email }}</td>
  </tr>
}
```

#### MainLayout — A moldura das páginas

O `MainLayoutComponent` é a **estrutura que envolve todas as páginas** (exceto o login). Ele contém a sidebar e o topbar. O conteúdo de cada página é inserido no lugar do `<router-outlet>`.

```html
<!-- main-layout.html -->
<div class="layout-wrapper">
  <aside class="layout-sidebar"> <!-- sidebar com menu --> </aside>
  <div class="layout-main">
    <header class="layout-topbar"> <!-- barra superior --> </header>
    <main class="layout-content">
      <router-outlet />  <!-- ← a página atual aparece aqui -->
    </main>
  </div>
</div>
```

---

## 6. Fluxo de Dados Completo

Este diagrama mostra o caminho completo desde o usuário clicar em algo até o dado aparecer na tela.

```mermaid
sequenceDiagram
    actor U as Usuário
    participant C as Componente<br/>(Presentation)
    participant UC as Use Case<br/>(Domain)
    participant R as Repository<br/>(Domain - Contrato)
    participant RI as RepositoryImpl<br/>(Data)
    participant DS as DataSource<br/>(Data)

    U->>C: Abre a página de Alunos
    C->>C: ngOnInit() é chamado
    C->>UC: getAlunosUseCase.execute()
    UC->>R: alunoRepository.getAll()
    R->>RI: (implementação concreta)
    RI->>DS: dataSource.getAll()
    DS-->>RI: AlunoModel[] (dado bruto)
    RI-->>RI: toAlunoEntity() para cada item
    RI-->>UC: Aluno[] (entidade do domínio)
    UC-->>C: Observable<Aluno[]>
    C-->>C: this.alunos = alunos
    C-->>U: Tabela renderizada na tela
```

---

## 7. Rotas e Navegação

O arquivo `app.routes.ts` define o mapeamento entre **URLs** e **componentes**.

```mermaid
graph TD
    Root["/"]
    Login["/login\nLoginComponent"]
    Layout["MainLayoutComponent\n🔒 Protegido por authGuard"]
    Dashboard["/dashboard\nDashboardComponent"]
    Alunos["/alunos\nAlunosComponent"]
    Avaliacoes["/avaliacoes\nAvaliacoesComponent"]
    Perfil["/perfil\nPerfilComponent"]

    Root -->|"redireciona para"| Dashboard
    Root --> Login
    Root --> Layout
    Layout --> Dashboard
    Layout --> Alunos
    Layout --> Avaliacoes
    Layout --> Perfil
```

### Lazy Loading

As páginas internas usam **Lazy Loading**: o código de cada página só é carregado quando o usuário navegar até ela. Isso acelera o carregamento inicial.

```typescript
// Em vez de importar diretamente (carrega tudo de uma vez):
import { AlunosComponent } from '...'

// Usamos importação dinâmica (carrega só quando necessário):
loadComponent: () => import('./presentation/pages/alunos/alunos')
                       .then(m => m.AlunosComponent)
```

---

## 8. Autenticação

```mermaid
flowchart TD
    A["Usuário acessa qualquer URL"] --> B{{"authGuard\nisLoggedIn?"}}
    B -- "Sim" --> C["Página carrega normalmente"]
    B -- "Não" --> D["Redireciona para /login"]
    D --> E["Usuário preenche e-mail e senha"]
    E --> F["LoginComponent\nchama AuthService.login()"]
    F --> G["AuthService salva token\nno localStorage"]
    G --> H["Router navega para /dashboard"]

    subgraph "Toda requisição HTTP"
        I["authInterceptor lê token\ndo localStorage"]
        I --> J["Adiciona header:\nAuthorization: Bearer TOKEN"]
    end
```

> **Atenção:** A autenticação atual é simulada (mock). Para integrar com o backend real, basta alterar o método `login()` dentro de `auth.service.ts` para fazer uma chamada HTTP. O restante do sistema não precisa mudar.

---

## 9. Como Adicionar uma Nova Funcionalidade

Vamos usar como exemplo a adição de um módulo de **"Fichas de Treino"**.

### Passo 1 — Criar a Entity (Domain)

```typescript
// src/app/domain/entities/ficha-treino.entity.ts
export interface FichaTreino {
  id: number;
  alunoId: number;
  titulo: string;
  descricao: string;
  criadoEm: string;
}
```

### Passo 2 — Criar o Repository (Domain)

```typescript
// src/app/domain/repositories/ficha-treino.repository.ts
import { Observable } from 'rxjs';
import { FichaTreino } from '../entities/ficha-treino.entity';

export abstract class FichaTreinoRepository {
  abstract getAll(): Observable<FichaTreino[]>;
  abstract create(ficha: Omit<FichaTreino, 'id'>): Observable<FichaTreino>;
}
```

### Passo 3 — Criar os Use Cases (Domain)

```typescript
// src/app/domain/usecases/ficha-treino/get-fichas.usecase.ts
@Injectable({ providedIn: 'root' })
export class GetFichasUseCase {
  constructor(private readonly fichaTreinoRepository: FichaTreinoRepository) {}

  execute(): Observable<FichaTreino[]> {
    return this.fichaTreinoRepository.getAll();
  }
}
```

### Passo 4 — Criar o Model e DataSource (Data)

```typescript
// src/app/data/models/ficha-treino.model.ts
export interface FichaTreinoModel { /* campos da API */ }
export function toFichaTreinoEntity(model: FichaTreinoModel): FichaTreino { ... }

// src/app/data/datasources/ficha-treino.datasource.ts
@Injectable({ providedIn: 'root' })
export class FichaTreinoDataSource {
  getAll(): Observable<FichaTreinoModel[]> {
    return of([/* dados mock */]);
  }
}
```

### Passo 5 — Criar a implementação do Repository (Data)

```typescript
// src/app/data/repositories/ficha-treino.repository.impl.ts
@Injectable({ providedIn: 'root' })
export class FichaTreinoRepositoryImpl extends FichaTreinoRepository {
  constructor(private dataSource: FichaTreinoDataSource) { super(); }

  getAll(): Observable<FichaTreino[]> {
    return this.dataSource.getAll().pipe(map(m => m.map(toFichaTreinoEntity)));
  }
}
```

### Passo 6 — Registrar o provider (app.config.ts)

```typescript
// Adicionar em providers no app.config.ts:
{ provide: FichaTreinoRepository, useClass: FichaTreinoRepositoryImpl }
```

### Passo 7 — Criar o componente (Presentation)

```bash
ng generate component presentation/pages/fichas-treino --skip-tests
```

```typescript
// fichas-treino.ts
export class FichasTreinoComponent implements OnInit {
  fichas: FichaTreino[] = [];
  constructor(private readonly getFichasUseCase: GetFichasUseCase) {}
  ngOnInit() { this.getFichasUseCase.execute().subscribe(f => this.fichas = f); }
}
```

### Passo 8 — Adicionar a rota (app.routes.ts)

```typescript
{
  path: 'fichas-treino',
  loadComponent: () => import('./presentation/pages/fichas-treino/fichas-treino')
                         .then(m => m.FichasTreinoComponent)
}
```

### Passo 9 — Adicionar no menu (main-layout.ts)

```typescript
navItems = [
  // ... itens existentes ...
  { label: 'Fichas de Treino', icon: 'pi pi-list', route: '/fichas-treino' }
];
```

---

## 10. Convenções de Nomenclatura

| Tipo | Convenção | Exemplo |
|---|---|---|
| Arquivo de Entity | `nome.entity.ts` | `aluno.entity.ts` |
| Arquivo de Repository (contrato) | `nome.repository.ts` | `aluno.repository.ts` |
| Arquivo de Repository (impl.) | `nome.repository.impl.ts` | `aluno.repository.impl.ts` |
| Arquivo de Use Case | `acao-entidade.usecase.ts` | `get-alunos.usecase.ts` |
| Arquivo de Model | `nome.model.ts` | `aluno.model.ts` |
| Arquivo de DataSource | `nome.datasource.ts` | `aluno.datasource.ts` |
| Arquivo de Guard | `nome.guard.ts` | `auth.guard.ts` |
| Arquivo de Interceptor | `nome.interceptor.ts` | `auth.interceptor.ts` |
| Arquivo de Service | `nome.service.ts` | `auth.service.ts` |
| Componente Angular | `nome/nome.ts` | `alunos/alunos.ts` |
| Classes TypeScript | PascalCase | `AlunosComponent` |
| Variáveis e métodos | camelCase | `getAlunosUseCase` |
| Constantes | camelCase ou UPPER_SNAKE | `authGuard` / `API_URL` |
| Pastas | kebab-case | `main-layout/` |

---

## 11. Como Executar o Projeto

### Pré-requisitos

- [Node.js](https://nodejs.org/) versão 18 ou superior
- [npm](https://www.npmjs.com/) versão 9 ou superior (já vem com o Node)
- [Angular CLI](https://angular.dev/) versão 20

### Instalando o Angular CLI (apenas uma vez)

```bash
npm install -g @angular/cli
```

### Instalando as dependências do projeto

```bash
# Entre na pasta do portal
cd Client/portal

# Instale as dependências listadas no package.json
npm install
```

### Rodando em modo de desenvolvimento

```bash
npm start
# ou
ng serve
```

O projeto estará disponível em: **http://localhost:4200**

O servidor detecta mudanças nos arquivos e recarrega automaticamente.

### Gerando o build de produção

```bash
npm run build
```

Os arquivos gerados ficam em `dist/portal/`.

### Verificando erros de compilação

```bash
ng build --configuration=development
```

---

## 12. Dependências

```mermaid
graph LR
    subgraph "Dependências de Produção"
        A["@angular/core ^20.1.0\nNúcleo do Angular"]
        B["@angular/router ^20.1.0\nSistema de rotas"]
        C["@angular/forms ^20.1.0\nFormulários"]
        D["primeng ^20.4.0\nComponentes UI"]
        E["@primeng/themes ^20.4.0\nTemas visuais"]
        F["primeicons ^7.0.0\nÍcones"]
        G["chart.js ^4.5.1\nGráficos"]
        H["rxjs ~7.8.0\nProgramação reativa"]
    end

    subgraph "Dependências de Desenvolvimento"
        I["@angular/cli ^20.1.5\nFerramenta de linha de comando"]
        J["typescript ~5.8.2\nCompilador TypeScript"]
        K["karma + jasmine\nTestes unitários"]
    end
```

| Pacote | Versão | Descrição |
|---|---|---|
| `@angular/core` | ^20.1.0 | Núcleo do framework Angular |
| `@angular/router` | ^20.1.0 | Roteamento entre páginas |
| `@angular/forms` | ^20.1.0 | Formulários e two-way binding |
| `@angular/common/http` | ^20.1.0 | Cliente HTTP para APIs |
| `primeng` | ^20.4.0 | Componentes visuais (tabelas, botões, etc.) |
| `@primeng/themes` | ^20.4.0 | Sistema de temas (Aura theme) |
| `primeicons` | ^7.0.0 | Biblioteca de ícones |
| `chart.js` | ^4.5.1 | Renderização de gráficos |
| `rxjs` | ~7.8.0 | Programação reativa com Observables |
| `typescript` | ~5.8.2 | Linguagem (transpila para JavaScript) |

---

*Documentação gerada em março de 2026 — FitPortal v0.0.0*
