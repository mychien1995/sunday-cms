using System;
using System.Linq;
using System.Threading.Tasks;
using Sunday.ContentManagement.Persistence.Application;
using Sunday.ContentManagement.Persistence.Entities;
using Sunday.Core;
using Sunday.Core.Extensions;
using Sunday.Core.Models.Base;
using Sunday.DataAccess.SqlServer.Attributes;
using Sunday.DataAccess.SqlServer.Database;
using Sunday.Foundation.Models;

namespace Sunday.ContentManagement.Persistence.Implementation.Repositories
{
    [ServiceTypeOf(typeof(ILayoutRepository))]
    public class DefaultLayoutRepository : ILayoutRepository
    {
        private readonly StoredProcedureRunner _dbRunner;
        public DefaultLayoutRepository(StoredProcedureRunner dbRunner)
        {
            _dbRunner = dbRunner;
        }
        public Task<SearchResult<LayoutEntity>> QueryAsync(LayoutQuery query)
            => _dbRunner.ExecuteMultipleAsync<int, LayoutEntity, OrganizationLayoutEntity>(ProcedureNames.Layout.Search,
                query).MapResultTo(rs =>
            {
                var result = new SearchResult<LayoutEntity>(rs.Item1.Single(), rs.Item2.ToArray());
                result.Result.Iter(layout =>
                {
                    layout.OrganizationIds = rs.Item3.Where(o => o.LayoutId == layout.Id).Select(o => o.OrganizationId)
                        .ToList();
                });
                return result;
            });

        public async Task CreateAsync(LayoutEntity layout)
        {
            if (layout.Id == Guid.Empty) layout.Id = Guid.NewGuid();
            await _dbRunner.ExecuteAsync(ProcedureNames.Layout.Create, layout.ToDapperParameters());
        }

        public Task UpdateAsync(LayoutEntity layout)
            => _dbRunner.ExecuteAsync(ProcedureNames.Layout.Update, layout.ToDapperParameters(DbOperation.Update));

        public Task DeleteAsync(Guid layoutId)
            => _dbRunner.ExecuteAsync(ProcedureNames.Layout.Delete, layoutId);
    }
}
