import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { Avaliacao } from '../../domain/entities/avaliacao.entity';
import { AvaliacaoRepository } from '../../domain/repositories/avaliacao.repository';
import { AvaliacaoDataSource } from '../datasources/avaliacao.datasource';
import { toAvaliacaoEntity, toAvaliacaoModel } from '../models/avaliacao.model';

@Injectable({ providedIn: 'root' })
export class AvaliacaoRepositoryImpl extends AvaliacaoRepository {
  constructor(private readonly dataSource: AvaliacaoDataSource) {
    super();
  }

  getAll(): Observable<Avaliacao[]> {
    return this.dataSource.getAll().pipe(
      map(models => models.map(toAvaliacaoEntity))
    );
  }

  getById(id: number): Observable<Avaliacao> {
    return this.dataSource.getById(id).pipe(
      map(model => toAvaliacaoEntity(model!))
    );
  }

  create(avaliacao: Omit<Avaliacao, 'id'>): Observable<Avaliacao> {
    return this.dataSource.create(toAvaliacaoModel(avaliacao)).pipe(
      map(toAvaliacaoEntity)
    );
  }

  update(id: number, avaliacao: Partial<Avaliacao>): Observable<Avaliacao> {
    return this.dataSource.update(id, avaliacao).pipe(
      map(toAvaliacaoEntity)
    );
  }

  delete(id: number): Observable<void> {
    return this.dataSource.delete(id);
  }
}
