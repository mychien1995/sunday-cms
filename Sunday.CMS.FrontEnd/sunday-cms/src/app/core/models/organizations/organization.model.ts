import { ApiResponse } from '@models/common/response.model';

export class OrganizationList extends ApiResponse {
  Total: number;
  Organizations: OrganizationItem[];
}

export class OrganizationItem {
  ID: string;
  OrganizationName: string;
  LogoLink: string;
  CreatedBy: string;
  CreatedDate: number;
  UpdatedBy: string;
  UpdatedDate: number;
  IsActive: boolean;
}

export class OrganizationMutationModel {
  ID?: string;
  OrganizationName: string;
  LogoBlobUri: string;
  Description: string;
  ColorScheme: string;
  IsActive: boolean;
  HostNames: string[];
  ModuleIds: number[];
}

export class OrganizationDetailResponse extends ApiResponse {
  ID?: string;
  OrganizationName: string;
  LogoLink: string;
  Description: string;
  LogoBlobUri: string;
  ColorScheme: string;
  IsActive: boolean;
  HostNames: string[];
  ModuleIds: string[];
}

export class OrganizationLookupResponse extends ApiResponse {
  List: OrganizationItem[];
}

export class OrganizationUserItem {
  OrganizationName: string;
  OrganizationId: number;
  IsActive: boolean;
}
