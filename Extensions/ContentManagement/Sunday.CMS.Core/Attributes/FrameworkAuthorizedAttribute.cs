using Microsoft.AspNetCore.Mvc;
using Sunday.CMS.Core.Filters;

namespace Sunday.CMS.Core.Attributes
{
    public class FrameworkAuthorizedAttribute : TypeFilterAttribute
    {
        public FrameworkAuthorizedAttribute() : base(typeof(FrameworkAuthorizedFilter))
        {
        }
    }
}
