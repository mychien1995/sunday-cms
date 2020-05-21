export class ApiUrl {
  public static Login = 'Authentication/Token';
  public static Users = {
    Search : 'cms/api/users/search',
    Create: 'cms/api/users/create'
  };
  public static Roles = {
    GetAll : 'cms/api/roles/'
  };
}

export class StorageKey {
  public static UserData = 'app_user_data';
}
