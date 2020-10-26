import { ApiResponse } from '@models/common/response.model';

export class OrganizationRoleItem {
  ID?: string;
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
  ID?: string;
  RoleName?: string;
  FeatureIds: number[] = [];
}

export class OrganizationRoleMutationData {
  ID?: string;
  RoleName?: string;
  FeatureIds: number[] = [];
}
