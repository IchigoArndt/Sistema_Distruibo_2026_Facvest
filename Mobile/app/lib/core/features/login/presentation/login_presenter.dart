import 'package:flutter/material.dart';
import 'package:sistema_distribuido/core/errors/auth_exceptions.dart';
import 'package:sistema_distribuido/core/features/login/domain/entities/UserAuthentication.dart';
import 'package:sistema_distribuido/core/features/login/domain/services/IUserAuthenticationSerivce.dart';
import 'package:sistema_distribuido/core/features/login/presentation/styles/login_input_styles.dart';
import 'package:sistema_distribuido/core/features/login/presentation/styles/login_snackbar_styles.dart';
import 'package:sistema_distribuido/core/shared/di/service_locator.dart';

class LoginPage extends StatefulWidget {
  const LoginPage({super.key});

  @override
  State<LoginPage> createState() => _loginPage();
}

class _loginPage extends State<LoginPage> {
  String username = "";
  String password = "";
  bool _isLoading = false;

  final IAuthenticationService _authService = sl<IAuthenticationService>();

  void authenticateUser() async {
    if (username.trim().isEmpty || password.trim().isEmpty) {
      ScaffoldMessenger.of(context).showSnackBar(StyleSnackBar.snackBarError);
      return;
    }

    setState(() => _isLoading = true);

    try {
      final user = UserAuthentication(username: username.trim(), password: password);
      final success = await _authService.login(user);

      if (!mounted) return;

      if (success) {
        Navigator.pushNamedAndRemoveUntil(
          context,
          '/home',
          (route) => false,
          arguments: username,
        );
      }
    } on InvalidCredentialsException catch (e) {
      if (!mounted) return;
      _showErrorSnackbar(e.message);
    } on TimeoutException catch (e) {
      if (!mounted) return;
      _showErrorSnackbar(e.message);
    } on NetworkException catch (e) {
      if (!mounted) return;
      _showErrorSnackbar(e.message);
    } on AuthException catch (e) {
      if (!mounted) return;
      _showErrorSnackbar(e.message);
    } catch (_) {
      if (!mounted) return;
      _showErrorSnackbar('Ocorreu um erro inesperado. Tente novamente.');
    } finally {
      if (mounted) setState(() => _isLoading = false);
    }
  }

  void _showErrorSnackbar(String message) {
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text(message),
        backgroundColor: const Color(0xFFD32F2F),
        behavior: SnackBarBehavior.floating,
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(8)),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Container(
        decoration: const BoxDecoration(
          gradient: LinearGradient(
            begin: Alignment.topLeft,
            end: Alignment.bottomRight,
            colors: [
              Color(0xFF0D1B2A),
              Color(0xFF1B2A3B),
              Color(0xFF3B1A2A),
            ],
            stops: [0.0, 0.55, 1.0],
          ),
        ),
        child: SafeArea(
          child: SingleChildScrollView(
            child: ConstrainedBox(
              constraints: BoxConstraints(
                minHeight: MediaQuery.of(context).size.height -
                    MediaQuery.of(context).padding.top -
                    MediaQuery.of(context).padding.bottom,
              ),
              child: IntrinsicHeight(
                child: Column(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    const SizedBox(height: 60),
                    _buildLogo(),
                    const SizedBox(height: 40),
                    _buildLoginCard(),
                    const SizedBox(height: 24),
                    _buildRegisterLink(),
                    const SizedBox(height: 40),
                  ],
                ),
              ),
            ),
          ),
        ),
      ),
    );
  }

  Widget _buildLogo() {
    return Column(
      children: [
        Container(
          width: 100,
          height: 100,
          decoration: BoxDecoration(
            color: Colors.white,
            shape: BoxShape.circle,
            boxShadow: [
              BoxShadow(
                color: Colors.black.withOpacity(0.3),
                blurRadius: 12,
                offset: const Offset(0, 4),
              ),
            ],
          ),
          child: ClipOval(
            child: Padding(
              padding: const EdgeInsets.all(10),
              child: Image.asset(
                'assets/images/logo-anfis.png',
                fit: BoxFit.contain,
              ),
            ),
          ),
        ),
        const SizedBox(height: 16),
        const Text(
          "AnFis",
          style: TextStyle(
            fontSize: 32,
            fontWeight: FontWeight.bold,
            color: Colors.white,
            letterSpacing: 1.2,
          ),
        ),
        const SizedBox(height: 4),
        const Text(
          "Sistema de Avaliação Física",
          style: TextStyle(
            fontSize: 14,
            color: Color(0xFFEF9A9A),
            fontWeight: FontWeight.w500,
          ),
        ),
        const SizedBox(height: 2),
        const Text(
          "Reliable Enterprise Developments",
          style: TextStyle(
            fontSize: 12,
            color: Color(0xFF90A4AE),
          ),
        ),
      ],
    );
  }

  Widget _buildLoginCard() {
    return Padding(
      padding: const EdgeInsets.symmetric(horizontal: 24),
      child: Container(
        width: double.infinity,
        decoration: BoxDecoration(
          color: Colors.white,
          borderRadius: BorderRadius.circular(16),
          boxShadow: [
            BoxShadow(
              color: Colors.black.withOpacity(0.25),
              blurRadius: 20,
              offset: const Offset(0, 8),
            ),
          ],
        ),
        padding: const EdgeInsets.all(28),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            const Center(
              child: Text(
                "Entrar",
                style: TextStyle(
                  fontSize: 24,
                  fontWeight: FontWeight.bold,
                  color: Color(0xFF1A1A1A),
                ),
              ),
            ),
            const SizedBox(height: 24),
            const Text(
              "Email",
              style: TextStyle(
                fontSize: 13,
                fontWeight: FontWeight.w600,
                color: Color(0xFF444444),
              ),
            ),
            const SizedBox(height: 6),
            TextField(
              onChanged: (value) => setState(() => username = value),
              keyboardType: TextInputType.emailAddress,
              enabled: !_isLoading,
              decoration: StyleInputs.getStyle("seu@email.com"),
            ),
            const SizedBox(height: 16),
            const Text(
              "Senha",
              style: TextStyle(
                fontSize: 13,
                fontWeight: FontWeight.w600,
                color: Color(0xFF444444),
              ),
            ),
            const SizedBox(height: 6),
            TextField(
              onChanged: (value) => setState(() => password = value),
              decoration: StyleInputs.getStyle("••••••••"),
              obscureText: true,
              enabled: !_isLoading,
            ),
            const SizedBox(height: 24),
            SizedBox(
              width: double.infinity,
              child: ElevatedButton(
                onPressed: _isLoading ? null : authenticateUser,
                style: ElevatedButton.styleFrom(
                  backgroundColor: const Color(0xFFD32F2F),
                  foregroundColor: Colors.white,
                  disabledBackgroundColor: const Color(0xFFD32F2F).withOpacity(0.6),
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(8),
                  ),
                  padding: const EdgeInsets.symmetric(vertical: 15),
                  elevation: 2,
                ),
                child: _isLoading
                    ? const SizedBox(
                        height: 20,
                        width: 20,
                        child: CircularProgressIndicator(
                          color: Colors.white,
                          strokeWidth: 2,
                        ),
                      )
                    : const Text(
                        "Entrar",
                        style: TextStyle(fontSize: 16, fontWeight: FontWeight.w600),
                      ),
              ),
            ),
            const SizedBox(height: 16),
            Center(
              child: TextButton(
                onPressed: _isLoading ? null : () {},
                child: const Text(
                  "Esqueceu sua senha?",
                  style: TextStyle(
                    color: Color(0xFFD32F2F),
                    fontSize: 13,
                    fontWeight: FontWeight.w500,
                  ),
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildRegisterLink() {
    return Row(
      mainAxisAlignment: MainAxisAlignment.center,
      children: [
        const Text(
          "Não tem uma conta? ",
          style: TextStyle(color: Color(0xFFB0BEC5), fontSize: 14),
        ),
        GestureDetector(
          onTap: () {},
          child: const Text(
            "Cadastre-se",
            style: TextStyle(
              color: Color(0xFF64B5F6),
              fontSize: 14,
              fontWeight: FontWeight.w600,
              decoration: TextDecoration.underline,
              decorationColor: Color(0xFF64B5F6),
            ),
          ),
        ),
      ],
    );
  }
}
