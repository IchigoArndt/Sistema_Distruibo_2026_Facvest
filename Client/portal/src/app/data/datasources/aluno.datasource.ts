import { Injectable } from '@angular/core';
import { Observable, of, switchMap } from 'rxjs';
import { AlunoModel } from '../models/aluno.model';
import { environment } from '../../../environment';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class AlunoDataSource {

  private apiUrl = `${environment.apiUrl}/Student`;

  private mockData: AlunoModel[] = [
    { id: 1, name: 'Ana Silva',    email: 'ana@email.com',    cellPhone: '47999990001', status: 1, lastReview: '15/03/2026', age: 25 },
    { id: 2, name: 'Bruno Mendes', email: 'bruno@email.com',  cellPhone: '47999990002', status: 1, lastReview: '14/03/2026', age: 25 },
    { id: 3, name: 'Carla Souza',  email: 'carla@email.com',  cellPhone: '47999990003', status: 2, lastReview: '12/03/2026', age: 25 },
    { id: 4, name: 'Diego Lima',   email: 'diego@email.com',  cellPhone: '47999990004', status: 1, lastReview: '10/03/2026', age: 25 },
    { id: 5, name: 'Eva Martins',  email: 'eva@email.com',    cellPhone: '47999990005', status: 1, lastReview: '08/03/2026', age: 25 },
  ];

  constructor(private http: HttpClient) {}

  getAll(): Observable<AlunoModel[]> {
    return this.http.get<AlunoModel[]>(`${this.apiUrl}/GetAll`);
  }

  getById(id: number): Observable<AlunoModel | undefined> {
    return this.http.get<AlunoModel>(`${this.apiUrl}/GetById/${id}`);
  }

  create(aluno: Omit<AlunoModel, 'id'>): Observable<AlunoModel> {
    const newAluno: AlunoModel = { ...aluno, id: this.mockData.length + 1 };
    this.http.post<AlunoModel>(`${this.apiUrl}/Create`, aluno).subscribe(response => {
      console.log(response);
    });
    return of(newAluno);
  }

  update(id: number, aluno: Partial<AlunoModel>): Observable<AlunoModel> {
    return this.http.put<void>(`${this.apiUrl}/UpdateStudent/${id}`, aluno).pipe(
      switchMap(() => this.http.get<AlunoModel>(`${this.apiUrl}/GetById/${id}`))
    );
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/DeleteStudent/${id}`);
  }
}
