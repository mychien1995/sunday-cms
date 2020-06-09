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
    public class UpdateUserDynamicParamter : BaseUpdateUserDynamicParameter, IDynamicParameters
    {

        public UpdateUserDynamicParamter(ApplicationUser user) : base(user)
        {
        }

        public void AddParameters(IDbCommand command, Identity identity)
        {
            AddCommonParam(command, identity);
            var sqlCommand = (SqlCommand)command;
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.Add("@ID", SqlDbType.Int).Value = User.ID;
            sqlCommand.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = User.UpdatedDate;
            sqlCommand.Parameters.Add("@AvatarBlobUri", SqlDbType.NVarChar).Value = User.AvatarBlobUri;

        }
    }
}
