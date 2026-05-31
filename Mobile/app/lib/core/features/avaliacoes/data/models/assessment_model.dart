import 'package:sistema_distribuido/core/features/avaliacoes/domain/entities/assessment.dart';

class BodyCompositionModel extends BodyComposition {
  BodyCompositionModel({
    super.weight,
    super.height,
    super.waist,
    super.abdomen,
    super.hips,
    super.chest,
    super.armRight,
    super.armLeft,
    super.rightThigh,
    super.leftThigh,
  });

  factory BodyCompositionModel.fromJson(Map<String, dynamic> json) {
    return BodyCompositionModel(
      weight: (json['weight'] as num?)?.toDouble(),
      height: (json['height'] as num?)?.toDouble(),
      waist: (json['waist'] as num?)?.toDouble(),
      abdomen: (json['abdomen'] as num?)?.toDouble(),
      hips: (json['hips'] as num?)?.toDouble(),
      chest: (json['chest'] as num?)?.toDouble(),
      armRight: (json['armRight'] as num?)?.toDouble(),
      armLeft: (json['armLeft'] as num?)?.toDouble(),
      rightThigh: (json['rightThigh'] as num?)?.toDouble(),
      leftThigh: (json['leftThigh'] as num?)?.toDouble(),
    );
  }
}

class AssessmentModel extends Assessment {
  AssessmentModel({
    required super.id,
    required super.studentId,
    required super.professionalId,
    super.professionalName,
    required super.date,
    required super.status,
    required super.typeAvaliation,
    super.imc,
    super.bodyFatPercentage,
    super.bodyComposition,
    super.technicalOpinion,
    super.dateNextAvaliation,
  });

  factory AssessmentModel.fromJson(Map<String, dynamic> json) {
    return AssessmentModel(
      id: json['id'] as int,
      studentId: json['studentId'] as int,
      professionalId: json['professionalId'] as int,
      professionalName: json['professionalName'] as String?,
      date: DateTime.parse(json['date'] as String),
      status: _parseStatus(json['status'] as int? ?? 2),
      typeAvaliation: _parseType(json['typeAvaliation'] as int? ?? 1),
      imc: json['imc'] as String?,
      bodyFatPercentage: json['bodyFatPercentage'] as String?,
      bodyComposition: json['bodyComposition'] != null
          ? BodyCompositionModel.fromJson(
              json['bodyComposition'] as Map<String, dynamic>)
          : null,
      technicalOpinion: json['technicalOpinion'] as String?,
      dateNextAvaliation: json['dateNextAvaliation'] != null
          ? DateTime.tryParse(json['dateNextAvaliation'] as String)
          : null,
    );
  }

  static AssessmentStatus _parseStatus(int value) {
    return value == 1 ? AssessmentStatus.completed : AssessmentStatus.pending;
  }

  static TypeAvaliation _parseType(int value) {
    switch (value) {
      case 2:
        return TypeAvaliation.complete;
      case 3:
        return TypeAvaliation.revaluation;
      default:
        return TypeAvaliation.basic;
    }
  }
}
