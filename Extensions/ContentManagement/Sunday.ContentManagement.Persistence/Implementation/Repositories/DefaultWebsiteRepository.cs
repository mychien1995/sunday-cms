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
using Sunday.DataAccess.SqlServer.Attributes;
using Sunday.DataAccess.SqlServer.Database;
using static LanguageExt.Prelude;

namespace Sunday.ContentManagement.Persistence.Implementation.Repositories
{
    [ServiceTypeOf(typeof(IWebsitesRepository))]
    public class DefaultWebsiteRepository : IWebsitesRepository
    {
        private readonly StoredProcedureRunner _dbRunner;
        public DefaultWebsiteRepository(StoredProcedureRunner dbRunner)
        {
            _dbRunner = dbRunner;
        }

        public Task<SearchResult<WebsiteEntity>> QueryAsync(WebsiteQuery query)
            => _dbRunner.ExecuteMultipleAsync<int, WebsiteEntity>(ProcedureNames.Websites.Search, query).MapResultTo(
                rs => new SearchResult<WebsiteEntity>
                {
                    Total = rs.Item1.Single(),
                    Result = rs.Item2.ToArray()
                });

        public Task<Option<WebsiteEntity>> GetByIdAsync(Guid layoutId)
            => _dbRunner.ExecuteAsync<WebsiteEntity>(ProcedureNames.Websites.GetById, new {Id = layoutId})
                .MapResultTo(rs => Optional(rs.FirstOrDefault()));

        public Task CreateAsync(WebsiteEntity website)
            => _dbRunner.ExecuteAsync(ProcedureNames.Websites.Create, website.ToDapperParameters());

        public Task UpdateAsync(WebsiteEntity website)
            => _dbRunner.ExecuteAsync(ProcedureNames.Websites.Update, website.ToDapperParameters(DbOperation.Update));

        public Task DeleteAsync(Guid websiteId)
            => _dbRunner.ExecuteAsync(ProcedureNames.Websites.Delete, new { Id = websiteId });

        public Task<Option<WebsiteEntity>> GetByHostNameAsync(string hostName)
            => _dbRunner.ExecuteAsync<WebsiteEntity>(ProcedureNames.Websites.GetByHostName, new { HostName = hostName })
                .MapResultTo(rs => Optional(rs.FirstOrDefault()));
    }
}
