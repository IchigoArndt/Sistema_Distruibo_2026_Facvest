import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { AvatarModule } from 'primeng/avatar';
import { DividerModule } from 'primeng/divider';
import { Profissional } from '../../../domain/entities/profissional.entity';

@Component({
  selector: 'app-perfil',
  imports: [
    FormsModule,
    ButtonModule,
    InputTextModule,
    TextareaModule,
    AvatarModule,
    DividerModule
  ],
  templateUrl: './perfil.html',
  styleUrl: './perfil.scss'
})
export class PerfilComponent {
  profissional: Profissional = {
    id: 1,
    nome: 'João Profissional',
    email: 'joao@fitportal.com',
    telefone: '(47) 99999-0000',
    cref: 'CREF 123456-G/SC',
    bio: 'Profissional de educação física com especialização em avaliação corporal e treinamento funcional.'
  };

  saving = false;

  getAvatar(): string {
    return this.profissional.nome.charAt(0).toUpperCase();
  }

  save(): void {
    this.saving = true;
    // TODO: chamar use case de atualizar perfil
    setTimeout(() => (this.saving = false), 1200);
  }
}
