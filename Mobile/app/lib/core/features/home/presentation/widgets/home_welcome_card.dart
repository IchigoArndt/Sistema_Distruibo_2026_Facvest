import 'package:flutter/material.dart';

class HomeWelcomeCard extends StatelessWidget{
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
          Text('Olá', style: const TextStyle(color: Colors.white, fontSize: 22, fontWeight: FontWeight.bold)),
          const SizedBox(height: 4),
          const Text('Bem vindo ao AnFis', style: TextStyle(color: Colors.white70, fontSize: 14)),
        ],
      ),
    );
  }
}