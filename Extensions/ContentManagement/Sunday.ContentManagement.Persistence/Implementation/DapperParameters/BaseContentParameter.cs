using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.SqlServer.Server;
using Sunday.ContentManagement.Persistence.Entities;

namespace Sunday.ContentManagement.Persistence.Implementation.DapperParameters
{
    public class BaseContentParameter
    {
        protected readonly ContentEntity Content;
        public BaseContentParameter(ContentEntity content)
        {
            Content = content;
        }

        protected void AddCommonParam(IDbCommand command)
        {
            var sqlCommand = (SqlCommand)command;
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = Content.Id;
            sqlCommand.Parameters.Add("@Name", SqlDbType.NVarChar, 1000).Value = Content.Name;
            sqlCommand.Parameters.Add("@DisplayName", SqlDbType.NVarChar, 1000).Value = Content.DisplayName;
            sqlCommand.Parameters.Add("@Path", SqlDbType.NVarChar, 2000).Value = Content.Path;
            sqlCommand.Parameters.Add("@ParentId", SqlDbType.UniqueIdentifier).Value = Content.ParentId;
            sqlCommand.Parameters.Add("@ParentType", SqlDbType.Int).Value = Content.ParentType;
            sqlCommand.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = Content.UpdatedDate;
            sqlCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar, 500).Value = Content.UpdatedBy;
            sqlCommand.Parameters.Add("@SortOrder", SqlDbType.Int).Value = 0;
            var activeVersion = Content.Versions.FirstOrDefault(v => v.IsActive);
            sqlCommand.Parameters.Add("@WorkId", SqlDbType.UniqueIdentifier).Value = activeVersion?.Id;
            var fields = Content.Fields;
            if (activeVersion != null && activeVersion.Fields.Any()) fields = activeVersion.Fields;
            if (fields.Any())
            {
                var fieldRecords = new List<SqlDataRecord>();
                foreach (var param in fields)
                {
                    var rec = new SqlDataRecord(
                        new SqlMetaData("Id", SqlDbType.UniqueIdentifier),
                        new SqlMetaData("FieldValue", SqlDbType.NVarChar, SqlMetaData.Max),
                        new SqlMetaData("TemplateFieldId", SqlDbType.UniqueIdentifier)
                    );
                    rec.SetValue(0, param.Id);
                    rec.SetValue(1, param.FieldValue);
                    rec.SetValue(2, param.TemplateFieldId);
                    fieldRecords.Add(rec);
                }
                var sqlParam = sqlCommand.Parameters.Add("@Fields", SqlDbType.Structured);
                sqlParam.Direction = ParameterDirection.Input;
                sqlParam.TypeName = "ContentFieldType";
                sqlParam.Value = fieldRecords;
            }
        }
    }
}
