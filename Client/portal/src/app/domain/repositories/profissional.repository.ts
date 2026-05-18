import { Observable } from 'rxjs';
import { Profissional } from '../entities/profissional.entity';

export abstract class ProfissionalRepository {
  abstract getAll(): Observable<Profissional[]>;
  abstract getById(id: number): Observable<Profissional>;
  abstract create(profissional: Omit<Profissional, 'id'>): Observable<Profissional>;
  abstract delete(id: number): Observable<void>;
}
