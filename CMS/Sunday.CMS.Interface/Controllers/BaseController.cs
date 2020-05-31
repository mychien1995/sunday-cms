using Microsoft.AspNetCore.Mvc;
using Sunday.CMS.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sunday.CMS.Interface.Controllers
{
    [FrameworkAuthorized]
    [OrganizationAuthorized]
    [FrameworkExceptionHandler]
    [ApiController]
    [Route("cms/api/[controller]")]
    public class BaseController : ControllerBase
    {
    }
}
