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
    public class UpdateUserDynamicParamter : IDynamicParameters
    {
        private readonly ApplicationUser _user;

        public UpdateUserDynamicParamter(ApplicationUser user)
        {
            _user = user;
        }

        public void AddParameters(IDbCommand command, Identity identity)
        {
            var sqlCommand = (SqlCommand)command;
            sqlCommand.CommandType = CommandType.StoredProcedure;
            var items = new List<SqlDataRecord>();
            var p = sqlCommand.Parameters.Add("@Organizations", SqlDbType.Structured);
            p.Direction = ParameterDirection.Input;
            p.TypeName = "OrganizationUserType";
            p.Value = null;
            if (_user.OrganizationUsers.Any())
            {
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
                p.Value = items;
            }
            sqlCommand.Parameters.Add("@ID", SqlDbType.Int).Value = _user.ID;
            sqlCommand.Parameters.Add("@Fullname", SqlDbType.NVarChar).Value = _user.Fullname;
            sqlCommand.Parameters.Add("@Email", SqlDbType.NVarChar).Value = _user.Email;
            sqlCommand.Parameters.Add("@Phone", SqlDbType.NVarChar).Value = _user.Phone;
            sqlCommand.Parameters.Add("@IsActive", SqlDbType.Bit).Value = _user.IsActive;
            sqlCommand.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar).Value = _user.UpdatedBy;
            sqlCommand.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = _user.UpdatedDate;
            sqlCommand.Parameters.Add("@AvatarBlobUri", SqlDbType.NVarChar).Value = _user.AvatarBlobUri;
            if (_user.Roles.Any())
            {
                var RoleIds = string.Join(",", _user.Roles.Select(x => x.ID));
                sqlCommand.Parameters.Add("@RoleIds", SqlDbType.NVarChar).Value = RoleIds;
            }
        }
    }
}
