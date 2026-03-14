import 'package:flutter/material.dart';

class StyleSnackBar {
  static final snackBarSucess = SnackBar(
    content: Text(
      "Sucesso, realizando autenticação",
      style: TextStyle(color: Colors.white),
    ),
    backgroundColor: Colors.green,
  );

  static final snackBarError = SnackBar(
    content: Text(
      "Usuario não reconhecido",
      style: TextStyle(color: Colors.white),
    ),
    backgroundColor: Colors.red,
  );
}