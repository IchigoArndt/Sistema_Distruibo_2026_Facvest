import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { AvaliacaoModel } from '../models/avaliacao.model';

@Injectable({ providedIn: 'root' })
export class AvaliacaoDataSource {
  private mockData: AvaliacaoModel[] = [
    {
      id: 1, aluno_id: 1, aluno_nome: 'Ana Silva', data: '15/03/2026',
      tipo: 'Completa', objetivo: 'Emagrecimento', status: 'Concluída', imc: '22.4', percentual_gordura: '18.2',
      composicao_corporal: { peso: 62, altura: 167, cintura: 72, abdomen: 78, quadril: 96, torax: 88, bracoDireito: 28, bracoEsquerdo: 27, coxaDireita: 54, coxaEsquerda: 53 },
      dobras_cutaneas: { peitoral: 12, axilarMedia: 10, tricipital: 18, subescapular: 14, supraIliaca: 16, abdominal: 22, coxa: 20 },
      anamnese: { possuiLesao: false, usaMedicamento: false, nivelAtividade: 'Moderadamente Ativo' },
      parecer_tecnico: 'Aluna apresenta boa composição corporal. Recomenda-se manter protocolo de treino atual.',
      proxima_avaliacao: '15/06/2026',
    },
    {
      id: 2, aluno_id: 2, aluno_nome: 'Bruno Mendes', data: '14/03/2026',
      tipo: 'Básica', objetivo: 'Hipertrofia', status: 'Pendente',
      composicao_corporal: { peso: 82, altura: 178 },
      anamnese: { possuiLesao: true, descricaoLesao: 'Tendinite no ombro direito', usaMedicamento: false, nivelAtividade: 'Muito Ativo' },
      proxima_avaliacao: '14/06/2026',
    },
    {
      id: 3, aluno_id: 3, aluno_nome: 'Carla Souza', data: '12/03/2026',
      tipo: 'Completa', objetivo: 'Condicionamento', status: 'Concluída', imc: '24.1', percentual_gordura: '22.5',
      composicao_corporal: { peso: 68, altura: 168, cintura: 76, abdomen: 82, quadril: 98, torax: 90, bracoDireito: 29, bracoEsquerdo: 29, coxaDireita: 56, coxaEsquerda: 55 },
      dobras_cutaneas: { peitoral: 14, axilarMedia: 12, tricipital: 20, subescapular: 16, supraIliaca: 18, abdominal: 24, coxa: 22 },
      anamnese: { possuiLesao: false, usaMedicamento: true, descricaoMedicamento: 'Anti-hipertensivo', nivelAtividade: 'Levemente Ativo' },
      parecer_tecnico: 'Paciente com boa adesão ao treino. Atenção à pressão arterial durante exercícios intensos.',
      proxima_avaliacao: '12/06/2026',
    },
    {
      id: 4, aluno_id: 4, aluno_nome: 'Diego Lima', data: '10/03/2026',
      tipo: 'Reavaliação', objetivo: 'Hipertrofia', status: 'Concluída', imc: '26.3', percentual_gordura: '14.8',
      composicao_corporal: { peso: 88, altura: 183, cintura: 84, abdomen: 88, quadril: 100, torax: 104, bracoDireito: 38, bracoEsquerdo: 37, coxaDireita: 60, coxaEsquerda: 59 },
      dobras_cutaneas: { peitoral: 8, axilarMedia: 7, tricipital: 10, subescapular: 12, supraIliaca: 14, abdominal: 16, coxa: 14 },
      anamnese: { possuiLesao: false, usaMedicamento: false, nivelAtividade: 'Muito Ativo' },
      parecer_tecnico: 'Excelente evolução em massa muscular. Continuar protocolo de força.',
      proxima_avaliacao: '10/06/2026',
    },
    {
      id: 5, aluno_id: 5, aluno_nome: 'Eva Martins', data: '08/03/2026',
      tipo: 'Básica', objetivo: 'Reabilitação', status: 'Concluída', imc: '21.8',
      composicao_corporal: { peso: 58, altura: 163 },
      anamnese: { possuiLesao: true, descricaoLesao: 'Hérnia de disco lombar L4-L5', usaMedicamento: true, descricaoMedicamento: 'Anti-inflamatório', nivelAtividade: 'Sedentário' },
      parecer_tecnico: 'Protocolo adaptado para reabilitação. Evitar exercícios de alto impacto.',
      proxima_avaliacao: '08/06/2026',
    },
  ];

  getAll(): Observable<AvaliacaoModel[]> {
    return of([...this.mockData]);
  }

  getById(id: number): Observable<AvaliacaoModel | undefined> {
    return of(this.mockData.find(a => a.id === id));
  }

  create(avaliacao: Omit<AvaliacaoModel, 'id'>): Observable<AvaliacaoModel> {
    const newAvaliacao: AvaliacaoModel = { ...avaliacao, id: this.mockData.length + 1 };
    this.mockData.push(newAvaliacao);
    return of(newAvaliacao);
  }

  update(id: number, avaliacao: Partial<AvaliacaoModel>): Observable<AvaliacaoModel> {
    const index = this.mockData.findIndex(a => a.id === id);
    this.mockData[index] = { ...this.mockData[index], ...avaliacao };
    return of(this.mockData[index]);
  }

  delete(id: number): Observable<void> {
    this.mockData = this.mockData.filter(a => a.id !== id);
    return of(undefined);
  }
}
