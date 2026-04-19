import 'package:flutter/material.dart';

import 'package:sistema_distribuido/core/features/login/presentation/styles/login_input_styles.dart';
import 'package:sistema_distribuido/core/features/login/presentation/styles/login_snackbar_styles.dart';
import 'package:sistema_distribuido/core/features/login/domain/entities/UserAuthentication.dart';

class LoginPage extends StatefulWidget {
  const LoginPage({super.key});

  @override
  State<LoginPage> createState() => _loginPage();
}

class _loginPage extends State<LoginPage> {
  String username = "";
  String password = "";

  void authenticateUser() async {
    UserAuthentication user = UserAuthentication(username: username, password: password);

    bool authentication = false;

    if (user.username == "admin" && user.password == "1234") {
      authentication = true;
    }

    if (!mounted) return;

    final snackbar = authentication
        ? StyleSnackBar.snackBarSucess
        : StyleSnackBar.snackBarError;

    ScaffoldMessenger.of(context).showSnackBar(snackbar);

    if (authentication) {
      Navigator.pushNamedAndRemoveUntil(
        context,
        '/home',
        (route) => false,
        arguments: username,
      );
    } else {
      showDialog(
        context: context,
        builder: (context) => _showingUserNotRegisteredDialog(),
      );
    }
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
          width: 80,
          height: 80,
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
          child: const Icon(
            Icons.fitness_center,
            size: 44,
            color: Color(0xFFD32F2F),
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
            ),
            const SizedBox(height: 24),
            SizedBox(
              width: double.infinity,
              child: ElevatedButton(
                onPressed: authenticateUser,
                style: ElevatedButton.styleFrom(
                  backgroundColor: const Color(0xFFD32F2F),
                  foregroundColor: Colors.white,
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(8),
                  ),
                  padding: const EdgeInsets.symmetric(vertical: 15),
                  elevation: 2,
                ),
                child: const Text(
                  "Entrar",
                  style: TextStyle(fontSize: 16, fontWeight: FontWeight.w600),
                ),
              ),
            ),
            const SizedBox(height: 16),
            Center(
              child: TextButton(
                onPressed: () {},
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

  AlertDialog _showingUserNotRegisteredDialog() {
    return AlertDialog(
      title: const Text("Hmmm, parece que esse usuário não está cadastrado"),
      content: const Text(
          "Precisamos de uma ajudinha extra para resolver isso\n Fale com o administrador do sistema."),
      actions: [
        TextButton(
          onPressed: () => Navigator.of(context).pop(false),
          child: const Text("Ok"),
        )
      ],
    );
  }
}