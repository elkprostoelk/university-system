import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { MainUserInfoDto } from 'src/app/models/mainUserInfoDto';
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
}