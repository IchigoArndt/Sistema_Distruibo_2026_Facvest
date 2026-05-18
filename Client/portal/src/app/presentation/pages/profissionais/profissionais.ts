import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { TagModule } from 'primeng/tag';
import { AvatarModule } from 'primeng/avatar';
import { InputTextModule } from 'primeng/inputtext';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { SkeletonModule } from 'primeng/skeleton';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { TooltipModule } from 'primeng/tooltip';
import { MessageService, ConfirmationService } from 'primeng/api';
import { Profissional } from '../../../domain/entities/profissional.entity';
import { GetProfissionaisUseCase } from '../../../domain/usecases/profissional/get-profissionais.usecase';
import { DeleteProfissionalUseCase } from '../../../domain/usecases/profissional/delete-profissional.usecase';

@Component({
  selector: 'app-profissionais',
  imports: [
    CommonModule,
    FormsModule,
    RouterLink,
    ButtonModule,
    TableModule,
    TagModule,
    AvatarModule,
    InputTextModule,
    IconFieldModule,
    InputIconModule,
    SkeletonModule,
    ToastModule,
    ConfirmDialogModule,
    TooltipModule,
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './profissionais.html',
  styleUrl: './profissionais.scss'
})
export class ProfissionaisComponent implements OnInit {
  loading = true;
  searchTerm = '';
  profissionais: Profissional[] = [];
  skeletonRows = [1, 2, 3, 4, 5];

  constructor(
    private readonly router: Router,
    private readonly messageService: MessageService,
    private readonly confirmationService: ConfirmationService,
    private readonly getProfissionaisUseCase: GetProfissionaisUseCase,
    private readonly deleteProfissionalUseCase: DeleteProfissionalUseCase,
  ) {}

  ngOnInit(): void {
    this.loadProfissionais();
  }

  private loadProfissionais(): void {
    this.loading = true;
    this.getProfissionaisUseCase.execute().subscribe({
      next: profissionais => {
        this.profissionais = profissionais;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  getAvatar(name: string): string {
    return name.charAt(0).toUpperCase();
  }

  getSeverity(status?: 'Ativo' | 'Inativo'): 'success' | 'danger' | undefined {
    return status === 'Ativo' ? 'success' : status === 'Inativo' ? 'danger' : undefined;
  }

  get filteredProfissionais(): Profissional[] {
    if (!this.searchTerm) return this.profissionais;
    const term = this.searchTerm.toLowerCase();
    return this.profissionais.filter(p =>
      p.nome.toLowerCase().includes(term) ||
      p.email.toLowerCase().includes(term) ||
      p.cref.toLowerCase().includes(term)
    );
  }

  confirmDelete(profissional: Profissional): void {
    this.confirmationService.confirm({
      message: `Deseja realmente excluir o profissional <strong>${profissional.nome}</strong>?`,
      header: 'Confirmar Exclusão',
      icon: 'pi pi-exclamation-triangle',
      acceptLabel: 'Excluir',
      rejectLabel: 'Cancelar',
      acceptButtonStyleClass: 'p-button-danger',
      accept: () => {
        this.deleteProfissionalUseCase.execute(profissional.id).subscribe({
          next: () => {
            this.messageService.add({ severity: 'success', summary: 'Sucesso', detail: 'Profissional excluído.' });
            this.loadProfissionais();
          },
          error: () => {
            this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Não foi possível excluir o profissional.' });
          }
        });
      }
    });
  }
}
