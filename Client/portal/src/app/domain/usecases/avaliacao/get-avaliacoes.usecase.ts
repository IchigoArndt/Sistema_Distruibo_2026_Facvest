import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Avaliacao } from '../../entities/avaliacao.entity';
import { AvaliacaoRepository } from '../../repositories/avaliacao.repository';

@Injectable({ providedIn: 'root' })
export class GetAvaliacoesUseCase {
  constructor(private readonly avaliacaoRepository: AvaliacaoRepository) {}

  execute(): Observable<Avaliacao[]> {
    return this.avaliacaoRepository.getAll();
  }
}
