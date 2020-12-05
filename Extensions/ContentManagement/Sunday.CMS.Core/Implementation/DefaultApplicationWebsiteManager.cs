using System;
using System.Linq;
using System.Threading.Tasks;
using Sunday.CMS.Core.Application;
using Sunday.CMS.Core.Models.ApplicationWebsites;
using Sunday.ContentManagement.Domain;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Services;
using Sunday.Core;
using Sunday.Core.Extensions;
using Sunday.Core.Models.Base;
using Sunday.Foundation.Context;

namespace Sunday.CMS.Core.Implementation
{
    [ServiceTypeOf(typeof(IApplicationWebsiteManager))]
    public class DefaultApplicationWebsiteManager : IApplicationWebsiteManager
    {
        private readonly ISundayContext _sundayContext;
        private readonly IWebsiteService _websiteService;

        public DefaultApplicationWebsiteManager(IWebsiteService websiteService, ISundayContext sundayContext)
        {
            _websiteService = websiteService;
            _sundayContext = sundayContext;
        }

        public Task<WebsiteListJsonResult> Search(WebsiteQuery criteria)
            => _websiteService.QueryAsync(EnsureQuery(criteria)).MapResultTo(rs => new WebsiteListJsonResult
            {
                Total = rs.Total,
                Websites = rs.Result.CastListTo<WebsiteItem>().ToList()
            });

        public async Task<BaseApiResponse> Create(WebsiteMutationModel data)
        {
            await _websiteService.CreateAsync(ToDomainModel(data));
            return BaseApiResponse.SuccessResult;
        }

        public async Task<BaseApiResponse> Update(WebsiteMutationModel data)
        {
            await _websiteService.UpdateAsync(ToDomainModel(data));
            return BaseApiResponse.SuccessResult;
        }

        public Task<WebsiteDetailJsonResult> GetById(Guid websiteId)
            => _websiteService.GetByIdAsync(websiteId).MapResultTo(rs => rs.Some(l => l.MapTo<WebsiteDetailJsonResult>())
                .None(() => BaseApiResponse.ErrorResult<WebsiteDetailJsonResult>("Website not found")));

        public async Task<BaseApiResponse> Activate(Guid websiteId)
        {
            var websiteOpt = await _websiteService.GetByIdAsync(websiteId);
            if (websiteOpt.IsNone) return BaseApiResponse.ErrorResult<BaseApiResponse>("Website not found");
            var website = websiteOpt.Get();
            website.IsActive = !website.IsActive;
            await _websiteService.UpdateAsync(website);
            return BaseApiResponse.SuccessResult;
        }

        public async Task<BaseApiResponse> Delete(Guid websiteId)
        {
            await _websiteService.DeleteAsync(websiteId);
            return BaseApiResponse.SuccessResult;
        }
        private WebsiteQuery EnsureQuery(WebsiteQuery query)
        {
            var user = _sundayContext.CurrentUser;
            if (user!.IsOrganizationMember())
            {
                query.OrganizationId = _sundayContext.CurrentOrganization!.Id;
            }
            return query;
        }

        private ApplicationWebsite ToDomainModel(WebsiteMutationModel data)
        {
            var model = data.MapTo<ApplicationWebsite>();
            model.HostNames = model.HostNames.Where(h => !string.IsNullOrWhiteSpace(h)).Distinct().ToArray();
            var user = _sundayContext.CurrentUser;
            if (user!.IsOrganizationMember())
            {
                model.OrganizationId = _sundayContext.CurrentOrganization!.Id;
            }
            return model;
        }
    }
}
