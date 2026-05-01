import 'package:flutter_secure_storage/flutter_secure_storage.dart';

class TokenStorage {
  static const String _tokenKey = 'auth_token';
  static const String _expirationKey = 'auth_expiration';

  final FlutterSecureStorage _storage;

  TokenStorage(this._storage);

  Future<void> saveToken(String token, DateTime expiration) async {
    await Future.wait([
      _storage.write(key: _tokenKey, value: token),
      _storage.write(key: _expirationKey, value: expiration.toIso8601String()),
    ]);
  }

  Future<String?> getToken() async {
    return _storage.read(key: _tokenKey);
  }

  Future<DateTime?> getExpiration() async {
    final raw = await _storage.read(key: _expirationKey);
    if (raw == null) return null;
    return DateTime.tryParse(raw);
  }

  Future<bool> isTokenValid() async {
    final token = await getToken();
    final expiration = await getExpiration();
    if (token == null || expiration == null) return false;
    return DateTime.now().isBefore(expiration);
  }

  Future<void> clearToken() async {
    await Future.wait([
      _storage.delete(key: _tokenKey),
      _storage.delete(key: _expirationKey),
    ]);
  }
}
