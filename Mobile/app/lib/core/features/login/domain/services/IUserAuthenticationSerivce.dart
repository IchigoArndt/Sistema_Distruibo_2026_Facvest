import 'package:sistema_distribuido/core/features/login/domain/entities/UserAuthentication.dart';

abstract class IAuthenticationService {
  Future<bool> login(UserAuthentication userAuthentication);
}