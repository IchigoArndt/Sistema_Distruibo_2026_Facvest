import 'dart:convert';
import 'package:sistema_distribuido/core/features/login/domain/entities/UserAuthentication.dart';
import 'package:sistema_distribuido/core/features/login/domain/services/IUserAuthenticationSerivce.dart';
import 'package:sistema_distribuido/core/features/login/data/services/auth_service.dart';
import 'package:sistema_distribuido/core/shared/storage/token_storage.dart';

class AuthRepositoryImpl implements IAuthenticationService {
  final AuthService _authService;
  final TokenStorage _tokenStorage;

  AuthRepositoryImpl({
    required AuthService authService,
    required TokenStorage tokenStorage,
  })  : _authService = authService,
        _tokenStorage = tokenStorage;

  @override
  Future<String> login(UserAuthentication userAuthentication) async {
    final response = await _authService.login(
      userAuthentication.username,
      userAuthentication.password,
    );

    await _tokenStorage.saveToken(response.token, response.expiration);

    return _extractNameFromJwt(response.token);
  }

  /// Decodifica o payload do JWT (base64url) e extrai a claim unique_name.
  String _extractNameFromJwt(String token) {
    try {
      final parts = token.split('.');
      if (parts.length != 3) return 'Usuário';

      final payload = utf8.decode(
        base64Url.decode(base64Url.normalize(parts[1])),
      );
      final Map<String, dynamic> claims = jsonDecode(payload);
      return claims['unique_name'] as String? ?? 'Usuário';
    } catch (_) {
      return 'Usuário';
    }
  }
}
