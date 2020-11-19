import { ApiResponse } from '@models/common/response.model';

export class WebsiteItem extends ApiResponse {
  Id: string;
  WebsiteName: string;
  OrganizationId: string;
  HostNames: string[] = [];
  LayoutId: string;
  IsActive: boolean;
}

export class WebsiteList extends ApiResponse {
  Total: number;
  Websites: WebsiteItem[] = [];
}
