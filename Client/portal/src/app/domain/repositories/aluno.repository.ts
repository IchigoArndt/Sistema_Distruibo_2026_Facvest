import { Observable } from 'rxjs';
import { Aluno } from '../entities/aluno.entity';

export abstract class AlunoRepository {
  abstract getAll(): Observable<Aluno[]>;
  abstract getById(id: number): Observable<Aluno>;
  abstract create(aluno: Omit<Aluno, 'id'>): Observable<Aluno>;
  abstract update(id: number, aluno: Partial<Aluno>): Observable<Aluno>;
  abstract delete(id: number): Observable<void>;
}
