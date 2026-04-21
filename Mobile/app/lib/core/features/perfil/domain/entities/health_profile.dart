class HealthProfile {
  String name;
  int age;
  String gender;
  double weight;
  int height;
  List<String> medicalConditions;
  List<String> medications;
  List<String> goals;

  HealthProfile({
    required this.name,
    required this.age,
    required this.gender,
    required this.weight,
    required this.height,
    required this.medicalConditions,
    required this.medications,
    required this.goals,
  });
}
