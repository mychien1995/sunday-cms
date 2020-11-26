using System;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.ContentManagement.Domain;
using Sunday.ContentManagement.Implementation.Pipelines.Arguments;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Persistence.Application;
using Sunday.ContentManagement.Persistence.Entities;
using Sunday.ContentManagement.Services;
using Sunday.Core;
using Sunday.Core.Extensions;
using Sunday.Core.Models.Base;
using Sunday.Core.Pipelines;
using Sunday.Core.Pipelines.Arguments;
using Sunday.DataAccess.SqlServer.Extensions;
using Sunday.Foundation.Context;

namespace Sunday.ContentManagement.Implementation.Services
{
    [ServiceTypeOf(typeof(IWebsiteService))]
    public class DefaultWebsiteService : IWebsiteService
    {
        private readonly IWebsitesRepository _websitesRepository;

        public DefaultWebsiteService(IWebsitesRepository websitesRepository)
        {
            _websitesRepository = websitesRepository;
        }

        public Task<SearchResult<ApplicationWebsite>> QueryAsync(WebsiteQuery query)
            => _websitesRepository.QueryAsync(query).MapResultTo(rs => new SearchResult<ApplicationWebsite>
            {
                Total = rs.Total,
                Result = rs.Result.Select(ToDomainModel).ToArray()
            });

        public Task<Option<ApplicationWebsite>> GetByIdAsync(Guid websiteId)
            => _websitesRepository.GetByIdAsync(websiteId).MapResultTo(rs => rs.Map(ToDomainModel));

        public async Task CreateAsync(ApplicationWebsite website)
        {
            await ApplicationPipelines.RunAsync("cms.entity.beforeCreate", new BeforeCreateEntityArg(website));
            await ApplicationPipelines.RunAsync("cms.websites.beforeCreate", new BeforeCreateWebsiteArg(website));
            await _websitesRepository.CreateAsync(ToEntity(website));
        }

        public async Task UpdateAsync(ApplicationWebsite website)
        {
            await ApplicationPipelines.RunAsync("cms.entity.beforeUpdate", new BeforeUpdateEntityArg(website));
            await ApplicationPipelines.RunAsync("cms.websites.beforeUpdate", new BeforeUpdateWebsiteArg(website));
            await _websitesRepository.UpdateAsync(ToEntity(website));
        }

        public Task DeleteAsync(Guid websiteId)
            => _websitesRepository.DeleteAsync(websiteId);

        private WebsiteEntity ToEntity(ApplicationWebsite model)
        {
            var entity = model.MapTo<WebsiteEntity>();
            entity.HostNames = model.HostNames.ToDatabaseList();
            entity.PageDesignMappings = model.PageDesignMappings.ToDatabaseDictionary();
            return entity;
        }
        private ApplicationWebsite ToDomainModel(WebsiteEntity entity)
        {
            var model = entity.MapTo<ApplicationWebsite>();
            model.HostNames = entity.HostNames.ToStringList().ToArray();
            model.PageDesignMappings = entity.PageDesignMappings.ToDictionary();
            return model;
        }
    }
}
