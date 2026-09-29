import 'package:flutter/foundation.dart';
import '../Model/Register.dart';
import '../Data/RegisterRepository.dart';

class RegisterViewModel extends ChangeNotifier {
  final RegisterRepository _repository = RegisterRepository();

  List<Register> _registers = [];
  List<Register> get registers => List.unmodifiable(_registers);

  // Load all registers from the database
  Future<void> loadRegisters() async {
    _registers = await _repository.getAllRegisters();
    notifyListeners();
  }

  // Add a new register
  Future<void> addRegister(Register register) async {
    await _repository.insertRegister(register);
    _registers = await _repository.getAllRegisters();
    notifyListeners();
  }


  // Update register by ID
  Future<void> updateRegister(int id, Register updatedRegister) async {
    await _repository.updateRegister(updatedRegister);
    _registers = await _repository.getAllRegisters();
    notifyListeners();
  }

  // Delete register by ID
  Future<void> deleteRegister(int id) async {
    await _repository.deleteRegister(id);
    _registers = await _repository.getAllRegisters();
    notifyListeners();
  }

  // Delete all registers
  Future<void> deleteAllRegisters() async {
    await _repository.deleteAll();
    _registers = await _repository.getAllRegisters();
    notifyListeners();
  }

  Future<Register?> login(String email, String password) async {
    return await _repository.getByEmailAndPassword(email, password);
  }
}

