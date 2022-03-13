import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RoleDto } from 'src/app/models/roleDto';
import { UserForAdminPanelDto } from 'src/app/models/userForAdminPanelDto';
import { AuthService } from 'src/app/services/auth/auth.service';
import { RoleService } from 'src/app/services/role/role.service';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  createRoleForm: FormGroup
  userDtos?: UserForAdminPanelDto[]
  roleDtos?: RoleDto[]
  constructor(
    private userService: UserService,
    private authService: AuthService,
    private roleService: RoleService,
    private builder: FormBuilder) {
      this.createRoleForm = this.builder.group({
        roleName: ['', Validators.required]
      });
   }

  ngOnInit(): void {
    this.userService.getAllUsers()
      .subscribe(data =>
        this.userDtos = data);
    this.roleService.getAllRoles()
    .subscribe(data =>
      this.roleDtos = data);
  }

  isMe(username: string): boolean {
    return this.authService.parseJwt()?.name === username;
  }

  createRole(value: any): void {
    if (value.roleName) {
      this.roleService.createRole(value.roleName)
        .subscribe(data => {
          this.roleService.getAllRoles()
            .subscribe(d =>
              this.roleDtos = d);
        });
    }
  }

  deleteUser(userId: number): void {
    if (confirm("Are you sure to delete this user? It couldn't be reverted!")) {
      this.userService.deleteUser(userId)
        .subscribe();
    }
  }

  deleteRole(roleId: number): void {
    if (confirm("Are you sure to delete this role? It couldn't be reverted!")) {
      this.roleService.deleteRole(roleId)
        .subscribe();
    }
  }

}
