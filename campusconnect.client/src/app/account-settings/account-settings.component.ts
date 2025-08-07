import { Component, inject } from '@angular/core';
import { DeleteAccountComponent } from '../delete-account/delete-account.component';
import { MatDialog } from '@angular/material/dialog';
import { EditUsernameFormComponent } from '../edit-username-form/edit-username-form.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-account-settings',
  templateUrl: './account-settings.component.html',
  styleUrl: './account-settings.component.scss'
})
export class AccountSettingsComponent {

  readonly dialog = inject(MatDialog);    // Instanz um Dialogfenster zu öffnen
  username: string = 'Saftrakete';

  /* Öffnet ein Fenster um den Löschvorgang zu bestätigen oder abzubrechen */
  openDeletionDialog() 
  {
    const dialogRef = this.dialog.open(DeleteAccountComponent);
  }

  openEditDialog()
  {
    const dialogRef = this.dialog.open(EditUsernameFormComponent);
  }

  openTwoFactorSetup()
  {
    this.router.navigate(['/two-factor-setup']);
  }

  constructor(private router: Router) {}

}
