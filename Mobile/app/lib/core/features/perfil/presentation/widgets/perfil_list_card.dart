import 'package:flutter/material.dart';

class PerfilListCard extends StatelessWidget {
  final IconData icon;
  final String title;
  final List<String> items;
  final bool isEditing;
  final String placeholder;
  final ValueChanged<List<String>> onChanged;

  const PerfilListCard({
    super.key,
    required this.icon,
    required this.title,
    required this.items,
    required this.isEditing,
    required this.placeholder,
    required this.onChanged,
  });

  void _addItem() {
    onChanged([...items, '']);
  }

  void _removeItem(int index) {
    final updated = List<String>.from(items)..removeAt(index);
    onChanged(updated);
  }

  void _updateItem(int index, String value) {
    final updated = List<String>.from(items);
    updated[index] = value;
    onChanged(updated);
  }

  @override
  Widget build(BuildContext context) {
    return Container(
      width: double.infinity,
      padding: const EdgeInsets.all(16),
      decoration: BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.circular(12),
        border: Border.all(color: const Color(0xFFFFCDD2)),
        boxShadow: [
          BoxShadow(color: Colors.black.withValues(alpha: 0.04), blurRadius: 6),
        ],
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              Row(
                children: [
                  Container(
                    padding: const EdgeInsets.all(8),
                    decoration: BoxDecoration(
                      color: const Color(0xFFFFEBEE),
                      borderRadius: BorderRadius.circular(8),
                    ),
                    child: Icon(icon, color: const Color(0xFFD32F2F), size: 20),
                  ),
                  const SizedBox(width: 12),
                  Text(title, style: const TextStyle(fontSize: 15, fontWeight: FontWeight.bold, color: Color(0xFF212121))),
                ],
              ),
              if (isEditing)
                GestureDetector(
                  onTap: _addItem,
                  child: Container(
                    padding: const EdgeInsets.all(4),
                    decoration: BoxDecoration(
                      color: const Color(0xFFFFEBEE),
                      borderRadius: BorderRadius.circular(6),
                    ),
                    child: const Icon(Icons.add, color: Color(0xFFD32F2F), size: 20),
                  ),
                ),
            ],
          ),
          const SizedBox(height: 12),
          if (items.isEmpty)
            Text(
              'Nenhum item adicionado.',
              style: TextStyle(color: Colors.grey.shade400, fontSize: 13),
            )
          else
            Column(
              children: List.generate(items.length, (index) {
                if (isEditing) {
                  return Padding(
                    padding: const EdgeInsets.only(bottom: 8),
                    child: Row(
                      children: [
                        Expanded(
                          child: TextFormField(
                            initialValue: items[index],
                            onChanged: (v) => _updateItem(index, v),
                            style: const TextStyle(fontSize: 13),
                            decoration: InputDecoration(
                              hintText: placeholder,
                              isDense: true,
                              contentPadding: const EdgeInsets.symmetric(horizontal: 12, vertical: 10),
                              filled: true,
                              fillColor: Colors.white,
                              enabledBorder: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(8),
                                borderSide: const BorderSide(color: Color(0xFFDDDDDD)),
                              ),
                              focusedBorder: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(8),
                                borderSide: const BorderSide(color: Color(0xFFD32F2F)),
                              ),
                            ),
                          ),
                        ),
                        const SizedBox(width: 8),
                        GestureDetector(
                          onTap: () => _removeItem(index),
                          child: const Icon(Icons.remove_circle_outline, color: Color(0xFFD32F2F), size: 22),
                        ),
                      ],
                    ),
                  );
                } else {
                  return Container(
                    width: double.infinity,
                    margin: const EdgeInsets.only(bottom: 8),
                    padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 10),
                    decoration: BoxDecoration(
                      color: const Color(0xFFFFEBEE),
                      borderRadius: BorderRadius.circular(8),
                    ),
                    child: Text(
                      items[index],
                      style: const TextStyle(fontSize: 13, color: Color(0xFF424242)),
                    ),
                  );
                }
              }),
            ),
        ],
      ),
    );
  }
}
