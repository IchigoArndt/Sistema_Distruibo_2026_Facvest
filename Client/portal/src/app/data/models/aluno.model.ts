import { Aluno } from '../../domain/entities/aluno.entity';

export interface AlunoModel {
  id: number;
  nome: string;
  email: string;
  telefone: string;
  status: string;
  ultima_avaliacao?: string;
}

export function toAlunoEntity(model: AlunoModel): Aluno {
  return {
    id: model.id,
    nome: model.nome,
    email: model.email,
    telefone: model.telefone,
    status: model.status as 'Ativo' | 'Inativo',
    ultimaAvaliacao: model.ultima_avaliacao
  };
}

export function toAlunoModel(entity: Omit<Aluno, 'id'>): Omit<AlunoModel, 'id'> {
  return {
    nome: entity.nome,
    email: entity.email,
    telefone: entity.telefone,
    status: entity.status,
    ultima_avaliacao: entity.ultimaAvaliacao
  };
}
