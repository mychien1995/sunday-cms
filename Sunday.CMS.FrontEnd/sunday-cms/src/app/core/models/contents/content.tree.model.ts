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
  Type: number;
  ParentId?: string;
  ChildNodes: ContentTreeNode[] = [];
  Open = false;
  Active = false;
}
export class ContextMenu extends ApiResponse {
  Items: ContextMenuItem[] = [];
}

export class ContextMenuItem {
  Icon?: string;
  Command?: string;
  Title?: string;
  Hint?: string;
}
