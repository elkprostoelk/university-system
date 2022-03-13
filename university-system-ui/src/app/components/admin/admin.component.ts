import { Component, OnInit } from '@angular/core';
import { UserForAdminPanelDto } from 'src/app/models/userForAdminPanelDto';
import { AuthService } from 'src/app/services/auth/auth.service';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {

  userDtos?: UserForAdminPanelDto[]
  constructor(
    private userService: UserService,
    private authService: AuthService) {

   }

  ngOnInit(): void {
    this.userService.getAllUsers()
      .subscribe(data =>
        this.userDtos = data);
  }

  isMe(username: string): boolean {
    return this.authService.parseJwt()?.name === username;
  }

  deleteUser(userId: number): void {
    if (confirm("Are you sure to delete this user? It couldn't be reverted!")) {
      this.userService.deleteUser(userId)
        .subscribe();
    }
  }

}
