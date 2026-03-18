import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { CheckboxModule } from 'primeng/checkbox';
import { DividerModule } from 'primeng/divider';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  imports: [
    FormsModule,
    ButtonModule,
    InputTextModule,
    PasswordModule,
    CheckboxModule,
    DividerModule
  ],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class LoginComponent {
  email = '';
  password = '';
  rememberMe = false;
  loading = false;

  constructor(
    private readonly authService: AuthService,
    private readonly router: Router
  ) {}

  onLogin() {
    this.loading = true;
    const success = this.authService.login(this.email, this.password);
    this.loading = false;

    if (success) {
      this.router.navigate(['/dashboard']);
    }
  }
}
