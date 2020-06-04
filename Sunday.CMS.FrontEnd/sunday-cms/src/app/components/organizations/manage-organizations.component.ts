import {
  Component,
  OnInit,
  ViewEncapsulation,
  TemplateRef,
} from '@angular/core';
import { ClientState } from '@services/layout/clientstate.service';
import { OrganizationService } from '@services/index';
import { OrganizationList } from '@models/index';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-manage-organizations',
  templateUrl: './manage-organizations.component.html',
  providers: [NgbModalConfig, NgbModal],
})
export class ManageOrganizationComponent implements OnInit {
  organizationList: OrganizationList = new OrganizationList();
  activeOrganizationId: number;

  constructor(
    private organizationService: OrganizationService,
    private clientState: ClientState,
    private modalService: NgbModal,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.getOrganizations();
    this.organizationService.getModules().subscribe((res) => {});
  }

  getOrganizations(query?: any): void {
    this.clientState.isBusy = true;
    this.organizationService.getOrganizations(query).subscribe((res) => {
      this.organizationList = <OrganizationList>res;
      this.clientState.isBusy = false;
    });
  }

  onSearch(ev?: any): void {
    this.getOrganizations(ev);
  }

  activateOrganization(orgId: number): void {
    this.clientState.isBusy = true;
    this.organizationService.activateOrganization(orgId).subscribe((res) => {
      if (res.Success) {
        this.toastr.success('Organization Activated');
        this.getOrganizations();
      }
      this.clientState.isBusy = false;
    });
  }

  deactivateOrganization(orgId: number): void {
    this.clientState.isBusy = true;
    this.organizationService.deactivateOrganization(orgId).subscribe((res) => {
      if (res.Success) {
        this.toastr.success('Organization Deactivated');
        this.getOrganizations();
      }
      this.clientState.isBusy = false;
    });
  }

  deleteOrganization(orgId: number, template: any): void {
    this.activeOrganizationId = orgId;
    this.modalService.open(template);
  }

  confirmDelete() {
    if (this.activeOrganizationId) {
      this.clientState.isBusy = true;
      this.organizationService
        .deleteOrganization(this.activeOrganizationId)
        .subscribe((res) => {
          if (res.Success) {
            this.toastr.success('Organization Deleted');
            this.modalService.dismissAll();
            this.getOrganizations();
          }
          this.clientState.isBusy = false;
        });
    }
  }
}
