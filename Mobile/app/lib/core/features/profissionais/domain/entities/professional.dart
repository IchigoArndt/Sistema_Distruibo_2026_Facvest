class Professional {
  final int id;
  final String name;
  final String email;
  final String? phone;
  final String cref;
  final String? bio;
  final int status;
  final String? specialty;
  final String? methodology;
  final double? price;
  final String? experience;

  Professional({
    required this.id,
    required this.name,
    required this.email,
    this.phone,
    required this.cref,
    this.bio,
    required this.status,
    this.specialty,
    this.methodology,
    this.price,
    this.experience,
  });
}
