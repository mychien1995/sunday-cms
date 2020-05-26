export class ApiUrl {
  public static Login = 'Authentication/Token';
  public static Users = {
    Search: 'cms/api/users/search',
    Create: 'cms/api/users/create',
    Edit: 'cms/api/users/update',
    GetById: 'cms/api/users/',
    Delete: 'cms/api/users/',
    Activate: 'cms/api/users/activate',
    Deactivate: 'cms/api/users/deactivate',
    ResetPassword: 'cms/api/users/resetPassword',
  };
  public static Profile = {
    Get: 'cms/api/profile/',
    Update: 'cms/api/profile/',
    ChangePassword: 'cms/api/profile/changePassword/',
    ChangeAvatar: 'cms/api/profile/changeAvatar/',
  };
  public static Roles = {
    GetAll: 'cms/api/roles/',
    GetById: 'cms/api/roles/',
  };
  public static Organizations = {
    Search: 'cms/api/organizations/search',
    Create: 'cms/api/organizations/create',
    Edit: 'cms/api/organizations/update',
    GetById: 'cms/api/organizations/',
    Delete: 'cms/api/organizations/',
    Activate: 'cms/api/organizations/activate',
    Deactivate: 'cms/api/organizations/deactivate'
  };
  public static FileUpload = 'cms/api/FileUpload';
}

export class StorageKey {
  public static UserData = 'app_user_data';
}
export const DefaultLogo = 'assets/images/avatars/1.jpg';
export const DefaultOrganizationLogo = 'assets/images/logo-inverse.png';
