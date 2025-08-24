import { Component, inject, OnInit } from '@angular/core';
import { DeleteAccountComponent } from '../delete-account/delete-account.component';
import { MatDialog } from '@angular/material/dialog';
import { EditUsernameFormComponent } from '../edit-username-form/edit-username-form.component';
import { Router } from '@angular/router';
import { TwoFactorService } from '../services/two-factor.service';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-account-settings',
  templateUrl: './account-settings.component.html',
  styleUrl: './account-settings.component.scss'
})
export class AccountSettingsComponent implements OnInit {

  readonly dialog = inject(MatDialog);
  username: string = 'Saftrakete';
  public twoFactorEnabled = false;

  constructor(private router: Router, private twoFactorService: TwoFactorService, private userService: UserService) {}

  ngOnInit(): void {
    this.checkTwoFactorStatus();
  }

  /* Öffnet ein Fenster um den Löschvorgang zu bestätigen oder abzubrechen */
  public openDeletionDialog(): void 
  {
    const dialogRef = this.dialog.open(DeleteAccountComponent);
  }

  public openEditDialog(): void
  {
    const dialogRef = this.dialog.open(EditUsernameFormComponent);
  }

  public openTwoFactorSetup(): void
  {
    if (this.twoFactorEnabled) {
      // Show disable option first
      this.showDisableTwoFactorDialog();
    } else {
      // Go to setup
      this.router.navigate(['/two-factor-setup']);
    }
  }

  private showDisableTwoFactorDialog(): void {
    const code = prompt('Enter your 6-digit 2FA code to disable:');
    if (code) {
      this.twoFactorService.disableTwoFactor(code).subscribe({
        next: () => {
          this.twoFactorEnabled = false;
          alert('2FA disabled successfully!');
        },
        error: () => {
          alert('Invalid code. Please try again.');
        }
      });
    }
  }

  private checkTwoFactorStatus(): void {
    this.userService.getTwoFactorStatus().subscribe({
      next: (enabled) => {
        this.twoFactorEnabled = enabled;
      },
      error: () => {
        this.twoFactorEnabled = false;
      }
    });
  }

}
