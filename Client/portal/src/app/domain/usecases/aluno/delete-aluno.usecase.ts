import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AlunoRepository } from '../../repositories/aluno.repository';

@Injectable({ providedIn: 'root' })
export class DeleteAlunoUseCase {
  constructor(private readonly alunoRepository: AlunoRepository) {}

  execute(id: number): Observable<void> {
    return this.alunoRepository.delete(id);
  }
}
