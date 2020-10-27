import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ClientState } from '@services/layout/clientstate.service';
import { UserService, RoleService, OrganizationService } from '@services/index';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import {
  UserMutationModel,
  RoleModel,
  UserDetailResponse,
  OrganizationLookupResponse,
  OrganizationItem,
  OrganizationUserItem,
} from '@models/index';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, Subscription, forkJoin } from 'rxjs';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.scss'],
})
export class AddUserComponent implements OnInit {
  public userForm: FormGroup = new FormGroup({});
  public roleLookup: RoleModel[] = [];
  public organizationLookup: OrganizationItem[] = [];
  public currentUser: UserDetailResponse = new UserDetailResponse();
  public isEdit: boolean;
  public formTitle = 'Create User';
  public showOrganization = false;

  constructor(
    private userService: UserService,
    private roleService: RoleService,
    private clientState: ClientState,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private toastr: ToastrService,
    private organizationService: OrganizationService
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
        Domain: new FormControl('', [Validators.required]),
        RoleId: new FormControl('', [Validators.required]),
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
        Domain: new FormControl(this.currentUser.Domain, [Validators.required]),
        RoleId: new FormControl(this.currentUser.RoleIds[0], [
          Validators.required,
        ]),
        IsActive: new FormControl(this.currentUser.IsActive),
      });
    }
  }

  getFormData(): void {
    this.clientState.isBusy = true;
    forkJoin([
      this.roleService.getAvailableRoles(),
      this.organizationService.getOrganizationsLookup(),
    ]).subscribe((response) => {
      if (response[0].Success) {
        this.roleLookup = <RoleModel[]>response[0].List;
        const currentRole = this.roleLookup.find(
          (c) => c.Id === this.currentUser.RoleIds[0]
        );
        if (currentRole && currentRole.RequireOrganization) {
          this.showOrganization = true;
        }
      }
      if (response[1].Success) {
        this.organizationLookup = <OrganizationItem[]>response[1].List;
      }
      this.clientState.isBusy = false;
    });
  }

  onRoleChange(): void {
    const roleId = this.userForm.controls['RoleId'].value;
    const role = this.roleLookup.find((c) => c.Id === roleId);
    if (role.RequireOrganization) {
      this.showOrganization = true;
    } else {
      this.showOrganization = false;
    }
  }

  onOrganizationSelected(organizationUsers: any): void {
    this.currentUser.Organizations = organizationUsers;
  }

  onSubmit(formValue: any): void {
    if (!this.userForm.valid) {
      return;
    }
    if (!this.isEdit && formValue.Password !== formValue.ConfirmPassword) {
      return;
    }
    const userData = <UserMutationModel>formValue;
    userData.RoleIds = [];
    userData.RoleIds.push(formValue.RoleId);
    if (this.currentUser) {
      userData.Id = this.currentUser.Id;
    }
    userData.Organizations = this.currentUser.Organizations;
    this.clientState.isBusy = true;
    const observ = this.isEdit
      ? this.userService.updateUser(userData)
      : this.userService.createUser(userData);
    observ.subscribe((res) => {
      if (res.Success) {
        this.toastr.success(this.isEdit ? 'User updated' : 'User created');
        this.router.navigate(['/users']);
      }
      this.clientState.isBusy = false;
    }, ex => this.clientState.isBusy = false);
  }
}
