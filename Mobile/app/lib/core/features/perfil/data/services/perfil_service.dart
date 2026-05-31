import 'package:dio/dio.dart';
import 'package:sistema_distribuido/core/features/perfil/data/models/health_profile_model.dart';

class PerfilService {
  final Dio _dio;

  PerfilService(this._dio);

  Future<HealthProfileModel> getProfile() async {
    final response = await _dio.get('/Student/GetMe');
    return HealthProfileModel.fromJson(response.data as Map<String, dynamic>);
  }
}
