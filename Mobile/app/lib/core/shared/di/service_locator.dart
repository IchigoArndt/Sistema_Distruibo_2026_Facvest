import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:get_it/get_it.dart';
import 'package:sistema_distribuido/core/features/login/data/repositories/auth_repository_impl.dart';
import 'package:sistema_distribuido/core/features/login/data/services/auth_service.dart';
import 'package:sistema_distribuido/core/features/login/domain/services/IUserAuthenticationSerivce.dart';
import 'package:sistema_distribuido/core/shared/http/api_client.dart';
import 'package:sistema_distribuido/core/shared/http/auth_interceptor.dart';
import 'package:sistema_distribuido/core/shared/storage/token_storage.dart';

final sl = GetIt.instance;

Future<void> setupServiceLocator() async {
  sl.registerLazySingleton<FlutterSecureStorage>(
    () => const FlutterSecureStorage(),
  );

  sl.registerLazySingleton<TokenStorage>(
    () => TokenStorage(sl<FlutterSecureStorage>()),
  );

  sl.registerLazySingleton<AuthInterceptor>(
    () => AuthInterceptor(sl<TokenStorage>()),
  );

  sl.registerLazySingleton<AuthService>(
    () => AuthService(ApiClient.createAuthDio()),
  );

  sl.registerLazySingleton<IAuthenticationService>(
    () => AuthRepositoryImpl(
      authService: sl<AuthService>(),
      tokenStorage: sl<TokenStorage>(),
    ),
  );
}
