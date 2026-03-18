export interface Avaliacao {
  id: number;
  alunoId: number;
  alunoNome: string;
  data: string;
  tipo: 'Completa' | 'Básica';
  status: 'Concluída' | 'Pendente';
  imc?: string;
}
