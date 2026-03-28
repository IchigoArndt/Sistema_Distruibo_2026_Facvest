import {
  Avaliacao,
  TipoAvaliacao,
  ObjetivoAluno,
  StatusAvaliacao,
  NivelAtividade,
  ComposicaoCorporal,
  DobrasСutaneas,
  Anamnese,
} from '../../domain/entities/avaliacao.entity';

export interface AvaliacaoModel {
  id:                    number;
  aluno_id:              number;
  aluno_nome:            string;
  data:                  string;
  tipo:                  string;
  objetivo:              string;
  status:                string;
  imc?:                  string;
  percentual_gordura?:   string;
  composicao_corporal?:  ComposicaoCorporal;
  dobras_cutaneas?:      DobrasСutaneas;
  anamnese?:             Anamnese;
  parecer_tecnico?:      string;
  proxima_avaliacao?:    string;
}

export function toAvaliacaoEntity(model: AvaliacaoModel): Avaliacao {
  return {
    id:                  model.id,
    alunoId:             model.aluno_id,
    alunoNome:           model.aluno_nome,
    data:                model.data,
    tipo:                model.tipo as TipoAvaliacao,
    objetivo:            model.objetivo as ObjetivoAluno,
    status:              model.status as StatusAvaliacao,
    imc:                 model.imc,
    percentualGordura:   model.percentual_gordura,
    composicaoCorporal:  model.composicao_corporal,
    dobrasCutaneas:      model.dobras_cutaneas,
    anamnese:            model.anamnese,
    parecerTecnico:      model.parecer_tecnico,
    proximaAvaliacao:    model.proxima_avaliacao,
  };
}

export function toAvaliacaoModel(entity: Omit<Avaliacao, 'id'>): Omit<AvaliacaoModel, 'id'> {
  return {
    aluno_id:            entity.alunoId,
    aluno_nome:          entity.alunoNome,
    data:                entity.data,
    tipo:                entity.tipo,
    objetivo:            entity.objetivo,
    status:              entity.status,
    imc:                 entity.imc,
    percentual_gordura:  entity.percentualGordura,
    composicao_corporal: entity.composicaoCorporal,
    dobras_cutaneas:     entity.dobrasCutaneas,
    anamnese:            entity.anamnese,
    parecer_tecnico:     entity.parecerTecnico,
    proxima_avaliacao:   entity.proximaAvaliacao,
  };
}
