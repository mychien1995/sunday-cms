using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Organizations.Implementation
{
    public class ProcedureNames
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
    }
}
