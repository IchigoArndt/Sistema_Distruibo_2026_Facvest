import 'package:flutter/material.dart';
import 'package:get_it/get_it.dart';
import 'package:sistema_distribuido/core/features/profissionais/domain/entities/professional.dart';
import 'package:sistema_distribuido/core/features/profissionais/domain/services/IProfissionaisService.dart';

class ProfessionalDetailPage extends StatefulWidget {
  final Professional professional;

  const ProfessionalDetailPage({super.key, required this.professional});

  @override
  State<ProfessionalDetailPage> createState() => _ProfessionalDetailPageState();
}

class _ProfessionalDetailPageState extends State<ProfessionalDetailPage> {
  bool _isRequesting = false;

  void _openRequestSheet() {
    showModalBottomSheet(
      context: context,
      isScrollControlled: true,
      backgroundColor: Colors.transparent,
      builder: (ctx) => _RequestAssessmentSheet(
        professional: widget.professional,
        onConfirm: (date, type, objective) async {
          Navigator.pop(ctx);
          await _handleRequestAssessment(date, type, objective);
        },
      ),
    );
  }

  Future<void> _handleRequestAssessment(
      DateTime date, int type, int objective) async {
    setState(() => _isRequesting = true);

    try {
      await GetIt.instance<IProfissionaisService>().requestAssessment(
        widget.professional.id,
        date: date,
        typeAvaliation: type,
        studentObjective: objective,
      );

      if (!mounted) return;
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: const Row(
            children: [
              Icon(Icons.check_circle, color: Colors.white, size: 20),
              SizedBox(width: 10),
              Expanded(
                child: Column(
                  mainAxisSize: MainAxisSize.min,
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text('Solicitação enviada com sucesso!',
                        style: TextStyle(fontWeight: FontWeight.bold)),
                    Text('Você receberá a confirmação em breve.',
                        style: TextStyle(fontSize: 12)),
                  ],
                ),
              ),
            ],
          ),
          backgroundColor: const Color(0xFF388E3C),
          behavior: SnackBarBehavior.floating,
          shape:
              RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
          duration: const Duration(seconds: 3),
        ),
      );
    } catch (_) {
      if (!mounted) return;
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: const Row(
            children: [
              Icon(Icons.error_outline, color: Colors.white, size: 20),
              SizedBox(width: 10),
              Expanded(
                child: Text('Erro ao enviar solicitação. Tente novamente.'),
              ),
            ],
          ),
          backgroundColor: const Color(0xFFD32F2F),
          behavior: SnackBarBehavior.floating,
          shape:
              RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
          duration: const Duration(seconds: 3),
        ),
      );
    } finally {
      if (mounted) setState(() => _isRequesting = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    final prof = widget.professional;

    return Scaffold(
      backgroundColor: const Color(0xFFF5F5F5),
      body: Stack(
        children: [
          CustomScrollView(
            slivers: [
              SliverAppBar(
                expandedHeight: 140,
                pinned: true,
                backgroundColor: const Color(0xFFD32F2F),
                foregroundColor: Colors.white,
                leading: IconButton(
                  icon: const Icon(Icons.arrow_back),
                  onPressed: () => Navigator.pop(context),
                ),
                flexibleSpace: FlexibleSpaceBar(
                  background: Container(
                    decoration: const BoxDecoration(
                      gradient: LinearGradient(
                        colors: [Color(0xFFD32F2F), Color(0xFFB71C1C)],
                        begin: Alignment.centerLeft,
                        end: Alignment.centerRight,
                      ),
                    ),
                    padding: const EdgeInsets.fromLTRB(20, 90, 20, 16),
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      mainAxisAlignment: MainAxisAlignment.end,
                      children: [
                        Text(prof.name,
                            style: const TextStyle(
                                color: Colors.white,
                                fontSize: 20,
                                fontWeight: FontWeight.bold)),
                        if (prof.specialty != null &&
                            prof.specialty!.isNotEmpty)
                          Text(prof.specialty!,
                              style: const TextStyle(
                                  color: Color(0xFFFFCDD2), fontSize: 13)),
                      ],
                    ),
                  ),
                ),
              ),
              SliverPadding(
                padding: const EdgeInsets.fromLTRB(16, 16, 16, 100),
                sliver: SliverList(
                  delegate: SliverChildListDelegate([
                    _DetailCard(
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          const Text('Contato',
                              style: TextStyle(
                                  fontSize: 15,
                                  fontWeight: FontWeight.bold,
                                  color: Color(0xFF212121))),
                          const SizedBox(height: 12),
                          if (prof.phone != null &&
                              prof.phone!.isNotEmpty) ...[
                            _ContactItem(
                                icon: Icons.phone,
                                label: 'Telefone',
                                value: prof.phone!),
                            const SizedBox(height: 10),
                          ],
                          _ContactItem(
                              icon: Icons.email,
                              label: 'Email',
                              value: prof.email),
                          const SizedBox(height: 10),
                          _ContactItem(
                              icon: Icons.badge,
                              label: 'CREF',
                              value: prof.cref),
                        ],
                      ),
                    ),
                    const SizedBox(height: 12),

                    if (prof.methodology != null &&
                        prof.methodology!.isNotEmpty) ...[
                      _DetailCard(
                        child: Row(
                          children: [
                            const Icon(Icons.schedule,
                                color: Color(0xFFD32F2F), size: 20),
                            const SizedBox(width: 8),
                            const Text('Metodologia',
                                style: TextStyle(
                                    fontSize: 15,
                                    fontWeight: FontWeight.bold,
                                    color: Color(0xFF212121))),
                            const SizedBox(width: 12),
                            Container(
                              padding: const EdgeInsets.symmetric(
                                  horizontal: 12, vertical: 4),
                              decoration: BoxDecoration(
                                color: const Color(0xFFFFEBEE),
                                borderRadius: BorderRadius.circular(20),
                              ),
                              child: Text(prof.methodology!,
                                  style: const TextStyle(
                                      fontSize: 13,
                                      color: Color(0xFFD32F2F),
                                      fontWeight: FontWeight.w500)),
                            ),
                          ],
                        ),
                      ),
                      const SizedBox(height: 12),
                    ],

                    if (prof.experience != null &&
                        prof.experience!.isNotEmpty) ...[
                      _DetailCard(
                        child: Row(
                          children: [
                            const Icon(Icons.work_history,
                                color: Color(0xFFD32F2F), size: 20),
                            const SizedBox(width: 8),
                            const Text('Experiência',
                                style: TextStyle(
                                    fontSize: 15,
                                    fontWeight: FontWeight.bold,
                                    color: Color(0xFF212121))),
                            const SizedBox(width: 12),
                            Expanded(
                              child: Text(prof.experience!,
                                  style: const TextStyle(
                                      fontSize: 13,
                                      color: Color(0xFF616161))),
                            ),
                          ],
                        ),
                      ),
                      const SizedBox(height: 12),
                    ],

                    if (prof.bio != null && prof.bio!.isNotEmpty) ...[
                      _DetailCard(
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            const Text('Sobre',
                                style: TextStyle(
                                    fontSize: 15,
                                    fontWeight: FontWeight.bold,
                                    color: Color(0xFF212121))),
                            const SizedBox(height: 8),
                            Text(prof.bio!,
                                style: const TextStyle(
                                    fontSize: 13,
                                    color: Color(0xFF616161),
                                    height: 1.6)),
                          ],
                        ),
                      ),
                      const SizedBox(height: 12),
                    ],

                    if (prof.price != null)
                      Container(
                        padding: const EdgeInsets.all(20),
                        decoration: BoxDecoration(
                          gradient: const LinearGradient(
                            colors: [Color(0xFFFFEBEE), Color(0xFFFFCDD2)],
                            begin: Alignment.centerLeft,
                            end: Alignment.centerRight,
                          ),
                          borderRadius: BorderRadius.circular(12),
                          border:
                              Border.all(color: const Color(0xFFEF9A9A)),
                        ),
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            const Text('Valor da avaliação',
                                style: TextStyle(
                                    fontSize: 13,
                                    color: Color(0xFF616161))),
                            const SizedBox(height: 4),
                            Text(
                              'R\$ ${prof.price!.toStringAsFixed(2)}',
                              style: const TextStyle(
                                  fontSize: 30,
                                  fontWeight: FontWeight.bold,
                                  color: Color(0xFFD32F2F)),
                            ),
                          ],
                        ),
                      ),
                  ]),
                ),
              ),
            ],
          ),

          // Botão fixo no rodapé
          Positioned(
            bottom: 0,
            left: 0,
            right: 0,
            child: Container(
              color: Colors.white,
              padding: const EdgeInsets.fromLTRB(16, 12, 16, 24),
              child: SizedBox(
                width: double.infinity,
                height: 52,
                child: ElevatedButton(
                  onPressed: _isRequesting ? null : _openRequestSheet,
                  style: ElevatedButton.styleFrom(
                    backgroundColor: const Color(0xFFD32F2F),
                    foregroundColor: Colors.white,
                    shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(12)),
                    elevation: 0,
                  ),
                  child: _isRequesting
                      ? const SizedBox(
                          width: 22,
                          height: 22,
                          child: CircularProgressIndicator(
                              color: Colors.white, strokeWidth: 2))
                      : const Text('Solicitar Avaliação',
                          style: TextStyle(
                              fontSize: 16, fontWeight: FontWeight.bold)),
                ),
              ),
            ),
          ),
        ],
      ),
    );
  }
}

// ─────────────────────────────────────────────
// Bottom sheet de solicitação
// ─────────────────────────────────────────────

class _RequestAssessmentSheet extends StatefulWidget {
  final Professional professional;
  final Future<void> Function(DateTime date, int type, int objective) onConfirm;

  const _RequestAssessmentSheet({
    required this.professional,
    required this.onConfirm,
  });

  @override
  State<_RequestAssessmentSheet> createState() =>
      _RequestAssessmentSheetState();
}

class _RequestAssessmentSheetState extends State<_RequestAssessmentSheet> {
  DateTime _date = DateTime.now().add(const Duration(days: 7));
  int _type = 1;
  int _objective = 3;
  bool _loading = false;

  static const _types = {
    1: 'Básica',
    2: 'Completa',
    3: 'Reavaliação',
  };

  static const _objectives = {
    1: 'Hipertrofia',
    2: 'Emagrecimento',
    3: 'Condicionamento',
    4: 'Reabilitação',
  };

  Future<void> _pickDate() async {
    final picked = await showDatePicker(
      context: context,
      initialDate: _date,
      firstDate: DateTime.now(),
      lastDate: DateTime.now().add(const Duration(days: 180)),
      builder: (ctx, child) => Theme(
        data: Theme.of(ctx).copyWith(
          colorScheme: const ColorScheme.light(
            primary: Color(0xFFD32F2F),
            onPrimary: Colors.white,
            surface: Colors.white,
          ),
        ),
        child: child!,
      ),
    );
    if (picked != null) setState(() => _date = picked);
  }

  String _formatDate(DateTime date) =>
      '${date.day.toString().padLeft(2, '0')}/'
      '${date.month.toString().padLeft(2, '0')}/'
      '${date.year}';

  @override
  Widget build(BuildContext context) {
    return Container(
      decoration: const BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.vertical(top: Radius.circular(20)),
      ),
      padding: EdgeInsets.only(
        bottom: MediaQuery.of(context).viewInsets.bottom + 24,
        left: 24,
        right: 24,
        top: 20,
      ),
      child: SingleChildScrollView(
        child: Column(
          mainAxisSize: MainAxisSize.min,
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            // Handle
            Center(
              child: Container(
                width: 40,
                height: 4,
                decoration: BoxDecoration(
                  color: const Color(0xFFDDDDDD),
                  borderRadius: BorderRadius.circular(2),
                ),
              ),
            ),
            const SizedBox(height: 16),

            // Título
            Row(
              children: [
                const Expanded(
                  child: Text(
                    'Solicitar Avaliação',
                    style: TextStyle(
                        fontSize: 18,
                        fontWeight: FontWeight.bold,
                        color: Color(0xFF212121)),
                  ),
                ),
                IconButton(
                  icon: const Icon(Icons.close, color: Colors.grey),
                  onPressed: () => Navigator.pop(context),
                ),
              ],
            ),
            Text(
              'com ${widget.professional.name}',
              style: const TextStyle(fontSize: 13, color: Colors.grey),
            ),
            const SizedBox(height: 24),

            // Data
            const Text('Data desejada',
                style: TextStyle(
                    fontSize: 13,
                    color: Colors.grey,
                    fontWeight: FontWeight.w500)),
            const SizedBox(height: 8),
            InkWell(
              onTap: _pickDate,
              borderRadius: BorderRadius.circular(10),
              child: Container(
                padding: const EdgeInsets.symmetric(
                    horizontal: 16, vertical: 14),
                decoration: BoxDecoration(
                  border: Border.all(color: const Color(0xFFDDDDDD)),
                  borderRadius: BorderRadius.circular(10),
                  color: const Color(0xFFFAFAFA),
                ),
                child: Row(
                  children: [
                    const Icon(Icons.calendar_today,
                        color: Color(0xFFD32F2F), size: 18),
                    const SizedBox(width: 12),
                    Text(
                      _formatDate(_date),
                      style: const TextStyle(
                          fontSize: 15, color: Color(0xFF212121)),
                    ),
                    const Spacer(),
                    const Icon(Icons.chevron_right,
                        color: Colors.grey, size: 18),
                  ],
                ),
              ),
            ),
            const SizedBox(height: 24),

            // Tipo da avaliação
            const Text('Tipo da avaliação',
                style: TextStyle(
                    fontSize: 13,
                    color: Colors.grey,
                    fontWeight: FontWeight.w500)),
            const SizedBox(height: 10),
            Wrap(
              spacing: 8,
              children: _types.entries.map((e) {
                final selected = _type == e.key;
                return GestureDetector(
                  onTap: () => setState(() => _type = e.key),
                  child: AnimatedContainer(
                    duration: const Duration(milliseconds: 150),
                    padding: const EdgeInsets.symmetric(
                        horizontal: 16, vertical: 10),
                    decoration: BoxDecoration(
                      color: selected
                          ? const Color(0xFFFFEBEE)
                          : const Color(0xFFF5F5F5),
                      borderRadius: BorderRadius.circular(20),
                      border: Border.all(
                        color: selected
                            ? const Color(0xFFD32F2F)
                            : const Color(0xFFDDDDDD),
                      ),
                    ),
                    child: Text(
                      e.value,
                      style: TextStyle(
                        fontSize: 13,
                        fontWeight: selected
                            ? FontWeight.bold
                            : FontWeight.normal,
                        color: selected
                            ? const Color(0xFFD32F2F)
                            : Colors.grey,
                      ),
                    ),
                  ),
                );
              }).toList(),
            ),
            const SizedBox(height: 24),

            // Objetivo
            const Text('Seu objetivo',
                style: TextStyle(
                    fontSize: 13,
                    color: Colors.grey,
                    fontWeight: FontWeight.w500)),
            const SizedBox(height: 10),
            Wrap(
              spacing: 8,
              runSpacing: 8,
              children: _objectives.entries.map((e) {
                final selected = _objective == e.key;
                return GestureDetector(
                  onTap: () => setState(() => _objective = e.key),
                  child: AnimatedContainer(
                    duration: const Duration(milliseconds: 150),
                    padding: const EdgeInsets.symmetric(
                        horizontal: 16, vertical: 10),
                    decoration: BoxDecoration(
                      color: selected
                          ? const Color(0xFFFFEBEE)
                          : const Color(0xFFF5F5F5),
                      borderRadius: BorderRadius.circular(20),
                      border: Border.all(
                        color: selected
                            ? const Color(0xFFD32F2F)
                            : const Color(0xFFDDDDDD),
                      ),
                    ),
                    child: Text(
                      e.value,
                      style: TextStyle(
                        fontSize: 13,
                        fontWeight: selected
                            ? FontWeight.bold
                            : FontWeight.normal,
                        color: selected
                            ? const Color(0xFFD32F2F)
                            : Colors.grey,
                      ),
                    ),
                  ),
                );
              }).toList(),
            ),
            const SizedBox(height: 32),

            // Resumo
            Container(
              width: double.infinity,
              padding: const EdgeInsets.all(14),
              decoration: BoxDecoration(
                color: const Color(0xFFFFEBEE),
                borderRadius: BorderRadius.circular(10),
              ),
              child: Column(
                children: [
                  _SummaryRow(
                      label: 'Profissional',
                      value: widget.professional.name),
                  const SizedBox(height: 6),
                  _SummaryRow(
                      label: 'Data', value: _formatDate(_date)),
                  const SizedBox(height: 6),
                  _SummaryRow(
                      label: 'Tipo',
                      value: _types[_type]!),
                  const SizedBox(height: 6),
                  _SummaryRow(
                      label: 'Objetivo',
                      value: _objectives[_objective]!),
                  if (widget.professional.price != null) ...[
                    const SizedBox(height: 6),
                    _SummaryRow(
                      label: 'Valor',
                      value:
                          'R\$ ${widget.professional.price!.toStringAsFixed(2)}',
                      valueColor: const Color(0xFFD32F2F),
                    ),
                  ],
                ],
              ),
            ),
            const SizedBox(height: 20),

            // Botão confirmar
            SizedBox(
              width: double.infinity,
              height: 52,
              child: ElevatedButton(
                onPressed: _loading
                    ? null
                    : () async {
                        setState(() => _loading = true);
                        await widget.onConfirm(_date, _type, _objective);
                      },
                style: ElevatedButton.styleFrom(
                  backgroundColor: const Color(0xFFD32F2F),
                  foregroundColor: Colors.white,
                  shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(12)),
                  elevation: 0,
                ),
                child: _loading
                    ? const SizedBox(
                        width: 22,
                        height: 22,
                        child: CircularProgressIndicator(
                            color: Colors.white, strokeWidth: 2))
                    : const Text(
                        'Confirmar Solicitação',
                        style: TextStyle(
                            fontSize: 16, fontWeight: FontWeight.bold),
                      ),
              ),
            ),
          ],
        ),
      ),
    );
  }
}

class _SummaryRow extends StatelessWidget {
  final String label;
  final String value;
  final Color? valueColor;

  const _SummaryRow(
      {required this.label, required this.value, this.valueColor});

  @override
  Widget build(BuildContext context) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.spaceBetween,
      children: [
        Text(label,
            style:
                const TextStyle(fontSize: 13, color: Color(0xFF616161))),
        Text(value,
            style: TextStyle(
                fontSize: 13,
                color: valueColor ?? const Color(0xFF212121),
                fontWeight: FontWeight.w600)),
      ],
    );
  }
}

// ─────────────────────────────────────────────
// Widgets de detalhe do profissional
// ─────────────────────────────────────────────

class _DetailCard extends StatelessWidget {
  final Widget child;

  const _DetailCard({required this.child});

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
          BoxShadow(
              color: Colors.black.withValues(alpha: 0.04), blurRadius: 6),
        ],
      ),
      child: child,
    );
  }
}

class _ContactItem extends StatelessWidget {
  final IconData icon;
  final String label;
  final String value;

  const _ContactItem(
      {required this.icon, required this.label, required this.value});

  @override
  Widget build(BuildContext context) {
    return Row(
      children: [
        Container(
          padding: const EdgeInsets.all(8),
          decoration: BoxDecoration(
            color: const Color(0xFFFFEBEE),
            borderRadius: BorderRadius.circular(8),
          ),
          child: Icon(icon, color: const Color(0xFFD32F2F), size: 18),
        ),
        const SizedBox(width: 12),
        Expanded(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(label,
                  style: const TextStyle(
                      fontSize: 11, color: Colors.grey)),
              Text(value,
                  style: const TextStyle(
                      fontSize: 14, color: Color(0xFF212121))),
            ],
          ),
        ),
      ],
    );
  }
}
