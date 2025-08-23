import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../services/user.service';
import { AuthorizationService } from '../services/authorization.service';

@Component({
  selector: 'app-two-factor',
  templateUrl: './two-factor.component.html',
  styleUrls: ['./two-factor.component.scss']
})
export class TwoFactorComponent implements OnInit {
  public twoFactorForm!: FormGroup;
  public loginName: string = '';
  public tempToken: string = '';
  public errorMessage: string = '';

  constructor(
    private formBuilder: FormBuilder,
    private userService: UserService,
    private authService: AuthorizationService,
    private router: Router
  ) {
    const navigation = this.router.getCurrentNavigation();
    if (navigation?.extras.state) {
      this.loginName = navigation.extras.state['loginName'] || '';
      this.tempToken = navigation.extras.state['tempToken'] || '';
    }
  }

  public ngOnInit(): void {
    if (!this.tempToken) {
      this.router.navigate(['/login']);
      return;
    }

    this.twoFactorForm = this.formBuilder.group({
      code: ['', [Validators.required, Validators.pattern(/^\d{6}$/)]]
    });
  }

  public verifyCode(): void {
    if (!this.twoFactorForm.valid) {
      this.errorMessage = 'Please enter a valid 6-digit code';
      return;
    }

    const code = this.twoFactorForm.get('code')?.value;
    this.userService.verifyTwoFactor(this.loginName, code, this.tempToken).subscribe({
      next: (response) => {
        this.authService.setToken(response.token);
        this.router.navigate(['/']);
      },
      error: (error) => {
        this.errorMessage = 'Invalid verification code';
        this.twoFactorForm.get('code')?.setValue('');
      }
    });
  }
}