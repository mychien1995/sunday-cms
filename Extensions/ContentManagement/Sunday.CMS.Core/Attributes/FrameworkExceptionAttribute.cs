using Microsoft.AspNetCore.Mvc;
using Sunday.CMS.Core.Filters;

namespace Sunday.CMS.Core.Attributes
{
    public class FrameworkExceptionHandlerAttribute : TypeFilterAttribute
    {
        public FrameworkExceptionHandlerAttribute() : base(typeof(FrameworkExceptionFilter))
        {
        }
    }
}
