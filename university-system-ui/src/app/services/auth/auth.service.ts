import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { JwtDto } from 'src/app/models/jwtDto';
import { UserModel } from 'src/app/models/userModel';

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

   parseJwt(): UserModel | null {
     const token = localStorage.getItem('jwt');
     if (token) {
      const identity = JSON.parse(atob(token.split('.')[1]));
      const idClaim: string = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier';
      const nameClaim: string = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name';
      const roleClaim: string = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/role';
      let userModel: UserModel = {
        id: identity[idClaim],
        name: identity[nameClaim],
        role: identity[roleClaim]
       };
       return userModel;
     }
     return null;
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
