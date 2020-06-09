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
    public class CreateUserDynamicParameter : BaseUpdateUserDynamicParameter, IDynamicParameters
    {

        public CreateUserDynamicParameter(ApplicationUser user) : base(user)
        {
        }

        public void AddParameters(IDbCommand command, Identity identity)
        {
            AddCommonParam(command, identity);
            var sqlCommand = (SqlCommand)command;
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = User.UserName;
            sqlCommand.Parameters.Add("@Domain", SqlDbType.NVarChar).Value = User.Domain;
            sqlCommand.Parameters.Add("@EmailConfirmed", SqlDbType.Bit).Value = User.EmailConfirmed;
            sqlCommand.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = User.CreatedBy;
            sqlCommand.Parameters.Add("@SecurityStamp", SqlDbType.NVarChar).Value = User.SecurityStamp;
            sqlCommand.Parameters.Add("@PasswordHash", SqlDbType.NVarChar).Value = User.PasswordHash;
        }
    }
}
