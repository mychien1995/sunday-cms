import { ApiResponse } from '@models/common/response.model';

export class UserList extends ApiResponse {
  Total: number;
  Users: UserItem[];
}

export class UserItem {
  ID: number;
  UserName: string;
  Email: string;
  Fullname: string;
  IsActive: boolean;
}

export class UserMutationModel {
  ID?: number;
  UserName: string;
  Fullname: string;
  Email: string;
  Password: string;
  Phone: string;
  IsActive: boolean;
  Domain: string;
  RoleIds: string[] = [];
}

export class CreateUserResponse extends ApiResponse {
  UserId: number;
}

export class UserDetailResponse extends ApiResponse {
  ID?: number;
  UserName: string;
  Fullname: string;
  Email: string;
  Phone: string;
  IsActive: boolean;
  Domain: string;
  RoleIds: number[] = [];
}
