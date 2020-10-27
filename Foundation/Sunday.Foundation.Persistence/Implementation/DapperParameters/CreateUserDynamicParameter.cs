using System.Data;
using System.Data.SqlClient;
using Dapper;
using Sunday.Foundation.Persistence.Entities;

namespace Sunday.Foundation.Persistence.Implementation.DapperParameters
{
    public class CreateUserDynamicParameter : BaseUserDynamicParameter, SqlMapper.IDynamicParameters
    {
        public CreateUserDynamicParameter(UserEntity user) : base(user)
        {
        }

        public void AddParameters(IDbCommand command, SqlMapper.Identity identity)
        {
            AddCommonParam(command);
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
