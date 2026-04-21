import 'package:sistema_distribuido/core/features/avaliacoes/domain/entities/assessment.dart';

abstract class IAvaliacoesService {
  Future<List<Assessment>> getAssessments();
  Future<Assessment> getAssessmentById(int id);
  Future<List<Assessment>> getAssessmentsByStatus(AssessmentStatus status);
}
