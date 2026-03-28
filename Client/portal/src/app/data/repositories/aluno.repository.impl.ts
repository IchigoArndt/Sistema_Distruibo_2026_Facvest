import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { Aluno } from '../../domain/entities/aluno.entity';
import { AlunoRepository } from '../../domain/repositories/aluno.repository';
import { AlunoDataSource } from '../datasources/aluno.datasource';
import { toAlunoEntity, toAlunoModel } from '../models/aluno.model';

@Injectable({ providedIn: 'root' })
export class AlunoRepositoryImpl extends AlunoRepository {
  constructor(private readonly dataSource: AlunoDataSource) {
    super();
  }

  getAll(): Observable<Aluno[]> {
    return this.dataSource.getAll().pipe(
      map(models => models.map(toAlunoEntity))
    );
  }

  getById(id: number): Observable<Aluno> {
    return this.dataSource.getById(id).pipe(
      map(model => toAlunoEntity(model!))
    );
  }

  create(aluno: Omit<Aluno, 'id'>): Observable<Aluno> {
    return this.dataSource.create(toAlunoModel(aluno)).pipe(
      map(toAlunoEntity)
    );
  }

  update(id: number, aluno: Partial<Aluno>): Observable<Aluno> {
    const partialModel = {
      ...aluno,
      status: aluno.status !== undefined
        ? (aluno.status === 'Ativo' ? 1 : 2)
        : undefined,
      cellPhone: aluno.cellPhone !== undefined
        ? aluno.cellPhone.replace(/\D/g, '')
        : undefined
    };
    return this.dataSource.update(id, partialModel).pipe(
      map(toAlunoEntity)
    );
  }

  delete(id: number): Observable<void> {
    return this.dataSource.delete(id);
  }
}
