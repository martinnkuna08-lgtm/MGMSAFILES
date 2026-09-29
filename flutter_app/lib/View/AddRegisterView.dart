import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import '../Model/Register.dart';
import '../ViewModel/RegisterViewModel.dart';
import 'LoginView.dart';
class AddRegisterView extends StatefulWidget { const AddRegisterView({super.key});

  @override
  State<AddRegisterView> createState() => _AddRegisterViewState();
}

class _AddRegisterViewState extends State<AddRegisterView> {
  final _formKey = GlobalKey<FormState>();
  final _firstNameController = TextEditingController();
  final _lastNameController = TextEditingController();
  final _phoneController = TextEditingController();
  final _emailController = TextEditingController();
  final _passwordController = TextEditingController();

  @override
  Widget build(BuildContext context) {

    final viewModel = Provider.of<RegisterViewModel>(context, listen: false);

    return Scaffold(
      appBar: AppBar(title: const Text('Add Register')),
      body: Padding(
        padding: const EdgeInsets.all(16),
        child: Form(
          key: _formKey,
          child: SingleChildScrollView(
            child: Column(
              children: [
                TextFormField(
                  controller: _firstNameController,
                  decoration: const InputDecoration(labelText: 'First Name'),
                ),
                TextFormField(
                  controller: _lastNameController,
                  decoration: const InputDecoration(labelText: 'Last Name'),
                ),
                TextFormField(
                  controller: _phoneController,
                  decoration: const InputDecoration(labelText: 'Phone Number'),
                ),
                TextFormField(
                  controller: _emailController,
                  decoration: const InputDecoration(labelText: 'Email'),
                ),
                TextFormField(
                  controller: _passwordController,
                  decoration: const InputDecoration(labelText: 'Password'),
                  obscureText: true,
                ),
                const SizedBox(height: 100),

                ElevatedButton(
                  onPressed: () async {
                    final newRegister = Register(
                      FirstName: _firstNameController.text,
                      LastName: _lastNameController.text,
                      PhoneNumber: _phoneController.text,
                      Email: _emailController.text,
                      Password: _passwordController.text,
                    );
                    await viewModel.addRegister(newRegister);
                    ScaffoldMessenger.of(context).showSnackBar(
                      const SnackBar(content: Text('Register added')),
                    );

                    _formKey.currentState?.reset();
                    _firstNameController.clear();
                    _lastNameController.clear();
                    _phoneController.clear();
                    _emailController.clear();
                    _passwordController.clear();
                  },
                  child: const Text('Add'),
                ),
                const SizedBox(height: 10),
                ElevatedButton(
                  onPressed: () {
                    Navigator.push(
                      context,
                      MaterialPageRoute(
                        builder: (_) => const LoginView(),
                      ),
                    );
                  },
                  style: ElevatedButton.styleFrom(
                  ),
                  child: const Text('Login'),
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
  @override
  void dispose() {
    _firstNameController.dispose();
    _lastNameController.dispose();
    _phoneController.dispose();
    _emailController.dispose();
    _passwordController.dispose();
    super.dispose();
  }
}
