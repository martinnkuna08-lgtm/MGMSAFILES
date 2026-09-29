import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import '../Model/Register.dart';
import '../ViewModel/RegisterViewModel.dart';

class UpdateRegisterView extends StatefulWidget {
  final Register register;
  const UpdateRegisterView({required this.register, super.key});

  @override
  State<UpdateRegisterView> createState() => _UpdateRegisterViewState();
}

class _UpdateRegisterViewState extends State<UpdateRegisterView> {
  late TextEditingController _firstNameController;
  late TextEditingController _lastNameController;
  late TextEditingController _phoneController;
  late TextEditingController _emailController;
  late TextEditingController _passwordController;

  @override
  void initState() {
    super.initState();
    _firstNameController = TextEditingController(text: widget.register.FirstName);
    _lastNameController = TextEditingController(text: widget.register.LastName);
    _phoneController = TextEditingController(text: widget.register.PhoneNumber);
    _emailController = TextEditingController(text: widget.register.Email);
    _passwordController = TextEditingController(text: widget.register.Password);
  }

  @override
  Widget build(BuildContext context) {
    final viewModel = Provider.of<RegisterViewModel>(context, listen: false);

    return Scaffold(
      appBar: AppBar(title: const Text('Update Register')),
      body: Padding(
        padding: const EdgeInsets.all(16),
        child: Column(
          children: [
            TextField(controller: _firstNameController, decoration: const InputDecoration(labelText: 'First Name')),
            TextField(controller: _lastNameController, decoration: const InputDecoration(labelText: 'Last Name')),
            TextField(controller: _phoneController, decoration: const InputDecoration(labelText: 'Phone Number')),
            TextField(controller: _emailController, decoration: const InputDecoration(labelText: 'Email')),
            TextField(controller: _passwordController, decoration: const InputDecoration(labelText: 'Password')),
            const SizedBox(height: 20),
            ElevatedButton(
              onPressed: () async {
                final updatedRegister = Register(
                  RegisterID: widget.register.RegisterID,
                  FirstName: _firstNameController.text,
                  LastName: _lastNameController.text,
                  PhoneNumber: _phoneController.text,
                  Email: _emailController.text,
                  Password: _passwordController.text,
                );
                await viewModel.updateRegister(widget.register.RegisterID!, updatedRegister);
                Navigator.pop(context);
              },
              child: const Text('Update'),
            ),
          ],
        ),
      ),
    );
  }
}
