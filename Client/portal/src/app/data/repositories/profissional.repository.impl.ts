import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { Profissional } from '../../domain/entities/profissional.entity';
import { ProfissionalRepository } from '../../domain/repositories/profissional.repository';
import { ProfissionalDataSource } from '../datasources/profissional.datasource';
import { toProfissionalEntity, toProfissionalModel, ProfissionalModel } from '../models/profissional.model';

@Injectable({ providedIn: 'root' })
export class ProfissionalRepositoryImpl extends ProfissionalRepository {
  constructor(private readonly dataSource: ProfissionalDataSource) {
    super();
  }

  getMe(): Observable<Profissional> {
    return this.dataSource.getMe().pipe(
      map(toProfissionalEntity)
    );
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

  update(id: number, profissional: Partial<Profissional>): Observable<Profissional> {
    const model: Partial<ProfissionalModel> = {
      name: profissional.nome,
      email: profissional.email,
      phone: profissional.telefone,
      bio: profissional.bio,
      specialty: profissional.specialty,
      methodology: profissional.methodology,
      price: profissional.price,
      experience: profissional.experience,
    };
    return this.dataSource.update(id, model).pipe(map(toProfissionalEntity));
  }
}
