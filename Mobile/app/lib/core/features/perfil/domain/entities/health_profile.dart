class HealthProfile {
  final int id;
  final String name;
  final String email;
  final int age;
  final String cellPhone;
  final DateTime? lastReview;

  HealthProfile({
    required this.id,
    required this.name,
    required this.email,
    required this.age,
    required this.cellPhone,
    this.lastReview,
  });
}
