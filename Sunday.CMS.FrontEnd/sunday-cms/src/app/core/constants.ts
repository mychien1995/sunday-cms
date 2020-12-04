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
  public static Modules = {
    Get: 'cms/api/modules/',
  };
  public static Features = {
    GetByOrganization: 'cms/api/features/getByOrganization/',
  };
  public static Roles = {
    GetAll: 'cms/api/roles/',
    GetById: 'cms/api/roles/',
  };
  public static Layout = {
    GetNavigation: 'cms/api/layout/getNavigation/',
    GetLayout: 'cms/api/layout/getLayout/',
  };
  public static OrganizationRoles = {
    Search: 'cms/api/organizationroles/search/',
    Create: 'cms/api/organizationroles/create/',
    Update: 'cms/api/organizationroles/update/',
    GetById: 'cms/api/organizationroles/',
    Delete: 'cms/api/organizationroles/',
    BulkUpdate: 'cms/api/organizationroles/bulkUpdate/',
  };
  public static Organizations = {
    Search: 'cms/api/organizations/search',
    Create: 'cms/api/organizations/create',
    Edit: 'cms/api/organizations/update',
    GetById: 'cms/api/organizations/',
    Delete: 'cms/api/organizations/',
    Lookup: 'cms/api/organizations/lookup',
    Activate: 'cms/api/organizations/activate',
    Deactivate: 'cms/api/organizations/deactivate',
    GetModules: 'cms/api/modules/getByOrganization',
  };
  public static Layouts = {
    Search: 'cms/api/appLayout/search',
    Create: 'cms/api/appLayout/create',
    Edit: 'cms/api/appLayout/update',
    GetById: 'cms/api/appLayout/',
    Delete: 'cms/api/appLayout/',
  };
  public static Websites = {
    Search: 'cms/api/websites/search',
    Create: 'cms/api/websites/create',
    Edit: 'cms/api/websites/update',
    GetById: 'cms/api/websites/',
    Delete: 'cms/api/websites/',
    Activate: 'cms/api/websites/activate',
  };
  public static Templates = {
    Search: 'cms/api/templates/search',
    Create: 'cms/api/templates/create',
    Edit: 'cms/api/templates/update',
    GetById: 'cms/api/templates/',
    Delete: 'cms/api/templates/',
    GetFieldTypes: 'cms/api/templates/getFieldTypes',
    LoadFields: 'cms/api/templates/getFields',
  };
  public static ContentTree = {
    GetRoots: 'cms/api/contenttree/getRoots/',
    GetChilds: 'cms/api/contenttree/getChilds/',
    GetContextMenu: 'cms/api/contenttree/getContextMenu/',
    GetByPath: 'cms/api/contenttree/getTreeByPath/',
    GetByQuery: 'cms/api/contenttree/getTreeByQuery/',
  };
  public static Contents = {
    GetContent: 'cms/api/contents/',
    GetMultiples: 'cms/api/contents/getMultiples',
    Create: 'cms/api/contents/create/',
    Update: 'cms/api/contents/update/',
    UpdateExplicit: 'cms/api/contents/updateExplicit/',
    NewVersion: 'cms/api/contents/',
    Publish: 'cms/api/contents/',
    Move: 'cms/api/contents/move',
    Delete: 'cms/api/contents/',
  };
  public static Renderings = {
    Search: 'cms/api/renderings/search',
    Create: 'cms/api/renderings/create/',
    Update: 'cms/api/renderings/update/',
    Delete: 'cms/api/renderings/',
    GetById: 'cms/api/renderings/',
    GetRenderingTypes: 'cms/api/renderings/getRenderingTypes',
    GetAvailableForContent: 'cms/api/renderings/getAvailableForContent',
  };
  public static FileUpload = 'cms/api/FileUpload';
  public static FilePreview = 'cms/api/File/Preview';
}

export class StorageKey {
  public static UserData = 'app_user_data';
  public static NavigationData = 'app_navigation_data';
  public static LayoutData = 'app_layout_data';
}
export const DefaultLogo = 'assets/images/avatars/1.jpg';
export const DefaultOrganizationLogo = 'assets/images/logo-inverse.png';
