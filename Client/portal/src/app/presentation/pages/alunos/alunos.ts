import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { TagModule } from 'primeng/tag';
import { AvatarModule } from 'primeng/avatar';
import { InputTextModule } from 'primeng/inputtext';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { DialogModule } from 'primeng/dialog';
import { Aluno } from '../../../domain/entities/aluno.entity';
import { GetAlunosUseCase } from '../../../domain/usecases/aluno/get-alunos.usecase';

@Component({
  selector: 'app-alunos',
  imports: [
    FormsModule,
    ButtonModule,
    TableModule,
    TagModule,
    AvatarModule,
    InputTextModule,
    IconFieldModule,
    InputIconModule,
    DialogModule
  ],
  templateUrl: './alunos.html',
  styleUrl: './alunos.scss'
})
export class AlunosComponent implements OnInit {
  searchTerm = '';
  showDialog = false;
  alunos: Aluno[] = [];

  constructor(private readonly getAlunosUseCase: GetAlunosUseCase) {}

  ngOnInit(): void {
    this.getAlunosUseCase.execute().subscribe(alunos => {
      this.alunos = alunos;
    });
  }

  getAvatar(nome: string): string {
    return nome.charAt(0).toUpperCase();
  }

  getSeverity(status: string): 'success' | 'danger' | undefined {
    return status === 'Ativo' ? 'success' : 'danger';
  }

  get filteredAlunos(): Aluno[] {
    if (!this.searchTerm) return this.alunos;
    const term = this.searchTerm.toLowerCase();
    return this.alunos.filter(a =>
      a.nome.toLowerCase().includes(term) ||
      a.email.toLowerCase().includes(term)
    );
  }
}
