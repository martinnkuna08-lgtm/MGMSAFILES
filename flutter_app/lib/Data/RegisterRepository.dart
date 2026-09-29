// register_repository.dart
import 'package:sqflite/sqflite.dart';
import '../Model/Register.dart';
import '../Data/DatabaseService.dart';

class RegisterRepository {
  final DatabaseService _dbService = DatabaseService();
  Future<int> insertRegister(Register register) async {
    // Basic null or empty string validation
    if (register.FirstName.trim().isEmpty ||
        register.LastName.trim().isEmpty ||
        register.PhoneNumber.trim().isEmpty ||
        register.Email.trim().isEmpty ||
        register.Password.trim().isEmpty) {
      throw Exception('All fields are required and cannot be empty.');
    }
    final db = await _dbService.database;
    return await db.insert(
      'registers',
      {
        'FirstName': register.FirstName.trim(),
        'LastName': register.LastName.trim(),
        'PhoneNumber': register.PhoneNumber.trim(),
        'Email': register.Email.trim(),
        'Password': register.Password.trim(),
      },
      conflictAlgorithm: ConflictAlgorithm.replace,
    );
  }

  Future<List<Register>> getAllRegisters() async {
    final db = await _dbService.database;
    final result = await db.query('registers');
    return result.map((json) => Register(
      RegisterID: json['RegisterID'] as int,
      FirstName: json['FirstName'] as String,
      LastName: json['LastName'] as String,
      PhoneNumber: json['PhoneNumber'] as String,
      Email: json['Email'] as String,
      Password: json['Password'] as String,
    )).toList();
  }

  Future<int> updateRegister(Register register) async {
    if (register.FirstName.trim().isEmpty ||
        register.LastName.trim().isEmpty ||
        register.PhoneNumber.trim().isEmpty ||
        register.Email.trim().isEmpty ||
        register.Password.trim().isEmpty) {
      throw Exception('All fields are required and cannot be empty.');
    }
    final db = await _dbService.database;
    return await db.update(
      'registers',
      {
        'FirstName': register.FirstName.trim(),
        'LastName': register.LastName.trim(),
        'PhoneNumber': register.PhoneNumber.trim(),
        'Email': register.Email.trim(),
        'Password': register.Password.trim(),
      },
      where: 'RegisterID = ?',
      whereArgs: [register.RegisterID],
    );
  }

  Future<int> deleteRegister(int id) async {
    final db = await _dbService.database;
    return await db.delete('registers', where: 'RegisterID = ?', whereArgs: [id]);
  }
  Future<int> deleteAll() async {
    final db = await _dbService.database;
    return await db.delete('registers');
  }

  Future<Register?> getByEmailAndPassword(String email, String password) async {
    final db = await _dbService.database;
    final result = await db.query(
      'registers',
      where: 'Email = ? AND Password = ?',
      whereArgs: [email, password],
      limit: 1,
    );
    if (result.isNotEmpty) {
      final json = result.first;
      return Register(
        RegisterID: json['RegisterID'] as int,
        FirstName: json['FirstName'] as String,
        LastName: json['LastName'] as String,
        PhoneNumber: json['PhoneNumber'] as String,
        Email: json['Email'] as String,
        Password: json['Password'] as String,
      );
    }
    return null;
  }


}
