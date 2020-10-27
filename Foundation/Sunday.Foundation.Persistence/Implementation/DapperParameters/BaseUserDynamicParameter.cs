using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.SqlServer.Server;
using Sunday.Foundation.Persistence.Entities;
using Sunday.Foundation.Persistence.Extensions;

namespace Sunday.Foundation.Persistence.Implementation.DapperParameters
{
    public class BaseUserDynamicParameter
    {
        protected readonly UserEntity User;
        public BaseUserDynamicParameter(UserEntity user)
        {
            this.User = user;
        }
        protected void AddCommonParam(IDbCommand command)
        {
            var sqlCommand = (SqlCommand)command;
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = User.Id;
            if (User.OrganizationUsers.Any())
            {
                var organizationUserRecords = new List<SqlDataRecord>();
                foreach (var param in User.OrganizationUsers)
                {
                    var rec = new SqlDataRecord(
                        new SqlMetaData("OrganizationId", SqlDbType.UniqueIdentifier),
                        new SqlMetaData("IsActive", SqlDbType.Bit)
                       );
                    rec.SetValue(0, param.OrganizationId);
                    rec.SetValue(1, param.IsActive);
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
                        new SqlMetaData("OrganizationId", SqlDbType.UniqueIdentifier),
                        new SqlMetaData("OrganizationRolesId", SqlDbType.NVarChar, 1000)
                       );
                    rec.SetValue(0, param.Organization?.Id ?? param.OrganizationId);
                    rec.SetValue(1, User.VirtualRoles.Select(r => r.OrganizationId).ToDatabaseList());
                    organizationRoleRecords.Add(rec);
                }
                var organizationRoleParam = sqlCommand.Parameters.Add("@OrganizationRoles", SqlDbType.Structured);
                organizationRoleParam.Direction = ParameterDirection.Input;
                organizationRoleParam.TypeName = "OrganizationUserRoleType";
                organizationRoleParam.Value = organizationRoleRecords;
            }
            if (User.Roles.Any())
            {
                sqlCommand.Parameters.Add("@RoleIds", SqlDbType.NVarChar).Value = User.RoleIds;
            }
            sqlCommand.Parameters.Add("@Fullname", SqlDbType.NVarChar).Value = User.Fullname;
            sqlCommand.Parameters.Add("@Email", SqlDbType.NVarChar).Value = User.Email;
            sqlCommand.Parameters.Add("@Phone", SqlDbType.NVarChar).Value = User.Phone;
            sqlCommand.Parameters.Add("@IsActive", SqlDbType.Bit).Value = User.IsActive;
            sqlCommand.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar).Value = User.UpdatedBy;
        }
    }
}
