import { Component, OnInit } from '@angular/core';
import { ClientState } from '@services/layout/clientstate.service';
import { OrganizationRoleService } from '@services/index';
import {
  OrganizationRoleListApiResponse,
  OrganizationRoleItem,
} from '@models/index';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-manage-organization-roles',
  templateUrl: './manage-organization-roles.component.html',
  providers: [NgbModalConfig, NgbModal],
})
export class ManageOrganizationRolesComponent implements OnInit {
  roleList: OrganizationRoleListApiResponse = new OrganizationRoleListApiResponse();
  activeRoleId: number;

  constructor(
    private roleService: OrganizationRoleService,
    private clientState: ClientState,
    private modalService: NgbModal,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.getRoles();
  }

  getRoles(query?: any): void {
    this.clientState.isBusy = true;
    this.roleService.getRoles(query).subscribe((res) => {
      if (res.Success) {
        this.roleList = <OrganizationRoleListApiResponse>res;
      }
      this.clientState.isBusy = false;
    });
  }

  onSearch(ev?: any): void {
    this.getRoles(ev);
  }

  deleteRole(roleId: number, template: any): void {
    this.activeRoleId = roleId;
    this.modalService.open(template);
  }

  confirmDelete() {
    if (this.activeRoleId) {
      this.clientState.isBusy = true;
      this.roleService
        .deleteRole(this.activeRoleId)
        .subscribe((res) => {
          if (res.Success) {
            this.toastr.success('Role Deleted');
            this.modalService.dismissAll();
            this.getRoles();
          }
          this.clientState.isBusy = false;
        });
    }
  }
}
