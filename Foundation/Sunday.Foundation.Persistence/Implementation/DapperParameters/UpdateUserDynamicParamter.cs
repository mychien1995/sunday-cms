using System.Data;
using System.Data.SqlClient;
using Dapper;
using Sunday.Foundation.Persistence.Entities;

namespace Sunday.Foundation.Persistence.Implementation.DapperParameters
{
    public class UpdateUserDynamicParameter : BaseUserDynamicParameter, SqlMapper.IDynamicParameters
    {

        public UpdateUserDynamicParameter(UserEntity user) : base(user)
        {
        }

        public void AddParameters(IDbCommand command, SqlMapper.Identity identity)
        {
            AddCommonParam(command);
            var sqlCommand = (SqlCommand)command;
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = User.UpdatedDate;
            sqlCommand.Parameters.Add("@AvatarBlobUri", SqlDbType.NVarChar).Value = User.AvatarBlobUri;

        }
    }
}
