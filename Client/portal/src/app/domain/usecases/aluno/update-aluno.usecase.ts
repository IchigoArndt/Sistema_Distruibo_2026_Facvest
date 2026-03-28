import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Aluno } from '../../entities/aluno.entity';
import { AlunoRepository } from '../../repositories/aluno.repository';

@Injectable({ providedIn: 'root' })
export class UpdateAlunoUseCase {
  constructor(private readonly alunoRepository: AlunoRepository) {}

  execute(id: number, aluno: Partial<Omit<Aluno, 'id'>>): Observable<Aluno> {
    return this.alunoRepository.update(id, aluno);
  }
}
