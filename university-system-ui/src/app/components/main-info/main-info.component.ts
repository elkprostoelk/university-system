import { Component, OnInit } from '@angular/core';
import { MainUserInfoDto } from 'src/app/models/mainUserInfoDto';
import { AuthService } from 'src/app/services/auth/auth.service';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-main-info',
  templateUrl: './main-info.component.html',
  styleUrls: ['./main-info.component.css']
})
export class MainInfoComponent implements OnInit {

  mainUserInfo?: MainUserInfoDto
  constructor(
    private userService: UserService,
    private authService: AuthService) {
   }

  ngOnInit(): void {
    let user = this.authService.parseJwt();
    if (user) {
      this.userService.getMainUserInfo(user.id)
        .subscribe(data =>
          this.mainUserInfo = data);
    }
  }
}
