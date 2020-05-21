import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ClientState } from '@services/layout/clientstate.service';
import { UserService } from '@services/index';
import { UserList } from '@models/index';

@Component({
  selector: 'app-manage-users',
  templateUrl: './manage-users.component.html',
})
export class ManageUsersComponent implements OnInit {
  userList: UserList = new UserList();

  constructor(
    private userService: UserService,
    private clientState: ClientState
  ) {}

  ngOnInit(): void {
    this.getUsers();
  }

  getUsers(): void {
    this.clientState.isBusy = true;
    this.userService.getUsers().subscribe((res) => {
      this.userList = <UserList>res;
      this.clientState.isBusy = false;
    });
  }
}
