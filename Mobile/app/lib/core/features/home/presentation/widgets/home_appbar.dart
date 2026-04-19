import 'package:flutter/material.dart';

//Widget
class HomeAppbar extends StatelessWidget implements PreferredSizeWidget {

  const HomeAppbar({super.key});

  @override
  // TODO: implement preferredSize
  Size get preferredSize => const Size.fromHeight(kToolbarHeight); //define o tamanho da tela

  @override
  Widget build(BuildContext context) {
    return AppBar(
      backgroundColor: const Color(0xFFD32F2F),
      leading: Padding(
        padding: const EdgeInsets.all(8.0), //Ver para não deixar fixo (talvez)
        child: CircleAvatar(
          backgroundColor: Colors.white,
          child: Icon(
            Icons.fitness_center,
            color: Color(0xFFD32F2F),
            size: 20,
          ),
        ),
      ),
      title: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text('Anfis',style: TextStyle(color: Colors.white,fontWeight: FontWeight.bold, fontSize: 18)),
          Text('Reliable Enterprise Developments', style: TextStyle(color: Colors.white70, fontSize: 11)),
        ],
      ),
    );
  }
}