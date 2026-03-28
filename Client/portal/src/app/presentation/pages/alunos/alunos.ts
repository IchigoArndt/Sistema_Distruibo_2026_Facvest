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
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { TooltipModule } from 'primeng/tooltip';
import { MessageService, ConfirmationService } from 'primeng/api';
import { Aluno } from '../../../domain/entities/aluno.entity';
import { GetAlunosUseCase } from '../../../domain/usecases/aluno/get-alunos.usecase';
import { DeleteAlunoUseCase } from '../../../domain/usecases/aluno/delete-aluno.usecase';

@Component({
  selector: 'app-alunos',
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
    ToastModule,
    ConfirmDialogModule,
    TooltipModule,
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './alunos.html',
  styleUrl: './alunos.scss'
})
export class AlunosComponent implements OnInit {
  searchTerm = '';
  alunos: Aluno[] = [];

  constructor(
    private readonly router: Router,
    private readonly messageService: MessageService,
    private readonly confirmationService: ConfirmationService,
    private readonly getAlunosUseCase: GetAlunosUseCase,
    private readonly deleteAlunoUseCase: DeleteAlunoUseCase,
  ) {}

  ngOnInit(): void {
    this.loadAlunos();
  }

  private loadAlunos(): void {
    this.getAlunosUseCase.execute().subscribe(alunos => {
      this.alunos = alunos;
    });
  }

  getAvatar(name: string): string {
    return name.charAt(0).toUpperCase();
  }

  getSeverity(status: string): 'success' | 'danger' | undefined {
    return status === 'Ativo' ? 'success' : 'danger';
  }

  get filteredAlunos(): Aluno[] {
    if (!this.searchTerm) return this.alunos;
    const term = this.searchTerm.toLowerCase();
    return this.alunos.filter(a =>
      a.name.toLowerCase().includes(term) ||
      a.email.toLowerCase().includes(term)
    );
  }

  verDetalhe(aluno: Aluno): void {
    this.router.navigate(['/alunos', aluno.id]);
  }

  editar(aluno: Aluno): void {
    this.router.navigate(['/alunos', aluno.id, 'editar']);
  }

  confirmDelete(aluno: Aluno): void {
    this.confirmationService.confirm({
      message: `Deseja realmente excluir o aluno <strong>${aluno.name}</strong>?`,
      header: 'Confirmar Exclusão',
      icon: 'pi pi-exclamation-triangle',
      acceptLabel: 'Excluir',
      rejectLabel: 'Cancelar',
      acceptButtonStyleClass: 'p-button-danger',
      accept: () => {
        this.deleteAlunoUseCase.execute(aluno.id).subscribe({
          next: () => {
            this.messageService.add({ severity: 'success', summary: 'Sucesso', detail: 'Aluno excluído.' });
            this.loadAlunos();
          },
          error: () => {
            this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Não foi possível excluir o aluno.' });
          }
        });
      }
    });
  }
}
