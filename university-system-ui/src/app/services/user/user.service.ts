import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { MainUserInfoDto } from 'src/app/models/mainUserInfoDto';
import { UserForAdminPanelDto } from 'src/app/models/userForAdminPanelDto';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private userPath: string;

  constructor(private http: HttpClient) {
    this.userPath = 'user/';
   }

   getMainUserInfo(userId: number): Observable<MainUserInfoDto> {
     return this.http.get<MainUserInfoDto>(environment.apiPath + this.userPath + userId);
   }

   getAllUsers(): Observable<UserForAdminPanelDto[]> {
     return this.http.get<UserForAdminPanelDto[]>(environment.apiPath + this.userPath + 'all');
   }

   deleteUser(userId: number): Observable<void> {
     return this.http.delete<void>(environment.apiPath + this.userPath + userId);
   }

  deleteUserFromRole(userId: number, roleId: number): Observable<void> {
    return this.http.patch<void>(`${environment.apiPath}${this.userPath}delete-from-role/${userId}/${roleId}`, {});
  }

  createUser(value: any): Observable<void> {
    return this.http.post<void>(`${environment.apiPath}${this.userPath}`, {
      userName: value.userName,
      firstName: value.firstName,
      lastName: value.lastName,
      secondName: value.secondName,
      gender: value.gender,
      birthDate: value.birthDate,
      email: value.email,
      password: value.password,
      passportNumber: value.passportNumber
    });
  }

  addUserToRole(userId: number, roleId: number): Observable<void> {
    return this.http.patch<void>(`${environment.apiPath}${this.userPath}add-to-role/${userId}/${roleId}`, {})
  }
}
