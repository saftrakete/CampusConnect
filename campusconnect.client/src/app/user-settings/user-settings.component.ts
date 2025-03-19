import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-settings',
  templateUrl: './user-settings.component.html',
  styleUrl: './user-settings.component.scss'
})
export class UserSettingsComponent {
  router: Router;

  constructor(r: Router) 
  {
    this.router = r;
  }

  public openAccountSettings() 
  {
    this.router.navigate(['accountsettings']);
  }

}
