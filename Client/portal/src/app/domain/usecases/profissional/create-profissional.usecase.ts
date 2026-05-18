import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Profissional } from '../../entities/profissional.entity';
import { ProfissionalRepository } from '../../repositories/profissional.repository';

@Injectable({ providedIn: 'root' })
export class CreateProfissionalUseCase {
  constructor(private readonly profissionalRepository: ProfissionalRepository) {}

  execute(profissional: Omit<Profissional, 'id'>): Observable<Profissional> {
    return this.profissionalRepository.create(profissional);
  }
}
