import { Component } from '@angular/core';
import { UserSettingsComponent } from '../user-settings/user-settings.component';

@Component({
  selector: 'app-account-settings',
  templateUrl: './account-settings.component.html',
  styleUrl: './account-settings.component.scss'
})
export class AccountSettingsComponent {
  buttonLabels = ['Kontoeinstellungen', 'Email-Einstellungen', 'Passwort']; 

  public handleButtonClick(label: String): void
  {

  }

}
