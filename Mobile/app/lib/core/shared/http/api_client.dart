import 'dart:io';
import 'package:dio/dio.dart';
import 'package:dio/io.dart';
import 'package:flutter/foundation.dart';
import 'auth_interceptor.dart';

class ApiClient {
  /// No emulador Android, 10.0.2.2 aponta para o localhost da máquina host.
  static String get _devHost => Platform.isAndroid ? '10.0.2.2' : 'localhost';

  static String get _authBaseUrl => 'https://$_devHost:7289';
  static const Duration _timeout = Duration(seconds: 15);

  static Dio createAuthDio() {
    final dio = Dio(
      BaseOptions(
        baseUrl: _authBaseUrl,
        connectTimeout: _timeout,
        receiveTimeout: _timeout,
        sendTimeout: _timeout,
        headers: {'Content-Type': 'application/json'},
      ),
    );

    // Ignora certificado auto-assinado do ambiente de desenvolvimento
    (dio.httpClientAdapter as IOHttpClientAdapter).createHttpClient = () {
      final client = HttpClient();
      client.badCertificateCallback = (cert, host, port) => true;
      return client;
    };

    dio.interceptors.add(LogInterceptor(
      requestBody: true,
      responseBody: true,
      logPrint: (obj) => debugPrint('[ApiClient] $obj'),
    ));

    return dio;
  }

  static Dio createMainDio(AuthInterceptor authInterceptor) {
    final dio = Dio(
      BaseOptions(
        baseUrl: 'https://$_devHost:7171',
        connectTimeout: _timeout,
        receiveTimeout: _timeout,
        sendTimeout: _timeout,
        headers: {'Content-Type': 'application/json'},
      ),
    );

    // Ignora certificado auto-assinado do ambiente de desenvolvimento
    (dio.httpClientAdapter as IOHttpClientAdapter).createHttpClient = () {
      final client = HttpClient();
      client.badCertificateCallback = (cert, host, port) => true;
      return client;
    };

    dio.interceptors.add(authInterceptor);
    dio.interceptors.add(LogInterceptor(
      requestBody: true,
      responseBody: true,
      logPrint: (obj) => debugPrint('[ApiClient] $obj'),
    ));

    return dio;
  }
}
