import 'package:dio/dio.dart';
import 'package:sistema_distribuido/core/errors/auth_exceptions.dart';
import 'package:sistema_distribuido/core/features/login/data/models/login_response_model.dart';
import 'dart:io';

class AuthService {
  final Dio _dio;

  AuthService(this._dio);

  Future<LoginResponseModel> login(String email, String password) async {
    try {
      final response = await _dio.post(
        '/Auth/Login',
        data: {'email': email, 'password': password},
      );

      return LoginResponseModel.fromJson(response.data as Map<String, dynamic>);
    } on DioException catch (e) {
      throw _mapDioError(e);
    } on SocketException {
      throw const NetworkException();
    }
  }

  AuthException _mapDioError(DioException e) {
    switch (e.type) {
      case DioExceptionType.connectionTimeout:
      case DioExceptionType.receiveTimeout:
      case DioExceptionType.sendTimeout:
        return const TimeoutException();
      case DioExceptionType.connectionError:
        return const NetworkException();
      case DioExceptionType.badResponse:
        final status = e.response?.statusCode;
        if (status == 401 || status == 403) {
          return const InvalidCredentialsException();
        }
        return ServerException(statusCode: status);
      default:
        return const NetworkException();
    }
  }
}
