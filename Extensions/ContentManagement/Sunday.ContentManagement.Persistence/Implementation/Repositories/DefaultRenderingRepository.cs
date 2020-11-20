using System;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Persistence.Application;
using Sunday.ContentManagement.Persistence.Entities;
using Sunday.Core;
using Sunday.Core.Extensions;
using Sunday.Core.Models.Base;
using Sunday.DataAccess.SqlServer.Database;
using static LanguageExt.Prelude;

namespace Sunday.ContentManagement.Persistence.Implementation.Repositories
{
    [ServiceTypeOf(typeof(IRenderingRepository))]
    public class DefaultRenderingRepository : IRenderingRepository
    {
        private readonly StoredProcedureRunner _dRunner;

        public DefaultRenderingRepository(StoredProcedureRunner dRunner)
        {
            _dRunner = dRunner;
        }

        public Task<SearchResult<RenderingEntity>> Search(RenderingQuery query)
            => _dRunner.ExecuteMultipleAsync<int, RenderingEntity>(ProcedureNames.Renderings.Search, query)
                .MapResultTo(rs => new SearchResult<RenderingEntity>
                {
                    Total = rs.Item1.First(),
                    Result = rs.Item2
                });

        public Task<Option<RenderingEntity>> GetRenderingById(Guid id)
            => _dRunner.ExecuteAsync<RenderingEntity>(ProcedureNames.Renderings.GetById, new {Id = id}).MapResultTo(
                rs => Optional(rs.FirstOrDefault()));

        public Task Save(RenderingEntity rendering)
            => _dRunner.ExecuteAsync(ProcedureNames.Renderings.CreateOrUpdate, rendering);

        public Task Delete(Guid id)
            => _dRunner.ExecuteAsync(ProcedureNames.Renderings.Delete, new {Id = id});
    }
}
