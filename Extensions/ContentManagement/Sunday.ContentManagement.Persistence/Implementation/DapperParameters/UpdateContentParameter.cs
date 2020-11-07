using System;
using System.Data;
using Dapper;
using Sunday.ContentManagement.Persistence.Entities;

namespace Sunday.ContentManagement.Persistence.Implementation.DapperParameters
{
    public class UpdateContentParameter : BaseContentParameter, SqlMapper.IDynamicParameters
    {

        public void AddParameters(IDbCommand command, SqlMapper.Identity identity)
        {
            AddCommonParam(command);
        }

        public UpdateContentParameter(ContentEntity content) : base(content)
        {
        }
    }
}