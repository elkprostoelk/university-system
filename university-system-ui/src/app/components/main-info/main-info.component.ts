import { Component, OnInit } from '@angular/core';
import { MainUserInfoDto } from 'src/app/models/mainUserInfoDto';
import { AuthService } from 'src/app/services/auth/auth.service';
import { UserService } from 'src/app/services/user/user.service';
import {RoleDto} from "../../models/roleDto";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {Router} from "@angular/router";

@Component({
  selector: 'app-main-info',
  templateUrl: './main-info.component.html',
  styleUrls: ['./main-info.component.css']
})
export class MainInfoComponent implements OnInit {

  reloginUserForm: FormGroup
  mainUserInfo?: MainUserInfoDto
  userRoles?: RoleDto[]
  constructor(
    private router: Router,
    private builder: FormBuilder,
    private userService: UserService,
    private authService: AuthService) {
    this.reloginUserForm = this.builder.group({
      reloginRole: ['', Validators.required]
    });
   }

  ngOnInit(): void {
    let user = this.authService.parseJwt();
    if (user) {
      this.userService.getMainUserInfo(user.id)
        .subscribe(data =>
          this.mainUserInfo = data);
      this.authService.getRoles(user.name)
        .subscribe(data => {
          this.userRoles = data;
          this.userRoles = this.userRoles
            .filter(r => r.name != user?.role);
        });
    }
  }

  reloginUser(value: any): void {
    this.authService.relogin(value.reloginRole)
      .subscribe(data => {
        localStorage.setItem('jwt', data.jwt);
        this.router.navigateByUrl('/');
      });
  }
}
