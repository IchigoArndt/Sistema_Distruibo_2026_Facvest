import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Aluno } from '../../entities/aluno.entity';
import { AlunoRepository } from '../../repositories/aluno.repository';

@Injectable({ providedIn: 'root' })
export class GetAlunosUseCase {
  constructor(private readonly alunoRepository: AlunoRepository) {}

  execute(): Observable<Aluno[]> {
    return this.alunoRepository.getAll();
  }
}
