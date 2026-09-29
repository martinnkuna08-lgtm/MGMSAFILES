import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import '../ViewModel/RegisterViewModel.dart';
import 'UpdateRegisterView.dart';

class ViewRegisterView extends StatelessWidget {
  const ViewRegisterView({super.key});

  @override
  Widget build(BuildContext context) {

    final viewModel = Provider.of<RegisterViewModel>(context, listen: false);
    return Scaffold(
      appBar: AppBar(title: const Text('All Registers'),
        actions: [
        ElevatedButton(
        style: ElevatedButton.styleFrom(
        elevation: 0, // keep it flat in app bar
        foregroundColor: Colors.white,
        backgroundColor: Colors.red,
      ),
      onPressed: () async {
        final shouldDelete = await showDialog<bool>(
          context: context,
          builder: (ctx) => AlertDialog(
            title: const Text('Delete All'),
            content: const Text('Are you sure you want to delete all entries?'),
            actions: [
              TextButton(onPressed: () => Navigator.of(ctx).pop(false), child: const Text('No')),
              TextButton(onPressed: () => Navigator.of(ctx).pop(true), child: const Text('Yes')),
            ],
          ),
        );
        if (shouldDelete == true) {
          await viewModel.deleteAllRegisters();
        }
      },
      child: const Text('Delete All'),
    ),
    const SizedBox(width: 8), // spacing
    ],
    ),

      body: Consumer<RegisterViewModel>(
        builder: (context, viewModel, child) {
          final registers = viewModel.registers;

          if (registers.isEmpty) {
            return const Center(child: Text('No records found.'));
          }

          return ListView.builder(
            itemCount: registers.length,
            itemBuilder: (context, index) {
              final register = registers[index];

              return ListTile(
                title: Text('${register.RegisterID}  ${register.FirstName} ${register.LastName}'),
                subtitle: Text(register.Email),
                trailing: Row(
                  mainAxisSize: MainAxisSize.min, // keeps the row tight
                  children: [
                    IconButton(
                      icon: const Icon(Icons.edit),
                      onPressed: () {
                        Navigator.push(
                          context,
                          MaterialPageRoute(
                            builder: (_) => UpdateRegisterView(register: register),
                          ),
                        );
                      },
                    ),
                    IconButton(
                      icon: const Icon(Icons.delete, color: Colors.red),
                      onPressed: () async {
                        final confirm = await showDialog<bool>(
                          context: context,
                          builder: (ctx) => AlertDialog(
                            title: const Text('Confirm Deletion'),
                            content: const Text('Are you sure you want to delete this?'),
                            actions: [
                              TextButton(onPressed: () => Navigator.of(ctx).pop(false), child: const Text('No')),
                              TextButton(onPressed: () => Navigator.of(ctx).pop(true), child: const Text('Yes')),
                            ],
                          ),
                        );

                        // Only delete if user tapped "Yes"
                        if (confirm == true) {
                          viewModel.deleteRegister(register.RegisterID!);
                        }
                      },
                    ),
                  ],
                ),
              );
            },
          );
        },
      ),
      floatingActionButton: FloatingActionButton(
        child: const Icon(Icons.add),
        onPressed: () {
          Navigator.pushNamed(context, '/add');
        },
      ),
    );
  }
}
