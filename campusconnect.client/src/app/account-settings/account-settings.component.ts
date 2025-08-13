import { Component, inject, OnInit } from '@angular/core';
import { DeleteAccountComponent } from '../delete-account/delete-account.component';
import { MatDialog } from '@angular/material/dialog';
import { EditUsernameFormComponent } from '../edit-username-form/edit-username-form.component';
import { UserService } from '../services/user.service';
import { AuthorizationService } from '../services/authorization.service';

@Component({
  selector: 'app-account-settings',
  templateUrl: './account-settings.component.html',
  styleUrl: './account-settings.component.scss'
})
export class AccountSettingsComponent implements OnInit {

  readonly dialog = inject(MatDialog);    // Instanz um Dialogfenster zu öffnen
  username: string = '';

  constructor(public us: UserService, public auth: AuthorizationService) {}

  ngOnInit(): void 
  {
    this.us.getUsername(this.auth.).subscribe(
      (response) => {
        this.username = response;
      }
    )
  }

  /* Öffnet ein Fenster um den Löschvorgang zu bestätigen oder abzubrechen */
  openDeletionDialog() 
  {
    const dialogRef = this.dialog.open(DeleteAccountComponent);
  }

  openEditDialog()
  {
    const dialogRef = this.dialog.open(EditUsernameFormComponent);
  }

}
