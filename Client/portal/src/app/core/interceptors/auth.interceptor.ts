import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';

function isJwt(token: string | null): boolean {
  if (!token) return false;
  const parts = token.split('.');
  return parts.length === 3;
}

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const token = localStorage.getItem('token');
  const router = inject(Router);

  // Não adicionar Authorization para requisições de login
  if (req.url.includes('/Auth/Login')) {
    return next(req);
  }

  let authReq = req;
  if (isJwt(token)) {
    authReq = req.clone({
      setHeaders: { Authorization: `Bearer ${token}` }
    });
  }

  return next(authReq).pipe(
    catchError((error) => {
      if (error.status === 401) {
        localStorage.clear();
        router.navigate(['/login']);
      }
      return throwError(() => error);
    })
  );
};
