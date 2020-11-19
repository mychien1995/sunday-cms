import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import {
  OrganizationRoleService,
  OrganizationService,
  WebsiteManagementService,
} from '@services/index';
import {
  EntityAccess,
  OrganizationItem,
  OrganizationRoleItem,
  WebsiteItem,
} from '@models/index';
import { forkJoin } from 'rxjs';
@Component({
  selector: 'app-entity-access-editor.component',
  styleUrls: ['./entity-access-editor.component.scss'],
  templateUrl: './entity-access-editor.component.html',
})
export class EntityAccessEditorComponent implements OnInit {
  innerEntityAccess: EntityAccess = new EntityAccess();
  @Input()
  get entityAccess(): EntityAccess {
    return this.innerEntityAccess;
  }
  set entityAccess(val: EntityAccess) {
    this.innerEntityAccess = val;
    this.entityAccessChange.emit(val);
  }
  @Output() entityAccessChange: EventEmitter<EntityAccess> = new EventEmitter();

  organizationLookup: OrganizationItem[] = [];
  websiteLookup: WebsiteItem[] = [];
  organizationRoleLookup: OrganizationRoleItem[] = [];

  constructor(
    private organizationService: OrganizationService,
    private websiteService: WebsiteManagementService,
    private roleService: OrganizationRoleService
  ) {}

  ngOnInit(): void {
    const options = { PageSize: 1000 };
    forkJoin([
      this.organizationService.getOrganizations(options),
      this.websiteService.getWebsites(options),
      this.roleService.getRoles(options),
    ]).subscribe((res) => {
      this.organizationLookup = res[0].Organizations;
      this.websiteLookup = res[1].Websites;
      this.organizationRoleLookup = res[2].Roles;
    });
  }
}
