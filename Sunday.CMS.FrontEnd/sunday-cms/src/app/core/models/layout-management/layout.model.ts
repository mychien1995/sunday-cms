import { ApiResponse } from '@models/common/response.model';
export class LayoutItem extends ApiResponse {
  Id: string;
  LayoutName: string;
  LayoutPath: string;
  OrganizationIds: string[] = [];
}

export class LayoutList extends ApiResponse {
  Total: number;
  Layouts: LayoutItem[] = [];
}
