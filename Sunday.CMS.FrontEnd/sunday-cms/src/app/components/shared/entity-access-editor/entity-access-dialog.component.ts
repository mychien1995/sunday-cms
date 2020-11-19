import { Component, OnInit } from '@angular/core';
import {
  EntityAccess,
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
  organizationLookup: OrganizationItem[] = [];
  websiteLookup: WebsiteItem[] = [];
  organizationRoleLookup: OrganizationRoleItem[] = [];
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
    this.organizationLookup = organizationLookup;
    this.websiteLookup = websiteLookup;
    this.organizationRoleLookup = organizationRoleLookup;
    this.entityAccess = entityAccess;
  }
}
