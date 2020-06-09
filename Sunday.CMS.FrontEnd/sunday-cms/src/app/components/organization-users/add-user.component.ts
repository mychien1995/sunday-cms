import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ClientState } from '@services/layout/clientstate.service';
import { UserService, OrganizationRoleService } from '@services/index';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import {
  UserMutationModel,
  RoleModel,
  UserDetailResponse,
  OrganizationLookupResponse,
  OrganizationItem,
  OrganizationUserItem,
  OrganizationRoleItem,
} from '@models/index';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, Subscription, forkJoin } from 'rxjs';

@Component({
  selector: 'app-add-organization-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.scss'],
})
export class AddOrganizationUserComponent implements OnInit {
  public userForm: FormGroup = new FormGroup({});
  public currentUser: UserDetailResponse = new UserDetailResponse();
  public isEdit: boolean;
  public roleLookup: OrganizationRoleItem[] = [];
  public formTitle = 'Create User';

  constructor(
    private userService: UserService,
    private roleService: OrganizationRoleService,
    private clientState: ClientState,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private toastr: ToastrService
  ) {}

  ngOnInit() {
    this.activatedRoute.data.subscribe((data: { user: UserDetailResponse }) => {
      if (data.user) {
        this.currentUser = data.user;
        this.isEdit = true;
        this.formTitle = 'Edit User';
      }
      this.buildForm();
      this.getFormData();
    });
  }

  buildForm(): void {
    if (!this.isEdit) {
      this.userForm = new FormGroup({
        UserName: new FormControl('', [Validators.required]),
        Fullname: new FormControl('', [Validators.required]),
        Email: new FormControl('', []),
        Phone: new FormControl('', []),
        Password: new FormControl('', [Validators.required]),
        ConfirmPassword: new FormControl('', [Validators.required]),
        IsActive: new FormControl(true),
      });
    } else {
      this.userForm = new FormGroup({
        UserName: new FormControl(this.currentUser.UserName),
        Fullname: new FormControl(this.currentUser.Fullname, [
          Validators.required,
        ]),
        Email: new FormControl(this.currentUser.Email, []),
        Phone: new FormControl(this.currentUser.Phone, []),
        IsActive: new FormControl(this.currentUser.IsActive),
      });
    }
  }

  getFormData(): void {
    this.clientState.isBusy = true;
    this.roleService.getRoles({ PageSize: 1000 }).subscribe((res) => {
      if (res.Success) {
        this.roleLookup = res.Roles;
      }
      this.clientState.isBusy = false;
    });
  }

  onSubmit(formValue: any): void {
    if (!this.userForm.valid) {
      return;
    }
    if (!this.isEdit && formValue.Password !== formValue.ConfirmPassword) {
      return;
    }
    const userData = <UserMutationModel>formValue;
    userData.OrganizationRoleIds = this.currentUser.OrganizationRoleIds;
    if (this.currentUser) {
      userData.ID = this.currentUser.ID;
    }
    this.clientState.isBusy = true;
    const observ = this.isEdit
      ? this.userService.updateUser(userData)
      : this.userService.createUser(userData);
    observ.subscribe((res) => {
      if (res.Success) {
        this.toastr.success(this.isEdit ? 'User updated' : 'User created');
        this.router.navigate(['/organization-users']);
      }
      this.clientState.isBusy = false;
    });
  }
}
