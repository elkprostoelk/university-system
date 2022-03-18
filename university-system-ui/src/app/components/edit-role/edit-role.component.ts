import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {Observable} from "rxjs";
import {RoleService} from "../../services/role/role.service";
import {RoleDto} from "../../models/roleDto";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {UserService} from "../../services/user/user.service";
import {HttpErrorResponse} from "@angular/common/http";

@Component({
  selector: 'app-edit-role',
  templateUrl: './edit-role.component.html',
  styleUrls: ['./edit-role.component.css']
})
export class EditRoleComponent implements OnInit {

  id: number | undefined;
  role$?: Observable<RoleDto>;
  editRoleForm: FormGroup
  constructor(private builder: FormBuilder,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private userService: UserService,
    private roleService: RoleService) {
    this.editRoleForm = this.builder.group({
      name: ['', Validators.required]
    });
    this.activatedRoute.params
      .subscribe(params => {
        this.id = params['id'];
        if (this.id) {
          this.role$ = this.roleService.getRole(this.id);
        }
      });
  }

  ngOnInit(): void {

  }

  deleteUserFromRole(roleId: number | undefined, userId: number): void {
    if (confirm('Are you sure to delete user from this role?') && roleId) {
      this.userService.deleteUserFromRole(userId, roleId)
        .subscribe((data) => {}, (err: HttpErrorResponse) => {
          alert(err.error.message);
        });
    }
  }

  editRole(value: any): void {
    if (this.id) {
      this.roleService.editRole(this.id, value)
        .subscribe();
      this.router.navigateByUrl('/admin');
    }
  }
}
