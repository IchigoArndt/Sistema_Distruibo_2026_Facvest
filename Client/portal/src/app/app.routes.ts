import { Routes } from '@angular/router';
import { LoginComponent } from './presentation/pages/login/login';
import { MainLayoutComponent } from './presentation/layout/main-layout/main-layout';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {
    path: '',
    component: MainLayoutComponent,
    canActivate: [authGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      {
        path: 'dashboard',
        loadComponent: () => import('./presentation/pages/dashboard/dashboard').then(m => m.DashboardComponent)
      },
      {
        path: 'alunos',
        loadComponent: () => import('./presentation/pages/alunos/alunos').then(m => m.AlunosComponent)
      },
      {
        path: 'avaliacoes',
        loadComponent: () => import('./presentation/pages/avaliacoes/avaliacoes').then(m => m.AvaliacoesComponent)
      },
      {
        path: 'perfil',
        loadComponent: () => import('./presentation/pages/perfil/perfil').then(m => m.PerfilComponent)
      }
    ]
  },
  { path: '**', redirectTo: '' }
];
