import { Component, OnInit } from '@angular/core';
import {
  EntityAccess,
  EntityOrganizationAccess,
  OrganizationItem,
  OrganizationRoleItem,
  WebsiteItem,
} from '@models/index';
@Component({
  selector: 'app-entity-access-editor.component',
  styleUrls: ['./entity-access-editor.component.scss'],
  templateUrl: './entity-access-editor.component.html',
})
export class EntityAccessDialogComponent implements OnInit {
  organizationLookup: Organization[] = [];
  entityAccess: EntityAccess = new EntityAccess();

  ngOnInit(): void {
    throw new Error('Method not implemented.');
  }

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
      org.Access = entityAccess.OrganizationAccess.find(
        (o) => o.OrganizationId === org.Id
      );
    });
    this.entityAccess = entityAccess;
  }
}

class Organization extends OrganizationItem {
  Websites: WebsiteItem[] = [];
  Roles: OrganizationRoleItem[] = [];
  Access: EntityOrganizationAccess = new EntityOrganizationAccess();
}
