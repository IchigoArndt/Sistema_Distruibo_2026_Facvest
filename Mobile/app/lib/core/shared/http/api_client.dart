import 'dart:io';
import 'package:dio/dio.dart';
import 'package:dio/io.dart';
import 'package:flutter/foundation.dart';
import 'auth_interceptor.dart';

class ApiClient {
  // 10.0.2.2 é o alias do emulador Android para a máquina host (localhost)
  static const String _authBaseUrl = 'https://10.0.2.2:7289';
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
        baseUrl: 'http://10.0.2.2:5252',
        connectTimeout: _timeout,
        receiveTimeout: _timeout,
        sendTimeout: _timeout,
        headers: {'Content-Type': 'application/json'},
      ),
    );

    dio.interceptors.add(authInterceptor);
    dio.interceptors.add(LogInterceptor(
      requestBody: true,
      responseBody: true,
      logPrint: (obj) => debugPrint('[ApiClient] $obj'),
    ));

    return dio;
  }
}
