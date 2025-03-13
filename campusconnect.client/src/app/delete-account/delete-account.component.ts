import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { UserService } from '../services/user.service';
import { Router } from '@angular/router';
import { baseApiRoute } from '../app-routing.module';

@Component({
  selector: 'app-delete-account',
  templateUrl: './delete-account.component.html',
  styleUrl: './delete-account.component.scss'
})
export class DeleteAccountComponent {

  dialog: MatDialogRef<DeleteAccountComponent>;   // speichert das geöffnete Dialogfenster
  userService: UserService;                       // UserService Instanz
  router: Router;                                 // Router-Instanz zum Navigieren zur Homepage nach Löschvorgang

  /* bekommt das geöffnete Dialogfenster übergeben */
  constructor(
    private dialogRef: MatDialogRef<DeleteAccountComponent>, 
    private us: UserService,
    private r: Router
  ) 
  {
    this.dialog = dialogRef;
    this.userService = us;
    this.router = r;
  }
  /* Schließt das Dialogfenster */
  closeDialog() 
  {
    this.dialog.close();
  }

  confirmDeletion(userID: number) 
  {
    this.userService.deleteUser(userID).subscribe(
      response => { 
        console.log("Eintrag gelöscht!"); 
        this.router.navigate(['/'])
      },
      error => { 
        console.error("Fehler beim Löschen"); 
      }
    );
    this.closeDialog();
  }

}
