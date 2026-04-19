import 'package:flutter/material.dart';

class Homemetricsrow extends StatelessWidget {
  final double WeightPeople;
  final double Imc;

  const Homemetricsrow({super.key, required this.WeightPeople, required this.Imc});

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.symmetric(horizontal: 16),
      child: Row(
        children: [
          Expanded(child: _MetricCard(label: 'Peso Atual', value: this.WeightPeople.toString(), icon: Icons.trending_up,)),
          const SizedBox(width: 12),
          Expanded(child: _MetricCard(label: 'IMC', value: this.Imc.toString(), icon: Icons.show_chart)),
        ],
      ),
    );
  }
}

class _MetricCard extends StatelessWidget{
  final String label;
  final String value;
  final IconData icon;

  const _MetricCard({
    required this.label,
    required this.value,
    required this.icon
});

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.all(16),
      decoration: BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.circular(12),
        border: Border.all(color: const Color(0xFFEEEEEE)),
        boxShadow: [BoxShadow(color: Colors.black.withOpacity(0.05), blurRadius: 6)], //Remover esse opção que está depreciada
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