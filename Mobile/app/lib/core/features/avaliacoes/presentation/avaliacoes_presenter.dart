import 'package:flutter/material.dart';
import 'package:get_it/get_it.dart';
import 'package:sistema_distribuido/core/features/avaliacoes/domain/entities/assessment.dart';
import 'package:sistema_distribuido/core/features/avaliacoes/domain/services/IAvaliacoesService.dart';
import 'package:sistema_distribuido/core/features/avaliacoes/presentation/widgets/assessment_card.dart';

class AvaliacoesPage extends StatefulWidget {
  const AvaliacoesPage({super.key});

  @override
  State<AvaliacoesPage> createState() => _AvaliacoesPageState();
}

class _AvaliacoesPageState extends State<AvaliacoesPage>
    with SingleTickerProviderStateMixin {
  late TabController _tabController;
  Assessment? _selectedAssessment;
  late Future<List<Assessment>> _assessmentsFuture;

  @override
  void initState() {
    super.initState();
    _tabController = TabController(length: 3, vsync: this);
    _assessmentsFuture =
        GetIt.instance<IAvaliacoesService>().getAssessments();
  }

  @override
  void dispose() {
    _tabController.dispose();
    super.dispose();
  }

  void _reload() {
    setState(() {
      _assessmentsFuture =
          GetIt.instance<IAvaliacoesService>().getAssessments();
    });
  }

  List<Assessment> _filterByStatus(
      List<Assessment> all, AssessmentStatus status) {
    return all.where((a) => a.status == status).toList();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: const Color(0xFFF5F5F5),
      appBar: AppBar(
        backgroundColor: const Color(0xFFD32F2F),
        foregroundColor: Colors.white,
        title: const Text('Minhas Avaliações',
            style: TextStyle(fontWeight: FontWeight.bold)),
        bottom: TabBar(
          controller: _tabController,
          indicatorColor: Colors.white,
          indicatorWeight: 3,
          labelColor: Colors.white,
          unselectedLabelColor: Colors.white60,
          labelStyle:
              const TextStyle(fontSize: 12, fontWeight: FontWeight.w600),
          unselectedLabelStyle: const TextStyle(fontSize: 12),
          tabs: const [
            Tab(text: 'Todas'),
            Tab(text: 'Pendentes'),
            Tab(text: 'Concluídas'),
          ],
        ),
      ),
      body: FutureBuilder<List<Assessment>>(
        future: _assessmentsFuture,
        builder: (context, snapshot) {
          if (snapshot.connectionState == ConnectionState.waiting) {
            return const Center(
              child: CircularProgressIndicator(color: Color(0xFFD32F2F)),
            );
          }

          if (snapshot.hasError) {
            return Center(
              child: Column(
                mainAxisSize: MainAxisSize.min,
                children: [
                  const Icon(Icons.error_outline, size: 48, color: Colors.grey),
                  const SizedBox(height: 12),
                  const Text(
                    'Erro ao carregar avaliações',
                    style: TextStyle(color: Colors.grey, fontSize: 14),
                  ),
                  const SizedBox(height: 12),
                  ElevatedButton(
                    onPressed: _reload,
                    style: ElevatedButton.styleFrom(
                      backgroundColor: const Color(0xFFD32F2F),
                      foregroundColor: Colors.white,
                    ),
                    child: const Text('Tentar novamente'),
                  ),
                ],
              ),
            );
          }

          final all = snapshot.data ?? [];
          final pending =
              _filterByStatus(all, AssessmentStatus.pending);
          final completed =
              _filterByStatus(all, AssessmentStatus.completed);

          return Stack(
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
                          items: all,
                          emptyMessage: 'Nenhuma avaliação encontrada',
                          onTap: (a) =>
                              setState(() => _selectedAssessment = a),
                        ),
                        _AssessmentList(
                          items: pending,
                          emptyMessage: 'Nenhuma avaliação pendente',
                          onTap: (a) =>
                              setState(() => _selectedAssessment = a),
                        ),
                        _AssessmentList(
                          items: completed,
                          emptyMessage: 'Nenhuma avaliação concluída',
                          onTap: (a) =>
                              setState(() => _selectedAssessment = a),
                        ),
                      ],
                    ),
                  ),
                ],
              ),

              if (_selectedAssessment != null)
                GestureDetector(
                  onTap: () => setState(() => _selectedAssessment = null),
                  child: Container(
                    color: Colors.black54,
                    child: Center(
                      child: GestureDetector(
                        onTap: () {},
                        child: Container(
                          margin:
                              const EdgeInsets.symmetric(horizontal: 20),
                          constraints: BoxConstraints(
                            maxHeight:
                                MediaQuery.of(context).size.height * 0.85,
                          ),
                          decoration: BoxDecoration(
                            color: Colors.white,
                            borderRadius: BorderRadius.circular(16),
                          ),
                          child: Column(
                            mainAxisSize: MainAxisSize.min,
                            children: [
                              Padding(
                                padding:
                                    const EdgeInsets.fromLTRB(20, 20, 12, 0),
                                child: Row(
                                  mainAxisAlignment:
                                      MainAxisAlignment.spaceBetween,
                                  children: [
                                    const Text(
                                      'Detalhes da Avaliação',
                                      style: TextStyle(
                                          fontSize: 17,
                                          fontWeight: FontWeight.bold,
                                          color: Color(0xFF212121)),
                                    ),
                                    IconButton(
                                      icon: const Icon(Icons.close,
                                          color: Colors.grey),
                                      onPressed: () => setState(
                                          () => _selectedAssessment = null),
                                    ),
                                  ],
                                ),
                              ),
                              Flexible(
                                child: SingleChildScrollView(
                                  padding: const EdgeInsets.fromLTRB(
                                      20, 8, 20, 24),
                                  child: _AssessmentDetailContent(
                                      assessment: _selectedAssessment!),
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
          );
        },
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
            const Icon(Icons.assignment_outlined,
                size: 48, color: Colors.grey),
            const SizedBox(height: 12),
            Text(emptyMessage,
                style:
                    const TextStyle(color: Colors.grey, fontSize: 14)),
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
      'janeiro',
      'fevereiro',
      'março',
      'abril',
      'maio',
      'junho',
      'julho',
      'agosto',
      'setembro',
      'outubro',
      'novembro',
      'dezembro'
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
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            Expanded(
              child: Text(
                assessment.professionalName ?? 'Profissional #${assessment.professionalId}',
                style: const TextStyle(
                    fontSize: 15,
                    fontWeight: FontWeight.bold,
                    color: Color(0xFF212121)),
              ),
            ),
            const SizedBox(width: 8),
            _DetailStatusBadge(status: assessment.status),
          ],
        ),
        const SizedBox(height: 16),
        _DetailRow(label: 'Data:', value: _formatDate(assessment.date)),
        const SizedBox(height: 8),
        _DetailRow(
            label: 'Tipo:', value: _typeLabel(assessment.typeAvaliation)),

        if (assessment.technicalOpinion != null &&
            assessment.technicalOpinion!.isNotEmpty) ...[
          const SizedBox(height: 12),
          const Text('Parecer técnico:',
              style: TextStyle(fontSize: 13, color: Colors.grey)),
          const SizedBox(height: 4),
          Container(
            width: double.infinity,
            padding: const EdgeInsets.all(12),
            decoration: BoxDecoration(
              color: const Color(0xFFF5F5F5),
              borderRadius: BorderRadius.circular(8),
            ),
            child: Text(assessment.technicalOpinion!,
                style: const TextStyle(
                    fontSize: 13, color: Color(0xFF424242))),
          ),
        ],

        if (assessment.imc != null ||
            assessment.bodyFatPercentage != null ||
            assessment.bodyComposition != null) ...[
          const SizedBox(height: 16),
          const Divider(height: 1, color: Color(0xFFEEEEEE)),
          const SizedBox(height: 16),
          Row(
            children: const [
              Icon(Icons.bar_chart, color: Color(0xFFD32F2F), size: 20),
              SizedBox(width: 8),
              Text('Resultados',
                  style: TextStyle(
                      fontSize: 15,
                      fontWeight: FontWeight.bold,
                      color: Color(0xFF212121))),
            ],
          ),
          const SizedBox(height: 12),
          if (assessment.imc != null)
            _ResultTile(label: 'IMC', value: assessment.imc!),
          if (assessment.bodyFatPercentage != null) ...[
            const SizedBox(height: 8),
            _ResultTile(
                label: '% Gordura',
                value: assessment.bodyFatPercentage!),
          ],
          if (assessment.bodyComposition != null) ...[
            if (assessment.bodyComposition!.weight != null) ...[
              const SizedBox(height: 8),
              _ResultTile(
                  label: 'Peso',
                  value:
                      '${assessment.bodyComposition!.weight!.toStringAsFixed(1)} kg'),
            ],
            if (assessment.bodyComposition!.height != null) ...[
              const SizedBox(height: 8),
              _ResultTile(
                  label: 'Altura',
                  value:
                      '${assessment.bodyComposition!.height!.toStringAsFixed(0)} cm'),
            ],
          ],
        ],

        if (assessment.dateNextAvaliation != null) ...[
          const SizedBox(height: 12),
          _DetailRow(
              label: 'Próxima avaliação:',
              value: _formatDate(assessment.dateNextAvaliation!)),
        ],
      ],
    );
  }
}

class _DetailRow extends StatelessWidget {
  final String label;
  final String value;

  const _DetailRow({required this.label, required this.value});

  @override
  Widget build(BuildContext context) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.spaceBetween,
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(label,
            style: const TextStyle(fontSize: 13, color: Colors.grey)),
        const SizedBox(width: 12),
        Flexible(
          child: Text(
            value,
            textAlign: TextAlign.right,
            style:
                const TextStyle(fontSize: 13, color: Color(0xFF212121)),
          ),
        ),
      ],
    );
  }
}

class _ResultTile extends StatelessWidget {
  final String label;
  final String value;

  const _ResultTile({required this.label, required this.value});

  @override
  Widget build(BuildContext context) {
    return Container(
      width: double.infinity,
      padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 8),
      decoration: BoxDecoration(
        color: const Color(0xFFFFEBEE),
        borderRadius: BorderRadius.circular(8),
      ),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.spaceBetween,
        children: [
          Text(label,
              style:
                  const TextStyle(fontSize: 13, color: Colors.grey)),
          Text(value,
              style: const TextStyle(
                  fontSize: 14,
                  fontWeight: FontWeight.bold,
                  color: Color(0xFF212121))),
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
    final bgColor = status == AssessmentStatus.completed
        ? const Color(0xFFE8F5E9)
        : const Color(0xFFFFF9C4);
    final textColor = status == AssessmentStatus.completed
        ? const Color(0xFF2E7D32)
        : const Color(0xFFF57F17);
    final label =
        status == AssessmentStatus.completed ? 'Concluída' : 'Pendente';

    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 4),
      decoration:
          BoxDecoration(color: bgColor, borderRadius: BorderRadius.circular(20)),
      child: Text(label,
          style: TextStyle(
              fontSize: 11,
              color: textColor,
              fontWeight: FontWeight.w600)),
    );
  }
}
