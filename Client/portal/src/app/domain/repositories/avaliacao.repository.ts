import { Observable } from 'rxjs';
import { Avaliacao } from '../entities/avaliacao.entity';

export abstract class AvaliacaoRepository {
  abstract getAll(): Observable<Avaliacao[]>;
  abstract getById(id: number): Observable<Avaliacao>;
  abstract create(avaliacao: Omit<Avaliacao, 'id'>): Observable<Avaliacao>;
  abstract update(id: number, avaliacao: Partial<Avaliacao>): Observable<Avaliacao>;
  abstract delete(id: number): Observable<void>;
}
