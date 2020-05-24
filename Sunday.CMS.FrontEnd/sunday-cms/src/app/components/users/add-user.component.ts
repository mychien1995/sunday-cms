import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ClientState } from '@services/layout/clientstate.service';
import { UserService, RoleService } from '@services/index';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import {
  UserMutationModel,
  RoleModel,
  UserDetailResponse
} from '@models/index';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.scss']
})
export class AddUserComponent implements OnInit {
  public userForm: FormGroup = new FormGroup({});
  public roleLookup: RoleModel[] = [];
  public currentUser: UserDetailResponse;
  public isEdit: boolean;
  public formTitle = 'Create User';

  constructor(
    private userService: UserService,
    private roleService: RoleService,
    private clientState: ClientState,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private toastr: ToastrService
  ) {}

  ngOnInit() {
    this.activatedRoute.data.subscribe((data: { user: UserDetailResponse }) => {
      this.currentUser = data.user;
      if (this.currentUser) {
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
        IsActive: new FormControl(true)
      });
    } else {
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
        ]),
        IsActive: new FormControl(this.currentUser.IsActive)
      });
    }
  }

  getFormData(): void {
    this.getRoleLookup();
  }

  getRoleLookup(): void {
    this.clientState.isBusy = true;
    this.roleService.getAvailableRoles().subscribe(res => {
      this.roleLookup = <RoleModel[]>res.List;
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
    userData.RoleIds = [];
    userData.RoleIds.push(formValue.RoleId);
    if (this.currentUser) {
      userData.ID = this.currentUser.ID;
    }
    this.clientState.isBusy = true;
    const observ = this.isEdit
      ? this.userService.updateUser(userData)
      : this.userService.createUser(userData);
    observ.subscribe(res => {
      if (res.Success) {
        this.toastr.success(this.isEdit ? 'User updated' : 'User created');
        this.router.navigate(['/users']);
      }
      this.clientState.isBusy = false;
    });
  }
}
