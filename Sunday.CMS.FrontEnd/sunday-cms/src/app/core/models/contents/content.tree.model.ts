import { ApiResponse } from '@models/common/response.model';
export class ContentTree extends ApiResponse {
  Roots: ContentTreeNode[] = [];
}

export class ContentTreeList extends ApiResponse {
  Nodes: ContentTreeNode[] = [];
}
export class ContentTreeNode extends ApiResponse {
  Id: string;
  Link: string;
  Icon: string;
  Name: string;
  Type: string;
  ParentId?: string;
  ChildNodes: ContentTreeNode[] = [];
  Open = false;
  Active = false;
}
