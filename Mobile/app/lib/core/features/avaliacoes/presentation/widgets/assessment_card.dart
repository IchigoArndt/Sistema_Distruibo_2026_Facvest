import 'package:flutter/material.dart';
import 'package:sistema_distribuido/core/features/avaliacoes/domain/entities/assessment.dart';

class AssessmentCard extends StatelessWidget {
  final Assessment assessment;
  final VoidCallback onTap;

  const AssessmentCard({
    super.key,
    required this.assessment,
    required this.onTap,
  });

  String _formatDate(DateTime date) {
    const months = [
      'janeiro', 'fevereiro', 'março', 'abril', 'maio', 'junho',
      'julho', 'agosto', 'setembro', 'outubro', 'novembro', 'dezembro'
    ];
    return '${date.day.toString().padLeft(2, '0')} de ${months[date.month - 1]} de ${date.year}';
  }

  String _typeLabel(TypeAvaliation type) {
    switch (type) {
      case TypeAvaliation.complete:
        return 'Completa';
      case TypeAvaliation.revaluation:
        return 'Reavaliação';
      default:
        return 'Básica';
    }
  }

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: onTap,
      child: Container(
        padding: const EdgeInsets.all(16),
        decoration: BoxDecoration(
          color: Colors.white,
          borderRadius: BorderRadius.circular(12),
          border: Border.all(color: const Color(0xFFFFCDD2)),
          boxShadow: [
            BoxShadow(
                color: Colors.black.withValues(alpha: 0.04), blurRadius: 6),
          ],
        ),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        assessment.professionalName ??
                            'Profissional #${assessment.professionalId}',
                        style: const TextStyle(
                            fontSize: 15,
                            fontWeight: FontWeight.bold,
                            color: Color(0xFF212121)),
                      ),
                      const SizedBox(height: 4),
                      Row(
                        children: [
                          const Icon(Icons.calendar_today,
                              size: 13, color: Colors.grey),
                          const SizedBox(width: 4),
                          Text(
                            _formatDate(assessment.date),
                            style: const TextStyle(
                                fontSize: 12, color: Colors.grey),
                          ),
                        ],
                      ),
                    ],
                  ),
                ),
                _StatusBadge(status: assessment.status),
              ],
            ),
            const SizedBox(height: 10),
            Row(
              children: [
                const Icon(Icons.assignment_outlined,
                    size: 14, color: Colors.grey),
                const SizedBox(width: 4),
                Text(
                  _typeLabel(assessment.typeAvaliation),
                  style: const TextStyle(
                      fontSize: 13, color: Color(0xFF424242)),
                ),
              ],
            ),
            if (assessment.imc != null || assessment.bodyFatPercentage != null) ...[
              const SizedBox(height: 10),
              const Divider(height: 1, color: Color(0xFFEEEEEE)),
              const SizedBox(height: 10),
              Row(
                children: [
                  if (assessment.imc != null) ...[
                    _MetricChip(label: 'IMC', value: assessment.imc!),
                    const SizedBox(width: 8),
                  ],
                  if (assessment.bodyFatPercentage != null)
                    _MetricChip(
                        label: '% Gord.',
                        value: assessment.bodyFatPercentage!),
                ],
              ),
            ],
          ],
        ),
      ),
    );
  }
}

class _MetricChip extends StatelessWidget {
  final String label;
  final String value;

  const _MetricChip({required this.label, required this.value});

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 4),
      decoration: BoxDecoration(
        color: const Color(0xFFFFEBEE),
        borderRadius: BorderRadius.circular(20),
      ),
      child: Text(
        '$label: $value',
        style: const TextStyle(
            fontSize: 12,
            color: Color(0xFFD32F2F),
            fontWeight: FontWeight.w500),
      ),
    );
  }
}

class _StatusBadge extends StatelessWidget {
  final AssessmentStatus status;

  const _StatusBadge({required this.status});

  @override
  Widget build(BuildContext context) {
    final bgColor = status == AssessmentStatus.completed
        ? const Color(0xFFE8F5E9)
        : const Color(0xFFFFF9C4);
    final textColor = status == AssessmentStatus.completed
        ? const Color(0xFF2E7D32)
        : const Color(0xFFF57F17);
    final icon = status == AssessmentStatus.completed
        ? Icons.check_circle_outline
        : Icons.access_time;
    final label =
        status == AssessmentStatus.completed ? 'Concluída' : 'Pendente';

    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 4),
      decoration: BoxDecoration(
        color: bgColor,
        borderRadius: BorderRadius.circular(20),
      ),
      child: Row(
        mainAxisSize: MainAxisSize.min,
        children: [
          Icon(icon, size: 12, color: textColor),
          const SizedBox(width: 4),
          Text(label,
              style: TextStyle(
                  fontSize: 11,
                  color: textColor,
                  fontWeight: FontWeight.w600)),
        ],
      ),
    );
  }
}
