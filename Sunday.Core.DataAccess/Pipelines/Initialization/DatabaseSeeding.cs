﻿using Microsoft.Extensions.Configuration;
using Sunday.Core.DataAccess.Database;
using Sunday.Core.Ultilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Sunday.Core.DataAccess.Pipelines.Initialization
{
    public class DatabaseSeeding
    {
        private readonly StoredProcedureRunner _databaseRunner;
        public DatabaseSeeding(StoredProcedureRunner storeProcRunner)
        {
            _databaseRunner = storeProcRunner;
        }
        public void Process(PipelineArg arg)
        {
            var adminPassword = ApplicationSettings.Get("Sunday.DefaultAdminPassword") ?? "123456a@";
            var securityHash = Guid.NewGuid().ToString("N");
            var adminPasswordHash = EncryptUtils.SHA256Encrypt(adminPassword, securityHash);
            var passwordParam = new SqlParameter("@PasswordHash", adminPasswordHash);
            var securityHashParam = new SqlParameter("@SecurityStamp", securityHash);
            var roleType = new DataTable("RoleType");
            roleType.Columns.Add("ID", typeof(int));
            roleType.Columns.Add("Code", typeof(string));
            roleType.Columns.Add("RoleName", typeof(string));
            roleType.Columns.Add("Description", typeof(string));
            foreach (var role in Enum.GetValues(typeof(SystemRoles)).Cast<SystemRoles>())
            {
                var row = roleType.NewRow();
                row["ID"] = (int)role;
                row["RoleName"] = role.ToString();
                var roleCode = "";
                switch (role)
                {
                    case SystemRoles.Developer:
                        roleCode = SystemRoleCodes.Developer;
                        break;
                    case SystemRoles.OrganizationAdmin:
                        roleCode = SystemRoleCodes.OrganizationAdmin;
                        break;
                    case SystemRoles.OrganizationUser:
                        roleCode = SystemRoleCodes.OrganizationUser;
                        break;
                    case SystemRoles.SystemAdmin:
                        roleCode = SystemRoleCodes.SystemAdmin;
                        break;
                    default:
                        break;
                }
                if (!string.IsNullOrEmpty(roleCode))
                {
                    row["Code"] = roleCode;
                    roleType.Rows.Add(row);
                }
            }
            var roleTypeParam = new SqlParameter
            {
                ParameterName = "@RoleType",
                SqlDbType = SqlDbType.Structured,
                Value = roleType,
                TypeName = "dbo.RoleType",
                Direction = ParameterDirection.Input
            };
            _databaseRunner.Execute(ProcedureNames.DatabaseSeeding, passwordParam, securityHashParam, roleTypeParam);
        }
    }
}
