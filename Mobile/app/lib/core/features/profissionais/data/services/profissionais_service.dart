import 'package:dio/dio.dart';
import 'package:sistema_distribuido/core/features/profissionais/data/models/professional_model.dart';

class ProfissionaisService {
  final Dio _dio;

  ProfissionaisService(this._dio);

  Future<List<ProfessionalModel>> getProfessionals() async {
    final response = await _dio.get('/Professional/GetAll');
    final List<dynamic> data = response.data as List<dynamic>;
    return data
        .map((json) => ProfessionalModel.fromJson(json as Map<String, dynamic>))
        .toList();
  }

  Future<ProfessionalModel> getProfessionalById(int id) async {
    final response = await _dio.get('/Professional/GetById/$id');
    return ProfessionalModel.fromJson(response.data as Map<String, dynamic>);
  }

  Future<void> requestAssessment(
    int professionalId, {
    required DateTime date,
    required int typeAvaliation,
    required int studentObjective,
  }) async {
    await _dio.post('/Avaliation/RequestAssessment/$professionalId', data: {
      'date': date.toIso8601String(),
      'typeAvaliation': typeAvaliation,
      'studentObjective': studentObjective,
    });
  }
}
