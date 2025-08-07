import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthorizationService } from '../services/authorization.service';

@Component({
  selector: 'app-user-settings',
  templateUrl: './user-settings.component.html',
  styleUrl: './user-settings.component.scss'
})
export class UserSettingsComponent {
  constructor(private authService: AuthorizationService) {
  }

  public logOutClick(): void {
    this.authService.logOut();
  }
}
