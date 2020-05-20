using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Sunday.CMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.CMS.Core.Filters
{
    public class FrameworkExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger;
        public FrameworkExceptionFilter(ILogger<FrameworkExceptionFilter> logger)
        {
            _logger = logger;
        }
        public void OnException(ExceptionContext context)
        {
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (actionDescriptor == null) return;
            var message = $"Error on executing action: {actionDescriptor.ActionName} on controller: {actionDescriptor.ControllerName}";
            _logger.LogError(context.Exception, message);
            var defaultObject = new BaseApiResponse();
            var errorMessage = context.Exception.Message;
            if (context.Exception.InnerException != null)
                errorMessage = context.Exception.InnerException.Message;
            defaultObject.AddError(errorMessage);
            context.Result = new OkObjectResult(defaultObject);
        }
    }
}
