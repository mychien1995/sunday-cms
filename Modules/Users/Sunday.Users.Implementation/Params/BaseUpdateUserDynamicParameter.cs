using Dapper;
using Microsoft.SqlServer.Server;
using Sunday.Users.Core;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Sunday.Users.Implementation
{
    public class BaseUpdateUserDynamicParameter
    {
        protected readonly ApplicationUser User;
        public BaseUpdateUserDynamicParameter(ApplicationUser user)
        {
            this.User = user;
        }
        protected void AddCommonParam(IDbCommand command, SqlMapper.Identity identity)
        {
            var sqlCommand = (SqlCommand)command;
            sqlCommand.CommandType = CommandType.StoredProcedure;
            if (User.OrganizationUsers.Any())
            {
                var organizationUserRecords = new List<SqlDataRecord>();
                foreach (var param in User.OrganizationUsers)
                {
                    var rec = new SqlDataRecord(
                        new SqlMetaData("OrganizationId", SqlDbType.Int),
                        new SqlMetaData("IsActive", SqlDbType.Bit)
                       );
                    rec.SetInt32(0, param.Organization.ID);
                    rec.SetBoolean(1, param.IsActive);
                    organizationUserRecords.Add(rec);
                }
                var organizationUserParam = sqlCommand.Parameters.Add("@Organizations", SqlDbType.Structured);
                organizationUserParam.Direction = ParameterDirection.Input;
                organizationUserParam.TypeName = "OrganizationUserType";
                organizationUserParam.Value = organizationUserRecords;
            }

            if (User.VirtualRoles.Any())
            {
                var organizationRoleRecords = new List<SqlDataRecord>();
                foreach (var param in User.VirtualRoles)
                {
                    var rec = new SqlDataRecord(
                        new SqlMetaData("OrganizationId", SqlDbType.Int),
                        new SqlMetaData("OrganizationRolesId", SqlDbType.NVarChar, 1000)
                       );
                    rec.SetInt32(0, param.Organization?.ID ?? param.OrganizationId);
                    var roles = string.Join(",", User.VirtualRoles.Where(x => x.OrganizationId == (param.Organization?.ID ?? param.OrganizationId)).Select(x => x.ID));
                    rec.SetString(1, roles);
                    organizationRoleRecords.Add(rec);
                }
                var organizationRoleParam = sqlCommand.Parameters.Add("@OrganizationRoles", SqlDbType.Structured);
                organizationRoleParam.Direction = ParameterDirection.Input;
                organizationRoleParam.TypeName = "OrganizationUserRoleType";
                organizationRoleParam.Value = organizationRoleRecords;
            }

            if (User.Roles.Any())
            {
                var RoleIds = string.Join(",", User.Roles.Select(x => x.ID));
                sqlCommand.Parameters.Add("@RoleIds", SqlDbType.NVarChar).Value = RoleIds;
            }

            sqlCommand.Parameters.Add("@Fullname", SqlDbType.NVarChar).Value = User.Fullname;
            sqlCommand.Parameters.Add("@Email", SqlDbType.NVarChar).Value = User.Email;
            sqlCommand.Parameters.Add("@Phone", SqlDbType.NVarChar).Value = User.Phone;
            sqlCommand.Parameters.Add("@IsActive", SqlDbType.Bit).Value = User.IsActive;
            sqlCommand.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar).Value = User.UpdatedBy;
        }
    }
}
