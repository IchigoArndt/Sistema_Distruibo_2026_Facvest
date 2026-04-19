import 'package:flutter/material.dart';
import 'package:sistema_distribuido/core/features/home/domain/entities/Avaliation.dart';
import 'package:sistema_distribuido/core/features/home/presentation/widgets/home_appbar.dart';
import 'package:sistema_distribuido/core/features/home/presentation/widgets/home_welcome_card.dart';
import 'package:sistema_distribuido/core/features/home/presentation/widgets/HomeMetricsRow.dart';
import 'package:sistema_distribuido/core/features/home/presentation/widgets/HomeQuickActions.dart';
import 'package:sistema_distribuido/core/features/home/presentation/widgets/HomeNextEvaluation.dart';

class HomePage extends StatefulWidget {
  const HomePage({super.key});

  @override
  State<HomePage> createState() => _HomePageState();
}

class _HomePageState extends State<HomePage> {
  int _currentIndex = 0;

  // Dados mock — futuramente virão de uma API
  final Avaliation _proximaAvaliacao = Avaliation(
    Professional: 'Dr. Carlos Mendes',
    Data: DateTime(2026,04,25),
    Type: 'Bioimpedância + Avaliação Postural',
    Price: 150,
  );

  @override
  Widget build(BuildContext context) {
    final String username =
        ModalRoute.of(context)?.settings.arguments as String? ?? 'Usuário';

    return Scaffold(
      backgroundColor: const Color(0xFFF5F5F5),
      appBar: const HomeAppbar(),
      body: SingleChildScrollView(
        child: Column(
          children: [
            HomeWelcomeCard(username: username),
            const SizedBox(height: 16),
            const Homemetricsrow(Imc: 25.6,WeightPeople: 78.5),
            const SizedBox(height: 24),
            const HomeQuickActions(),
            const SizedBox(height: 24),
            HomeNextEvaluation(avaliation: _proximaAvaliacao),
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