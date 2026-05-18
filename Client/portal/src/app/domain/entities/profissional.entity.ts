export interface Profissional {
  id: number;
  nome: string;
  email: string;
  telefone: string;
  cref: string;
  bio?: string;
  status?: 'Ativo' | 'Inativo';
  senha?: string;
}
