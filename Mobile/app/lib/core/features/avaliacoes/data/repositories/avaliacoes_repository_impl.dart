import 'package:sistema_distribuido/core/features/avaliacoes/data/services/avaliacoes_service.dart';
import 'package:sistema_distribuido/core/features/avaliacoes/domain/entities/assessment.dart';
import 'package:sistema_distribuido/core/features/avaliacoes/domain/services/IAvaliacoesService.dart';

class AvaliacoesRepositoryImpl implements IAvaliacoesService {
  final AvaliacoesService _service;

  AvaliacoesRepositoryImpl(this._service);

  @override
  Future<List<Assessment>> getAssessments() {
    return _service.getAssessments();
  }

  @override
  Future<Assessment> getAssessmentById(int id) {
    return _service.getAssessmentById(id);
  }
}
