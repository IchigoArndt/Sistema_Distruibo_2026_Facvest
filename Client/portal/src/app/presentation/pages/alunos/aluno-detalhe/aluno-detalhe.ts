import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { AvatarModule } from 'primeng/avatar';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { MessageService, ConfirmationService } from 'primeng/api';
import { Aluno } from '../../../../domain/entities/aluno.entity';
import { GetAlunoByIdUseCase } from '../../../../domain/usecases/aluno/get-aluno-by-id.usecase';
import { DeleteAlunoUseCase } from '../../../../domain/usecases/aluno/delete-aluno.usecase';

@Component({
  selector: 'app-aluno-detalhe',
  imports: [
    RouterLink,
    ButtonModule,
    TagModule,
    AvatarModule,
    ToastModule,
    ConfirmDialogModule,
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './aluno-detalhe.html',
  styleUrl: './aluno-detalhe.scss'
})
export class AlunoDetalheComponent implements OnInit {
  aluno: Aluno | null = null;
  loading = true;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly messageService: MessageService,
    private readonly confirmationService: ConfirmationService,
    private readonly getAlunoByIdUseCase: GetAlunoByIdUseCase,
    private readonly deleteAlunoUseCase: DeleteAlunoUseCase,
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) {
      this.router.navigate(['/alunos']);
      return;
    }
    this.loadAluno(Number(id));
  }

  private loadAluno(id: number): void {
    this.loading = true;
    this.getAlunoByIdUseCase.execute(id).subscribe({
      next: (aluno) => {
        this.aluno = aluno;
        this.loading = false;
      },
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Aluno não encontrado.' });
        this.loading = false;
        this.router.navigate(['/alunos']);
      }
    });
  }

  getAvatar(name: string): string {
    return name.charAt(0).toUpperCase();
  }

  getSeverity(status: string): 'success' | 'danger' | undefined {
    return status === 'Ativo' ? 'success' : 'danger';
  }

  confirmDelete(): void {
    this.confirmationService.confirm({
      message: `Deseja realmente excluir o aluno <strong>${this.aluno?.name}</strong>? Esta ação não pode ser desfeita.`,
      header: 'Confirmar Exclusão',
      icon: 'pi pi-exclamation-triangle',
      acceptLabel: 'Excluir',
      rejectLabel: 'Cancelar',
      acceptButtonStyleClass: 'p-button-danger',
      accept: () => this.deleteAluno(),
    });
  }

  private deleteAluno(): void {
    if (!this.aluno) return;
    this.deleteAlunoUseCase.execute(this.aluno.id).subscribe({
      next: () => {
        this.messageService.add({ severity: 'success', summary: 'Sucesso', detail: 'Aluno excluído com sucesso.' });
        setTimeout(() => this.router.navigate(['/alunos']), 1500);
      },
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Não foi possível excluir o aluno.' });
      }
    });
  }
}
