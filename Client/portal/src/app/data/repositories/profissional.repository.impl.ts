import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { Profissional } from '../../domain/entities/profissional.entity';
import { ProfissionalRepository } from '../../domain/repositories/profissional.repository';
import { ProfissionalDataSource } from '../datasources/profissional.datasource';
import { toProfissionalEntity, toProfissionalModel } from '../models/profissional.model';

@Injectable({ providedIn: 'root' })
export class ProfissionalRepositoryImpl extends ProfissionalRepository {
  constructor(private readonly dataSource: ProfissionalDataSource) {
    super();
  }

  getAll(): Observable<Profissional[]> {
    return this.dataSource.getAll().pipe(
      map(models => models.map(toProfissionalEntity))
    );
  }

  getById(id: number): Observable<Profissional> {
    return this.dataSource.getById(id).pipe(
      map(toProfissionalEntity)
    );
  }

  create(profissional: Omit<Profissional, 'id'>): Observable<Profissional> {
    return this.dataSource.create(toProfissionalModel(profissional)).pipe(
      map(toProfissionalEntity)
    );
  }

  delete(id: number): Observable<void> {
    return this.dataSource.delete(id);
  }
}
