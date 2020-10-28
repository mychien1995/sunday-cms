using System;
using System.Linq;
using System.Threading.Tasks;
using Sunday.CMS.Core.Application;
using Sunday.CMS.Core.Models.ApplicationLayouts;
using Sunday.ContentManagement.Domain;
using Sunday.ContentManagement.Services;
using Sunday.Core;
using Sunday.Core.Extensions;
using Sunday.Core.Models.Base;
using Sunday.Foundation.Context;
using Sunday.Foundation.Models;

namespace Sunday.CMS.Core.Implementation
{
    [ServiceTypeOf(typeof(IApplicationLayoutManager))]
    public class DefaultApplicationLayoutManager : IApplicationLayoutManager
    {
        private readonly ILayoutService _layoutService;
        private readonly ISundayContext _sundayContext;

        public DefaultApplicationLayoutManager(ILayoutService layoutService, ISundayContext sundayContext)
        {
            _layoutService = layoutService;
            _sundayContext = sundayContext;
        }

        public Task<LayoutListJsonResult> SearchLayout(LayoutQuery criteria)
        => _layoutService.QueryAsync(EnsureQuery(criteria)).MapResultTo(rs => new LayoutListJsonResult
        {
            Total = rs.Total,
            Layouts = rs.Result.CastListTo<LayoutItem>().ToList()
        });

        public async Task<BaseApiResponse> CreateLayout(LayoutMutationModel data)
        {
            await _layoutService.CreateAsync(data.MapTo<ApplicationLayout>());
            return BaseApiResponse.SuccessResult;
        }

        public async Task<BaseApiResponse> UpdateLayout(LayoutMutationModel data)
        {
            await _layoutService.UpdateAsync(data.MapTo<ApplicationLayout>());
            return BaseApiResponse.SuccessResult;
        }

        public Task<LayoutDetailJsonResult> GetLayoutById(Guid layoutId)
            => _layoutService.GetByIdAsync(layoutId).MapResultTo(rs => rs.Some(l => l.MapTo<LayoutDetailJsonResult>())
                .None(() => BaseApiResponse.ErrorResult<LayoutDetailJsonResult>("Layout not found")));

        public async Task<BaseApiResponse> DeleteLayout(Guid layoutId)
        {
            await _layoutService.DeleteAsync(layoutId);
            return BaseApiResponse.SuccessResult;
        }

        private LayoutQuery EnsureQuery(LayoutQuery criteria)
        {
            var currentUser = _sundayContext.CurrentUser;
            if (currentUser!.IsOrganizationMember())
            {
                criteria.OrganizationId = _sundayContext.CurrentOrganization!.Id;
            }

            return criteria;
        }
    }
}
