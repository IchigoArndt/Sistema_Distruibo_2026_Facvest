class Avaliation {
  String Professional = ""; //Inicializar todas as variaveis
  DateTime Data = DateTime.now();
  String Type = ""; //Transformar em enum ?
  double Price = 0.0;

  //construtor
  Avaliation({
    required this.Professional,
    required this.Data,
    required this.Type,
    required this.Price
  });
}