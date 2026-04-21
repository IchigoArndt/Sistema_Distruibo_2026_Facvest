enum AssessmentStatus { requested, inProgress, completed }

class AssessmentResults {
  final double weight;
  final int height;
  final double bodyFat;
  final double muscleMass;
  final double imc;

  AssessmentResults({
    required this.weight,
    required this.height,
    required this.bodyFat,
    required this.muscleMass,
    required this.imc,
  });
}

class Assessment {
  final int id;
  final String professionalName;
  final DateTime date;
  final AssessmentStatus status;
  final String methodology;
  final double price;
  final String? notes;
  final AssessmentResults? results;

  Assessment({
    required this.id,
    required this.professionalName,
    required this.date,
    required this.status,
    required this.methodology,
    required this.price,
    this.notes,
    this.results,
  });
}
