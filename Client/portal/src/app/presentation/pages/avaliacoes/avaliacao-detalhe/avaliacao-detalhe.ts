import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { AvatarModule } from 'primeng/avatar';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DividerModule } from 'primeng/divider';
import { MessageService, ConfirmationService } from 'primeng/api';
import { Avaliacao } from '../../../../domain/entities/avaliacao.entity';
import { GetAvaliacaoByIdUseCase } from '../../../../domain/usecases/avaliacao/get-avaliacao-by-id.usecase';
import { DeleteAvaliacaoUseCase } from '../../../../domain/usecases/avaliacao/delete-avaliacao.usecase';

@Component({
  selector: 'app-avaliacao-detalhe',
  imports: [
    RouterLink,
    ButtonModule,
    TagModule,
    AvatarModule,
    ToastModule,
    ConfirmDialogModule,
    DividerModule,
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './avaliacao-detalhe.html',
  styleUrl: './avaliacao-detalhe.scss',
})
export class AvaliacaoDetalheComponent implements OnInit {
  avaliacao: Avaliacao | null = null;
  loading = true;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly messageService: MessageService,
    private readonly confirmationService: ConfirmationService,
    private readonly getAvaliacaoByIdUseCase: GetAvaliacaoByIdUseCase,
    private readonly deleteAvaliacaoUseCase: DeleteAvaliacaoUseCase,
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) { this.router.navigate(['/avaliacoes']); return; }
    this.loadAvaliacao(Number(id));
  }

  private loadAvaliacao(id: number): void {
    this.loading = true;
    this.getAvaliacaoByIdUseCase.execute(id).subscribe({
      next: (av) => { this.avaliacao = av; this.loading = false; },
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Avaliação não encontrada.' });
        this.loading = false;
        this.router.navigate(['/avaliacoes']);
      },
    });
  }

  getAvatar(nome: string): string {
    return nome.charAt(0).toUpperCase();
  }

  getSeverity(status: string): 'success' | 'warn' | undefined {
    return status === 'Concluída' ? 'success' : 'warn';
  }

  getImcClassificacao(imc: string): string {
    const v = parseFloat(imc);
    if (v < 18.5) return 'Abaixo do peso';
    if (v < 25)   return 'Peso normal';
    if (v < 30)   return 'Sobrepeso';
    if (v < 35)   return 'Obesidade grau I';
    if (v < 40)   return 'Obesidade grau II';
    return 'Obesidade grau III';
  }

  confirmDelete(): void {
    this.confirmationService.confirm({
      message: `Deseja realmente excluir esta avaliação de <strong>${this.avaliacao?.alunoNome}</strong>?`,
      header: 'Confirmar Exclusão',
      icon: 'pi pi-exclamation-triangle',
      acceptLabel: 'Excluir',
      rejectLabel: 'Cancelar',
      acceptButtonStyleClass: 'p-button-danger',
      accept: () => this.deleteAvaliacao(),
    });
  }

  private deleteAvaliacao(): void {
    if (!this.avaliacao) return;
    this.deleteAvaliacaoUseCase.execute(this.avaliacao.id).subscribe({
      next: () => {
        this.messageService.add({ severity: 'success', summary: 'Sucesso', detail: 'Avaliação excluída.' });
        setTimeout(() => this.router.navigate(['/avaliacoes']), 1500);
      },
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Não foi possível excluir.' });
      },
    });
  }
}
