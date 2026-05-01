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
  Future<bool> login(UserAuthentication userAuthentication) async {
    final response = await _authService.login(
      userAuthentication.username,
      userAuthentication.password,
    );

    await _tokenStorage.saveToken(response.token, response.expiration);

    return true;
  }
}
