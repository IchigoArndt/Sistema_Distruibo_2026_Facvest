// StatusAssessmentEnum: Completed = 1, Pending = 2
enum AssessmentStatus { completed, pending }

// TypeAvaliationEnum: Basic = 1, Complete = 2, Revaluation = 3
enum TypeAvaliation { basic, complete, revaluation }

class BodyComposition {
  final double? weight;
  final double? height;
  final double? waist;
  final double? abdomen;
  final double? hips;
  final double? chest;
  final double? armRight;
  final double? armLeft;
  final double? rightThigh;
  final double? leftThigh;

  BodyComposition({
    this.weight,
    this.height,
    this.waist,
    this.abdomen,
    this.hips,
    this.chest,
    this.armRight,
    this.armLeft,
    this.rightThigh,
    this.leftThigh,
  });
}

class Assessment {
  final int id;
  final int studentId;
  final int professionalId;
  final String? professionalName;
  final DateTime date;
  final AssessmentStatus status;
  final TypeAvaliation typeAvaliation;
  final String? imc;
  final String? bodyFatPercentage;
  final BodyComposition? bodyComposition;
  final String? technicalOpinion;
  final DateTime? dateNextAvaliation;

  Assessment({
    required this.id,
    required this.studentId,
    required this.professionalId,
    this.professionalName,
    required this.date,
    required this.status,
    required this.typeAvaliation,
    this.imc,
    this.bodyFatPercentage,
    this.bodyComposition,
    this.technicalOpinion,
    this.dateNextAvaliation,
  });
}
