import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { RoleDto } from 'src/app/models/roleDto';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RoleService {

  rolePath: string
  constructor(private http: HttpClient) {
    this.rolePath = 'role/';
   }

   getAllRoles(): Observable<RoleDto[]> {
     return this.http.get<RoleDto[]>(environment.apiPath + this.rolePath + 'all');
   }

   deleteRole(roleId: number): Observable<void> {
     return this.http.delete<void>(environment.apiPath + this.rolePath + roleId);
   }
}
