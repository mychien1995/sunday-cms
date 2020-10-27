import { ApiResponse } from '@models/common/response.model';

export class OrganizationRoleItem {
  Id?: string;
  RoleName?: string;
  CreatedBy?: string;
  UpdatedBy?: string;
  CreatedDate?: number;
  UpdatedDate?: number;
  FeatureIds?: number[] = [];
}

export class OrganizationRoleListApiResponse extends ApiResponse {
  Total?: number;
  Roles: OrganizationRoleItem[] = [];
}

export class OrganizationRoleDetailApiResponse extends ApiResponse {
  Id?: string;
  RoleName?: string;
  FeatureIds: number[] = [];
}

export class OrganizationRoleMutationData {
  Id?: string;
  RoleName?: string;
  FeatureIds: number[] = [];
}
