using Dapper;
using Sunday.Users.Core;
using System.Data;
using System.Data.SqlClient;
using static Dapper.SqlMapper;

namespace Sunday.Users.Implementation
{
    public class UpdateUserDynamicParamter : BaseUpdateUserDynamicParameter, IDynamicParameters
    {

        public UpdateUserDynamicParamter(ApplicationUser user) : base(user)
        {
        }

        public void AddParameters(IDbCommand command, SqlMapper.Identity identity)
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
