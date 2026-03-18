import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Avaliacao } from '../../entities/avaliacao.entity';
import { AvaliacaoRepository } from '../../repositories/avaliacao.repository';

@Injectable({ providedIn: 'root' })
export class CreateAvaliacaoUseCase {
  constructor(private readonly avaliacaoRepository: AvaliacaoRepository) {}

  execute(avaliacao: Omit<Avaliacao, 'id'>): Observable<Avaliacao> {
    return this.avaliacaoRepository.create(avaliacao);
  }
}
