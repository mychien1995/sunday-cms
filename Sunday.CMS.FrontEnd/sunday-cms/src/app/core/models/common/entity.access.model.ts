export class EntityAccess {
  OrganizationAccess: EntityOrganizationAccess[] = [];
}

export class EntityOrganizationAccess {
  OrganizationId: string;
  WebsiteIds: string[] = [];
  OrganizationRoleIds: string[] = [];
}
