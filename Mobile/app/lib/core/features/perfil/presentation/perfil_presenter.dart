import 'package:flutter/material.dart';
import 'package:sistema_distribuido/core/features/perfil/domain/entities/health_profile.dart';
import 'package:sistema_distribuido/core/features/perfil/presentation/widgets/perfil_personal_info_card.dart';
import 'package:sistema_distribuido/core/features/perfil/presentation/widgets/perfil_list_card.dart';

class PerfilPage extends StatefulWidget {
  const PerfilPage({super.key});

  @override
  State<PerfilPage> createState() => _PerfilPageState();
}

class _PerfilPageState extends State<PerfilPage> {
  bool _isEditing = false;

  HealthProfile _profile = HealthProfile(
    name: 'João da Silva',
    age: 30,
    gender: 'Masculino',
    weight: 78.5,
    height: 175,
    medicalConditions: ['Hipertensão leve', 'Diabetes tipo 2'],
    medications: ['Losartana 50mg', 'Metformina 500mg'],
    goals: ['Perder 5kg', 'Melhorar condicionamento físico', 'Controlar glicemia'],
  );

  void _handleEditSave() {
    setState(() => _isEditing = !_isEditing);
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: const Color(0xFFF5F5F5),
      appBar: AppBar(
        backgroundColor: const Color(0xFFD32F2F),
        foregroundColor: Colors.white,
        title: const Text('Meu Perfil', style: TextStyle(fontWeight: FontWeight.bold)),
        actions: [
          Padding(
            padding: const EdgeInsets.only(right: 12),
            child: OutlinedButton.icon(
              onPressed: _handleEditSave,
              icon: Icon(
                _isEditing ? Icons.save : Icons.edit,
                size: 16,
                color: Colors.white,
              ),
              label: Text(
                _isEditing ? 'Salvar' : 'Editar',
                style: const TextStyle(color: Colors.white),
              ),
              style: OutlinedButton.styleFrom(
                side: const BorderSide(color: Colors.white70),
                padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 6),
              ),
            ),
          ),
        ],
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(16),
        child: Column(
          children: [
            PerfilPersonalInfoCard(
              profile: _profile,
              isEditing: _isEditing,
              onChanged: (updated) => setState(() => _profile = updated),
            ),
            const SizedBox(height: 16),
            PerfilListCard(
              icon: Icons.favorite,
              title: 'Condições Médicas',
              items: _profile.medicalConditions,
              isEditing: _isEditing,
              placeholder: 'Ex: Hipertensão leve',
              onChanged: (list) => setState(() => _profile.medicalConditions = list),
            ),
            const SizedBox(height: 16),
            PerfilListCard(
              icon: Icons.medication,
              title: 'Medicamentos',
              items: _profile.medications,
              isEditing: _isEditing,
              placeholder: 'Ex: Losartana 50mg',
              onChanged: (list) => setState(() => _profile.medications = list),
            ),
            const SizedBox(height: 16),
            PerfilListCard(
              icon: Icons.flag,
              title: 'Objetivos',
              items: _profile.goals,
              isEditing: _isEditing,
              placeholder: 'Ex: Perder 5kg',
              onChanged: (list) => setState(() => _profile.goals = list),
            ),
            const SizedBox(height: 24),
          ],
        ),
      ),
    );
  }
}
