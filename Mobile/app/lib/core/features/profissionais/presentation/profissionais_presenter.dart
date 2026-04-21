import 'package:flutter/material.dart';
import 'package:sistema_distribuido/core/features/profissionais/domain/entities/professional.dart';
import 'package:sistema_distribuido/core/features/profissionais/presentation/widgets/professional_card.dart';
import 'package:sistema_distribuido/core/features/profissionais/presentation/professional_detail_presenter.dart';

class ProfissionaisPage extends StatefulWidget {
  const ProfissionaisPage({super.key});

  @override
  State<ProfissionaisPage> createState() => _ProfissionaisPageState();
}

class _ProfissionaisPageState extends State<ProfissionaisPage> {
  final TextEditingController _searchController = TextEditingController();
  String _searchTerm = '';

  // Dados mock — futuramente virão de uma API
  final List<Professional> _professionals = [
    Professional(
      id: 1,
      name: 'Dr. Carlos Mendes',
      specialty: 'Educação Física e Biomecânica',
      rating: 4.9,
      reviews: 128,
      phone: '(47) 99123-4567',
      email: 'carlos.mendes@anfis.com',
      methodology: 'Funcional',
      price: 150.00,
      experience: '10 anos de experiência',
      about: 'Especialista em avaliação funcional e biomecânica do movimento humano. Atua com foco na prevenção de lesões e melhora do desempenho físico, utilizando protocolos baseados em evidências científicas.',
      certifications: [
        'CREF 012345-G/SC',
        'Especialização em Biomecânica – USP',
        'Certificação CSCS (NSCA)',
        'Curso de Avaliação Funcional do Movimento (FMS)',
      ],
    ),
    Professional(
      id: 2,
      name: 'Dra. Ana Paula Costa',
      specialty: 'Fisioterapia e Posturologia',
      rating: 4.8,
      reviews: 95,
      phone: '(47) 98876-5432',
      email: 'ana.costa@anfis.com',
      methodology: 'Postural',
      price: 180.00,
      experience: '8 anos de experiência',
      about: 'Fisioterapeuta com especialização em posturologia e reabilitação ortopédica. Realiza avaliações posturais completas e elabora programas individualizados de correção e prevenção.',
      certifications: [
        'CREFITO-10 12345-F',
        'Especialização em Fisioterapia Ortopédica – UNICAMP',
        'Certificação em RPG (Reeducação Postural Global)',
        'Curso de Pilates Clínico',
      ],
    ),
    Professional(
      id: 3,
      name: 'Dr. Roberto Lima',
      specialty: 'Nutrição Esportiva',
      rating: 4.7,
      reviews: 74,
      phone: '(47) 97654-3210',
      email: 'roberto.lima@anfis.com',
      methodology: 'Clínico',
      price: 120.00,
      experience: '6 anos de experiência',
      about: 'Nutricionista esportivo focado em otimização da composição corporal e desempenho atlético. Trabalha com análise de bioimpedância e prescrição nutricional personalizada.',
      certifications: [
        'CRN-10 12345',
        'Especialização em Nutrição Esportiva – UNIFESP',
        'Curso de Bioimpedância Avançada',
      ],
    ),
    Professional(
      id: 4,
      name: 'Dra. Fernanda Rocha',
      specialty: 'Personal Trainer e Pilates',
      rating: 5.0,
      reviews: 210,
      phone: '(47) 96543-2109',
      email: 'fernanda.rocha@anfis.com',
      methodology: 'Pilates',
      price: 200.00,
      experience: '12 anos de experiência',
      about: 'Personal trainer e instrutora de Pilates com vasta experiência no atendimento a adultos e idosos. Especialista em condicionamento físico, flexibilidade e qualidade de vida.',
      certifications: [
        'CREF 054321-G/SC',
        'Certificação Stott Pilates (Mat & Equipment)',
        'Especialização em Treinamento Funcional',
        'Curso de Treinamento para Idosos',
      ],
    ),
  ];

  List<Professional> get _filtered {
    if (_searchTerm.isEmpty) return _professionals;
    final term = _searchTerm.toLowerCase();
    return _professionals.where((p) =>
      p.name.toLowerCase().contains(term) ||
      p.specialty.toLowerCase().contains(term) ||
      p.methodology.toLowerCase().contains(term),
    ).toList();
  }

  @override
  void dispose() {
    _searchController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: const Color(0xFFF5F5F5),
      appBar: AppBar(
        backgroundColor: const Color(0xFFD32F2F),
        foregroundColor: Colors.white,
        title: const Text('Profissionais', style: TextStyle(fontWeight: FontWeight.bold)),
      ),
      body: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Container(
            color: Colors.white,
            padding: const EdgeInsets.fromLTRB(16, 16, 16, 12),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                const Text(
                  'Encontre o especialista ideal para você',
                  style: TextStyle(fontSize: 13, color: Colors.grey),
                ),
                const SizedBox(height: 12),
                TextField(
                  controller: _searchController,
                  onChanged: (v) => setState(() => _searchTerm = v),
                  style: const TextStyle(fontSize: 14),
                  decoration: InputDecoration(
                    hintText: 'Buscar por nome, especialidade...',
                    hintStyle: const TextStyle(color: Colors.grey, fontSize: 13),
                    prefixIcon: const Icon(Icons.search, color: Colors.grey, size: 20),
                    suffixIcon: _searchTerm.isNotEmpty
                        ? IconButton(
                            icon: const Icon(Icons.clear, size: 18, color: Colors.grey),
                            onPressed: () {
                              _searchController.clear();
                              setState(() => _searchTerm = '');
                            },
                          )
                        : null,
                    isDense: true,
                    filled: true,
                    fillColor: const Color(0xFFF5F5F5),
                    contentPadding: const EdgeInsets.symmetric(vertical: 10),
                    border: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(10),
                      borderSide: const BorderSide(color: Color(0xFFDDDDDD)),
                    ),
                    enabledBorder: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(10),
                      borderSide: const BorderSide(color: Color(0xFFDDDDDD)),
                    ),
                    focusedBorder: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(10),
                      borderSide: const BorderSide(color: Color(0xFFD32F2F)),
                    ),
                  ),
                ),
              ],
            ),
          ),
          Expanded(
            child: _filtered.isEmpty
                ? const Center(
                    child: Column(
                      mainAxisSize: MainAxisSize.min,
                      children: [
                        Icon(Icons.search_off, size: 48, color: Colors.grey),
                        SizedBox(height: 12),
                        Text(
                          'Nenhum profissional encontrado',
                          style: TextStyle(color: Colors.grey, fontSize: 14),
                        ),
                      ],
                    ),
                  )
                : ListView.separated(
                    padding: const EdgeInsets.all(16),
                    itemCount: _filtered.length,
                    separatorBuilder: (context, index) => const SizedBox(height: 12),
                    itemBuilder: (context, index) {
                      final prof = _filtered[index];
                      return ProfessionalCard(
                        professional: prof,
                        onTap: () {
                          Navigator.push(
                            context,
                            MaterialPageRoute(
                              builder: (context) => ProfessionalDetailPage(professional: prof),
                            ),
                          );
                        },
                      );
                    },
                  ),
          ),
        ],
      ),
    );
  }
}
