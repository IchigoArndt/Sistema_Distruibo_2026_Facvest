import { Profissional } from '../../domain/entities/profissional.entity';

export interface ProfissionalModel {
  id: number;
  name: string;
  email: string;
  phone: string;
  cref: string;
  bio?: string;
  status?: number;
  password?: string;
}

export function toProfissionalEntity(model: ProfissionalModel): Profissional {
  return {
    id: model.id,
    nome: model.name,
    email: model.email,
    telefone: model.phone,
    cref: model.cref,
    bio: model.bio,
    status: model.status !== undefined
      ? (model.status === 1 ? 'Ativo' : 'Inativo')
      : undefined,
  };
}

export function toProfissionalModel(entity: Omit<Profissional, 'id'>): Omit<ProfissionalModel, 'id'> {
  return {
    name: entity.nome,
    email: entity.email,
    phone: entity.telefone.replace(/\D/g, ''),
    cref: entity.cref,
    bio: entity.bio,
    status: entity.status === 'Ativo' ? 1 : entity.status === 'Inativo' ? 2 : undefined,
    password: entity.senha,
  };
}
