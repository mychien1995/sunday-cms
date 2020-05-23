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
  userToDelete: UserItem;

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
    this.userToDelete = user;
    this.modalService.open(template);
  }

  confirmDelete() {
    if (this.userToDelete) {
      this.clientState.isBusy = true;
      this.userService.deleteUser(this.userToDelete.ID).subscribe(res => {
        if (res.Success) {
          this.toastr.success('User deleted');
          this.modalService.dismissAll();
          this.getUsers();
        }
        this.clientState.isBusy = false;
      });
    }
  }
}
