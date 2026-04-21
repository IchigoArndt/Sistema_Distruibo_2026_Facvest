import 'package:flutter/material.dart';
import 'package:sistema_distribuido/core/features/avaliacoes/domain/entities/assessment.dart';
import 'package:sistema_distribuido/core/features/avaliacoes/presentation/widgets/assessment_card.dart';

class AvaliacoesPage extends StatefulWidget {
  const AvaliacoesPage({super.key});

  @override
  State<AvaliacoesPage> createState() => _AvaliacoesPageState();
}

class _AvaliacoesPageState extends State<AvaliacoesPage> with SingleTickerProviderStateMixin {
  late TabController _tabController;
  Assessment? _selectedAssessment;

  // Dados mock — futuramente virão de uma API
  final List<Assessment> _assessments = [
    Assessment(
      id: 1,
      professionalName: 'Dr. Carlos Mendes',
      date: DateTime(2026, 4, 25),
      status: AssessmentStatus.requested,
      methodology: 'Bioimpedância + Avaliação Postural',
      price: 150.00,
      notes: 'Primeira avaliação — trazer exames recentes.',
    ),
    Assessment(
      id: 2,
      professionalName: 'Dra. Ana Paula Costa',
      date: DateTime(2026, 4, 18),
      status: AssessmentStatus.inProgress,
      methodology: 'Avaliação Postural Completa',
      price: 180.00,
      notes: 'Aguardando laudo do profissional.',
    ),
    Assessment(
      id: 3,
      professionalName: 'Dr. Carlos Mendes',
      date: DateTime(2026, 3, 10),
      status: AssessmentStatus.completed,
      methodology: 'Bioimpedância + Avaliação Funcional',
      price: 150.00,
      notes: 'Ótima evolução desde a última avaliação.',
      results: AssessmentResults(
        weight: 78.5,
        height: 175,
        bodyFat: 18.2,
        muscleMass: 36.4,
        imc: 25.6,
      ),
    ),
    Assessment(
      id: 4,
      professionalName: 'Dra. Fernanda Rocha',
      date: DateTime(2026, 1, 20),
      status: AssessmentStatus.completed,
      methodology: 'Avaliação Física Geral + Pilates',
      price: 200.00,
      results: AssessmentResults(
        weight: 80.2,
        height: 175,
        bodyFat: 20.1,
        muscleMass: 35.0,
        imc: 26.2,
      ),
    ),
  ];

  @override
  void initState() {
    super.initState();
    _tabController = TabController(length: 4, vsync: this);
  }

  @override
  void dispose() {
    _tabController.dispose();
    super.dispose();
  }

  List<Assessment> _filterByStatus(AssessmentStatus status) =>
      _assessments.where((a) => a.status == status).toList();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: const Color(0xFFF5F5F5),
      appBar: AppBar(
        backgroundColor: const Color(0xFFD32F2F),
        foregroundColor: Colors.white,
        title: const Text('Minhas Avaliações', style: TextStyle(fontWeight: FontWeight.bold)),
        bottom: TabBar(
          controller: _tabController,
          indicatorColor: Colors.white,
          indicatorWeight: 3,
          labelColor: Colors.white,
          unselectedLabelColor: Colors.white60,
          labelStyle: const TextStyle(fontSize: 12, fontWeight: FontWeight.w600),
          unselectedLabelStyle: const TextStyle(fontSize: 12),
          tabs: const [
            Tab(text: 'Todas'),
            Tab(text: 'Solicitadas'),
            Tab(text: 'Andamento'),
            Tab(text: 'Concluídas'),
          ],
        ),
      ),
      body: Stack(
        children: [
          Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Container(
                color: Colors.white,
                padding: const EdgeInsets.fromLTRB(16, 12, 16, 12),
                child: const Text(
                  'Acompanhe seu histórico e progresso',
                  style: TextStyle(fontSize: 13, color: Colors.grey),
                ),
              ),
              Expanded(
                child: TabBarView(
                  controller: _tabController,
                  children: [
                    _AssessmentList(
                      items: _assessments,
                      emptyMessage: 'Nenhuma avaliação encontrada',
                      onTap: (a) => setState(() => _selectedAssessment = a),
                    ),
                    _AssessmentList(
                      items: _filterByStatus(AssessmentStatus.requested),
                      emptyMessage: 'Nenhuma avaliação solicitada',
                      onTap: (a) => setState(() => _selectedAssessment = a),
                    ),
                    _AssessmentList(
                      items: _filterByStatus(AssessmentStatus.inProgress),
                      emptyMessage: 'Nenhuma avaliação em andamento',
                      onTap: (a) => setState(() => _selectedAssessment = a),
                    ),
                    _AssessmentList(
                      items: _filterByStatus(AssessmentStatus.completed),
                      emptyMessage: 'Nenhuma avaliação concluída',
                      onTap: (a) => setState(() => _selectedAssessment = a),
                    ),
                  ],
                ),
              ),
            ],
          ),

          // Dialog de detalhes
          if (_selectedAssessment != null)
            GestureDetector(
              onTap: () => setState(() => _selectedAssessment = null),
              child: Container(
                color: Colors.black54,
                child: Center(
                  child: GestureDetector(
                    onTap: () {},
                    child: Container(
                      margin: const EdgeInsets.symmetric(horizontal: 20),
                      constraints: BoxConstraints(
                        maxHeight: MediaQuery.of(context).size.height * 0.85,
                      ),
                      decoration: BoxDecoration(
                        color: Colors.white,
                        borderRadius: BorderRadius.circular(16),
                      ),
                      child: Column(
                        mainAxisSize: MainAxisSize.min,
                        children: [
                          Padding(
                            padding: const EdgeInsets.fromLTRB(20, 20, 12, 0),
                            child: Row(
                              mainAxisAlignment: MainAxisAlignment.spaceBetween,
                              children: [
                                const Text(
                                  'Detalhes da Avaliação',
                                  style: TextStyle(fontSize: 17, fontWeight: FontWeight.bold, color: Color(0xFF212121)),
                                ),
                                IconButton(
                                  icon: const Icon(Icons.close, color: Colors.grey),
                                  onPressed: () => setState(() => _selectedAssessment = null),
                                ),
                              ],
                            ),
                          ),
                          Flexible(
                            child: SingleChildScrollView(
                              padding: const EdgeInsets.fromLTRB(20, 8, 20, 24),
                              child: _AssessmentDetailContent(assessment: _selectedAssessment!),
                            ),
                          ),
                        ],
                      ),
                    ),
                  ),
                ),
              ),
            ),
        ],
      ),
    );
  }
}

class _AssessmentList extends StatelessWidget {
  final List<Assessment> items;
  final String emptyMessage;
  final ValueChanged<Assessment> onTap;

  const _AssessmentList({
    required this.items,
    required this.emptyMessage,
    required this.onTap,
  });

  @override
  Widget build(BuildContext context) {
    if (items.isEmpty) {
      return Center(
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            const Icon(Icons.assignment_outlined, size: 48, color: Colors.grey),
            const SizedBox(height: 12),
            Text(emptyMessage, style: const TextStyle(color: Colors.grey, fontSize: 14)),
          ],
        ),
      );
    }

    return ListView.separated(
      padding: const EdgeInsets.all(16),
      itemCount: items.length,
      separatorBuilder: (context, index) => const SizedBox(height: 12),
      itemBuilder: (context, index) => AssessmentCard(
        assessment: items[index],
        onTap: () => onTap(items[index]),
      ),
    );
  }
}

class _AssessmentDetailContent extends StatelessWidget {
  final Assessment assessment;

  const _AssessmentDetailContent({required this.assessment});

  String _formatDate(DateTime date) {
    const months = [
      'janeiro', 'fevereiro', 'março', 'abril', 'maio', 'junho',
      'julho', 'agosto', 'setembro', 'outubro', 'novembro', 'dezembro'
    ];
    return '${date.day.toString().padLeft(2, '0')} de ${months[date.month - 1]} de ${date.year}';
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            Expanded(
              child: Text(
                assessment.professionalName,
                style: const TextStyle(fontSize: 15, fontWeight: FontWeight.bold, color: Color(0xFF212121)),
              ),
            ),
            const SizedBox(width: 8),
            _DetailStatusBadge(status: assessment.status),
          ],
        ),
        const SizedBox(height: 16),
        _DetailRow(label: 'Data:', value: _formatDate(assessment.date)),
        const SizedBox(height: 8),
        _DetailRow(label: 'Metodologia:', value: assessment.methodology, valueAlign: CrossAxisAlignment.end),
        const SizedBox(height: 8),
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            const Text('Valor:', style: TextStyle(fontSize: 13, color: Colors.grey)),
            Text(
              'R\$ ${assessment.price.toStringAsFixed(2)}',
              style: const TextStyle(fontSize: 13, fontWeight: FontWeight.w600, color: Color(0xFFD32F2F)),
            ),
          ],
        ),
        if (assessment.notes != null) ...[
          const SizedBox(height: 12),
          const Text('Observações:', style: TextStyle(fontSize: 13, color: Colors.grey)),
          const SizedBox(height: 4),
          Container(
            width: double.infinity,
            padding: const EdgeInsets.all(12),
            decoration: BoxDecoration(
              color: const Color(0xFFF5F5F5),
              borderRadius: BorderRadius.circular(8),
            ),
            child: Text(assessment.notes!, style: const TextStyle(fontSize: 13, color: Color(0xFF424242))),
          ),
        ],
        if (assessment.results != null) ...[
          const SizedBox(height: 16),
          const Divider(height: 1, color: Color(0xFFEEEEEE)),
          const SizedBox(height: 16),
          Row(
            children: const [
              Icon(Icons.bar_chart, color: Color(0xFFD32F2F), size: 20),
              SizedBox(width: 8),
              Text('Resultados', style: TextStyle(fontSize: 15, fontWeight: FontWeight.bold, color: Color(0xFF212121))),
            ],
          ),
          const SizedBox(height: 12),
          GridView.count(
            shrinkWrap: true,
            physics: const NeverScrollableScrollPhysics(),
            crossAxisCount: 2,
            childAspectRatio: 2.2,
            mainAxisSpacing: 8,
            crossAxisSpacing: 8,
            children: [
              _ResultTile(label: 'Peso', value: '${assessment.results!.weight} kg'),
              _ResultTile(label: 'Altura', value: '${assessment.results!.height} cm'),
              _ResultTile(label: '% Gordura', value: '${assessment.results!.bodyFat}%'),
              _ResultTile(label: 'Massa Muscular', value: '${assessment.results!.muscleMass} kg'),
            ],
          ),
          const SizedBox(height: 8),
          _ResultTile(
            label: 'IMC',
            value: assessment.results!.imc.toStringAsFixed(1),
            fullWidth: true,
          ),
        ],
      ],
    );
  }
}

class _DetailRow extends StatelessWidget {
  final String label;
  final String value;
  final CrossAxisAlignment valueAlign;

  const _DetailRow({
    required this.label,
    required this.value,
    this.valueAlign = CrossAxisAlignment.end,
  });

  @override
  Widget build(BuildContext context) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.spaceBetween,
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(label, style: const TextStyle(fontSize: 13, color: Colors.grey)),
        const SizedBox(width: 12),
        Flexible(
          child: Text(
            value,
            textAlign: TextAlign.right,
            style: const TextStyle(fontSize: 13, color: Color(0xFF212121)),
          ),
        ),
      ],
    );
  }
}

class _ResultTile extends StatelessWidget {
  final String label;
  final String value;
  final bool fullWidth;

  const _ResultTile({required this.label, required this.value, this.fullWidth = false});

  @override
  Widget build(BuildContext context) {
    return Container(
      width: fullWidth ? double.infinity : null,
      padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 8),
      decoration: BoxDecoration(
        color: const Color(0xFFFFEBEE),
        borderRadius: BorderRadius.circular(8),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          Text(label, style: const TextStyle(fontSize: 11, color: Colors.grey)),
          const SizedBox(height: 2),
          Text(value, style: const TextStyle(fontSize: 14, fontWeight: FontWeight.bold, color: Color(0xFF212121))),
        ],
      ),
    );
  }
}

class _DetailStatusBadge extends StatelessWidget {
  final AssessmentStatus status;

  const _DetailStatusBadge({required this.status});

  @override
  Widget build(BuildContext context) {
    late Color bgColor;
    late Color textColor;
    late String label;

    switch (status) {
      case AssessmentStatus.requested:
        bgColor = const Color(0xFFFFF9C4);
        textColor = const Color(0xFFF57F17);
        label = 'Solicitada';
      case AssessmentStatus.inProgress:
        bgColor = const Color(0xFFE3F2FD);
        textColor = const Color(0xFF1565C0);
        label = 'Em Andamento';
      case AssessmentStatus.completed:
        bgColor = const Color(0xFFE8F5E9);
        textColor = const Color(0xFF2E7D32);
        label = 'Concluída';
    }

    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 4),
      decoration: BoxDecoration(color: bgColor, borderRadius: BorderRadius.circular(20)),
      child: Text(label, style: TextStyle(fontSize: 11, color: textColor, fontWeight: FontWeight.w600)),
    );
  }
}
