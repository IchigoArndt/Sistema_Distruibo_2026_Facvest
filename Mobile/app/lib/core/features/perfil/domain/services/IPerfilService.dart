import 'package:sistema_distribuido/core/features/perfil/domain/entities/health_profile.dart';

abstract class IPerfilService {
  Future<HealthProfile> getProfile();
  Future<bool> updateProfile(HealthProfile profile);
}
