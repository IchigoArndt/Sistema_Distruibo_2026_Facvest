import 'package:flutter/material.dart';
import 'package:sistema_distribuido/core/features/perfil/domain/entities/health_profile.dart';

class PerfilPersonalInfoCard extends StatelessWidget {
  final HealthProfile profile;
  final bool isEditing;
  final ValueChanged<HealthProfile> onChanged;

  const PerfilPersonalInfoCard({
    super.key,
    required this.profile,
    required this.isEditing,
    required this.onChanged,
  });

  @override
  Widget build(BuildContext context) {
    return _PerfilCard(
      icon: Icons.person,
      title: 'Informações Pessoais',
      child: Column(
        children: [
          _LabeledField(
            label: 'Nome Completo',
            value: profile.name,
            enabled: isEditing,
            onChanged: (v) => onChanged(HealthProfile(
              name: v,
              age: profile.age,
              gender: profile.gender,
              weight: profile.weight,
              height: profile.height,
              medicalConditions: profile.medicalConditions,
              medications: profile.medications,
              goals: profile.goals,
            )),
          ),
          const SizedBox(height: 12),
          Row(
            children: [
              Expanded(
                child: _LabeledField(
                  label: 'Idade',
                  value: profile.age.toString(),
                  enabled: isEditing,
                  keyboardType: TextInputType.number,
                  onChanged: (v) => onChanged(HealthProfile(
                    name: profile.name,
                    age: int.tryParse(v) ?? profile.age,
                    gender: profile.gender,
                    weight: profile.weight,
                    height: profile.height,
                    medicalConditions: profile.medicalConditions,
                    medications: profile.medications,
                    goals: profile.goals,
                  )),
                ),
              ),
              const SizedBox(width: 12),
              Expanded(
                child: _LabeledField(
                  label: 'Gênero',
                  value: profile.gender,
                  enabled: isEditing,
                  onChanged: (v) => onChanged(HealthProfile(
                    name: profile.name,
                    age: profile.age,
                    gender: v,
                    weight: profile.weight,
                    height: profile.height,
                    medicalConditions: profile.medicalConditions,
                    medications: profile.medications,
                    goals: profile.goals,
                  )),
                ),
              ),
            ],
          ),
          const SizedBox(height: 12),
          Row(
            children: [
              Expanded(
                child: _LabeledField(
                  label: 'Peso (kg)',
                  value: profile.weight.toString(),
                  enabled: isEditing,
                  keyboardType: const TextInputType.numberWithOptions(decimal: true),
                  onChanged: (v) => onChanged(HealthProfile(
                    name: profile.name,
                    age: profile.age,
                    gender: profile.gender,
                    weight: double.tryParse(v) ?? profile.weight,
                    height: profile.height,
                    medicalConditions: profile.medicalConditions,
                    medications: profile.medications,
                    goals: profile.goals,
                  )),
                ),
              ),
              const SizedBox(width: 12),
              Expanded(
                child: _LabeledField(
                  label: 'Altura (cm)',
                  value: profile.height.toString(),
                  enabled: isEditing,
                  keyboardType: TextInputType.number,
                  onChanged: (v) => onChanged(HealthProfile(
                    name: profile.name,
                    age: profile.age,
                    gender: profile.gender,
                    weight: profile.weight,
                    height: int.tryParse(v) ?? profile.height,
                    medicalConditions: profile.medicalConditions,
                    medications: profile.medications,
                    goals: profile.goals,
                  )),
                ),
              ),
            ],
          ),
        ],
      ),
    );
  }
}

class _LabeledField extends StatelessWidget {
  final String label;
  final String value;
  final bool enabled;
  final TextInputType? keyboardType;
  final ValueChanged<String> onChanged;

  const _LabeledField({
    required this.label,
    required this.value,
    required this.enabled,
    required this.onChanged,
    this.keyboardType,
  });

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(label, style: const TextStyle(fontSize: 12, color: Colors.grey, fontWeight: FontWeight.w500)),
        const SizedBox(height: 4),
        TextFormField(
          initialValue: value,
          enabled: enabled,
          keyboardType: keyboardType,
          onChanged: onChanged,
          style: const TextStyle(fontSize: 14),
          decoration: InputDecoration(
            isDense: true,
            contentPadding: const EdgeInsets.symmetric(horizontal: 12, vertical: 10),
            filled: true,
            fillColor: enabled ? Colors.white : const Color(0xFFF5F5F5),
            enabledBorder: OutlineInputBorder(
              borderRadius: BorderRadius.circular(8),
              borderSide: const BorderSide(color: Color(0xFFDDDDDD)),
            ),
            disabledBorder: OutlineInputBorder(
              borderRadius: BorderRadius.circular(8),
              borderSide: const BorderSide(color: Color(0xFFEEEEEE)),
            ),
            focusedBorder: OutlineInputBorder(
              borderRadius: BorderRadius.circular(8),
              borderSide: const BorderSide(color: Color(0xFFD32F2F)),
            ),
          ),
        ),
      ],
    );
  }
}

class _PerfilCard extends StatelessWidget {
  final IconData icon;
  final String title;
  final Widget child;

  const _PerfilCard({
    required this.icon,
    required this.title,
    required this.child,
  });

  @override
  Widget build(BuildContext context) {
    return Container(
      width: double.infinity,
      padding: const EdgeInsets.all(16),
      decoration: BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.circular(12),
        border: Border.all(color: const Color(0xFFFFCDD2)),
        boxShadow: [
          BoxShadow(color: Colors.black.withValues(alpha: 0.04), blurRadius: 6),
        ],
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            children: [
              Container(
                padding: const EdgeInsets.all(8),
                decoration: BoxDecoration(
                  color: const Color(0xFFFFEBEE),
                  borderRadius: BorderRadius.circular(8),
                ),
                child: Icon(icon, color: const Color(0xFFD32F2F), size: 20),
              ),
              const SizedBox(width: 12),
              Text(title, style: const TextStyle(fontSize: 15, fontWeight: FontWeight.bold, color: Color(0xFF212121))),
            ],
          ),
          const SizedBox(height: 16),
          child,
        ],
      ),
    );
  }
}
