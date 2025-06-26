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
  
  private readonly emptyString: string =  '';
    
  public ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      loginName: [this.emptyString, Validators.email],
      password: [this.emptyString, Validators.required]
    });
  }

  public loginClick(): void {
    if (!this.loginForm.valid.valueOf()) {
      console.log('Login Form is invalid');
      return;
    }

    let loginDto = this.userService.createLoginDto(this.loginForm.get('loginName')?.value, this.loginForm.get('password')?.value);
    this.userService.sendLoginRequest(loginDto).subscribe(
      response => {
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
        console.error('Login failed:', error);
      }
    )
  }
}