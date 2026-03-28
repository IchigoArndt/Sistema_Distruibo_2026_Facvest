import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AvaliacaoRepository } from '../../repositories/avaliacao.repository';

@Injectable({ providedIn: 'root' })
export class DeleteAvaliacaoUseCase {
  constructor(private readonly avaliacaoRepository: AvaliacaoRepository) {}

  execute(id: number): Observable<void> {
    return this.avaliacaoRepository.delete(id);
  }
}
