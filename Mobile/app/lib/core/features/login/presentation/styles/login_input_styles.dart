import 'package:flutter/material.dart';

class StyleInputs {
  static InputDecoration getStyle(String label) {
    return InputDecoration(
      hintText: label, // 🔹 Agora recebe a label dinamicamente
      filled: true,
      fillColor: Colors.white.withOpacity(0.8),
      border: OutlineInputBorder(
        borderRadius: BorderRadius.circular(20),
        borderSide: BorderSide(color: Colors.transparent),
      ),
      focusedBorder: OutlineInputBorder(
        borderRadius: BorderRadius.circular(20),
        borderSide: BorderSide(color: Colors.blue, width: 2),
      ),
    );
  }
}