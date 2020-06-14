using Microsoft.AspNetCore.Mvc;
using Sunday.CMS.Core.Attributes;
using Sunday.Core;
using System;

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
        public BaseController()
        {

        }
        public BaseController(ISundayContext context)
        {
            this.Context = context;
        }


        protected int CurrentOrganizationId
        {
            get
            {
                var currentOrgId = Context.CurrentOrganization?.ID;
                if (currentOrgId == null) throw new InvalidOperationException("Cannot resolve current organization");
                return currentOrgId.Value;
            }
        }

    }
}
