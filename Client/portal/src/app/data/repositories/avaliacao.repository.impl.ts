import { Injectable } from '@angular/core';
import { Observable, map, switchMap } from 'rxjs';
import { Avaliacao } from '../../domain/entities/avaliacao.entity';
import { AvaliacaoRepository } from '../../domain/repositories/avaliacao.repository';
import { AvaliacaoDataSource } from '../datasources/avaliacao.datasource';
import { toAvaliacaoEntity, toAvaliacaoApiPayload, toAvaliacaoEditPayload } from '../models/avaliacao.model';
import { AuthService } from '../../core/services/auth.service';

@Injectable({ providedIn: 'root' })
export class AvaliacaoRepositoryImpl extends AvaliacaoRepository {
  constructor(
    private readonly dataSource: AvaliacaoDataSource,
    private readonly authService: AuthService,
  ) {
    super();
  }

  getAll(): Observable<Avaliacao[]> {
    return this.dataSource.getAll().pipe(
      map(dtos => dtos.map(toAvaliacaoEntity))
    );
  }

  getById(id: number): Observable<Avaliacao> {
    return this.dataSource.getById(id).pipe(
      map(dto => toAvaliacaoEntity(dto))
    );
  }

  create(avaliacao: Omit<Avaliacao, 'id'>): Observable<Avaliacao> {
    const professionalId = this.authService.getEntityId() ?? 0;
    const payload = toAvaliacaoApiPayload(avaliacao, professionalId);
    return this.dataSource.create(payload).pipe(
      map(dto => toAvaliacaoEntity(dto))
    );
  }

  update(id: number, avaliacao: Partial<Avaliacao>): Observable<Avaliacao> {
    const payload = toAvaliacaoEditPayload(avaliacao);
    return this.dataSource.update(id, payload).pipe(
      switchMap(() => this.dataSource.getById(id)),
      map(dto => toAvaliacaoEntity(dto))
    );
  }

  delete(id: number): Observable<void> {
    return this.dataSource.delete(id);
  }
}
