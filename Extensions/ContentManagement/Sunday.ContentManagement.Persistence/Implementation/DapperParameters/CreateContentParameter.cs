using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Sunday.ContentManagement.Persistence.Entities;

namespace Sunday.ContentManagement.Persistence.Implementation.DapperParameters
{
    public class CreateContentParameter : BaseContentParameter, SqlMapper.IDynamicParameters
    {

        public void AddParameters(IDbCommand command, SqlMapper.Identity identity)
        {
            AddCommonParam(command);
            var sqlCommand = (SqlCommand)command;
            sqlCommand.Parameters.Add("@TemplateId", SqlDbType.UniqueIdentifier).Value = Content.TemplateId;
            sqlCommand.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Content.CreatedDate;
            sqlCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar, 500).Value = Content.CreatedBy;
        }

        public CreateContentParameter(ContentEntity content) : base(content)
        {
        }
    }
}
