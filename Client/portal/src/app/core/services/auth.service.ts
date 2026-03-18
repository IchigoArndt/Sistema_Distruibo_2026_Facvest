import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private _isAuthenticated = false;

  constructor(private readonly router: Router) {}

  login(email: string, _password: string): boolean {
    // TODO: substituir por chamada HTTP real
    this._isAuthenticated = true;
    localStorage.setItem('token', btoa(email));
    return true;
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
