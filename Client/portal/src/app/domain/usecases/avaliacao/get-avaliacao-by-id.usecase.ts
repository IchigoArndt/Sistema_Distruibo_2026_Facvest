import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Avaliacao } from '../../entities/avaliacao.entity';
import { AvaliacaoRepository } from '../../repositories/avaliacao.repository';

@Injectable({ providedIn: 'root' })
export class GetAvaliacaoByIdUseCase {
  constructor(private readonly avaliacaoRepository: AvaliacaoRepository) {}

  execute(id: number): Observable<Avaliacao> {
    return this.avaliacaoRepository.getById(id);
  }
}
