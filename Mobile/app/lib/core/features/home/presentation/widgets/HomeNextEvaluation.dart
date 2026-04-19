import 'package:flutter/material.dart';
import 'package:sistema_distribuido/core/features/home/domain/entities/Avaliation.dart';

class HomeNextEvaluation extends StatelessWidget{
  final Avaliation avaliation;

  const HomeNextEvaluation({super.key, required this.avaliation});

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
                    Text(avaliation.Professional,
                        style: const TextStyle(
                            color: Color(0xFFD32F2F), fontWeight: FontWeight.bold, fontSize: 15)),
                    Container(
                      padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 4),
                      decoration: BoxDecoration(
                        color: const Color(0xFFD32F2F),
                        borderRadius: BorderRadius.circular(20),
                      ),
                      child: Text('R\$ ${avaliation.Price.toStringAsFixed(0)}',
                          style: const TextStyle(color: Colors.white, fontSize: 12, fontWeight: FontWeight.bold)),
                    ),
                  ],
                ),
                const SizedBox(height: 6),
                Text(avaliation.Data.toString(),
                    style: const TextStyle(color: Color(0xFFE57373), fontSize: 13)),
                const SizedBox(height: 4),
                Text(avaliation.Type,
                    style: const TextStyle(color: Color(0xFFE57373), fontSize: 13)),
              ],
            ),
          ),
        ],
      ),
    );
  }
}