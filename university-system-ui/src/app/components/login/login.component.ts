import { Component, OnInit } from '@angular/core';
import {AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators} from '@angular/forms';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { RoleDto } from 'src/app/models/roleDto';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  roles$: Observable<RoleDto[]>;
  loginForm: FormGroup;
  loginField: AbstractControl;
  passwordField: AbstractControl;
  roleField: AbstractControl;
  errorMessages: {
    required: string,
    minlength: string,
    maxlength: string
  }
  constructor(
    private authService: AuthService,
    private builder: FormBuilder,
    private router: Router) {
      this.loginForm = this.builder.group({
        login: ['',
          [
            Validators.required,
            Validators.minLength(3),
            Validators.maxLength(20)
          ]],
        password: ['',
          [
            Validators.required,
            Validators.minLength(8),
            Validators.maxLength(20)
          ]],
        roleId: ['', Validators.required]
      });
      this.errorMessages = {
        required: 'You should fill this field.',
        minlength: 'Not enough symbols entered.',
        maxlength: 'You entered too many symbols.'
      };
      this.loginField = this.loginForm.controls['login'];
      this.passwordField = this.loginForm.controls['password'];
      this.roleField = this.loginForm.controls['role'];
      this.roles$ = new Observable<RoleDto[]>();
   }

  ngOnInit(): void {
  }

  onLoginInput(event: any) {
    let login: string | null = event.target.value;
    this.loginForm.get('login')?.updateValueAndValidity();
    if (login && this.loginForm.get('login')?.valid) {
      this.roles$ = this.authService.getRoles(login);
    }
  }

  login(value: any) {
    if (value.login && value.password) {
      this.authService.login(value.login, value.password, value.roleId)
        .subscribe(data => {
          localStorage.setItem('jwt', data.jwt);
          this.router.navigateByUrl('/');
        });
    }
  }

  logErrors(control: AbstractControl): string {
    let result: string = '';
    if (control && control.errors) {
      Object.keys(control.errors).forEach((key: string) => {
        if (!control.touched && key !== 'required') {
          result += (this.errorMessages as any)[key] + '\n';
        }
      });
    }
    return result;
  }
}
