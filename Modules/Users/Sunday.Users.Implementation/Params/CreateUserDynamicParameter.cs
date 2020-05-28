using Dapper;
using Microsoft.SqlServer.Server;
using Sunday.Core.Domain.Organizations;
using Sunday.Users.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static Dapper.SqlMapper;

namespace Sunday.Users.Implementation
{
    public class CreateUserDynamicParameter : IDynamicParameters
    {
        private readonly ApplicationUser _user;

        public CreateUserDynamicParameter(ApplicationUser user)
        {
            _user = user;
        }

        public void AddParameters(IDbCommand command, Identity identity)
        {
            var sqlCommand = (SqlCommand)command;
            sqlCommand.CommandType = CommandType.StoredProcedure;
            var items = new List<SqlDataRecord>();
            foreach (var param in _user.OrganizationUsers)
            {
                var rec = new SqlDataRecord(
                    new SqlMetaData("OrganizationId", SqlDbType.Int),
                    new SqlMetaData("IsActive", SqlDbType.Bit)
                   );
                rec.SetInt32(0, param.Organization.ID);
                rec.SetBoolean(1, param.IsActive);
                items.Add(rec);
            }

            var p = sqlCommand.Parameters.Add("@Organizations", SqlDbType.Structured);
            p.Direction = ParameterDirection.Input;
            p.TypeName = "OrganizationUserType";
            p.Value = items;
            sqlCommand.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = _user.UserName;
            sqlCommand.Parameters.Add("@Fullname", SqlDbType.NVarChar).Value = _user.Fullname;
            sqlCommand.Parameters.Add("@Email", SqlDbType.NVarChar).Value = _user.Email;
            sqlCommand.Parameters.Add("@Phone", SqlDbType.NVarChar).Value = _user.Phone;
            sqlCommand.Parameters.Add("@IsActive", SqlDbType.Bit).Value = _user.IsActive;
            sqlCommand.Parameters.Add("@Domain", SqlDbType.NVarChar).Value = _user.Domain;
            sqlCommand.Parameters.Add("@EmailConfirmed", SqlDbType.Bit).Value = _user.EmailConfirmed;
            sqlCommand.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = _user.CreatedBy;
            sqlCommand.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar).Value = _user.UpdatedBy;
            sqlCommand.Parameters.Add("@SecurityStamp", SqlDbType.NVarChar).Value = _user.SecurityStamp;
            sqlCommand.Parameters.Add("@PasswordHash", SqlDbType.NVarChar).Value = _user.PasswordHash;
            var RoleIds = string.Join(",", _user.Roles.Select(x => x.ID));
            sqlCommand.Parameters.Add("@RoleIds", SqlDbType.NVarChar).Value = RoleIds;
        }
    }
}
