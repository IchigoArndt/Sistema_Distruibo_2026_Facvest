import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { CreateAlunoUseCase } from '../../../../domain/usecases/aluno/create-aluno.usecase';
import { GetAlunoByIdUseCase } from '../../../../domain/usecases/aluno/get-aluno-by-id.usecase';
import { UpdateAlunoUseCase } from '../../../../domain/usecases/aluno/update-aluno.usecase';

@Component({
  selector: 'app-aluno-form',
  imports: [
    ReactiveFormsModule,
    RouterLink,
    ButtonModule,
    InputTextModule,
    SelectModule,
    ToastModule,
  ],
  providers: [MessageService],
  templateUrl: './aluno-form.html',
  styleUrl: './aluno-form.scss'
})
export class AlunoFormComponent implements OnInit {
  form!: FormGroup;
  isEditMode = false;
  alunoId: number | null = null;
  loading = false;
  submitting = false;

  statusOptions = [
    { label: 'Ativo',   value: 'Ativo'   },
    { label: 'Inativo', value: 'Inativo' },
  ];

  constructor(
    private readonly fb: FormBuilder,
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly messageService: MessageService,
    private readonly createAlunoUseCase: CreateAlunoUseCase,
    private readonly getAlunoByIdUseCase: GetAlunoByIdUseCase,
    private readonly updateAlunoUseCase: UpdateAlunoUseCase,
  ) {}

  ngOnInit(): void {
    this.buildForm();

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.alunoId = Number(id);
      this.loadAluno(this.alunoId);
    }
  }

  private buildForm(): void {
    this.form = this.fb.group({
      nome:     ['', [Validators.required, Validators.minLength(3)]],
      email:    ['', [Validators.required, Validators.email]],
      telefone: ['', [Validators.required, Validators.pattern(/^\(\d{2}\)\s\d{4,5}-\d{4}$/)]],
      status:   ['Ativo', Validators.required],
    });
  }

  private loadAluno(id: number): void {
    this.loading = true;
    this.getAlunoByIdUseCase.execute(id).subscribe({
      next: (aluno) => {
        this.form.patchValue({
          nome:     aluno.nome,
          email:    aluno.email,
          telefone: aluno.telefone,
          status:   aluno.status,
        });
        this.loading = false;
      },
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Aluno não encontrado.' });
        this.loading = false;
        this.router.navigate(['/alunos']);
      }
    });
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.submitting = true;
    const payload = this.form.getRawValue();

    const request$ = this.isEditMode && this.alunoId !== null
      ? this.updateAlunoUseCase.execute(this.alunoId, payload)
      : this.createAlunoUseCase.execute(payload);

    request$.subscribe({
      next: () => {
        this.messageService.add({
          severity: 'success',
          summary: 'Sucesso',
          detail: this.isEditMode ? 'Aluno atualizado com sucesso!' : 'Aluno cadastrado com sucesso!',
        });
        setTimeout(() => this.router.navigate(['/alunos']), 1500);
      },
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Ocorreu um erro. Tente novamente.' });
        this.submitting = false;
      }
    });
  }

  hasError(field: string, error: string): boolean {
    const control = this.form.get(field);
    return !!(control?.hasError(error) && control.touched);
  }

  formatTelefone(event: Event): void {
    const input = event.target as HTMLInputElement;
    let value = input.value.replace(/\D/g, '');
    if (value.length > 11) value = value.slice(0, 11);

    if (value.length > 6) {
      value = `(${value.slice(0, 2)}) ${value.slice(2, 7)}-${value.slice(7)}`;
    } else if (value.length > 2) {
      value = `(${value.slice(0, 2)}) ${value.slice(2)}`;
    } else if (value.length > 0) {
      value = `(${value}`;
    }

    this.form.get('telefone')!.setValue(value, { emitEvent: false });
    input.value = value;
  }
}
