import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import {
  UserService,
  FileUploadService,
  LayoutService,
  AuthenticationService,
} from '@services/index';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ClientState } from '@services/layout/clientstate.service';
import { UserDetailResponse } from '@models/index';
import { DefaultLogo } from '@core/constants';

@Component({
  selector: 'app-change-avatar',
  templateUrl: './change-avatar.component.html',
  styleUrls: ['./change-avatar.component.scss'],
})
export class ChangeAvatarDialogComponent implements OnInit {
  public avatarForm: FormGroup = new FormGroup({});

  public logoImage: string;
  public logoImageFile: File;

  constructor(
    private userService: UserService,
    private clientState: ClientState,
    private toastr: ToastrService,
    private layoutService: LayoutService,
    private dialogService: MatDialog,
    private fileUploadService: FileUploadService,
    private authService: AuthenticationService
  ) {}

  ngOnInit(): void {
    const user = this.authService.getUser();
    this.logoImage = user.AvatarLink || DefaultLogo;
  }

  onSelectAvatar(event: any): void {
    const file: File = event.target.files && <File>event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = (ev: any) => {
        this.logoImage = ev.target.result;
      };
      this.logoImageFile = file;
    }
  }

  onSubmit(form: FormGroup): void {
    if (form.invalid || !this.logoImageFile) {
      return;
    }
    this.clientState.isBusy = true;
    this.fileUploadService
      .uploadBlob('avatars', this.logoImageFile)
      .subscribe((res) => {
        if (res.Success) {
          const blobIdentifier = res.BlobIdentifier;
          this.userService
            .updateAvatar(blobIdentifier)
            .subscribe((updateRes) => {
              if (updateRes.Success) {
                this.toastr.success('Avatar Updated');
                const user = this.authService.getUser();
                user.AvatarLink = res.PreviewLink;
                this.authService.updateToken(<UserDetailResponse>user);
                this.layoutService.layoutUpdated({
                  event: 'user-updated',
                });
                this.dialogService.closeAll();
              }
              this.clientState.isBusy = false;
            });
        }
        this.clientState.isBusy = false;
      });
  }
}
