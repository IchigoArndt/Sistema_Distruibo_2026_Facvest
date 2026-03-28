import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { InputNumberModule } from 'primeng/inputnumber';
import { SelectModule } from 'primeng/select';
import { TextareaModule } from 'primeng/textarea';
import { DatePickerModule } from 'primeng/datepicker';
import { ToggleButtonModule } from 'primeng/togglebutton';
import { ToastModule } from 'primeng/toast';
import { DividerModule } from 'primeng/divider';
import { MessageService } from 'primeng/api';
import { Aluno } from '../../../../domain/entities/aluno.entity';
import { GetAlunosUseCase } from '../../../../domain/usecases/aluno/get-alunos.usecase';
import { CreateAvaliacaoUseCase } from '../../../../domain/usecases/avaliacao/create-avaliacao.usecase';
import { GetAvaliacaoByIdUseCase } from '../../../../domain/usecases/avaliacao/get-avaliacao-by-id.usecase';
import { UpdateAvaliacaoUseCase } from '../../../../domain/usecases/avaliacao/update-avaliacao.usecase';
import { TipoAvaliacao, ObjetivoAluno, NivelAtividade } from '../../../../domain/entities/avaliacao.entity';

@Component({
  selector: 'app-avaliacao-form',
  imports: [
    ReactiveFormsModule,
    RouterLink,
    ButtonModule,
    InputTextModule,
    InputNumberModule,
    SelectModule,
    TextareaModule,
    DatePickerModule,
    ToggleButtonModule,
    ToastModule,
    DividerModule,
  ],
  providers: [MessageService],
  templateUrl: './avaliacao-form.html',
  styleUrl: './avaliacao-form.scss',
})
export class AvaliacaoFormComponent implements OnInit {
  form!: FormGroup;
  isEditMode = false;
  avaliacaoId: number | null = null;
  loading = false;
  submitting = false;

  alunos: Aluno[] = [];
  hoje = new Date();

  tipoOptions: { label: string; value: TipoAvaliacao }[] = [
    { label: 'Básica',      value: 'Básica'      },
    { label: 'Completa',    value: 'Completa'    },
    { label: 'Reavaliação', value: 'Reavaliação' },
  ];

  objetivoOptions: { label: string; value: ObjetivoAluno }[] = [
    { label: 'Hipertrofia',     value: 'Hipertrofia'     },
    { label: 'Emagrecimento',   value: 'Emagrecimento'   },
    { label: 'Condicionamento', value: 'Condicionamento' },
    { label: 'Reabilitação',    value: 'Reabilitação'    },
  ];

  nivelAtividadeOptions: { label: string; value: NivelAtividade }[] = [
    { label: 'Sedentário',              value: 'Sedentário'              },
    { label: 'Levemente Ativo',         value: 'Levemente Ativo'         },
    { label: 'Moderadamente Ativo',     value: 'Moderadamente Ativo'     },
    { label: 'Muito Ativo',             value: 'Muito Ativo'             },
    { label: 'Atleta',                  value: 'Atleta'                  },
  ];

  constructor(
    private readonly fb: FormBuilder,
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly messageService: MessageService,
    private readonly getAlunosUseCase: GetAlunosUseCase,
    private readonly createAvaliacaoUseCase: CreateAvaliacaoUseCase,
    private readonly getAvaliacaoByIdUseCase: GetAvaliacaoByIdUseCase,
    private readonly updateAvaliacaoUseCase: UpdateAvaliacaoUseCase,
  ) {}

  ngOnInit(): void {
    this.buildForm();
    this.loadAlunos();

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.avaliacaoId = Number(id);
      this.loadAvaliacao(this.avaliacaoId);
    }
  }

  get tipoSelecionado(): TipoAvaliacao {
    return this.form.get('tipo')?.value;
  }

  get possuiLesao(): boolean {
    return !!this.form.get('anamnese.possuiLesao')?.value;
  }

  get usaMedicamento(): boolean {
    return !!this.form.get('anamnese.usaMedicamento')?.value;
  }

  private buildForm(): void {
    this.form = this.fb.group({
      alunoId:   [null, Validators.required],
      tipo:      ['Básica', Validators.required],
      objetivo:  [null, Validators.required],

      composicaoCorporal: this.fb.group({
        peso:          [null, [Validators.required, Validators.min(1)]],
        altura:        [null, [Validators.required, Validators.min(1)]],
        cintura:       [null],
        abdomen:       [null],
        quadril:       [null],
        torax:         [null],
        bracoDireito:  [null],
        bracoEsquerdo: [null],
        coxaDireita:   [null],
        coxaEsquerda:  [null],
      }),

      dobrasCutaneas: this.fb.group({
        peitoral:    [null],
        axilarMedia: [null],
        tricipital:  [null],
        subescapular:[null],
        supraIliaca: [null],
        abdominal:   [null],
        coxa:        [null],
      }),

      anamnese: this.fb.group({
        possuiLesao:          [false],
        descricaoLesao:       [''],
        usaMedicamento:       [false],
        descricaoMedicamento: [''],
        nivelAtividade:       ['Sedentário', Validators.required],
      }),

      parecerTecnico:   [''],
      proximaAvaliacao: [null],
    });
  }

  private loadAlunos(): void {
    this.getAlunosUseCase.execute().subscribe(alunos => {
      this.alunos = alunos;
    });
  }

  private loadAvaliacao(id: number): void {
    this.loading = true;
    this.getAvaliacaoByIdUseCase.execute(id).subscribe({
      next: (av) => {
        this.form.patchValue({
          alunoId:  av.alunoId,
          tipo:     av.tipo,
          objetivo: av.objetivo,
          composicaoCorporal:  av.composicaoCorporal  ?? {},
          dobrasCutaneas:      av.dobrasCutaneas      ?? {},
          anamnese:            av.anamnese            ?? {},
          parecerTecnico:      av.parecerTecnico      ?? '',
          proximaAvaliacao:    av.proximaAvaliacao
            ? this.parseDate(av.proximaAvaliacao)
            : null,
        });
        this.loading = false;
      },
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Avaliação não encontrada.' });
        this.loading = false;
        this.router.navigate(['/avaliacoes']);
      },
    });
  }

  private parseDate(dateStr: string): Date | null {
    const parts = dateStr.split('/');
    if (parts.length !== 3) return null;
    return new Date(Number(parts[2]), Number(parts[1]) - 1, Number(parts[0]));
  }

  private formatDate(date: Date | null): string | undefined {
    if (!date) return undefined;
    const d = String(date.getDate()).padStart(2, '0');
    const m = String(date.getMonth() + 1).padStart(2, '0');
    const y = date.getFullYear();
    return `${d}/${m}/${y}`;
  }

  calcularImc(): string | undefined {
    const cc = this.form.get('composicaoCorporal')?.value;
    if (!cc?.peso || !cc?.altura) return undefined;
    const alturaM = cc.altura / 100;
    return (cc.peso / (alturaM * alturaM)).toFixed(1);
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      this.messageService.add({ severity: 'warn', summary: 'Atenção', detail: 'Preencha todos os campos obrigatórios.' });
      return;
    }

    this.submitting = true;
    const raw = this.form.getRawValue();
    const alunoSelecionado = this.alunos.find(a => a.id === raw.alunoId);

    const payload = {
      alunoId:             raw.alunoId,
      alunoNome:           alunoSelecionado?.name ?? '',
      data:                this.formatDate(new Date()) ?? '',
      tipo:                raw.tipo,
      objetivo:            raw.objetivo,
      status:              'Concluída' as const,
      composicaoCorporal:  raw.composicaoCorporal,
      dobrasCutaneas:      raw.tipo === 'Completa' ? raw.dobrasCutaneas : undefined,
      anamnese:            raw.anamnese,
      parecerTecnico:      raw.parecerTecnico || undefined,
      proximaAvaliacao:    this.formatDate(raw.proximaAvaliacao),
      imc:                 this.calcularImc(),
    };

    const request$ = this.isEditMode && this.avaliacaoId !== null
      ? this.updateAvaliacaoUseCase.execute(this.avaliacaoId, payload)
      : this.createAvaliacaoUseCase.execute(payload);

    request$.subscribe({
      next: () => {
        this.messageService.add({
          severity: 'success',
          summary: 'Sucesso',
          detail: this.isEditMode ? 'Avaliação atualizada!' : 'Avaliação cadastrada!',
        });
        setTimeout(() => this.router.navigate(['/avaliacoes']), 1500);
      },
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Ocorreu um erro. Tente novamente.' });
        this.submitting = false;
      },
    });
  }

  hasError(path: string, error: string): boolean {
    const control = this.form.get(path);
    return !!(control?.hasError(error) && control.touched);
  }
}
