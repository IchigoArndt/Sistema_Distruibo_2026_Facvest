export type TipoAvaliacao    = 'Básica' | 'Completa' | 'Reavaliação';
export type ObjetivoAluno    = 'Hipertrofia' | 'Emagrecimento' | 'Condicionamento' | 'Reabilitação';
export type NivelAtividade   = 'Sedentário' | 'Levemente Ativo' | 'Moderadamente Ativo' | 'Muito Ativo' | 'Atleta';
export type StatusAvaliacao  = 'Concluída' | 'Pendente';

export interface ComposicaoCorporal {
  peso:        number;
  altura:      number;
  cintura?:    number;
  abdomen?:    number;
  quadril?:    number;
  torax?:      number;
  bracoDireito?:    number;
  bracoEsquerdo?:   number;
  coxaDireita?:     number;
  coxaEsquerda?:    number;
}

export interface DobrasСutaneas {
  peitoral?:       number;
  axilarMedia?:    number;
  tricipital?:     number;
  subescapular?:   number;
  supraIliaca?:    number;
  abdominal?:      number;
  coxa?:           number;
}

export interface Anamnese {
  possuiLesao:         boolean;
  descricaoLesao?:     string;
  usaMedicamento:      boolean;
  descricaoMedicamento?: string;
  nivelAtividade:      NivelAtividade;
}

export interface Avaliacao {
  id:                   number;
  alunoId:              number;
  alunoNome:            string;
  data:                 string;
  tipo:                 TipoAvaliacao;
  objetivo:             ObjetivoAluno;
  status:               StatusAvaliacao;
  composicaoCorporal?:  ComposicaoCorporal;
  dobrasCutaneas?:      DobrasСutaneas;
  anamnese?:            Anamnese;
  parecerTecnico?:      string;
  proximaAvaliacao?:    string;
  imc?:                 string;
  percentualGordura?:   string;
}
