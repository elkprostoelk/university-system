import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
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
  constructor(
    private authService: AuthService,
    private builder: FormBuilder,
    private router: Router) {
      this.loginForm = this.builder.group({
        login: ['', Validators.required],
        password: ['', Validators.required],
        roleId: ['', Validators.required]
      });
      this.roles$ = new Observable<RoleDto[]>();
   }

  ngOnInit(): void {
  }

  onLoginInput(event: any) {
    let login: string | null = event.target.value;
    if (login) {
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

}
