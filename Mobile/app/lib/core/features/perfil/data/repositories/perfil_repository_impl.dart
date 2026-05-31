import 'package:sistema_distribuido/core/features/perfil/data/services/perfil_service.dart';
import 'package:sistema_distribuido/core/features/perfil/domain/entities/health_profile.dart';
import 'package:sistema_distribuido/core/features/perfil/domain/services/IPerfilService.dart';

class PerfilRepositoryImpl implements IPerfilService {
  final PerfilService _service;

  PerfilRepositoryImpl(this._service);

  @override
  Future<HealthProfile> getProfile() {
    return _service.getProfile();
  }
}
