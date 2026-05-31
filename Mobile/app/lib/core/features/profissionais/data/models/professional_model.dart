import 'package:sistema_distribuido/core/features/profissionais/domain/entities/professional.dart';

class ProfessionalModel extends Professional {
  ProfessionalModel({
    required super.id,
    required super.name,
    required super.email,
    super.phone,
    required super.cref,
    super.bio,
    required super.status,
    super.specialty,
    super.methodology,
    super.price,
    super.experience,
  });

  factory ProfessionalModel.fromJson(Map<String, dynamic> json) {
    return ProfessionalModel(
      id: json['id'] as int,
      name: json['name'] as String? ?? '',
      email: json['email'] as String? ?? '',
      phone: json['phone'] as String?,
      cref: json['cref'] as String? ?? '',
      bio: json['bio'] as String?,
      status: json['status'] as int? ?? 1,
      specialty: json['specialty'] as String?,
      methodology: json['methodology'] as String?,
      price: (json['price'] as num?)?.toDouble(),
      experience: json['experience'] as String?,
    );
  }
}
