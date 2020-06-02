using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.VirtualRoles.Implementation
{
    internal class ProcedureNames
    {
        public class OrganizationRoles
        {
            public const string GetByOrganization = "sp_organizationRoles_getByOrganization";
            public const string GetById = "sp_organizationRoles_getById";
            public const string Delete = "sp_organizationRoles_delete";
            public const string Update = "sp_organizationRoles_update";
            public const string Create = "sp_organizationRoles_create";
        }
    }
}
