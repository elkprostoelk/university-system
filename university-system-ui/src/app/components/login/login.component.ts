import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup
  constructor(
    private authService: AuthService,
    private builder: FormBuilder,
    private router: Router) {
      this.loginForm = this.builder.group({
        login: ['', Validators.required],
        password: ['', Validators.required]
      });
   }

  ngOnInit(): void {
  }

  login(value: any) {
    if (value.login && value.password) {
      this.authService.login(value.login, value.password)
        .subscribe(data => {
          localStorage.setItem('jwt', data.jwt);
          this.router.navigateByUrl('/');
        });
    }
  }

}
