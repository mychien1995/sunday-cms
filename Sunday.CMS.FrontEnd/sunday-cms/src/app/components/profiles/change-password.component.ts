import { Component, Inject } from '@angular/core';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { UserService, RoleService } from '@services/index';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ClientState } from '@services/layout/clientstate.service';
import { ChangePasswordModel } from '@core/models';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html'
})
export class ChangePasswordDialogComponent {
  public passwordForm: FormGroup = new FormGroup({
    OldPwd: new FormControl(),
    NewPwd: new FormControl(),
    NewPwdConfirm: new FormControl()
  });

  constructor(
    private userService: UserService,
    private clientState: ClientState,
    private toastr: ToastrService,
    private dialogService: MatDialog
  ) {}

  onSubmit(formValue: any): void {
    if (!this.passwordForm.valid) {
      return;
    }
    if(formValue.NewPwd !== formValue.NewPwdConfirm){
        return;
    }
    const data = <ChangePasswordModel>{
      OldPassword: formValue.OldPwd,
      NewPassword: formValue.NewPwd
    };
    this.clientState.isBusy = true;
    this.userService.changePassword(data).subscribe(res => {
      if (res.Success) {
        this.clientState.isBusy = false;
        this.toastr.success('Password changed');
        this.dialogService.closeAll();
      }
      this.clientState.isBusy = false;
    });
  }
}
