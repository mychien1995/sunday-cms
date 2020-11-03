using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.SqlServer.Server;
using Sunday.ContentManagement.Persistence.Entities;

namespace Sunday.ContentManagement.Persistence.Implementation.DapperParameters
{
    public class SaveTemplatePropertiesParameter : SqlMapper.IDynamicParameters
    {
        private readonly TemplateEntity _template;

        public SaveTemplatePropertiesParameter(TemplateEntity template)
        {
            this._template = template;
        }

        public void AddParameters(IDbCommand command, SqlMapper.Identity identity)
        {
            var sqlCommand = (SqlCommand)command;
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.Add("@TemplateId", SqlDbType.UniqueIdentifier).Value = _template.Id;
            if (!_template.Fields.Any()) return;
            var records = new List<SqlDataRecord>();
            foreach (var field in _template.Fields)
            {
                var rec = new SqlDataRecord(
                    new SqlMetaData("Id", SqlDbType.UniqueIdentifier),
                    new SqlMetaData("FieldName", SqlDbType.NVarChar, 500),
                    new SqlMetaData("DisplayName", SqlDbType.NVarChar, 1000),
                    new SqlMetaData("FieldType", SqlDbType.Int),
                    new SqlMetaData("Title", SqlDbType.NVarChar, 1000),
                    new SqlMetaData("IsUnversioned", SqlDbType.Bit),
                    new SqlMetaData("Properties", SqlDbType.NVarChar, 1000),
                    new SqlMetaData("Section", SqlDbType.VarChar, 500),
                    new SqlMetaData("SortOrder", SqlDbType.Int),
                    new SqlMetaData("IsRequired", SqlDbType.Bit),
                    new SqlMetaData("ValidationRules", SqlDbType.VarChar, 500)
                );
                rec.SetValue(0, field.Id);
                rec.SetValue(1, field.FieldName);
                rec.SetValue(2, field.DisplayName);
                rec.SetValue(3, field.FieldType);
                rec.SetValue(4, field.Title);
                rec.SetValue(5, field.IsUnversioned);
                rec.SetValue(6, field.Properties);
                rec.SetValue(7, "");
                rec.SetValue(8, field.SortOrder);
                rec.SetValue(9, field.IsRequired);
                rec.SetValue(10, "");
                records.Add(rec);
            }
            var fieldsParam = sqlCommand.Parameters.Add("@Fields", SqlDbType.Structured);
            fieldsParam.Direction = ParameterDirection.Input;
            fieldsParam.TypeName = "TemplateFieldType";
            fieldsParam.Value = records;
        }
    }
}
