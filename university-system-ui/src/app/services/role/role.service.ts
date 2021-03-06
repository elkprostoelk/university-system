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

   getRole(id: number): Observable<RoleDto> {
    return this.http.get<RoleDto>(environment.apiPath + this.rolePath + id);
   }

   createRole(roleName: string, fullRoleName: string): Observable<void> {
    return this.http.post<void>(environment.apiPath + this.rolePath, {
      roleName: roleName,
      fullRoleName: fullRoleName
    });
   }

   deleteRole(roleId: number): Observable<void> {
     return this.http.delete<void>(environment.apiPath + this.rolePath + roleId);
   }

  editRole(id: number, value: any): Observable<void> {
    return this.http.put<void>(`${environment.apiPath}${this.rolePath}${id}`, {
      name: value.name,
      fullRoleName: value.fullRoleName
    });
  }
}
