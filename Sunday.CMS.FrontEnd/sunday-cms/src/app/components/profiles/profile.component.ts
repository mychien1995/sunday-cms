import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ClientState } from '@services/layout/clientstate.service';
import {
  UserService,
  RoleService,
  AuthenticationService,
  LayoutService
} from '@services/index';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import {
  UserMutationModel,
  RoleModel,
  UserDetailResponse
} from '@models/index';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-user=profile',
  templateUrl: './profile.component.html'
})
export class UserProfileComponent implements OnInit {
  public userForm: FormGroup = new FormGroup({
    UserName: new FormControl(),
    Fullname: new FormControl(),
    Email: new FormControl(),
    Phone: new FormControl(),
    Domain: new FormControl(),
    RoleId: new FormControl()
  });
  public roleLookup: RoleModel[] = [];
  public currentUser: UserDetailResponse;

  constructor(
    private userService: UserService,
    private roleService: RoleService,
    private clientState: ClientState,
    private authService: AuthenticationService,
    private layoutService: LayoutService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.getUser();
  }

  getUser(): void {
    this.clientState.isBusy = true;
    this.userService.getCurrentProfile().subscribe(res => {
      if (res.Success) {
        this.currentUser = res;
        this.getFormData(res);
        this.buildForm();
      }
      this.clientState.isBusy = false;
    });
  }

  buildForm(): void {
    this.userForm = new FormGroup({
      UserName: new FormControl(this.currentUser.UserName),
      Fullname: new FormControl(this.currentUser.Fullname, [
        Validators.required
      ]),
      Email: new FormControl(this.currentUser.Email, []),
      Phone: new FormControl(this.currentUser.Phone, []),
      Domain: new FormControl(this.currentUser.Domain, [Validators.required]),
      RoleId: new FormControl(this.currentUser.RoleIds[0], [
        Validators.required
      ])
    });
  }

  getFormData(user: UserDetailResponse): void {
    this.getRoleLookup(user.RoleIds[0]);
  }

  getRoleLookup(roleId: number): void {
    this.clientState.isBusy = true;
    this.roleService.getRoleById(roleId).subscribe(res => {
      const roleList = [res];
      this.roleLookup = <RoleModel[]>roleList;
      this.clientState.isBusy = false;
    });
  }

  onSubmit(formValue: any): void {
    if (!this.userForm.valid) {
      return;
    }
    const userData = <UserMutationModel>formValue;
    userData.RoleIds = [];
    userData.RoleIds.push(formValue.RoleId);
    this.clientState.isBusy = true;
    this.userService.updateProfile(userData).subscribe(res => {
      if (res.Success) {
        this.toastr.success('Profile Updated');
        this.getUser();
        this.userService.getCurrentProfile().subscribe(user => {
          if (user.Success) {
            this.authService.updateToken(user);
            this.layoutService.layoutUpdated({
              event: 'user-updated'
            });
          }
        });
      }
      this.clientState.isBusy = false;
    });
  }
}
