import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { AlunoModel } from '../models/aluno.model';

@Injectable({ providedIn: 'root' })
export class AlunoDataSource {
  private mockData: AlunoModel[] = [
    { id: 1, nome: 'Ana Silva',    email: 'ana@email.com',    telefone: '(47) 99999-0001', status: 'Ativo',   ultima_avaliacao: '15/03/2026' },
    { id: 2, nome: 'Bruno Mendes', email: 'bruno@email.com',  telefone: '(47) 99999-0002', status: 'Ativo',   ultima_avaliacao: '14/03/2026' },
    { id: 3, nome: 'Carla Souza',  email: 'carla@email.com',  telefone: '(47) 99999-0003', status: 'Inativo', ultima_avaliacao: '12/03/2026' },
    { id: 4, nome: 'Diego Lima',   email: 'diego@email.com',  telefone: '(47) 99999-0004', status: 'Ativo',   ultima_avaliacao: '10/03/2026' },
    { id: 5, nome: 'Eva Martins',  email: 'eva@email.com',    telefone: '(47) 99999-0005', status: 'Ativo',   ultima_avaliacao: '08/03/2026' },
  ];

  getAll(): Observable<AlunoModel[]> {
    return of([...this.mockData]);
  }

  getById(id: number): Observable<AlunoModel | undefined> {
    return of(this.mockData.find(a => a.id === id));
  }

  create(aluno: Omit<AlunoModel, 'id'>): Observable<AlunoModel> {
    const newAluno: AlunoModel = { ...aluno, id: this.mockData.length + 1 };
    this.mockData.push(newAluno);
    return of(newAluno);
  }

  update(id: number, aluno: Partial<AlunoModel>): Observable<AlunoModel> {
    const index = this.mockData.findIndex(a => a.id === id);
    this.mockData[index] = { ...this.mockData[index], ...aluno };
    return of(this.mockData[index]);
  }

  delete(id: number): Observable<void> {
    this.mockData = this.mockData.filter(a => a.id !== id);
    return of(undefined);
  }
}
