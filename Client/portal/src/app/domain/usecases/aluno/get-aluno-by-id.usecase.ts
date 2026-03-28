import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Aluno } from '../../entities/aluno.entity';
import { AlunoRepository } from '../../repositories/aluno.repository';

@Injectable({ providedIn: 'root' })
export class GetAlunoByIdUseCase {
  constructor(private readonly alunoRepository: AlunoRepository) {}

  execute(id: number): Observable<Aluno> {
    return this.alunoRepository.getById(id);
  }
}
