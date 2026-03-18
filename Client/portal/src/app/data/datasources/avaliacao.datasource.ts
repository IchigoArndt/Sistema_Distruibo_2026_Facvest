import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { AvaliacaoModel } from '../models/avaliacao.model';

@Injectable({ providedIn: 'root' })
export class AvaliacaoDataSource {
  private mockData: AvaliacaoModel[] = [
    { id: 1, aluno_id: 1, aluno_nome: 'Ana Silva',    data: '15/03/2026', tipo: 'Completa', status: 'Concluída', imc: '22.4' },
    { id: 2, aluno_id: 2, aluno_nome: 'Bruno Mendes', data: '14/03/2026', tipo: 'Básica',   status: 'Pendente',  imc: undefined },
    { id: 3, aluno_id: 3, aluno_nome: 'Carla Souza',  data: '12/03/2026', tipo: 'Completa', status: 'Concluída', imc: '24.1' },
    { id: 4, aluno_id: 4, aluno_nome: 'Diego Lima',   data: '10/03/2026', tipo: 'Completa', status: 'Concluída', imc: '26.3' },
    { id: 5, aluno_id: 5, aluno_nome: 'Eva Martins',  data: '08/03/2026', tipo: 'Básica',   status: 'Concluída', imc: '21.8' },
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
