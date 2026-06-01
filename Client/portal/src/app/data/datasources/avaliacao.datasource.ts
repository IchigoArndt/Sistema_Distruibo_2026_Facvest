import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AvaliacaoApiDTO } from '../models/avaliacao.model';
import { environment } from '../../../environment';

@Injectable({ providedIn: 'root' })
export class AvaliacaoDataSource {
  private readonly apiUrl = `${environment.apiUrl}/Avaliation`;

  constructor(private readonly http: HttpClient) {}

  getAll(): Observable<AvaliacaoApiDTO[]> {
    return this.http.get<AvaliacaoApiDTO[]>(`${this.apiUrl}/GetAll`);
  }

  getById(id: number): Observable<AvaliacaoApiDTO> {
    return this.http.get<AvaliacaoApiDTO>(`${this.apiUrl}/GetById/${id}`);
  }

  create(payload: Omit<AvaliacaoApiDTO, 'id' | 'studentName' | 'professionalName'>): Observable<AvaliacaoApiDTO> {
    return this.http.post<AvaliacaoApiDTO>(`${this.apiUrl}/Create`, payload);
  }

  update(id: number, payload: Partial<AvaliacaoApiDTO>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/Update/${id}`, payload);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/Delete/${id}`);
  }
}
