import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import {
  EntityAccess,
  EntityOrganizationAccess,
  OrganizationItem,
  OrganizationRoleItem,
  WebsiteItem,
} from '@models/index';
@Component({
  selector: 'app-entity-access-dialog',
  styleUrls: ['./entity-access-dialog.component.scss'],
  templateUrl: './entity-access-dialog.component.html',
})
export class EntityAccessDialogComponent implements OnInit {
  organizationLookup: Organization[] = [];
  entityAccess: EntityAccess = new EntityAccess();

  constructor(private dialogService: MatDialog) {}
  ngOnInit(): void {}

  load(
    organizationLookup: OrganizationItem[],
    websiteLookup: WebsiteItem[],
    organizationRoleLookup: OrganizationRoleItem[],
    entityAccess: EntityAccess
  ): void {
    this.organizationLookup = organizationLookup.map((o) => <Organization>o);
    this.organizationLookup.forEach((org) => {
      org.Websites = websiteLookup.filter((w) => w.OrganizationId === org.Id);
      org.Roles = organizationRoleLookup.filter(
        (r) => r.OrganizationId === org.Id
      );
      const existingOrgAccess = entityAccess.OrganizationAccess.find(
        (o) => o.OrganizationId === org.Id
      );
      if (existingOrgAccess) {
        org.Access = <EntityOrganizationAccess>{
          OrganizationId: existingOrgAccess.OrganizationId,
          WebsiteIds: [...existingOrgAccess.WebsiteIds],
          OrganizationRoleIds: [...existingOrgAccess.OrganizationRoleIds],
        };
      }
      if (org.Access) {
        org.Open = true;
      }
    });
    this.entityAccess = entityAccess;
  }

  websiteSelected(organization: Organization, website: WebsiteItem): boolean {
    return (
      organization.Access &&
      organization.Access.WebsiteIds.filter((w) => w === website.Id).length > 0
    );
  }

  selectOrganization(organization: Organization) {
    if (!organization.Access) {
      organization.Access = new EntityOrganizationAccess();
    }
    organization.Open = !organization.Open;
  }

  selectWebsite(organization: Organization, website: WebsiteItem) {
    if (!organization.Access) {
      organization.Access = new EntityOrganizationAccess();
    }
    const index = organization.Access.WebsiteIds.indexOf(website.Id);
    if (index > -1) {
      organization.Access.WebsiteIds.splice(index, 1);
    } else {
      organization.Access.WebsiteIds.push(website.Id);
    }
  }

  selectRole(organization: Organization, role: OrganizationRoleItem) {
    if (!organization.Access) {
      organization.Access = new EntityOrganizationAccess();
    }
    const index = organization.Access.OrganizationRoleIds.indexOf(role.Id);
    if (index > -1) {
      organization.Access.OrganizationRoleIds.splice(index, 1);
    } else {
      organization.Access.OrganizationRoleIds.push(role.Id);
    }
  }

  roleSelected(
    organization: Organization,
    role: OrganizationRoleItem
  ): boolean {
    return (
      organization.Access &&
      organization.Access.OrganizationRoleIds.filter((w) => w === role.Id)
        .length > 0
    );
  }

  onSubmit(): void {
    this.entityAccess.OrganizationAccess = this.organizationLookup
      .filter((o) => o.Access && o.Open)
      .map(
        (o) =>
          <EntityOrganizationAccess>{
            OrganizationId: o.Id,
            WebsiteIds: o.Access.WebsiteIds,
            OrganizationRoleIds: o.Access.OrganizationRoleIds,
          }
      );
    this.dialogService.closeAll();
  }
}

class Organization extends OrganizationItem {
  Open: boolean;
  Websites: WebsiteItem[] = [];
  Roles: OrganizationRoleItem[] = [];
  Access: EntityOrganizationAccess = new EntityOrganizationAccess();
}
