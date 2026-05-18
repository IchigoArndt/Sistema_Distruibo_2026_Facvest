import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ProfissionalRepository } from '../../repositories/profissional.repository';

@Injectable({ providedIn: 'root' })
export class DeleteProfissionalUseCase {
  constructor(private readonly profissionalRepository: ProfissionalRepository) {}

  execute(id: number): Observable<void> {
    return this.profissionalRepository.delete(id);
  }
}
