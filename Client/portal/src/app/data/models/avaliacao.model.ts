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

// -----------------------------------------------------------------------
// Interfaces que espelham exatamente o JSON retornado pelo backend .NET
// -----------------------------------------------------------------------

export interface ApiBodyComposition {
  weight?:     number;
  height?:     number;
  waist?:      number;
  abdomen?:    number;
  hips?:       number;
  chest?:      number;
  armRight?:   number;
  armLeft?:    number;
  rightThigh?: number;
  leftThigh?:  number;
}

export interface ApiSkinfolds {
  pectoral?:    number;
  midAxillary?: number;
  triceps?:     number;
  subscapular?: number;
  suprailiac?:  number;
  abdominal?:   number;
  thigh?:       number;
}

export interface ApiAnamnesis {
  hasInjury:             boolean;
  injuryDescription?:    string;
  hasMedication:         boolean;
  medicationDescription?: string;
  activityLevel:         number; // ActivityLevelEnum int
}

export interface AvaliacaoApiDTO {
  id:                  number;
  studentId:           number;
  studentName?:        string;
  professionalId:      number;
  professionalName?:   string;
  date:                string;        // ISO 8601
  typeAvaliation:      number;        // TypeAvaliationEnum int
  studentObjective:    number;        // StudentObjectiveEnum int
  status:              number;        // StatusAssessmentEnum int
  bodyComposition?:    ApiBodyComposition;
  skinfolds?:          ApiSkinfolds;
  anamnesis?:          ApiAnamnesis;
  technicalOpinion?:   string;
  dateNextAvaliation?: string;        // ISO 8601
  imc?:                string;
  bodyFatPercentage?:  string;
}

// -----------------------------------------------------------------------
// Tabelas de conversão de enums (backend int <-> frontend PT string)
// -----------------------------------------------------------------------

const TIPO_FROM_INT: Record<number, TipoAvaliacao> = {
  1: 'Básica',
  2: 'Completa',
  3: 'Reavaliação',
};

const TIPO_TO_INT: Record<TipoAvaliacao, number> = {
  'Básica':      1,
  'Completa':    2,
  'Reavaliação': 3,
};

const OBJETIVO_FROM_INT: Record<number, ObjetivoAluno> = {
  1: 'Hipertrofia',
  2: 'Emagrecimento',
  3: 'Condicionamento',
  4: 'Reabilitação',
};

const OBJETIVO_TO_INT: Record<ObjetivoAluno, number> = {
  'Hipertrofia':     1,
  'Emagrecimento':   2,
  'Condicionamento': 3,
  'Reabilitação':    4,
};

const STATUS_FROM_INT: Record<number, StatusAvaliacao> = {
  1: 'Concluída',
  2: 'Pendente',
};

const STATUS_TO_INT: Record<StatusAvaliacao, number> = {
  'Concluída': 1,
  'Pendente':  2,
};

const NIVEL_FROM_INT: Record<number, NivelAtividade> = {
  1: 'Sedentário',
  2: 'Levemente Ativo',
  3: 'Moderadamente Ativo',
  4: 'Muito Ativo',
  5: 'Atleta',
};

const NIVEL_TO_INT: Record<NivelAtividade, number> = {
  'Sedentário':           1,
  'Levemente Ativo':      2,
  'Moderadamente Ativo':  3,
  'Muito Ativo':          4,
  'Atleta':               5,
};

// -----------------------------------------------------------------------
// Helpers de formatação de data
// -----------------------------------------------------------------------

function isoToDisplayDate(iso: string): string {
  const d = new Date(iso);
  const day = String(d.getDate()).padStart(2, '0');
  const month = String(d.getMonth() + 1).padStart(2, '0');
  return `${day}/${month}/${d.getFullYear()}`;
}

function displayDateToIso(display: string): string {
  const [day, month, year] = display.split('/');
  return new Date(Number(year), Number(month) - 1, Number(day)).toISOString();
}

// -----------------------------------------------------------------------
// Conversão ApiBodyComposition <-> ComposicaoCorporal
// -----------------------------------------------------------------------

function bodyCompositionToEntity(api?: ApiBodyComposition): ComposicaoCorporal | undefined {
  if (!api) return undefined;
  return {
    peso:          api.weight   ?? 0,
    altura:        api.height   ?? 0,
    cintura:       api.waist,
    abdomen:       api.abdomen,
    quadril:       api.hips,
    torax:         api.chest,
    bracoDireito:  api.armRight,
    bracoEsquerdo: api.armLeft,
    coxaDireita:   api.rightThigh,
    coxaEsquerda:  api.leftThigh,
  };
}

function bodyCompositionToApi(entity?: ComposicaoCorporal): ApiBodyComposition | undefined {
  if (!entity) return undefined;
  return {
    weight:     entity.peso,
    height:     entity.altura,
    waist:      entity.cintura,
    abdomen:    entity.abdomen,
    hips:       entity.quadril,
    chest:      entity.torax,
    armRight:   entity.bracoDireito,
    armLeft:    entity.bracoEsquerdo,
    rightThigh: entity.coxaDireita,
    leftThigh:  entity.coxaEsquerda,
  };
}

// -----------------------------------------------------------------------
// Conversão ApiSkinfolds <-> DobrasСutaneas
// -----------------------------------------------------------------------

function skinfoldsToEntity(api?: ApiSkinfolds): DobrasСutaneas | undefined {
  if (!api) return undefined;
  return {
    peitoral:    api.pectoral,
    axilarMedia: api.midAxillary,
    tricipital:  api.triceps,
    subescapular:api.subscapular,
    supraIliaca: api.suprailiac,
    abdominal:   api.abdominal,
    coxa:        api.thigh,
  };
}

function skinfoldsToApi(entity?: DobrasСutaneas): ApiSkinfolds | undefined {
  if (!entity) return undefined;
  return {
    pectoral:    entity.peitoral,
    midAxillary: entity.axilarMedia,
    triceps:     entity.tricipital,
    subscapular: entity.subescapular,
    suprailiac:  entity.supraIliaca,
    abdominal:   entity.abdominal,
    thigh:       entity.coxa,
  };
}

// -----------------------------------------------------------------------
// Conversão ApiAnamnesis <-> Anamnese
// -----------------------------------------------------------------------

function anamnesisToEntity(api?: ApiAnamnesis): Anamnese | undefined {
  if (!api) return undefined;
  return {
    possuiLesao:          api.hasInjury,
    descricaoLesao:       api.injuryDescription,
    usaMedicamento:       api.hasMedication,
    descricaoMedicamento: api.medicationDescription,
    nivelAtividade:       NIVEL_FROM_INT[api.activityLevel] ?? 'Sedentário',
  };
}

function anamnesisToApi(entity?: Anamnese): ApiAnamnesis | undefined {
  if (!entity) return undefined;
  return {
    hasInjury:             entity.possuiLesao,
    injuryDescription:     entity.descricaoLesao,
    hasMedication:         entity.usaMedicamento,
    medicationDescription: entity.descricaoMedicamento,
    activityLevel:         NIVEL_TO_INT[entity.nivelAtividade] ?? 1,
  };
}

// -----------------------------------------------------------------------
// Conversões principais exportadas
// -----------------------------------------------------------------------

export function toAvaliacaoEntity(api: AvaliacaoApiDTO): Avaliacao {
  return {
    id:                api.id,
    alunoId:           api.studentId,
    alunoNome:         api.studentName ?? '',
    data:              isoToDisplayDate(api.date),
    tipo:              TIPO_FROM_INT[api.typeAvaliation]    ?? 'Básica',
    objetivo:          OBJETIVO_FROM_INT[api.studentObjective] ?? 'Hipertrofia',
    status:            STATUS_FROM_INT[api.status]          ?? 'Pendente',
    imc:               api.imc,
    percentualGordura: api.bodyFatPercentage,
    composicaoCorporal: bodyCompositionToEntity(api.bodyComposition),
    dobrasCutaneas:    skinfoldsToEntity(api.skinfolds),
    anamnese:          anamnesisToEntity(api.anamnesis),
    parecerTecnico:    api.technicalOpinion,
    proximaAvaliacao:  api.dateNextAvaliation
      ? isoToDisplayDate(api.dateNextAvaliation)
      : undefined,
  };
}

export function toAvaliacaoApiPayload(entity: Omit<Avaliacao, 'id'>, professionalId: number): Omit<AvaliacaoApiDTO, 'id' | 'studentName' | 'professionalName'> {
  return {
    studentId:          entity.alunoId,
    professionalId,
    date:               entity.data
      ? displayDateToIso(entity.data)
      : new Date().toISOString(),
    typeAvaliation:     TIPO_TO_INT[entity.tipo]    ?? 1,
    studentObjective:   OBJETIVO_TO_INT[entity.objetivo] ?? 1,
    status:             STATUS_TO_INT[entity.status] ?? 2,
    bodyComposition:    bodyCompositionToApi(entity.composicaoCorporal),
    skinfolds:          skinfoldsToApi(entity.dobrasCutaneas),
    anamnesis:          anamnesisToApi(entity.anamnese),
    technicalOpinion:   entity.parecerTecnico,
    dateNextAvaliation: entity.proximaAvaliacao
      ? displayDateToIso(entity.proximaAvaliacao)
      : undefined,
    imc:                entity.imc,
    bodyFatPercentage:  entity.percentualGordura,
  };
}

export function toAvaliacaoEditPayload(entity: Partial<Avaliacao>): Partial<AvaliacaoApiDTO> {
  const payload: Partial<AvaliacaoApiDTO> = {};
  if (entity.composicaoCorporal) payload.bodyComposition  = bodyCompositionToApi(entity.composicaoCorporal);
  if (entity.dobrasCutaneas)     payload.skinfolds         = skinfoldsToApi(entity.dobrasCutaneas);
  if (entity.anamnese)           payload.anamnesis         = anamnesisToApi(entity.anamnese);
  if (entity.parecerTecnico !== undefined) payload.technicalOpinion = entity.parecerTecnico;
  if (entity.imc !== undefined)            payload.imc = entity.imc;
  if (entity.percentualGordura !== undefined) payload.bodyFatPercentage = entity.percentualGordura;
  if (entity.proximaAvaliacao)   payload.dateNextAvaliation = displayDateToIso(entity.proximaAvaliacao);
  if (entity.status !== undefined) payload.status = STATUS_TO_INT[entity.status];
  return payload;
}
