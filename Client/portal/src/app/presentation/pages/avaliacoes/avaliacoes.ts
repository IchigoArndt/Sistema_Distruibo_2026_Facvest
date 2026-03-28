import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { TagModule } from 'primeng/tag';
import { AvatarModule } from 'primeng/avatar';
import { InputTextModule } from 'primeng/inputtext';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { SelectModule } from 'primeng/select';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { MessageService, ConfirmationService } from 'primeng/api';
import { Avaliacao } from '../../../domain/entities/avaliacao.entity';
import { GetAvaliacoesUseCase } from '../../../domain/usecases/avaliacao/get-avaliacoes.usecase';
import { DeleteAvaliacaoUseCase } from '../../../domain/usecases/avaliacao/delete-avaliacao.usecase';

@Component({
  selector: 'app-avaliacoes',
  imports: [
    FormsModule,
    RouterLink,
    ButtonModule,
    TableModule,
    TagModule,
    AvatarModule,
    InputTextModule,
    IconFieldModule,
    InputIconModule,
    SelectModule,
    ToastModule,
    ConfirmDialogModule,
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './avaliacoes.html',
  styleUrl: './avaliacoes.scss'
})
export class AvaliacoesComponent implements OnInit {
  searchTerm = '';
  statusFilter: string | null = null;
  avaliacoes: Avaliacao[] = [];

  statusOptions = [
    { label: 'Todos',     value: null        },
    { label: 'Concluída', value: 'Concluída' },
    { label: 'Pendente',  value: 'Pendente'  },
  ];

  constructor(
    private readonly router: Router,
    private readonly messageService: MessageService,
    private readonly confirmationService: ConfirmationService,
    private readonly getAvaliacoesUseCase: GetAvaliacoesUseCase,
    private readonly deleteAvaliacaoUseCase: DeleteAvaliacaoUseCase,
  ) {}

  ngOnInit(): void {
    this.loadAvaliacoes();
  }

  private loadAvaliacoes(): void {
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

  verDetalhe(av: Avaliacao): void {
    this.router.navigate(['/avaliacoes', av.id]);
  }

  editar(av: Avaliacao): void {
    this.router.navigate(['/avaliacoes', av.id, 'editar']);
  }

  confirmDelete(av: Avaliacao): void {
    this.confirmationService.confirm({
      message: `Deseja excluir a avaliação de <strong>${av.alunoNome}</strong> (${av.data})?`,
      header: 'Confirmar Exclusão',
      icon: 'pi pi-exclamation-triangle',
      acceptLabel: 'Excluir',
      rejectLabel: 'Cancelar',
      acceptButtonStyleClass: 'p-button-danger',
      accept: () => {
        this.deleteAvaliacaoUseCase.execute(av.id).subscribe({
          next: () => {
            this.messageService.add({ severity: 'success', summary: 'Sucesso', detail: 'Avaliação excluída.' });
            this.loadAvaliacoes();
          },
          error: () => {
            this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Não foi possível excluir.' });
          },
        });
      },
    });
  }
}
