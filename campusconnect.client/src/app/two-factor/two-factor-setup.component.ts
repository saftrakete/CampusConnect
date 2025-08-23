import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TwoFactorService } from '../services/two-factor.service';
import { TwoFactorSetupDto } from '../entities/twoFactorDto';

@Component({
  selector: 'app-two-factor-setup',
  templateUrl: './two-factor-setup.component.html',
  styleUrls: ['./two-factor-setup.component.scss']
})
export class TwoFactorSetupComponent implements OnInit {
  public setupForm!: FormGroup;
  public setupData: TwoFactorSetupDto | null = null;
  public isEnabled = false;
  public message = '';

  constructor(
    private formBuilder: FormBuilder,
    private twoFactorService: TwoFactorService
  ) {}

  public ngOnInit(): void {
    this.setupForm = this.formBuilder.group({
      code: ['', [Validators.required, Validators.pattern(/^\d{6}$/)]]
    });
  }

  public setupTwoFactor(): void {
    // Get current user from token/auth service instead of prompt
    const loginName = 'your-username'; // TODO: Get from auth service
    
    this.twoFactorService.setupTwoFactor(loginName).subscribe({
      next: (data) => {
        this.setupData = data;
      },
      error: (error) => {
        this.message = 'Failed to setup 2FA';
      }
    });
  }

  public verifySetup(): void {
    if (!this.setupForm.valid) return;

    const loginName = 'your-username'; // TODO: Get from auth service
    const code = this.setupForm.get('code')?.value;
    this.twoFactorService.verifyTwoFactorSetup(loginName, code).subscribe({
      next: () => {
        this.isEnabled = true;
        this.message = '2FA enabled successfully!';
        this.setupData = null;
      },
      error: () => {
        this.message = 'Invalid code. Please try again.';
      }
    });
  }

  public disableTwoFactor(): void {
    const code = this.setupForm.get('code')?.value;
    this.twoFactorService.disableTwoFactor(code).subscribe({
      next: () => {
        this.isEnabled = false;
        this.message = '2FA disabled successfully!';
      },
      error: () => {
        this.message = 'Invalid code. Please try again.';
      }
    });
  }
}