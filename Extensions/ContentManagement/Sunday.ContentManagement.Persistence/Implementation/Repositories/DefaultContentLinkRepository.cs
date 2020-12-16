using System;
using System.Linq;
using System.Threading.Tasks;
using Sunday.ContentManagement.Persistence.Application;
using Sunday.Core;
using Sunday.Core.Extensions;
using Sunday.DataAccess.SqlServer.Database;
using Sunday.DataAccess.SqlServer.Extensions;

namespace Sunday.ContentManagement.Persistence.Implementation.Repositories
{
    [ServiceTypeOf(typeof(IContentLinkRepository))]
    public class DefaultContentLinkRepository : IContentLinkRepository
    {
        private readonly StoredProcedureRunner _dbRunner;

        public DefaultContentLinkRepository(StoredProcedureRunner dbRunner)
        {
            _dbRunner = dbRunner;
        }

        public Task Save(Guid contentId, Guid[] references)
            => _dbRunner.ExecuteAsync(ProcedureNames.ContentLinks.Save,
                new {ContentId = contentId, ReferenceIds = references.ToDatabaseList()});

        public Task<Guid[]> GetReferencesTo(Guid contentId)
            => _dbRunner.ExecuteAsync<Guid>(ProcedureNames.ContentLinks.GetReferencesTo, new {ContentId = contentId}).MapResultTo(rs => rs.ToArray());

        public Task<Guid[]> GetReferencesFrom(Guid contentId)
            => _dbRunner.ExecuteAsync<Guid>(ProcedureNames.ContentLinks.GetReferencesFrom, new { ContentId = contentId }).MapResultTo(rs => rs.ToArray());
    }
}
