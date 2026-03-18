export interface Aluno {
  id: number;
  nome: string;
  email: string;
  telefone: string;
  status: 'Ativo' | 'Inativo';
  ultimaAvaliacao?: string;
}
