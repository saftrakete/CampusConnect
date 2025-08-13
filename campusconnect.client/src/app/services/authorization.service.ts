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
    localStorage.setItem("jwt", token); //Anfällig für XSS-Angriffe
  }

  public logOut(): void {
    localStorage.removeItem("jwt");
    this.router.navigate(['/login']);
  }

  public isLoggedIn(): boolean {
    const token = this.getDecodedToken();
    
    if (!token) {
      return false;
    }

    return true;
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
