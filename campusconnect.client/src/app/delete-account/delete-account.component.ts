import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-delete-account',
  templateUrl: './delete-account.component.html',
  styleUrl: './delete-account.component.scss'
})
export class DeleteAccountComponent {
  dialog;   // speichert das geöffnete Dialogfenster

  /* bekommt das geöffnete Dialogfenster übergeben */
  constructor(private dialogRef: MatDialogRef<DeleteAccountComponent>) {
    this.dialog = dialogRef;
  }
  /* Schließt das Dialogfenster */
  closeDialog() {
    this.dialog.close();
  }
}
