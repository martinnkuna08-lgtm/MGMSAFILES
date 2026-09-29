import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import '../ViewModel/RegisterViewModel.dart';
import 'View/MitUpView/MitUpView.dart';

void main() {
  runApp(
    ChangeNotifierProvider(
      create: (_) => RegisterViewModel()..loadRegisters(),
      child: const MyApp(),
    ),
  );
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Register App',
      theme: ThemeData(
        colorScheme: ColorScheme.fromSeed(seedColor: Colors.deepPurple),
        useMaterial3: true,
      ),
      home: const MitUpView(), // Start with AddRegisterView
      routes: {
        '/add': (context) => const MitUpView(),

      },
    );
  }
}
