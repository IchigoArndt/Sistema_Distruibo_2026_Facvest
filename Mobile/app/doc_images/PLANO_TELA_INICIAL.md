# Plano de Desenvolvimento — Tela Inicial (Home)

> Referência visual: `doc_images/Tela_inicial.png`

---

## O que essa tela tem?

A tela é composta por **6 partes principais**:

| # | Parte | Descrição |
|---|---|---|
| 1 | **AppBar** | Fundo vermelho com logo, "AnFis" e subtítulo |
| 2 | **Card de boas-vindas** | Fundo vermelho, saudação com nome do usuário |
| 3 | **Métricas** | Dois cards lado a lado: Peso Atual e IMC |
| 4 | **Ações Rápidas** | Dois botões: Buscar Profissional e Minhas Avaliações |
| 5 | **Próxima Avaliação** | Card com profissional, data, tipo e preço |
| 6 | **BottomNavigationBar** | 4 abas: Início, Profissionais, Avaliações, Perfil |

---

## Estrutura de arquivos a criar

```
lib/core/features/home/
├── domain/
│   └── entities/
│       └── avaliacao.dart                  ← Modelo de dados da avaliação
└── presentation/
    ├── home_presenter.dart                 ← Tela principal (substituir o atual)
    └── widgets/
        ├── home_appbar.dart                ← AppBar customizada vermelha
        ├── home_welcome_card.dart          ← Card vermelho de boas-vindas
        ├── home_metrics_row.dart           ← Linha com cards de Peso e IMC
        ├── home_quick_actions.dart         ← Seção "Ações Rápidas"
        └── home_next_evaluation.dart       ← Seção "Próxima Avaliação"
```

---

## Passo a Passo

---

### PASSO 1 — Criar a entidade `Avaliacao`

**Arquivo:** `lib/core/features/home/domain/entities/avaliacao.dart`

Esse arquivo define o modelo de dados que representa uma avaliação agendada.

```dart
class Avaliacao {
  final String profissional;
  final String data;
  final String tipo;
  final double preco;

  Avaliacao({
    required this.profissional,
    required this.data,
    required this.tipo,
    required this.preco,
  });
}
```

---

### PASSO 2 — Criar a pasta `widgets/`

Criar manualmente a pasta:
```
lib/core/features/home/presentation/widgets/
```

---

### PASSO 3 — Criar o widget `HomeAppBar`

**Arquivo:** `lib/core/features/home/presentation/widgets/home_appbar.dart`

- Fundo vermelho `Color(0xFFD32F2F)`
- À esquerda: ícone circular branco com `Icons.fitness_center` vermelho dentro
- Título: coluna com "AnFis" (branco, negrito) e "Reliable Enterprise Developments" (branco, pequeno)

```dart
import 'package:flutter/material.dart';

class HomeAppBar extends StatelessWidget implements PreferredSizeWidget {
  const HomeAppBar({super.key});

  @override
  Size get preferredSize => const Size.fromHeight(kToolbarHeight);

  @override
  Widget build(BuildContext context) {
    return AppBar(
      backgroundColor: const Color(0xFFD32F2F),
      leading: Padding(
        padding: const EdgeInsets.all(8.0),
        child: CircleAvatar(
          backgroundColor: Colors.white,
          child: Icon(Icons.fitness_center, color: Color(0xFFD32F2F), size: 20),
        ),
      ),
      title: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: const [
          Text('AnFis', style: TextStyle(color: Colors.white, fontWeight: FontWeight.bold, fontSize: 18)),
          Text('Reliable Enterprise Developments', style: TextStyle(color: Colors.white70, fontSize: 11)),
        ],
      ),
    );
  }
}
```

---

### PASSO 4 — Criar o widget `HomeWelcomeCard`

**Arquivo:** `lib/core/features/home/presentation/widgets/home_welcome_card.dart`

- Container vermelho com bordas arredondadas
- Recebe o nome do usuário por parâmetro
- Exibe "Olá, {nome}! 👋" e "Bem-vindo ao AnFis"

```dart
import 'package:flutter/material.dart';

class HomeWelcomeCard extends StatelessWidget {
  final String username;
  const HomeWelcomeCard({super.key, required this.username});

  @override
  Widget build(BuildContext context) {
    return Container(
      margin: const EdgeInsets.all(16),
      padding: const EdgeInsets.all(20),
      decoration: BoxDecoration(
        color: const Color(0xFFD32F2F),
        borderRadius: BorderRadius.circular(12),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text('Olá, $username! 👋',
              style: const TextStyle(color: Colors.white, fontSize: 22, fontWeight: FontWeight.bold)),
          const SizedBox(height: 4),
          const Text('Bem-vindo ao AnFis',
              style: TextStyle(color: Colors.white70, fontSize: 14)),
        ],
      ),
    );
  }
}
```

---

### PASSO 5 — Criar o widget `HomeMetricsRow`

**Arquivo:** `lib/core/features/home/presentation/widgets/home_metrics_row.dart`

- `Row` com dois cards brancos de tamanho igual (`Expanded`)
- Card esquerdo: "Peso Atual" + "78.5 kg" + ícone seta vermelha
- Card direito: "IMC" + "25.6" + ícone onda vermelha
- Valores fixos (mock) por enquanto

```dart
import 'package:flutter/material.dart';

class HomeMetricsRow extends StatelessWidget {
  const HomeMetricsRow({super.key});

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.symmetric(horizontal: 16),
      child: Row(
        children: [
          Expanded(child: _MetricCard(label: 'Peso Atual', value: '78.5 kg', icon: Icons.trending_up)),
          const SizedBox(width: 12),
          Expanded(child: _MetricCard(label: 'IMC', value: '25.6', icon: Icons.show_chart)),
        ],
      ),
    );
  }
}

class _MetricCard extends StatelessWidget {
  final String label;
  final String value;
  final IconData icon;

  const _MetricCard({required this.label, required this.value, required this.icon});

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.all(16),
      decoration: BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.circular(12),
        border: Border.all(color: const Color(0xFFEEEEEE)),
        boxShadow: [BoxShadow(color: Colors.black.withOpacity(0.05), blurRadius: 6)],
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              Text(label, style: const TextStyle(fontSize: 12, color: Colors.grey)),
              Icon(icon, color: const Color(0xFFD32F2F), size: 20),
            ],
          ),
          const SizedBox(height: 8),
          Text(value, style: const TextStyle(fontSize: 22, fontWeight: FontWeight.bold)),
        ],
      ),
    );
  }
}
```

---

### PASSO 6 — Criar o widget `HomeQuickActions`

**Arquivo:** `lib/core/features/home/presentation/widgets/home_quick_actions.dart`

- Título "Ações Rápidas"
- Dois cards com ícone, título e subtítulo
- Item 1: "Buscar Profissional" / "Encontre especialistas"
- Item 2: "Minhas Avaliações" / "Acompanhe seu progresso"

```dart
import 'package:flutter/material.dart';

class HomeQuickActions extends StatelessWidget {
  const HomeQuickActions({super.key});

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.symmetric(horizontal: 16),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          const Text('Ações Rápidas',
              style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold)),
          const SizedBox(height: 12),
          _ActionCard(
            icon: Icons.person_search,
            title: 'Buscar Profissional',
            subtitle: 'Encontre especialistas',
            onTap: () {},
          ),
          const SizedBox(height: 10),
          _ActionCard(
            icon: Icons.calendar_month,
            title: 'Minhas Avaliações',
            subtitle: 'Acompanhe seu progresso',
            onTap: () {},
          ),
        ],
      ),
    );
  }
}

class _ActionCard extends StatelessWidget {
  final IconData icon;
  final String title;
  final String subtitle;
  final VoidCallback onTap;

  const _ActionCard({
    required this.icon,
    required this.title,
    required this.subtitle,
    required this.onTap,
  });

  @override
  Widget build(BuildContext context) {
    return InkWell(
      onTap: onTap,
      borderRadius: BorderRadius.circular(12),
      child: Container(
        padding: const EdgeInsets.all(16),
        decoration: BoxDecoration(
          color: Colors.white,
          borderRadius: BorderRadius.circular(12),
          border: Border.all(color: const Color(0xFFEEEEEE)),
          boxShadow: [BoxShadow(color: Colors.black.withOpacity(0.04), blurRadius: 6)],
        ),
        child: Row(
          children: [
            Container(
              padding: const EdgeInsets.all(10),
              decoration: BoxDecoration(
                color: const Color(0xFFFFEBEE),
                borderRadius: BorderRadius.circular(8),
              ),
              child: Icon(icon, color: const Color(0xFFD32F2F), size: 22),
            ),
            const SizedBox(width: 16),
            Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(title, style: const TextStyle(fontWeight: FontWeight.bold, fontSize: 14)),
                const SizedBox(height: 2),
                Text(subtitle, style: const TextStyle(color: Colors.grey, fontSize: 12)),
              ],
            ),
          ],
        ),
      ),
    );
  }
}
```

---

### PASSO 7 — Criar o widget `HomeNextEvaluation`

**Arquivo:** `lib/core/features/home/presentation/widgets/home_next_evaluation.dart`

- Título "Próxima Avaliação"
- Card com fundo rosa claro `Color(0xFFFFEBEE)`
- Nome do profissional em vermelho + badge de preço
- Data e tipo da avaliação
- Recebe um objeto `Avaliacao` por parâmetro

```dart
import 'package:flutter/material.dart';
import 'package:sistema_distribuido/core/features/home/domain/entities/avaliacao.dart';

class HomeNextEvaluation extends StatelessWidget {
  final Avaliacao avaliacao;
  const HomeNextEvaluation({super.key, required this.avaliacao});

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.symmetric(horizontal: 16),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          const Text('Próxima Avaliação',
              style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold)),
          const SizedBox(height: 12),
          Container(
            width: double.infinity,
            padding: const EdgeInsets.all(16),
            decoration: BoxDecoration(
              color: const Color(0xFFFFEBEE),
              borderRadius: BorderRadius.circular(12),
            ),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    Text(avaliacao.profissional,
                        style: const TextStyle(
                            color: Color(0xFFD32F2F), fontWeight: FontWeight.bold, fontSize: 15)),
                    Container(
                      padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 4),
                      decoration: BoxDecoration(
                        color: const Color(0xFFD32F2F),
                        borderRadius: BorderRadius.circular(20),
                      ),
                      child: Text('R\$ ${avaliacao.preco.toStringAsFixed(0)}',
                          style: const TextStyle(color: Colors.white, fontSize: 12, fontWeight: FontWeight.bold)),
                    ),
                  ],
                ),
                const SizedBox(height: 6),
                Text(avaliacao.data,
                    style: const TextStyle(color: Color(0xFFE57373), fontSize: 13)),
                const SizedBox(height: 4),
                Text(avaliacao.tipo,
                    style: const TextStyle(color: Color(0xFFE57373), fontSize: 13)),
              ],
            ),
          ),
        ],
      ),
    );
  }
}
```

---

### PASSO 8 — Reescrever o `home_presenter.dart`

**Arquivo:** `lib/core/features/home/presentation/home_presenter.dart`

- Transformar em `StatefulWidget` para controlar a aba selecionada
- Usar `AppBar` personalizada via `HomeAppBar`
- Corpo com `SingleChildScrollView` + `Column` com todos os widgets
- `BottomNavigationBar` com 4 abas
- Passar `username` dos `arguments` da rota para `HomeWelcomeCard`

```dart
import 'package:flutter/material.dart';
import 'package:sistema_distribuido/core/features/home/domain/entities/avaliacao.dart';
import 'package:sistema_distribuido/core/features/home/presentation/widgets/home_appbar.dart';
import 'package:sistema_distribuido/core/features/home/presentation/widgets/home_welcome_card.dart';
import 'package:sistema_distribuido/core/features/home/presentation/widgets/home_metrics_row.dart';
import 'package:sistema_distribuido/core/features/home/presentation/widgets/home_quick_actions.dart';
import 'package:sistema_distribuido/core/features/home/presentation/widgets/home_next_evaluation.dart';

class HomePage extends StatefulWidget {
  const HomePage({super.key});

  @override
  State<HomePage> createState() => _HomePageState();
}

class _HomePageState extends State<HomePage> {
  int _currentIndex = 0;

  // Dados mock — futuramente virão de uma API
  final Avaliacao _proximaAvaliacao = Avaliacao(
    profissional: 'Dr. Carlos Mendes',
    data: '25 de Abril, 2026',
    tipo: 'Bioimpedância + Avaliação Postural',
    preco: 150,
  );

  @override
  Widget build(BuildContext context) {
    final String username =
        ModalRoute.of(context)?.settings.arguments as String? ?? 'Usuário';

    return Scaffold(
      backgroundColor: const Color(0xFFF5F5F5),
      appBar: const HomeAppBar(),
      body: SingleChildScrollView(
        child: Column(
          children: [
            HomeWelcomeCard(username: username),
            const SizedBox(height: 16),
            const HomeMetricsRow(),
            const SizedBox(height: 24),
            const HomeQuickActions(),
            const SizedBox(height: 24),
            HomeNextEvaluation(avaliacao: _proximaAvaliacao),
            const SizedBox(height: 24),
          ],
        ),
      ),
      bottomNavigationBar: BottomNavigationBar(
        currentIndex: _currentIndex,
        onTap: (index) => setState(() => _currentIndex = index),
        type: BottomNavigationBarType.fixed,
        selectedItemColor: const Color(0xFFD32F2F),
        unselectedItemColor: Colors.grey,
        items: const [
          BottomNavigationBarItem(icon: Icon(Icons.home), label: 'Início'),
          BottomNavigationBarItem(icon: Icon(Icons.people), label: 'Profissionais'),
          BottomNavigationBarItem(icon: Icon(Icons.calendar_month), label: 'Avaliações'),
          BottomNavigationBarItem(icon: Icon(Icons.person), label: 'Perfil'),
        ],
      ),
    );
  }
}
```

---

## Ordem de execução resumida

| # | Arquivo a criar | O que faz |
|---|---|---|
| 1 | `domain/entities/avaliacao.dart` | Modelo de dados da avaliação |
| 2 | `widgets/home_appbar.dart` | AppBar vermelha com logo |
| 3 | `widgets/home_welcome_card.dart` | Card de boas-vindas vermelho |
| 4 | `widgets/home_metrics_row.dart` | Cards de Peso e IMC |
| 5 | `widgets/home_quick_actions.dart` | Botões de ação rápida |
| 6 | `widgets/home_next_evaluation.dart` | Card da próxima avaliação |
| 7 | `home_presenter.dart` | Tela completa montando tudo |

---

## Dicas importantes

- **Cores principais:** vermelho `Color(0xFFD32F2F)`, rosa claro `Color(0xFFFFEBEE)`, fundo cinza `Color(0xFFF5F5F5)`
- **Dados mockados:** todos os valores (peso, IMC, avaliação) são fixos por enquanto. Depois você conecta com uma API.
- **Navegação do BottomBar:** por enquanto as abas só mudam o índice visual. As outras telas serão criadas depois.
- **Username:** vem da tela de login via `Navigator.pushNamedAndRemoveUntil(..., arguments: username)`
