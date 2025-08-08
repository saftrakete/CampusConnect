import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { jwtDecode } from 'jwt-decode';
import { AuthorizationService } from '../services/authorization.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {
  constructor(private userService: UserService,
    private formBuilder: FormBuilder,
    private authService: AuthorizationService,
    private router: Router
  ) {}
  
  public loginForm!: FormGroup;
  public errorMessage: string = '';
  public failedAttempts: number = 0;
  public showCaptcha: boolean = false;
  public captchaQuestion: string = '';
  public captchaAnswer: number = 0;
  
  private readonly emptyString: string =  '';
    
  public ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      loginName: [this.emptyString, Validators.email],
      password: [this.emptyString, Validators.required],
      captcha: [this.emptyString]
    });
  }

  private generateCaptcha(): void {
    const num1 = Math.floor(Math.random() * 10) + 1;
    const num2 = Math.floor(Math.random() * 10) + 1;
    this.captchaAnswer = num1 + num2;
    this.captchaQuestion = `${num1} + ${num2} = ?`;
    this.loginForm.get('captcha')?.setValidators([Validators.required]);
    this.loginForm.get('captcha')?.updateValueAndValidity();
  }

  public loginClick(): void {
    if (!this.loginForm.valid.valueOf()) {
      console.log('Login Form is invalid');
      return;
    }

    // Check captcha if required
    if (this.showCaptcha) {
      const captchaInput = parseInt(this.loginForm.get('captcha')?.value);
      if (captchaInput !== this.captchaAnswer) {
        this.errorMessage = 'Falsches Captcha. Bitte versuchen Sie es erneut.';
        this.generateCaptcha();
        return;
      }
    }

    this.errorMessage = '';
    let loginDto = this.userService.createLoginDto(this.loginForm.get('loginName')?.value, this.loginForm.get('password')?.value);
    this.userService.sendLoginRequest(loginDto).subscribe(
      response => {
        // Reset on successful login
        this.failedAttempts = 0;
        this.showCaptcha = false;
        
        if (response.requiresTwoFactor && response.tempToken) {
          this.router.navigate(['/two-factor'], {
            state: {
              loginName: loginDto.loginName,
              tempToken: response.tempToken
            }
          });
        } else {
          this.authService.setToken(response.token);
          this.router.navigate(['/']);
        }
      },
      error => {
        this.failedAttempts++;
        
        if (this.failedAttempts >= 3) {
          this.showCaptcha = true;
          this.generateCaptcha();
        }
        
        if (error.status === 400) {
          this.errorMessage = 'Ungültige Anmeldedaten. Bitte überprüfen Sie Ihre E-Mail und Ihr Passwort.';
        } else if (error.status === 404) {
          this.errorMessage = 'Benutzer nicht gefunden.';
        } else {
          this.errorMessage = 'Anmeldung fehlgeschlagen. Bitte versuchen Sie es erneut.';
        }
      }
    )
  }
}