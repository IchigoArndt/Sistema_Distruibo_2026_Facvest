import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { TagModule } from 'primeng/tag';
import { AvatarModule } from 'primeng/avatar';
import { InputTextModule } from 'primeng/inputtext';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { SelectModule } from 'primeng/select';
import { DialogModule } from 'primeng/dialog';
import { Avaliacao } from '../../../domain/entities/avaliacao.entity';
import { GetAvaliacoesUseCase } from '../../../domain/usecases/avaliacao/get-avaliacoes.usecase';

@Component({
  selector: 'app-avaliacoes',
  imports: [
    FormsModule,
    ButtonModule,
    TableModule,
    TagModule,
    AvatarModule,
    InputTextModule,
    IconFieldModule,
    InputIconModule,
    SelectModule,
    DialogModule
  ],
  templateUrl: './avaliacoes.html',
  styleUrl: './avaliacoes.scss'
})
export class AvaliacoesComponent implements OnInit {
  searchTerm = '';
  statusFilter: string | null = null;
  showDialog = false;
  avaliacoes: Avaliacao[] = [];

  statusOptions = [
    { label: 'Todos',     value: null         },
    { label: 'Concluída', value: 'Concluída'  },
    { label: 'Pendente',  value: 'Pendente'   },
  ];

  constructor(private readonly getAvaliacoesUseCase: GetAvaliacoesUseCase) {}

  ngOnInit(): void {
    this.getAvaliacoesUseCase.execute().subscribe(avaliacoes => {
      this.avaliacoes = avaliacoes;
    });
  }

  getAvatar(nome: string): string {
    return nome.charAt(0).toUpperCase();
  }

  getSeverity(status: string): 'success' | 'warn' | undefined {
    return status === 'Concluída' ? 'success' : 'warn';
  }

  get filteredAvaliacoes(): Avaliacao[] {
    return this.avaliacoes.filter(a => {
      const matchSearch = !this.searchTerm ||
        a.alunoNome.toLowerCase().includes(this.searchTerm.toLowerCase());
      const matchStatus = !this.statusFilter || a.status === this.statusFilter;
      return matchSearch && matchStatus;
    });
  }
}
