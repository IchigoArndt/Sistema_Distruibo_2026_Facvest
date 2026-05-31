import 'package:flutter/material.dart';
import 'package:get_it/get_it.dart';
import 'package:sistema_distribuido/core/features/profissionais/domain/entities/professional.dart';
import 'package:sistema_distribuido/core/features/profissionais/domain/services/IProfissionaisService.dart';
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
  late Future<List<Professional>> _professionalsFuture;

  @override
  void initState() {
    super.initState();
    _professionalsFuture = GetIt.instance<IProfissionaisService>().getProfessionals();
  }

  void _reload() {
    setState(() {
      _professionalsFuture = GetIt.instance<IProfissionaisService>().getProfessionals();
    });
  }

  List<Professional> _filter(List<Professional> list) {
    if (_searchTerm.isEmpty) return list;
    final term = _searchTerm.toLowerCase();
    return list.where((p) =>
      (p.name.toLowerCase().contains(term)) ||
      (p.specialty?.toLowerCase().contains(term) ?? false) ||
      (p.methodology?.toLowerCase().contains(term) ?? false),
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
      body: FutureBuilder<List<Professional>>(
        future: _professionalsFuture,
        builder: (context, snapshot) {
          if (snapshot.connectionState == ConnectionState.waiting) {
            return const Center(child: CircularProgressIndicator(color: Color(0xFFD32F2F)));
          }

          if (snapshot.hasError) {
            return Center(
              child: Column(
                mainAxisSize: MainAxisSize.min,
                children: [
                  const Icon(Icons.error_outline, size: 48, color: Colors.grey),
                  const SizedBox(height: 12),
                  const Text(
                    'Erro ao carregar profissionais',
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

          final professionals = _filter(snapshot.data ?? []);

          return Column(
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
                child: professionals.isEmpty
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
                        itemCount: professionals.length,
                        separatorBuilder: (context, index) => const SizedBox(height: 12),
                        itemBuilder: (context, index) {
                          final prof = professionals[index];
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
          );
        },
      ),
    );
  }
}
