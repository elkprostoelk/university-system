import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RoleDto } from 'src/app/models/roleDto';
import { UserForAdminPanelDto } from 'src/app/models/userForAdminPanelDto';
import { AuthService } from 'src/app/services/auth/auth.service';
import { RoleService } from 'src/app/services/role/role.service';
import { UserService } from 'src/app/services/user/user.service';
import {HttpErrorResponse} from "@angular/common/http";

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  createRoleForm: FormGroup
  createUserForm: FormGroup
  addToRoleForm: FormGroup
  userDtos?: UserForAdminPanelDto[]
  roleDtos?: RoleDto[]
  constructor(
    private userService: UserService,
    private authService: AuthService,
    private roleService: RoleService,
    private builder: FormBuilder) {
      this.createUserForm = this.builder.group({
        userName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(20)]],
        firstName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
        secondName: ['', Validators.maxLength(100)],
        lastName: ['', [Validators.required, Validators.maxLength(100)]],
        gender: ['', Validators.required],
        birthDate: ['', Validators.required],
        email: [''],
        password: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(20)]],
        passportNumber: ['', Validators.required]
      });
      this.createRoleForm = this.builder.group({
        roleName: ['', Validators.required]
      });
      this.addToRoleForm = this.builder.group({
        userToAdd: ['', Validators.required],
        roleToAdd: ['', Validators.required]
      });
   }

  ngOnInit(): void {
    this.userService.getAllUsers()
      .subscribe(data => {
        this.userDtos = data;
        this.userDtos.sort((a, b) =>
          a.fullName.localeCompare(b.fullName));
      });
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

  createUser(value: any): void {
    this.userService.createUser(value)
      .subscribe();
  }

  addUserToRole(value: any): void {
    this.userService.addUserToRole(value.userToAdd, value.roleToAdd)
      .subscribe(data => {}, (err: HttpErrorResponse) => {
        if (err.status === 400) {
          alert(err.error.message);
        }
      });
  }

  searchUser(input: HTMLInputElement): void {
    let searchString: string = input.value;
    if (searchString !== '') {
      this.userService.getAllUsers()
        .subscribe(data => {
          this.userDtos = data;
          this.userDtos.sort((a, b) =>
            a.fullName.localeCompare(b.fullName));
          if (searchString.match(/^[a-zA-Z0-9]*$/i)) {
            this.userDtos = this.userDtos?.filter(u =>
              u.userName.toLowerCase().includes(searchString.toLowerCase()));
          }
          else {
            this.userDtos = this.userDtos?.filter(u =>
              u.fullName.toLowerCase().includes(searchString.toLowerCase()));
          }
          this.userDtos?.sort((a, b) =>
            a.fullName.localeCompare(b.fullName));
        });
    }
  }

  resetResults(): void {
    this.userService.getAllUsers()
      .subscribe(data => {
        this.userDtos = data;
        this.userDtos.sort((a, b) =>
          a.fullName.localeCompare(b.fullName));
      });
  }
}
