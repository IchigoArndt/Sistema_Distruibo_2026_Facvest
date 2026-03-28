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
        path: 'alunos/novo',
        loadComponent: () => import('./presentation/pages/alunos/aluno-form/aluno-form').then(m => m.AlunoFormComponent)
      },
      {
        path: 'alunos/:id',
        loadComponent: () => import('./presentation/pages/alunos/aluno-detalhe/aluno-detalhe').then(m => m.AlunoDetalheComponent)
      },
      {
        path: 'alunos/:id/editar',
        loadComponent: () => import('./presentation/pages/alunos/aluno-form/aluno-form').then(m => m.AlunoFormComponent)
      },
      {
        path: 'avaliacoes',
        loadComponent: () => import('./presentation/pages/avaliacoes/avaliacoes').then(m => m.AvaliacoesComponent)
      },
      {
        path: 'avaliacoes/nova',
        loadComponent: () => import('./presentation/pages/avaliacoes/avaliacao-form/avaliacao-form').then(m => m.AvaliacaoFormComponent)
      },
      {
        path: 'avaliacoes/:id',
        loadComponent: () => import('./presentation/pages/avaliacoes/avaliacao-detalhe/avaliacao-detalhe').then(m => m.AvaliacaoDetalheComponent)
      },
      {
        path: 'avaliacoes/:id/editar',
        loadComponent: () => import('./presentation/pages/avaliacoes/avaliacao-form/avaliacao-form').then(m => m.AvaliacaoFormComponent)
      },
      {
        path: 'perfil',
        loadComponent: () => import('./presentation/pages/perfil/perfil').then(m => m.PerfilComponent)
      }
    ]
  },
  { path: '**', redirectTo: '' }
];
