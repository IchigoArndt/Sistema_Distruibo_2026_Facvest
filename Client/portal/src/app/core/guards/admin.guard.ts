import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { MessageService } from 'primeng/api';

function parseJwt(token: string | null): any {
  if (!token) return null;
  const parts = token.split('.');
  if (parts.length !== 3) return null;

  try {
    return JSON.parse(atob(parts[1]));
  } catch {
    return null;
  }
}

function isAdminToken(token: string | null): boolean {
  const payload = parseJwt(token);
  if (!payload) return false;

  const roles = payload.role ?? payload.roles ?? payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
  if (!roles) return false;
  if (Array.isArray(roles)) {
    return roles.includes('Admin');
  }
  return roles === 'Admin';
}

export const adminGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const messageService = inject(MessageService, { optional: true });
  const token = localStorage.getItem('token');

  if (authService.isLoggedIn() && isAdminToken(token)) {
    return true;
  }

  try {
    messageService?.add({ severity: 'error', summary: 'Acesso negado', detail: 'Apenas administradores podem acessar esta área.' });
  } catch {
    // ignore if message service not available
  }

  return router.createUrlTree(['/dashboard']);
};
