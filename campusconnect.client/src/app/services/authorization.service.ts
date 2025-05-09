import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';
import { UserRoleEntity } from '../entities/userRoleEntity';

@Injectable({
  providedIn: 'root'
})
export class AuthorizationService {
  constructor() { }

  public setToken(token: string): void {
    localStorage.setItem("jwt", token); //Anfällig für XSS-Angriffe
  }

  public isLoggedIn(): boolean {
    const token = this.getToken();

    if (!token) {
      return false;
    }

    try {
      const decoded: any = jwtDecode(token);
      const exp = decoded.exp;
      const now = Math.floor(Date.now() / 1000);

      return exp > now;
    } catch {
      return false;
    }
  }

  public getUserRole(): UserRoleEntity | null {
    const token = this.getToken();
    console.log(token);

    if (!token) {
      return null;
    }

    try {
      const decoded: any = jwtDecode(token);
      console.log(decoded);
      return decoded.role; //TODO: Fix AdminGuard
    } catch {
      return null;
    }
  }

  private getToken(): string | null {
    return localStorage.getItem("jwt");
  }
}
