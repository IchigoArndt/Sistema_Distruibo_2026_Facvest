import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { AvatarModule } from 'primeng/avatar';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { CreateProfissionalUseCase } from '../../../../domain/usecases/profissional/create-profissional.usecase';

@Component({
  selector: 'app-profissional-form',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    ButtonModule,
    InputTextModule,
    TextareaModule,
    AvatarModule,
    ToastModule,
  ],
  providers: [MessageService],
  templateUrl: './profissional-form.html',
  styleUrl: './profissional-form.scss'
})
export class ProfissionalFormComponent implements OnInit {
  form!: FormGroup;
  submitting = false;

  constructor(
    private readonly fb: FormBuilder,
    private readonly router: Router,
    private readonly messageService: MessageService,
    private readonly createProfissionalUseCase: CreateProfissionalUseCase,
  ) {}

  ngOnInit(): void {
    this.buildForm();
  }

  private buildForm(): void {
    this.form = this.fb.group({
      nome: ['', [Validators.required, Validators.minLength(3)]],
      cref: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      telefone: ['', [Validators.required, Validators.pattern(/^\(\d{2}\)\s\d{4,5}-\d{4}$/)]],
      bio: [''],
      senha: ['', [Validators.required, Validators.minLength(8)]],
      confirmacaoSenha: ['', [Validators.required]],
    }, {
      validators: this.passwordsMatchValidator
    });
  }

  get nome(): string {
    return this.form.get('nome')?.value ?? '';
  }

  getAvatar(): string {
    return this.nome.charAt(0).toUpperCase();
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.submitting = true;
    const { nome, cref, email, telefone, bio, senha } = this.form.getRawValue();

    this.createProfissionalUseCase.execute({
      nome,
      cref,
      email,
      telefone,
      bio,
      senha,
    }).subscribe({
      next: () => {
        this.messageService.add({ severity: 'success', summary: 'Sucesso', detail: 'Profissional cadastrado com sucesso.' });
        this.router.navigate(['/profissionais']);
      },
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Não foi possível cadastrar o profissional.' });
        this.submitting = false;
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/profissionais']);
  }

  hasError(field: string, error: string): boolean {
    const control = this.form.get(field);
    return !!(control?.hasError(error) && control.touched);
  }

  get confirmacaoSenhaError(): boolean {
    const control = this.form.get('confirmacaoSenha');
    return !!(control?.hasError('mismatch') && control.touched);
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

  private passwordsMatchValidator(group: FormGroup): null | object {
    const senha = group.get('senha')?.value;
    const confirmacaoSenhaControl = group.get('confirmacaoSenha');

    if (senha && confirmacaoSenhaControl?.value && senha !== confirmacaoSenhaControl.value) {
      confirmacaoSenhaControl.setErrors({ ...confirmacaoSenhaControl.errors, mismatch: true });
      return { mismatch: true };
    }

    if (confirmacaoSenhaControl?.errors?.['mismatch']) {
      const { mismatch, ...remainingErrors } = confirmacaoSenhaControl.errors;
      if (Object.keys(remainingErrors).length) {
        confirmacaoSenhaControl.setErrors(remainingErrors);
      } else {
        confirmacaoSenhaControl.setErrors(null);
      }
    }

    return null;
  }
}
