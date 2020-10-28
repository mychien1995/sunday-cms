import { ApiResponse } from '@models/common/response.model';

export class NavigationTree {
  Sections?: NavigationSection[] = [];
  CreatedDate?: number;
}

export class NavigationSection {
  Section?: string;
  Items?: NavigationItem[] = [];
}

export class NavigationItem {
  Title?: string;
  Link?: string;
  IconClass?: string;
}

export class LayoutModel extends ApiResponse {
  OrganizationName: string;
  Color: string;
  LogoUri: string;
  CreatedDate?: number;
}
