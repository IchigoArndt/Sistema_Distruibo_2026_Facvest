import 'package:sistema_distribuido/core/features/profissionais/domain/entities/professional.dart';

abstract class IProfissionaisService {
  Future<List<Professional>> getProfessionals();
  Future<Professional> getProfessionalById(int id);
  Future<void> requestAssessment(
    int professionalId, {
    required DateTime date,
    required int typeAvaliation,
    required int studentObjective,
  });
}
