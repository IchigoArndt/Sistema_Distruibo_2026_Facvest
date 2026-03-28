import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Avaliacao } from '../../entities/avaliacao.entity';
import { AvaliacaoRepository } from '../../repositories/avaliacao.repository';

@Injectable({ providedIn: 'root' })
export class UpdateAvaliacaoUseCase {
  constructor(private readonly avaliacaoRepository: AvaliacaoRepository) {}

  execute(id: number, avaliacao: Partial<Omit<Avaliacao, 'id'>>): Observable<Avaliacao> {
    return this.avaliacaoRepository.update(id, avaliacao);
  }
}
