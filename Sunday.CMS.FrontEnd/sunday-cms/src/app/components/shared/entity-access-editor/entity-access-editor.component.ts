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
import { MatDialog } from '@angular/material/dialog';
import { EntityAccessDialogComponent } from './entity-access-dialog.component';
@Component({
  selector: 'app-entity-access-editor',
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
    private roleService: OrganizationRoleService,
    private dialogService: MatDialog
  ) {}

  ngOnInit(): void {
    const options = { PageSize: 100000 };
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

  displayOrg(orgId: string): string {
    return this.organizationLookup.find((o) => o.Id === orgId)
      ?.OrganizationName;
  }

  displayWebsite(websiteId: string): string {
    return this.websiteLookup.find((o) => o.Id === websiteId)?.WebsiteName;
  }

  displayRole(roleId: string): string {
    return this.organizationRoleLookup.find((o) => o.Id === roleId)?.RoleName;
  }

  openDialog(): void {
    const ref = this.dialogService.open(EntityAccessDialogComponent, {
      minWidth: 600,
      disableClose: true,
    });
    if (!this.innerEntityAccess) {
      this.innerEntityAccess = new EntityAccess();
    }
    ref.componentInstance.load(
      this.organizationLookup,
      this.websiteLookup,
      this.organizationRoleLookup,
      this.innerEntityAccess
    );
    ref.afterClosed().subscribe(res =>{
      this.entityAccess = this.innerEntityAccess;
    });
  }
}
