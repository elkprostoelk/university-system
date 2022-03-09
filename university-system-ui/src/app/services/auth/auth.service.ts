import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { JwtDto } from 'src/app/models/jwtDto';

import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private loginPath: string
  constructor(
    private http: HttpClient,
    private router: Router) {
      this.loginPath = 'auth/login';
   }

   login(login: string, password: string): Observable<JwtDto> {
     return this.http.post<JwtDto>(environment.apiPath + this.loginPath, {
      login: login,
      password: password,
      roleId: 1
     });
   }

   isSignedIn(): boolean {
     return localStorage.getItem('jwt') !== null;
   }

   logout(): void {
     if (localStorage.getItem('jwt')) {
      localStorage.removeItem('jwt');
      this.router.navigateByUrl('/login');
     }
   }
}
