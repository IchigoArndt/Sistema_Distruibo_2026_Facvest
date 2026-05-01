class AuthException implements Exception {
  final String message;
  const AuthException(this.message);

  @override
  String toString() => message;
}

class InvalidCredentialsException extends AuthException {
  const InvalidCredentialsException()
      : super('Credenciais inválidas. Verifique seu e-mail e senha.');
}

class ServerException extends AuthException {
  final int? statusCode;
  const ServerException({this.statusCode})
      : super('Erro no servidor. Tente novamente mais tarde.');
}

class TimeoutException extends AuthException {
  const TimeoutException() : super('Tempo de conexão esgotado. Verifique sua internet.');
}

class NetworkException extends AuthException {
  const NetworkException() : super('Sem conexão com o servidor. Verifique sua rede.');
}
