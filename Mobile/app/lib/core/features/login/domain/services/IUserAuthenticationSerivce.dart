import 'package:sistema_distribuido/core/features/login/domain/entities/UserAuthentication.dart';

abstract class IAuthenticationService {
  /// Autentica o usuário e retorna o nome exibível (claim unique_name do JWT).
  Future<String> login(UserAuthentication userAuthentication);
}