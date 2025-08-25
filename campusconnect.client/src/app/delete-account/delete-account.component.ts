import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { UserService } from '../services/user.service';
import { Router } from '@angular/router';
import { baseApiRoute } from '../app-routing.module';
import { AuthorizationService } from '../services/authorization.service';

@Component({
  selector: 'app-delete-account',
  templateUrl: './delete-account.component.html',
  styleUrl: './delete-account.component.scss'
})
export class DeleteAccountComponent {

  dialog: MatDialogRef<DeleteAccountComponent>;   // speichert das geöffnete Dialogfenster
  router: Router;                                 // Router-Instanz zum Navigieren zur Homepage nach Löschvorgang

  /* bekommt das geöffnete Dialogfenster übergeben */
  constructor(
    private dialogRef: MatDialogRef<DeleteAccountComponent>, 
    private us: UserService,
    private r: Router,
    private auth: AuthorizationService
  ) 
  {
    this.dialog = dialogRef;
    this.router = r;
  }
  /* Schließt das Dialogfenster */
  closeDialog() 
  {
    this.dialog.close();
  }

  confirmDeletion() 
  {
    const loginName = this.auth.getUserName();

    if (loginName) {
      this.us.deleteUser(loginName).subscribe(
        (response) => 
        {
          console.log("User deleted.");
          this.r.navigate(['/']);
        },
        (error) => 
        {
          console.log("Deletion failed.")
        })
    }
    this.closeDialog();
    localStorage.removeItem("jwt");
  }

}
