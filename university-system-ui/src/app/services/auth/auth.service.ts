import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Router} from '@angular/router';
import {Observable} from 'rxjs';
import {JwtDto} from 'src/app/models/jwtDto';
import {RoleDto} from 'src/app/models/roleDto';
import {UserModel} from 'src/app/models/userModel';

import {environment} from '../../../environments/environment';

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

   login(login: string, password: string, roleId: number): Observable<JwtDto> {
     return this.http.post<JwtDto>(environment.apiPath + this.loginPath, {
      login: login,
      password: password,
      roleId: roleId
     });
   }

   parseJwt(): UserModel | null {
     const token = localStorage.getItem('jwt');
     if (token) {
      const identity = JSON.parse(atob(token.split('.')[1]));
      const idClaim: string = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier';
      const nameClaim: string = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name';
      const roleClaim: string = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';
      const expires: Date = new Date(0);
      expires.setUTCSeconds(identity['exp']);
      if (expires < new Date()) {
        localStorage.removeItem('jwt');
        return null;
      }
       return {
         id: identity[idClaim],
         name: identity[nameClaim],
         role: identity[roleClaim]
       };
     }
     return null;
   }

   getRoles(login: string): Observable<RoleDto[]> {
    return this.http.get<RoleDto[]>(environment.apiPath + 'user/roles/' + login);
   }

   isSignedIn(): boolean {
     return localStorage.getItem('jwt') !== null;
   }

   isInRole(role: string): boolean {
     let user = this.parseJwt();
     if (user) {
       return user.role === role;
     }
     return false;
   }

   logout(): void {
     if (localStorage.getItem('jwt')) {
      localStorage.removeItem('jwt');
      this.router.navigateByUrl('/login');
     }
   }
}
