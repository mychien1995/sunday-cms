import { ApiResponse } from '@models/common/response.model';
import { RoleModel } from '../roles/roles.models';
import { OrganizationUserItem } from '@models/organizations/organization.model';

export class UserList extends ApiResponse {
  Total: number;
  Users: UserItem[];
}

export class UserItem {
  Id: number;
  UserName: string;
  Email: string;
  Domain: string;
  Roles?: RoleModel[];
  Fullname: string;
  IsActive: boolean;
}

export class UserMutationModel {
  Id?: string;
  UserName: string;
  Fullname: string;
  Email: string;
  Password: string;
  Phone: string;
  IsActive: boolean;
  Domain: string;
  RoleIds: string[] = [];
  OrganizationRoleIds?: string[] = [];
  Organizations: OrganizationUserItem[] = [];
}

export class CreateUserResponse extends ApiResponse {
  UserId: string;
}

export class UserDetailResponse extends ApiResponse {
  constructor() {
    super();
    this.Organizations = [];
    this.RoleIds = [];
    this.OrganizationRoleIds = [];
  }
  Id?: string;
  UserName: string;
  Fullname: string;
  Email: string;
  Phone: string;
  IsActive: boolean;
  Domain: string;
  RoleIds: number[] = [];
  OrganizationRoleIds?: string[] = [];
  Organizations: OrganizationUserItem[] = [];
  AvatarLink?: string;
}

export class ChangePasswordModel {
  OldPassword: string;
  NewPassword: string;
}
