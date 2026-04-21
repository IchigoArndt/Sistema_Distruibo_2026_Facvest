import 'package:flutter/material.dart';
import 'package:sistema_distribuido/core/features/profissionais/domain/entities/professional.dart';

class ProfessionalDetailPage extends StatefulWidget {
  final Professional professional;

  const ProfessionalDetailPage({super.key, required this.professional});

  @override
  State<ProfessionalDetailPage> createState() => _ProfessionalDetailPageState();
}

class _ProfessionalDetailPageState extends State<ProfessionalDetailPage> {
  bool _showConfirmDialog = false;

  void _handleRequestAssessment() {
    setState(() => _showConfirmDialog = false);
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
                  Text('Solicitação enviada com sucesso!', style: TextStyle(fontWeight: FontWeight.bold)),
                  Text('Você receberá a confirmação em breve.', style: TextStyle(fontSize: 12)),
                ],
              ),
            ),
          ],
        ),
        backgroundColor: const Color(0xFF388E3C),
        behavior: SnackBarBehavior.floating,
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
        duration: const Duration(seconds: 3),
      ),
    );
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
                        Text(prof.name, style: const TextStyle(color: Colors.white, fontSize: 20, fontWeight: FontWeight.bold)),
                        const SizedBox(height: 2),
                        Text(prof.specialty, style: const TextStyle(color: Color(0xFFFFCDD2), fontSize: 13)),
                      ],
                    ),
                  ),
                ),
              ),
              SliverPadding(
                padding: const EdgeInsets.fromLTRB(16, 16, 16, 100),
                sliver: SliverList(
                  delegate: SliverChildListDelegate([
                    // Rating
                    _DetailCard(
                      child: Row(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: [
                          Row(
                            children: [
                              const Icon(Icons.star, color: Color(0xFFFFC107), size: 28),
                              const SizedBox(width: 8),
                              Text(
                                prof.rating.toStringAsFixed(1),
                                style: const TextStyle(fontSize: 24, fontWeight: FontWeight.bold, color: Color(0xFF212121)),
                              ),
                            ],
                          ),
                          Column(
                            crossAxisAlignment: CrossAxisAlignment.end,
                            children: [
                              Text('${prof.reviews} avaliações', style: const TextStyle(fontSize: 13, color: Colors.grey)),
                              const SizedBox(height: 2),
                              Text(prof.experience, style: const TextStyle(fontSize: 13, color: Colors.grey)),
                            ],
                          ),
                        ],
                      ),
                    ),
                    const SizedBox(height: 12),

                    // Contato
                    _DetailCard(
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          const Text('Contato', style: TextStyle(fontSize: 15, fontWeight: FontWeight.bold, color: Color(0xFF212121))),
                          const SizedBox(height: 12),
                          _ContactItem(icon: Icons.phone, label: 'Telefone', value: prof.phone),
                          const SizedBox(height: 10),
                          _ContactItem(icon: Icons.email, label: 'Email', value: prof.email),
                        ],
                      ),
                    ),
                    const SizedBox(height: 12),

                    // Metodologia
                    _DetailCard(
                      child: Row(
                        children: [
                          const Icon(Icons.schedule, color: Color(0xFFD32F2F), size: 20),
                          const SizedBox(width: 8),
                          const Text('Metodologia', style: TextStyle(fontSize: 15, fontWeight: FontWeight.bold, color: Color(0xFF212121))),
                          const SizedBox(width: 12),
                          Container(
                            padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 4),
                            decoration: BoxDecoration(
                              color: const Color(0xFFFFEBEE),
                              borderRadius: BorderRadius.circular(20),
                            ),
                            child: Text(
                              prof.methodology,
                              style: const TextStyle(fontSize: 13, color: Color(0xFFD32F2F), fontWeight: FontWeight.w500),
                            ),
                          ),
                        ],
                      ),
                    ),
                    const SizedBox(height: 12),

                    // Sobre
                    _DetailCard(
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          const Text('Sobre', style: TextStyle(fontSize: 15, fontWeight: FontWeight.bold, color: Color(0xFF212121))),
                          const SizedBox(height: 8),
                          Text(prof.about, style: const TextStyle(fontSize: 13, color: Color(0xFF616161), height: 1.6)),
                        ],
                      ),
                    ),
                    const SizedBox(height: 12),

                    // Certificações
                    _DetailCard(
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          Row(
                            children: const [
                              Icon(Icons.workspace_premium, color: Color(0xFFD32F2F), size: 20),
                              SizedBox(width: 8),
                              Text('Certificações', style: TextStyle(fontSize: 15, fontWeight: FontWeight.bold, color: Color(0xFF212121))),
                            ],
                          ),
                          const SizedBox(height: 12),
                          ...prof.certifications.map(
                            (cert) => Padding(
                              padding: const EdgeInsets.only(bottom: 8),
                              child: Row(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: [
                                  const Icon(Icons.check_circle, color: Color(0xFF388E3C), size: 18),
                                  const SizedBox(width: 8),
                                  Expanded(
                                    child: Text(cert, style: const TextStyle(fontSize: 13, color: Color(0xFF424242))),
                                  ),
                                ],
                              ),
                            ),
                          ),
                        ],
                      ),
                    ),
                    const SizedBox(height: 12),

                    // Preço
                    Container(
                      padding: const EdgeInsets.all(20),
                      decoration: BoxDecoration(
                        gradient: const LinearGradient(
                          colors: [Color(0xFFFFEBEE), Color(0xFFFFCDD2)],
                          begin: Alignment.centerLeft,
                          end: Alignment.centerRight,
                        ),
                        borderRadius: BorderRadius.circular(12),
                        border: Border.all(color: const Color(0xFFEF9A9A)),
                      ),
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          const Text('Valor da avaliação', style: TextStyle(fontSize: 13, color: Color(0xFF616161))),
                          const SizedBox(height: 4),
                          Text(
                            'R\$ ${prof.price.toStringAsFixed(2)}',
                            style: const TextStyle(fontSize: 30, fontWeight: FontWeight.bold, color: Color(0xFFD32F2F)),
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
                  onPressed: () => setState(() => _showConfirmDialog = true),
                  style: ElevatedButton.styleFrom(
                    backgroundColor: const Color(0xFFD32F2F),
                    foregroundColor: Colors.white,
                    shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
                    elevation: 0,
                  ),
                  child: const Text('Solicitar Avaliação', style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold)),
                ),
              ),
            ),
          ),

          // Dialog de confirmação
          if (_showConfirmDialog)
            GestureDetector(
              onTap: () => setState(() => _showConfirmDialog = false),
              child: Container(
                color: Colors.black54,
                child: Center(
                  child: GestureDetector(
                    onTap: () {},
                    child: Container(
                      margin: const EdgeInsets.symmetric(horizontal: 24),
                      padding: const EdgeInsets.all(24),
                      decoration: BoxDecoration(
                        color: Colors.white,
                        borderRadius: BorderRadius.circular(16),
                      ),
                      child: Column(
                        mainAxisSize: MainAxisSize.min,
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          const Text('Confirmar Solicitação', style: TextStyle(fontSize: 17, fontWeight: FontWeight.bold, color: Color(0xFF212121))),
                          const SizedBox(height: 16),
                          Text.rich(
                            TextSpan(
                              text: 'Deseja solicitar uma avaliação com ',
                              style: const TextStyle(fontSize: 14, color: Color(0xFF616161)),
                              children: [
                                TextSpan(text: prof.name, style: const TextStyle(fontWeight: FontWeight.bold, color: Color(0xFF212121))),
                                const TextSpan(text: '?'),
                              ],
                            ),
                          ),
                          const SizedBox(height: 16),
                          Container(
                            padding: const EdgeInsets.all(14),
                            decoration: BoxDecoration(
                              color: const Color(0xFFFFEBEE),
                              borderRadius: BorderRadius.circular(10),
                            ),
                            child: Column(
                              children: [
                                _DialogRow(label: 'Profissional:', value: prof.name),
                                const SizedBox(height: 8),
                                _DialogRow(label: 'Metodologia:', value: prof.methodology),
                                const SizedBox(height: 8),
                                _DialogRow(
                                  label: 'Valor:',
                                  value: 'R\$ ${prof.price.toStringAsFixed(2)}',
                                  valueStyle: const TextStyle(fontSize: 14, color: Color(0xFFD32F2F), fontWeight: FontWeight.bold),
                                ),
                              ],
                            ),
                          ),
                          const SizedBox(height: 20),
                          Row(
                            children: [
                              Expanded(
                                child: OutlinedButton(
                                  onPressed: () => setState(() => _showConfirmDialog = false),
                                  style: OutlinedButton.styleFrom(
                                    side: const BorderSide(color: Color(0xFFDDDDDD)),
                                    shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
                                    padding: const EdgeInsets.symmetric(vertical: 14),
                                  ),
                                  child: const Text('Cancelar', style: TextStyle(color: Color(0xFF616161))),
                                ),
                              ),
                              const SizedBox(width: 12),
                              Expanded(
                                child: ElevatedButton(
                                  onPressed: _handleRequestAssessment,
                                  style: ElevatedButton.styleFrom(
                                    backgroundColor: const Color(0xFFD32F2F),
                                    foregroundColor: Colors.white,
                                    shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
                                    padding: const EdgeInsets.symmetric(vertical: 14),
                                    elevation: 0,
                                  ),
                                  child: const Text('Confirmar', style: TextStyle(fontWeight: FontWeight.bold)),
                                ),
                              ),
                            ],
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
          BoxShadow(color: Colors.black.withValues(alpha: 0.04), blurRadius: 6),
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

  const _ContactItem({required this.icon, required this.label, required this.value});

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
        Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(label, style: const TextStyle(fontSize: 11, color: Colors.grey)),
            Text(value, style: const TextStyle(fontSize: 14, color: Color(0xFF212121))),
          ],
        ),
      ],
    );
  }
}

class _DialogRow extends StatelessWidget {
  final String label;
  final String value;
  final TextStyle? valueStyle;

  const _DialogRow({required this.label, required this.value, this.valueStyle});

  @override
  Widget build(BuildContext context) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.spaceBetween,
      children: [
        Text(label, style: const TextStyle(fontSize: 13, color: Color(0xFF616161))),
        Text(value, style: valueStyle ?? const TextStyle(fontSize: 13, color: Color(0xFF212121), fontWeight: FontWeight.w500)),
      ],
    );
  }
}
