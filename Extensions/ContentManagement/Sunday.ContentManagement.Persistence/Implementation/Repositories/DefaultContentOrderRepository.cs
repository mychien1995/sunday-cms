using System;
using System.Linq;
using System.Threading.Tasks;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Persistence.Application;
using Sunday.ContentManagement.Persistence.Implementation.DapperParameters;
using Sunday.Core;
using Sunday.Core.Extensions;
using Sunday.DataAccess.SqlServer.Database;
using Sunday.DataAccess.SqlServer.Extensions;

namespace Sunday.ContentManagement.Persistence.Implementation.Repositories
{
    [ServiceTypeOf(typeof(IContentOrderRepository))]
    public class DefaultContentOrderRepository : IContentOrderRepository
    {
        private readonly StoredProcedureRunner _dbRunner;

        public DefaultContentOrderRepository(StoredProcedureRunner dbRunner)
        {
            _dbRunner = dbRunner;
        }

        public async Task SaveOrder(ContentOrder[] orders)
        {
            if (!orders.Any()) return;
            await _dbRunner.ExecuteAsync(ProcedureNames.Contents.SaveOrders, new SaveContentOrderParameter(orders));
        }

        public async Task<ContentOrder[]> GetOrders(Guid[] contentIds)
        {
            if (!contentIds.Any()) return Array.Empty<ContentOrder>();
            return await _dbRunner.ExecuteAsync<ContentOrder>(ProcedureNames.Contents.GetOrders, new { ids = contentIds.ToDatabaseList() })
                .MapResultTo(rs => rs.ToArray());
        }
    }
}
