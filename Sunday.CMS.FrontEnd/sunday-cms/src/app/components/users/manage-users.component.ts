import {
  Component,
  OnInit,
  ViewEncapsulation,
  TemplateRef
} from '@angular/core';
import { ClientState } from '@services/layout/clientstate.service';
import { UserService } from '@services/index';
import { UserList, UserItem } from '@models/index';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-manage-users',
  templateUrl: './manage-users.component.html',
  providers: [NgbModalConfig, NgbModal]
})
export class ManageUsersComponent implements OnInit {
  userList: UserList = new UserList();
  activeUserId?: number;

  constructor(
    private userService: UserService,
    private clientState: ClientState,
    private modalService: NgbModal,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.getUsers();
  }

  getUsers(userQuery?: any): void {
    this.clientState.isBusy = true;
    this.userService.getUsers(userQuery).subscribe(res => {
      this.userList = <UserList>res;
      this.clientState.isBusy = false;
    });
  }

  deleteUser(user: UserItem, template: any): void {
    this.activeUserId = user.ID;
    this.modalService.open(template);
  }

  confirmDelete() {
    if (this.activeUserId) {
      this.clientState.isBusy = true;
      this.userService.deleteUser(this.activeUserId).subscribe(res => {
        if (res.Success) {
          this.toastr.success('User deleted');
          this.modalService.dismissAll();
          this.getUsers();
        }
        this.clientState.isBusy = false;
      });
    }
  }

  activeUser(userId: number): void {
    this.clientState.isBusy = true;
    this.userService.activateUser(userId).subscribe(res => {
      if (res.Success) {
        this.toastr.success('User Activated');
        this.getUsers();
      }
      this.clientState.isBusy = false;
    });
  }

  deactiveUser(userId: number): void {
    this.clientState.isBusy = true;
    this.userService.deactivateUser(userId).subscribe(res => {
      if (res.Success) {
        this.toastr.success('User Deactivated');
        this.getUsers();
      }
      this.clientState.isBusy = false;
    });
  }

  resetPassword(userId: number, template: any): void {
    this.activeUserId = userId;
    this.modalService.open(template);
  }

  confirmResetPassword(): void {
    if (this.activeUserId) {
      this.clientState.isBusy = true;
      this.userService.resetPassword(this.activeUserId).subscribe(res => {
        if (res.Success) {
          this.toastr.success(
            'User password has beed reseted. An email has been sent to the user'
          );
          this.modalService.dismissAll();
        }
        this.clientState.isBusy = false;
      });
    }
  }
}
