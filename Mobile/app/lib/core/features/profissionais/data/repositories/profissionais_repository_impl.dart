import 'package:sistema_distribuido/core/features/profissionais/data/services/profissionais_service.dart';
import 'package:sistema_distribuido/core/features/profissionais/domain/entities/professional.dart';
import 'package:sistema_distribuido/core/features/profissionais/domain/services/IProfissionaisService.dart';

class ProfissionaisRepositoryImpl implements IProfissionaisService {
  final ProfissionaisService _service;

  ProfissionaisRepositoryImpl(this._service);

  @override
  Future<List<Professional>> getProfessionals() {
    return _service.getProfessionals();
  }

  @override
  Future<Professional> getProfessionalById(int id) {
    return _service.getProfessionalById(id);
  }

  @override
  Future<void> requestAssessment(
    int professionalId, {
    required DateTime date,
    required int typeAvaliation,
    required int studentObjective,
  }) {
    return _service.requestAssessment(
      professionalId,
      date: date,
      typeAvaliation: typeAvaliation,
      studentObjective: studentObjective,
    );
  }
}
