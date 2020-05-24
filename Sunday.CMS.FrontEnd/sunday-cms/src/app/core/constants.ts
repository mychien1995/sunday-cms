export class ApiUrl {
  public static Login = 'Authentication/Token';
  public static Users = {
    Search : 'cms/api/users/search',
    Create: 'cms/api/users/create',
    Edit: 'cms/api/users/update',
    GetById: 'cms/api/users/',
    Delete: 'cms/api/users/',
    Activate : 'cms/api/users/activate',
    Deactivate : 'cms/api/users/deactivate',
    ResetPassword : 'cms/api/users/resetPassword'
  };
  public static Roles = {
    GetAll : 'cms/api/roles/'
  };
}

export class StorageKey {
  public static UserData = 'app_user_data';
}
