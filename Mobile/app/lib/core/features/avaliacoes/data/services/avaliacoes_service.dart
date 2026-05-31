import 'package:dio/dio.dart';
import 'package:sistema_distribuido/core/features/avaliacoes/data/models/assessment_model.dart';

class AvaliacoesService {
  final Dio _dio;

  AvaliacoesService(this._dio);

  Future<List<AssessmentModel>> getAssessments() async {
    final response = await _dio.get('/Avaliation/GetByStudent');
    final List<dynamic> data = response.data as List<dynamic>;
    return data
        .map((json) => AssessmentModel.fromJson(json as Map<String, dynamic>))
        .toList();
  }

  Future<AssessmentModel> getAssessmentById(int id) async {
    final response = await _dio.get('/Avaliation/GetById/$id');
    return AssessmentModel.fromJson(response.data as Map<String, dynamic>);
  }
}
