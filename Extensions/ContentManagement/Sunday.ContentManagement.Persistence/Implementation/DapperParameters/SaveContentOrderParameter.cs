using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.SqlServer.Server;
using Sunday.ContentManagement.Models;

namespace Sunday.ContentManagement.Persistence.Implementation.DapperParameters
{
    public class SaveContentOrderParameter : SqlMapper.IDynamicParameters
    {
        public SaveContentOrderParameter(ContentOrder[] orders)
        {
            Orders = orders;
        }

        private ContentOrder[] Orders { get; }
        public void AddParameters(IDbCommand command, SqlMapper.Identity identity)
        {
            var sqlCommand = (SqlCommand)command;
            sqlCommand.CommandType = CommandType.StoredProcedure;
            var records = new List<SqlDataRecord>();
            foreach (var param in Orders)
            {
                var rec = new SqlDataRecord(
                    new SqlMetaData("ContentId", SqlDbType.UniqueIdentifier),
                    new SqlMetaData("Order", SqlDbType.Int)
                );
                rec.SetValue(0, param.ContentId);
                rec.SetValue(1, param.Order);
                records.Add(rec);
            }
            var sqlParam = sqlCommand.Parameters.Add("@Orders", SqlDbType.Structured);
            sqlParam.Direction = ParameterDirection.Input;
            sqlParam.TypeName = "ContentOrderType";
            sqlParam.Value = records;
        }
    }
}
