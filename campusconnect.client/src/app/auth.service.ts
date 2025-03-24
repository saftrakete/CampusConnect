import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class AuthService {

    constructor() { }

    isLoggedIn(): boolean {
        console.log(localStorage.getItem('token')+"<<-- token")
        return !!localStorage.getItem('token') && localStorage.getItem('token') != '-1';
    }
    /* change login to here maybe in the future?
    login(token: string) {
        localStorage.setItem('token', token);
    }

    logout() {
        localStorage.removeItem('token');
    }*/
}
