import { Avaliacao } from '../../domain/entities/avaliacao.entity';

export interface AvaliacaoModel {
  id: number;
  aluno_id: number;
  aluno_nome: string;
  data: string;
  tipo: string;
  status: string;
  imc?: string;
}

export function toAvaliacaoEntity(model: AvaliacaoModel): Avaliacao {
  return {
    id: model.id,
    alunoId: model.aluno_id,
    alunoNome: model.aluno_nome,
    data: model.data,
    tipo: model.tipo as 'Completa' | 'Básica',
    status: model.status as 'Concluída' | 'Pendente',
    imc: model.imc
  };
}

export function toAvaliacaoModel(entity: Omit<Avaliacao, 'id'>): Omit<AvaliacaoModel, 'id'> {
  return {
    aluno_id: entity.alunoId,
    aluno_nome: entity.alunoNome,
    data: entity.data,
    tipo: entity.tipo,
    status: entity.status,
    imc: entity.imc
  };
}
