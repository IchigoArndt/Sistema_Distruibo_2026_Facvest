export interface Aluno {
  id: number;
  name: string;
  email: string;
  cellPhone: string;
  status: 'Ativo' | 'Inativo';
  lastReview?: string;
  age: number;
}
