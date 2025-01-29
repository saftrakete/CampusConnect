import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent implements OnInit {
  constructor(private userService: UserService,
    private formBuilder: FormBuilder
  ) {}

  public registerForm!: FormGroup;

  private emptyString: string = '';

  public ngOnInit(): void {
      this.registerForm = this.formBuilder.group({
        loginName: [this.emptyString, Validators.email],
        nickname: [this.emptyString, Validators.required],
        password: [this.emptyString, Validators.required],
        confirmPassword: [this.emptyString, [Validators.required, this.matchPasswordValidator('password')]]
      }
    );
  }

  private matchPasswordValidator(passwordControlName: string): ValidatorFn{
    return (control: AbstractControl) => {
      const password = control.root?.get(passwordControlName)?.value;
      return password === control.value ? null : { passwordMismatch: true };
    }
  }

  public registerClick(): void {
    if (!this.registerForm.valid.valueOf()) {
      console.log('unvalid form');
      return;
    }

    let userEntity = this.userService.createUserEntity(
      this.registerForm.get('nickname')?.value,
      this.registerForm.get('loginName')?.value,
      this.registerForm.get('password')?.value
    );
    
    this.userService.postNewUser(userEntity).subscribe(
      response => {
        console.log(response);
      },
      error => {

      }
    );
  }
}
