import { ApiResponse } from '@models/common/response.model';

export class OrganizationList extends ApiResponse {
  Total: number;
  Organizations: OrganizationItem[];
}

export class OrganizationItem {
  ID: number;
  OrganizationName: string;
  LogoLink: string;
  CreatedBy: string;
  CreatedDate: number;
  UpdatedBy: string;
  UpdatedDate: number;
  IsActive: boolean;
}

export class OrganizationMutationModel {
  ID?: number;
  OrganizationName: string;
  LogoBlobUri: string;
  Description: string;
  ColorScheme: string;
  IsActive: boolean;
  HostNames: string[];
}

export class OrganizationDetailResponse extends ApiResponse {
  ID?: number;
  OrganizationName: string;
  LogoLink: string;
  Description: string;
  LogoBlobUri: string;
  ColorScheme: string;
  IsActive: boolean;
  HostNames: string[];
}
