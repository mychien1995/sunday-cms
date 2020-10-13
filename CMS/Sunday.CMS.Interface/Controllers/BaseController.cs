using Microsoft.AspNetCore.Mvc;
using Sunday.CMS.Core.Attributes;
using System;
using Sunday.Foundation.Context;

namespace Sunday.CMS.Interface.Controllers
{
    [FrameworkAuthorized]
    [OrganizationAuthorized]
    [FrameworkExceptionHandler]
    [ApiController]
    [Route("cms/api/[controller]")]
    public class BaseController : ControllerBase
    {
        protected readonly ISundayContext Context;
        public BaseController(ISundayContext context)
        {
            this.Context = context;
        }


        protected Guid CurrentOrganizationId
        {
            get
            {
                var currentOrgId = Context.CurrentOrganization?.Id;
                if (currentOrgId == null) throw new InvalidOperationException("Cannot resolve current organization");
                return currentOrgId.Value;
            }
        }

    }
}
