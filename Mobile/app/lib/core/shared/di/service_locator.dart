import 'package:dio/dio.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:get_it/get_it.dart';
import 'package:sistema_distribuido/core/features/avaliacoes/data/repositories/avaliacoes_repository_impl.dart';
import 'package:sistema_distribuido/core/features/avaliacoes/data/services/avaliacoes_service.dart';
import 'package:sistema_distribuido/core/features/avaliacoes/domain/services/IAvaliacoesService.dart';
import 'package:sistema_distribuido/core/features/login/data/repositories/auth_repository_impl.dart';
import 'package:sistema_distribuido/core/features/login/data/services/auth_service.dart';
import 'package:sistema_distribuido/core/features/login/domain/services/IUserAuthenticationSerivce.dart';
import 'package:sistema_distribuido/core/features/perfil/data/repositories/perfil_repository_impl.dart';
import 'package:sistema_distribuido/core/features/perfil/data/services/perfil_service.dart';
import 'package:sistema_distribuido/core/features/perfil/domain/services/IPerfilService.dart';
import 'package:sistema_distribuido/core/features/profissionais/data/repositories/profissionais_repository_impl.dart';
import 'package:sistema_distribuido/core/features/profissionais/data/services/profissionais_service.dart';
import 'package:sistema_distribuido/core/features/profissionais/domain/services/IProfissionaisService.dart';
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

  sl.registerLazySingleton<Dio>(
    () => ApiClient.createMainDio(sl<AuthInterceptor>()),
  );

  sl.registerLazySingleton<ProfissionaisService>(
    () => ProfissionaisService(sl<Dio>()),
  );

  sl.registerLazySingleton<IProfissionaisService>(
    () => ProfissionaisRepositoryImpl(sl<ProfissionaisService>()),
  );

  sl.registerLazySingleton<AvaliacoesService>(
    () => AvaliacoesService(sl<Dio>()),
  );

  sl.registerLazySingleton<IAvaliacoesService>(
    () => AvaliacoesRepositoryImpl(sl<AvaliacoesService>()),
  );

  sl.registerLazySingleton<PerfilService>(
    () => PerfilService(sl<Dio>()),
  );

  sl.registerLazySingleton<IPerfilService>(
    () => PerfilRepositoryImpl(sl<PerfilService>()),
  );
}
