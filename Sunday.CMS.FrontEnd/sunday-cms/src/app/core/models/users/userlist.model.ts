export class UserList {
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
