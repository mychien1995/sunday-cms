import { Component, OnInit } from '@angular/core';
import { ClientState } from '@services/layout/clientstate.service';
import { OrganizationRoleService, OrganizationService } from '@services/index';
import {
  OrganizationRoleListApiResponse,
  OrganizationRoleItem,
  ModuleListApiResponse,
  FeatureItem,
  OrganizationRoleMutationData,
} from '@models/index';
import { forkJoin, Observable } from 'rxjs';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-manage-organization-roles',
  templateUrl: './manage-permissions.component.html',
  providers: [NgbModalConfig, NgbModal],
})
export class ManagePermissionsComponent implements OnInit {
  roleList: OrganizationRoleListApiResponse = new OrganizationRoleListApiResponse();
  modulesList: ModuleListApiResponse = new ModuleListApiResponse();
  featureList: FeatureItem[] = [];
  activeRoleId: number;

  constructor(
    private roleService: OrganizationRoleService,
    private organizationService: OrganizationService,
    private clientState: ClientState,
    private modalService: NgbModal,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.clientState.isBusy = true;
    forkJoin([this.getRoles(), this.getModules()]).subscribe((res) => {
      if (res[0].Success) {
        this.roleList = <OrganizationRoleListApiResponse>res[0];
      }
      if (res[1].Success) {
        this.modulesList = <ModuleListApiResponse>res[1];
        this.featureList = this.modulesList.Modules.map(
          (c) => c.Features
        ).reduce((a, b) => {
          return a.concat(b);
        });
      }
      this.clientState.isBusy = false;
    });
  }

  getRoles(query?: any): Observable<any> {
    if (!query) {
      query = <any>{};
    }
    query.FetchFeatures = true;
    return this.roleService.getRoles(query);
  }

  onPaging(query?: any): void {
    this.clientState.isBusy = true;
    this.getRoles().subscribe((res) => {
      if (res.Success) {
        this.roleList = <OrganizationRoleListApiResponse>res;
      }
      this.clientState.isBusy = false;
    });
  }

  selectFeature(role: OrganizationRoleItem, featureId: number): void {
    const index = role.FeatureIds.indexOf(featureId);
    if (index > -1) {
      role.FeatureIds.splice(index, 1);
    } else {
      role.FeatureIds.push(featureId);
    }
  }

  getModules(): Observable<any> {
    return this.organizationService.getModules();
  }

  confirmUpdate(): void {
    const data = this.roleList.Roles.map(
      (c) => <OrganizationRoleMutationData>c
    );
    this.clientState.isBusy = true;
    this.roleService.bulkUpdate(data).subscribe((res) => {
      if (res.Success) {
        this.clientState.isBusy = false;
        this.toastr.success('Permissions Updated');
        this.modalService.dismissAll();
      }
    });
  }

  updatePermission(template: any): void {
    this.modalService.open(template);
  }
}
