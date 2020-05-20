using Microsoft.AspNetCore.Mvc;
using Sunday.CMS.Core.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.CMS.Core.Attributes
{
    public class FrameworkExceptionHandlerAttribute : TypeFilterAttribute
    {
        public FrameworkExceptionHandlerAttribute() : base(typeof(FrameworkExceptionFilter))
        {
        }
    }
}
