import { Component, inject } from '@angular/core';
import { DeleteAccountComponent } from '../delete-account/delete-account.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-user-settings',
  templateUrl: './user-settings.component.html',
  styleUrl: './user-settings.component.scss'
})
export class UserSettingsComponent {
  readonly dialog = inject(MatDialog);    // Instanz um Dialogfenster zu öffnen

  /* Öffnet ein Fenster um den Löschvorgang zu bestätigen oder abzubrechen */
  deleteAccount() 
  {
    const dialogRef = this.dialog.open(DeleteAccountComponent);
  }
}
