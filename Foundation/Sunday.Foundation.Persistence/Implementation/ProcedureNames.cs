using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Foundation.Implementation
{
    internal class ProcedureNames
    {
        public class Organizations
        {
            public const string GetById = "sp_organizations_getById";
            public const string Search = "sp_organizations_search";
            public const string Insert = "sp_organizations_create";
            public const string Update = "sp_organizations_update";
            public const string Delete = "sp_organizations_delete";
            public const string Activate = "sp_organizations_activate";
            public const string Deactivate = "sp_organizations_deactivate";
        }
        public class Users
        {
            public const string GetByIdWithOptions = "sp_users_getById_withOptions";
            public const string Search = "sp_users_search";
            public const string Insert = "sp_users_insert";
            public const string Update = "sp_users_update";
            public const string Delete = "sp_users_delete";
            public const string Activate = "sp_users_activate";
            public const string Deactivate = "sp_users_deactivate";
            public const string ChangePassword = "sp_users_changePassword";
            public const string FindUserByUserName = "sp_users_findbyusername";
        }
        public class Modules
        {
            public const string GetAll = "sp_modules_getAll";
        }

        public class Features
        {
            public const string GetByModules = "sp_features_getByModules";
        }
        public class Roles
        {
            public const string GetAll = "sp_roles_getAll";
            public const string GetById = "sp_roles_getById";
        }
        public class OrganizationRoles
        {
            public const string Search = "sp_organizationRoles_search";
            public const string GetById = "sp_organizationRoles_getById";
            public const string Delete = "sp_organizationRoles_delete";
            public const string Update = "sp_organizationRoles_update";
            public const string Create = "sp_organizationRoles_create";
            public const string BulkUpdate = "sp_organizationRoles_bulkUpdate";
        }
        public class EntityAccess
        {
            public const string Save = "sp_entityAccess_save"; 
            public const string GetByEntity = "sp_entityAccess_getByEntity";
            public const string GetByEntities = "sp_entityAccess_getByEntities";
            public const string GetByOrganization = "sp_entityAccess_getByOrganization";
        }
    }
}
