import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ProfissionalModel } from '../models/profissional.model';
import { environment } from '../../../environment';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class ProfissionalDataSource {
  private apiUrl = `${environment.apiUrl}/Professional`;

  constructor(private http: HttpClient) {}

  getMe(): Observable<ProfissionalModel> {
    return this.http.get<ProfissionalModel>(`${this.apiUrl}/GetMe`);
  }

  getAll(): Observable<ProfissionalModel[]> {
    return this.http.get<ProfissionalModel[]>(`${this.apiUrl}/GetAll`);
  }

  getById(id: number): Observable<ProfissionalModel> {
    return this.http.get<ProfissionalModel>(`${this.apiUrl}/GetById/${id}`);
  }

  create(profissional: Omit<ProfissionalModel, 'id'>): Observable<ProfissionalModel> {
    return this.http.post<ProfissionalModel>(`${this.apiUrl}/Create`, profissional);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/Delete/${id}`);
  }

  update(id: number, profissional: Partial<ProfissionalModel>): Observable<ProfissionalModel> {
    return this.http.put<ProfissionalModel>(`${this.apiUrl}/Update/${id}`, profissional);
  }
}
