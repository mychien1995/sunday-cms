import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ClientState } from '@services/layout/clientstate.service';
import { UserService, RoleService } from '@services/index';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { UserMutationModel, RoleModel } from '@models/index';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
})
export class AddUserComponent implements OnInit {
  public userForm: FormGroup;
  public roleLookup: RoleModel[] = [];

  constructor(
    private userService: UserService,
    private roleService: RoleService,
    private clientState: ClientState,
    private router: Router
  ) {}

  ngOnInit() {
    this.buildForm();
    this.getFormData();
  }

  buildForm(): void {
    this.userForm = new FormGroup({
      UserName: new FormControl('', [Validators.required]),
      Fullname: new FormControl('', [Validators.required]),
      Email: new FormControl('', [Validators.required]),
      Phone: new FormControl('', [Validators.required]),
      Password: new FormControl('', [Validators.required]),
      ConfirmPassword: new FormControl('', [Validators.required]),
      Domain: new FormControl('', [Validators.required]),
      RoleIds: new FormControl('', [Validators.required]),
      IsActive: new FormControl(true),
    });
  }

  getFormData(): void {
    this.getRoleLookup();
  }

  getRoleLookup(): void {
    this.clientState.isBusy = true;
    this.roleService.getAvailableRoles().subscribe((res) => {
      this.roleLookup = <RoleModel[]>res.List;
      this.clientState.isBusy = false;
    });
  }

  onSubmit(formValue: any): void {
    if (!this.userForm.valid) {
      return;
    }
    const userData = <UserMutationModel>formValue;
    this.clientState.isBusy = true;
    this.userService.createUser(userData).subscribe((res) => {
      this.router.navigate(['/users']);
      this.clientState.isBusy = false;
    });
  }
}
