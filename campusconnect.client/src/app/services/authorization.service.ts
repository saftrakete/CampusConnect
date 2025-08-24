import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';
import { UserRoleEntity } from '../entities/userRoleEntity';
import { Router } from '@angular/router';

export interface DecodedToken {
  exp: number;
  [key: string]: any;
}

@Injectable({
  providedIn: 'root'
})
export class AuthorizationService {
  constructor(private router: Router) { 
  }

  public setToken(token: string): void {
    localStorage.setItem("jwt", token);
  }

  public getToken(): string | null {
    return localStorage.getItem("jwt");
  }

  public logOut(): void {
    localStorage.removeItem("jwt");
    this.router.navigate(['/login']);
  }

  public isLoggedIn(): boolean {
    const token = this.getDecodedToken();
    return token !== null && token.exp > Date.now() / 1000;
  }

  public getUserRole(): string | null {
    let token = this.getDecodedToken();
    return token?.['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || null;
  }

  public getLoginName(): string | null {
    let token = this.getDecodedToken();
    return token?.["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"] || null;
  }


  private getDecodedToken(): DecodedToken | null {
    let token = localStorage.getItem("jwt");
    if (!token) return null;

    try {
      return jwtDecode<DecodedToken>(token);
    } catch(error) {
      console.error(error);
      return null;
    }
  }
}
