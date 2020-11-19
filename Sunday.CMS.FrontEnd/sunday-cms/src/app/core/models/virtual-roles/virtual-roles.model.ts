import { ApiResponse } from '@models/common/response.model';

export class OrganizationRoleItem {
  Id?: string;
  RoleName?: string;
  CreatedBy?: string;
  OrganizationId: string;
  UpdatedBy?: string;
  CreatedDate?: number;
  UpdatedDate?: number;
  FeatureIds?: string[] = [];
}

export class OrganizationRoleListApiResponse extends ApiResponse {
  Total?: number;
  Roles: OrganizationRoleItem[] = [];
}

export class OrganizationRoleDetailApiResponse extends ApiResponse {
  Id?: string;
  RoleName?: string;
  FeatureIds: string[] = [];
}

export class OrganizationRoleMutationData {
  Id?: string;
  RoleName?: string;
  FeatureIds: string[] = [];
}
