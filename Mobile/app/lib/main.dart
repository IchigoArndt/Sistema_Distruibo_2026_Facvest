import 'package:flutter/material.dart';
import 'package:sistema_distribuido/core/features/login/presentation/login_presenter.dart';
import 'package:sistema_distribuido/core/features/home/presentation/home_presenter.dart';
import 'package:sistema_distribuido/core/features/perfil/presentation/perfil_presenter.dart';
import 'package:sistema_distribuido/core/features/profissionais/presentation/profissionais_presenter.dart';
import 'package:sistema_distribuido/core/features/avaliacoes/presentation/avaliacoes_presenter.dart';

void main() {
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Sistema Distribuído',
      theme: ThemeData(
        colorScheme: ColorScheme.fromSeed(seedColor: Colors.deepPurple),
      ),
      initialRoute: '/login',
      routes: {
        '/login': (context) => const LoginPage(),
        '/home': (context) => const HomePage(),
        '/perfil': (context) => const PerfilPage(),
        '/profissionais': (context) => const ProfissionaisPage(),
        '/avaliacoes': (context) => const AvaliacoesPage(),
      },
    );
  }
}
