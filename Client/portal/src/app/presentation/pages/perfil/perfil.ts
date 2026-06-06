import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { AvatarModule } from 'primeng/avatar';
import { DividerModule } from 'primeng/divider';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { Profissional } from '../../../domain/entities/profissional.entity';
import { GetProfissionalMeUseCase } from '../../../domain/usecases/profissional/get-profissional-me.usecase';
import { UpdateProfissionalUseCase } from '../../../domain/usecases/profissional/update-profissional.usecase';

@Component({
  selector: 'app-perfil',
  imports: [
    FormsModule, ButtonModule, InputTextModule,
    TextareaModule, AvatarModule, DividerModule, ProgressSpinnerModule
  ],
  templateUrl: './perfil.html',
  styleUrl: './perfil.scss'
})
export class PerfilComponent implements OnInit {
  profissional: Profissional = { id: 0, nome: '', email: '', telefone: '', cref: '' };
  loading = true;
  saving = false;
  error: string | null = null;

  constructor(
    private readonly getProfissionalMeUseCase: GetProfissionalMeUseCase,
    private readonly updateProfissionalUseCase: UpdateProfissionalUseCase
  ) {}

  ngOnInit(): void {
    this.getProfissionalMeUseCase.execute().subscribe({
      next: (profissional) => {
        this.profissional = profissional;
        this.loading = false;
      },
      error: () => {
        this.error = 'Erro ao carregar perfil.';
        this.loading = false;
      }
    });
  }

  getAvatar(): string {
    return this.profissional.nome?.charAt(0).toUpperCase() ?? '?';
  }

  save(): void {
    this.saving = true;
    this.updateProfissionalUseCase.execute(this.profissional.id, this.profissional).subscribe({
      next: (updated) => {
        this.profissional = updated;
        this.saving = false;
      },
      error: () => {
        this.saving = false;
      }
    });
  }
}
