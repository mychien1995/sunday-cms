import { ApiResponse } from '@models/common/response.model';

export class OrganizationRoleItem {
  ID?: number;
  RoleName?: string;
  CreatedBy?: string;
  UpdatedBy?: string;
  CreatedDate?: number;
  UpdatedDate?: number;
}

export class OrganizationRoleListApiResponse extends ApiResponse {
  Total?: number;
  Roles: OrganizationRoleItem[] = [];
}

export class OrganizationRoleDetailApiResponse extends ApiResponse {
  ID?: number;
  RoleName?: string;
  FeatureIds: number[] = [];
}

export class OrganizationRoleMutationData {
  ID?: number;
  RoleName?: string;
  FeatureIds: number[] = [];
}
