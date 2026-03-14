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
      body: Stack(
        children: [
          // 🖼️ Imagem de fundo ocupando toda a tela
          Positioned.fill(

            child: Opacity(
              opacity: 0.7,
              child: Image.asset(
                "assets/images/background_login.png",
                fit: BoxFit.cover, // Ajusta a imagem ao tamanho da tela
              ),
            ),
          ),
          // 🔹 Área do login sobreposta à imagem
          Center(
            child: Padding(
              padding: const EdgeInsets.all(16.0),
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  Text(
                    "Bem Vindo",
                    style: TextStyle(
                      fontSize: 28,
                      fontWeight: FontWeight.bold,
                      color: Colors.white,
                    ),
                  ),
                  SizedBox(height: 20),
                  TextField(
                    onChanged: (value) => setState(() => username = value),
                    decoration: StyleInputs.getStyle("Nome do usuario"),
                  ),
                  SizedBox(height: 10),
                  TextField(
                    onChanged: (value) => setState(() => password = value),
                    decoration: StyleInputs.getStyle("Senha"),
                    obscureText: true,
                  ),
                  SizedBox(height: 20),
                  FractionallySizedBox(
                    widthFactor: 0.9,
                    child: ElevatedButton(
                      onPressed: authenticateUser,
                      style: ElevatedButton.styleFrom(
                        backgroundColor: Colors.blue,
                        foregroundColor: Colors.white,
                        shape: RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(20),
                        ),
                        padding: EdgeInsets.symmetric(
                          horizontal: 40,
                          vertical: 15,
                        ),
                      ),
                      child: Text("Logar", style: TextStyle(fontSize: 16)),
                    ),
                  ),
                ],
              ),
            ),
          ),
        ],
      ),
    );
  }

  AlertDialog _showingUserNotRegisteredDialog() {
    return AlertDialog(
      title: Text("Hmmm, parece que esse usuário não está cadastrado"),
      content: Text("Precisamos de uma ajudinha extra para resolver isso\n Fale com o administrador do sistema."),
      actions: [
        TextButton(
          onPressed: () => Navigator.of(context).pop(false),
          child: Text("Ok"),
        )
      ],
    );
  }

}