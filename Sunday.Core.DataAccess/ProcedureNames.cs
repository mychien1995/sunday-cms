using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Core.DataAccess
{
    public class ProcedureNames
    {
        public const string ClearSchemaVersion = "sp_clearschemaversion";
        public const string DatabaseSeeding = "sp_database_seeding";
        public class Users
        {
            public const string FindUserByUserName = "sp_users_findbyusername";
            public const string GetById = "sp_users_getById";
            public const string GetByIdWithOptions = "sp_users_getById_withOptions";
            public const string Search = "sp_users_search";
            public const string Insert = "sp_users_insert";
            public const string Update = "sp_users_update";
            public const string Delete = "sp_users_delete";
            public const string FetchRoles = "sp_users_fetchRoles";
            public const string Activate = "sp_users_activate";
            public const string Deactivate = "sp_users_deactivate";
            public const string ChangePassword = "sp_users_changePassword";
        }

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

        public class Roles
        {
            public const string GetAll = "sp_roles_getAll";
            public const string GetById = "sp_roles_getById";
        }
    }
}
