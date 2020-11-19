using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.SqlServer.Server;
using Sunday.Foundation.Persistence.Entities;

namespace Sunday.Foundation.Persistence.Implementation.DapperParameters
{
    public class SaveEntityAccessParameter : SqlMapper.IDynamicParameters
    {
        private readonly Guid _entityId;
        private readonly string _entityType;
        private readonly EntityAccessEntity[] _organizations;
        public SaveEntityAccessParameter(Guid entityId, string entityType, EntityAccessEntity[] organizations)
        {
            _organizations = organizations;
            _entityType = entityType;
            _entityId = entityId;
        }
        public void AddParameters(IDbCommand command, SqlMapper.Identity identity)
        {
            var sqlCommand = (SqlCommand)command;
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.Add("@EntityId", SqlDbType.UniqueIdentifier).Value = _entityId;
            sqlCommand.Parameters.Add("@EntityType", SqlDbType.VarChar, 50).Value = _entityType;
            if (!_organizations.Any()) return;
            var records = new List<SqlDataRecord>();
            foreach (var org in _organizations)
            {
                var rec = new SqlDataRecord(
                    new SqlMetaData("OrganizationId", SqlDbType.UniqueIdentifier),
                    new SqlMetaData("WebsiteIds", SqlDbType.NVarChar, SqlMetaData.Max),
                    new SqlMetaData("OrganizationRoleIds", SqlDbType.NVarChar, SqlMetaData.Max)
                );
                rec.SetValue(0, org.OrganizationId);
                rec.SetValue(1, org.WebsiteIds);
                rec.SetValue(2, org.OrganizationRoleIds);
                records.Add(rec);
            }
            var sqlParam = sqlCommand.Parameters.Add("@EntityAccess", SqlDbType.Structured);
            sqlParam.Direction = ParameterDirection.Input;
            sqlParam.TypeName = "EntityAccessOrganizationType";
            sqlParam.Value = records;
        }
    }
}
