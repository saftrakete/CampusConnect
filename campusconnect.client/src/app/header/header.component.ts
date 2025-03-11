import { Component, inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DeleteAccountComponent } from '../delete-account/delete-account.component';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  readonly dialog = inject(MatDialog);    // Instanz um Dialogfenster zu öffnen

  /* Öffnet ein Fenster um den Löschvorgang zu bestätigen oder abzubrechen */
  deleteAccount() {
    const dialogRef = this.dialog.open(DeleteAccountComponent);
  }
}
