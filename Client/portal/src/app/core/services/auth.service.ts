import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

const TOKEN_KEY = 'token';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private _isAuthenticated = false;
  private _token: string | null = null;

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
        this._token = response.token;
        this.setToken(response.token);
        return true;
      }),
      catchError(() => {
        this.clearAuthState();
        return of(false);
      })
    );
  }

  logout(): void {
    this.clearAuthState();
    this.router.navigate(['/login']);
  }

  isLoggedIn(): boolean {
    return this._isAuthenticated || !!this._token || !!this.getToken();
  }

  private setToken(token: string): void {
    localStorage.setItem(TOKEN_KEY, token);
    sessionStorage.removeItem(TOKEN_KEY);
  }

  private getToken(): string | null {
    return localStorage.getItem(TOKEN_KEY) ?? sessionStorage.getItem(TOKEN_KEY);
  }

  private clearAuthState(): void {
    this._isAuthenticated = false;
    this._token = null;
    localStorage.removeItem(TOKEN_KEY);
    sessionStorage.removeItem(TOKEN_KEY);
  }
}
