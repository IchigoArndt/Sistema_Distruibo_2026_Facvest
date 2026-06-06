import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Profissional } from '../../entities/profissional.entity';
import { ProfissionalRepository } from '../../repositories/profissional.repository';

@Injectable({ providedIn: 'root' })
export class GetProfissionalMeUseCase {
  constructor(private readonly profissionalRepository: ProfissionalRepository) {}

  execute(): Observable<Profissional> {
    return this.profissionalRepository.getMe();
  }
}
