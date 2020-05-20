﻿using System;
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
            public const string Search = "sp_users_search";
            public const string Insert = "sp_users_insert";
        }
    }
}
