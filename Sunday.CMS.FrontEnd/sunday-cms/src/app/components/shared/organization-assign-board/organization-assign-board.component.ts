import {
  Component,
  OnInit,
  ViewEncapsulation,
  Input,
  Output,
} from '@angular/core';
import { OrganizationItem, OrganizationUserItem } from '@models/index';
import { EventEmitter } from '@angular/core';

@Component({
  selector: 'app-organization-assign-board',
  templateUrl: './organization-assign-board.component.html',
  styleUrls: ['./organization-assign-board.component.scss'],
})
export class OrganizationAssignBoardComponent implements OnInit {
  @Input() organizationList: OrganizationItem[] = [];
  @Input() selectedOrganizations: OrganizationUserItem[] = [];
  @Output() selectedOrganizationChanged: EventEmitter<any> = new EventEmitter();

  selectedAvailableOrganizationIds: string[] = [];
  selectedOrganizationIds: string[] = [];

  get availableOrganizations() {
    return this.organizationList.filter(
      (c) => !this.selectedOrganizations.find((e) => e.OrganizationId === c.Id)
    );
  }

  selectOrganization(): void {
    if (this.selectedAvailableOrganizationIds.length > 0) {
      this.selectedOrganizations = this.selectedOrganizations.concat(
        this.selectedAvailableOrganizationIds.map((c) => {
          const organizationName = this.organizationList.find(
            (o) => o.Id.toString() === c.toString()
          ).OrganizationName;
          return <OrganizationUserItem>{
            OrganizationName: organizationName,
            OrganizationId: c.toString(),
            IsActive: true,
          };
        })
      );
      this.selectedOrganizationChanged.emit(this.selectedOrganizations);
      this.selectedAvailableOrganizationIds = [];
    }
  }

  deselectOrganization(): void {
    if (this.selectedOrganizationIds.length > 0) {
      this.selectedOrganizations = this.selectedOrganizations.filter(
        (c) =>
          this.selectedOrganizationIds.filter(
            (o) => o.toString() === c.OrganizationId.toString()
          ).length === 0
      );
      this.selectedOrganizationIds = [];
      this.selectedOrganizationChanged.emit(this.selectedOrganizations);
    }
  }

  selectCurrentOrganization(id: string): void {
    const index = this.selectedOrganizationIds.indexOf(id);
    if (index > -1) {
      this.selectedOrganizationIds.splice(index, 1);
    } else {
      this.selectedOrganizationIds.push(id);
    }
  }

  tableRowClass(id: string): string {
    const index = this.selectedOrganizationIds.indexOf(id);
    if (index > -1) {
      return 'selected';
    }
    return '';
  }

  onActiveChange($event: any, orgId: string): void {
    const checked = $event.target.checked;
    const organizationUser = this.selectedOrganizations.find(
      (c) => c.OrganizationId === orgId
    );
    organizationUser.IsActive = checked;
    this.selectedOrganizationChanged.emit(this.selectedOrganizations);
  }

  getOrganizationName(org: OrganizationUserItem): string {
    if (org.OrganizationName) { return org.OrganizationName; }
    const organization = this.organizationList.find(o => o.Id == org.OrganizationId);
    return organization ? organization.OrganizationName : '';
  }

  ngOnInit(): void {}
}
