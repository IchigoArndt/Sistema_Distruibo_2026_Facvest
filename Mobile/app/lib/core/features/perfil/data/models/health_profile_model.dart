import 'package:sistema_distribuido/core/features/perfil/domain/entities/health_profile.dart';

class HealthProfileModel extends HealthProfile {
  HealthProfileModel({
    required super.id,
    required super.name,
    required super.email,
    required super.age,
    required super.cellPhone,
    super.lastReview,
  });

  factory HealthProfileModel.fromJson(Map<String, dynamic> json) {
    return HealthProfileModel(
      id: (json['id'] as num).toInt(),
      name: json['name'] as String? ?? '',
      email: json['email'] as String? ?? '',
      age: json['age'] as int? ?? 0,
      cellPhone: json['cellPhone'] as String? ?? '',
      lastReview: json['lastReview'] != null
          ? DateTime.tryParse(json['lastReview'] as String)
          : null,
    );
  }
}
