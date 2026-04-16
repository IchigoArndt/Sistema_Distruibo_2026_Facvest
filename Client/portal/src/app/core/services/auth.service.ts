import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private _isAuthenticated = false;

  constructor(
    private readonly http: HttpClient,
    private readonly router: Router
  ) {}

  login(email: string, password: string): Observable<boolean> {
    // TODO: substituir pela URL real da API de auth
    const loginUrl = 'http://localhost:5000/auth/login'; // Ajustar conforme necessário

    return this.http.post<{ token: string }>(loginUrl, { email, password }).pipe(
      map(response => {
        this._isAuthenticated = true;
        localStorage.setItem('token', response.token);
        return true;
      }),
      catchError(() => {
        this._isAuthenticated = false;
        return of(false);
      })
    );
  }

  logout(): void {
    this._isAuthenticated = false;
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }

  isLoggedIn(): boolean {
    return this._isAuthenticated || !!localStorage.getItem('token');
  }
}
