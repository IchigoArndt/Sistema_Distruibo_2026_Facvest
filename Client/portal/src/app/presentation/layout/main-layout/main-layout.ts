import { Component, signal } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { AvatarModule } from 'primeng/avatar';
import { BadgeModule } from 'primeng/badge';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-main-layout',
  imports: [
    RouterOutlet,
    RouterLink,
    RouterLinkActive,
    AvatarModule,
    BadgeModule,
    ButtonModule,
    RippleModule,
    ToastModule
  ],
  providers: [MessageService],
  templateUrl: './main-layout.html',
  styleUrl: './main-layout.scss'
})
export class MainLayoutComponent {
  sidebarCollapsed = signal(false);

  navItems = [
    { label: 'Dashboard',  icon: 'pi pi-home',      route: '/dashboard'  },
    { label: 'Alunos',     icon: 'pi pi-users',     route: '/alunos'     },
    { label: 'Avaliações', icon: 'pi pi-clipboard', route: '/avaliacoes' },
    { label: 'Meu Perfil', icon: 'pi pi-user',      route: '/perfil'     },
  ];

  constructor(private readonly authService: AuthService) {}

  toggleSidebar(): void {
    this.sidebarCollapsed.update(v => !v);
  }

  logout(): void {
    this.authService.logout();
  }
}
