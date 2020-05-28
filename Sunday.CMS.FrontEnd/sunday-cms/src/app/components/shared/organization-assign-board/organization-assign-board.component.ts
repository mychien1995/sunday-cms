import { Component, OnInit, ViewEncapsulation, Input, Output } from '@angular/core';
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

  selectedAvailableOrganizationIds: number[] = [];
  selectedOrganizationIds: number[] = [];

  get availableOrganizations() {
    return this.organizationList.filter(
      (c) => !this.selectedOrganizations.find((e) => e.OrganizationId === c.ID)
    );
  }

  selectOrganization(): void {
    if (this.selectedAvailableOrganizationIds.length > 0) {
      this.selectedOrganizations = this.selectedOrganizations.concat(
        this.selectedAvailableOrganizationIds.map((c) => {
          const organizationName = this.organizationList.find(
            (o) => o.ID.toString() === c.toString()
          ).OrganizationName;
          return <OrganizationUserItem>{
            OrganizationName: organizationName,
            OrganizationId: parseInt(c.toString(), null),
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

  selectCurrentOrganization(id: number): void {
    const index = this.selectedOrganizationIds.indexOf(
      parseInt(id.toString(), null)
    );
    if (index > -1) {
      this.selectedOrganizationIds.splice(index, 1);
    } else {
      this.selectedOrganizationIds.push(id);
    }
  }

  tableRowClass(id: number): string {
    const index = this.selectedOrganizationIds.indexOf(
      parseInt(id.toString(), null)
    );
    if (index > -1) {
      return 'selected';
    }
    return '';
  }

  ngOnInit(): void {}
}
