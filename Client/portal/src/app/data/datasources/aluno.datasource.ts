import { Injectable } from '@angular/core';
import { Observable, switchMap } from 'rxjs';
import { AlunoModel } from '../models/aluno.model';
import { environment } from '../../../environment';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../core/services/auth.service';

@Injectable({ providedIn: 'root' })
export class AlunoDataSource {

  private readonly apiUrl = `${environment.apiUrl}/Student`;

  constructor(
    private readonly http: HttpClient,
    private readonly authService: AuthService,
  ) {}

  getAll(): Observable<AlunoModel[]> {
    const role = this.authService.getRole();
    if (role === 'Professional') {
      return this.http.get<AlunoModel[]>(`${this.apiUrl}/GetAllStudentsByProfessionalId`);
    }
    return this.http.get<AlunoModel[]>(`${this.apiUrl}/GetAll`);
  }

  getById(id: number): Observable<AlunoModel> {
    return this.http.get<AlunoModel>(`${this.apiUrl}/GetById/${id}`);
  }

  create(aluno: Omit<AlunoModel, 'id'>): Observable<AlunoModel> {
    return this.http.post<AlunoModel>(`${this.apiUrl}/Create`, aluno);
  }

  update(id: number, aluno: Partial<AlunoModel>): Observable<AlunoModel> {
    return this.http.put<void>(`${this.apiUrl}/UpdateStudent/${id}`, aluno).pipe(
      switchMap(() => this.http.get<AlunoModel>(`${this.apiUrl}/GetById/${id}`))
    );
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/DeleteStudent/${id}`);
  }

  activate(id: number): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/ActivateStudent/${id}`, {});
  }
}
