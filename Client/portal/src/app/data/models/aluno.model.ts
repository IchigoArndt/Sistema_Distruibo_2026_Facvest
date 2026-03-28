import { Aluno } from '../../domain/entities/aluno.entity';

export interface AlunoModel {
  id: number;
  name: string;
  email: string;
  cellPhone: string;
  status: number;
  lastReview?: string;
  age: number;
}

const INVALID_DATE = '0001-01-01T00:00:00';

function parseLastReview(value?: string): string | undefined {
  if (!value || value.startsWith(INVALID_DATE)) return undefined;
  return value;
}

function formatCellPhone(value: string): string {
  const digits = value.replace(/\D/g, '');
  if (digits.length === 11) {
    return `(${digits.slice(0, 2)}) ${digits.slice(2, 7)}-${digits.slice(7)}`;
  }
  if (digits.length === 10) {
    return `(${digits.slice(0, 2)}) ${digits.slice(2, 6)}-${digits.slice(6)}`;
  }
  return value;
}

export function toAlunoEntity(model: AlunoModel): Aluno {
  return {
    id: model.id,
    name: model.name,
    email: model.email,
    cellPhone: formatCellPhone(model.cellPhone),
    status: model.status === 1 ? 'Ativo' : 'Inativo',
    lastReview: parseLastReview(model.lastReview),
    age: model.age
  };
}

export function toAlunoModel(entity: Omit<Aluno, 'id'>): Omit<AlunoModel, 'id'> {
  return {
    name: entity.name,
    email: entity.email,
    cellPhone: entity.cellPhone.replace(/\D/g, ''),
    status: entity.status === 'Ativo' ? 1 : 2,
    lastReview: entity.lastReview,
    age: entity.age
  };
}
