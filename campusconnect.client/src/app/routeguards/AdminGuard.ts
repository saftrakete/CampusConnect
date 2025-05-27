import { Injectable } from "@angular/core";
import { AuthorizationService } from "../services/authorization.service";
import { CanActivate, Router } from "@angular/router";

@Injectable({providedIn: 'root'})
export class AdminGuard implements CanActivate {
    constructor(private authService: AuthorizationService, private router: Router) {}

    canActivate(): boolean {
        if (!this.authService.isLoggedIn()) {
            this.router.navigate(['forbidden']);
            return false;
        }

        const role = this.authService.getUserRole();

        if (!role) {
            console.log("role is null");
            return false;
        }

        if (role === "Admin") {
            return true;
        } else {
            this.router.navigate(['/forbidden']);
            return false;
        }
    }
}