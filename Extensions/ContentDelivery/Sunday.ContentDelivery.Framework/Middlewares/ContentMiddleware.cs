using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LanguageExt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Sunday.ContentDelivery.Core;
using Sunday.ContentDelivery.Core.Services;
using Sunday.ContentManagement.Domain;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Services;
using Sunday.Core.Extensions;

namespace Sunday.ContentDelivery.Framework.Middlewares
{
    public class ContentMiddleware
    {
        private readonly IWebsiteService _websiteService;
        private readonly IContentReader _contentReader;
        private readonly IRenderingService _renderingService;
        private readonly IActionInvokerFactory _actionInvokerFactory;
        private readonly ILayoutReader _layoutReader;
        private readonly Dictionary<string, ControllerActionDescriptor> _actionDescriptorCache;
        private readonly RequestDelegate _next;

        public ContentMiddleware(IWebsiteService websiteService, RequestDelegate next, IContentReader contentReader, IRenderingService renderingService
            , IActionInvokerFactory actionInvokerFactory, IActionDescriptorCollectionProvider actionDescriptorCollectionProvider, ILayoutReader layoutReader)
        {
            _websiteService = websiteService;
            _next = next;
            _contentReader = contentReader;
            _renderingService = renderingService;
            _actionInvokerFactory = actionInvokerFactory;
            _layoutReader = layoutReader;
            _actionDescriptorCache = actionDescriptorCollectionProvider.ActionDescriptors.Items
                .OfType<ControllerActionDescriptor>()
                .ToDictionary(GetActionKey);
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var requestPath = httpContext.Request.Path;
            var hostName = httpContext.Request.Host.Value;
            var websiteOpt = await _websiteService.GetByHostNameAsync(hostName);
            if (websiteOpt.IsNone) goto NEXT;
            var website = websiteOpt.Get();
            var contentOpt = await _contentReader.GetPage(website.Id, requestPath);
            if (contentOpt.IsNone) goto NEXT;
            var content = contentOpt.Get();
            var renderingOpt = await website.PageDesignMappings.Get(content.TemplateId.ToString()).MatchAsync(
                async renderingId => await _renderingService.GetRenderingById(Guid.Parse(renderingId)),
                () => Option<Rendering>.None);
            if (renderingOpt.IsNone) goto NEXT;
            var rendering = renderingOpt.Get();
            await InvokeActionContext(content, website,
                rendering, httpContext);
            return;
            NEXT:
            await _next(httpContext);
        }

        private async Task InvokeActionContext(Content content, ApplicationWebsite website,
            Rendering rendering, HttpContext httpContext)
        {
            var actionKey = FormalActionKey(rendering);
            var descriptorOption = _actionDescriptorCache.Get(actionKey);
            if (descriptorOption.IsNone) return;
            var descriptor = descriptorOption.Get();
            httpContext.Items.Add(RouteDataKey.Website, website);
            httpContext.Items.Add(RouteDataKey.Content, content);
            httpContext.Items.Add(RouteDataKey.Layout, await _layoutReader.GetLayout(website));
            var routeData = new RouteData(new RouteValueDictionary(new Dictionary<string, object>
            {
                {"action", descriptor.ActionName},
                {"controller", descriptor.ControllerName}
            }));
            var actionContext = new ActionContext(httpContext, routeData, descriptorOption.Get());
            await _actionInvokerFactory.CreateInvoker(actionContext).InvokeAsync();
        }

        private string GetActionKey(ControllerActionDescriptor descriptor)
        => $"{descriptor.ControllerTypeInfo.FullName},{descriptor.ControllerTypeInfo.Assembly.GetName().Name}|{descriptor.ActionName}".ToLower();

        private string FormalActionKey(Rendering rendering)
        {
            var action = rendering.Properties.Get("Action").IfNone(string.Empty);
            var controller = rendering.Properties.Get("Controller").IfNone(string.Empty);
            if (string.IsNullOrEmpty(action) || string.IsNullOrEmpty(controller)) return string.Empty;
            return $"{controller}|{action}".Replace(" ", string.Empty).ToLower();
        }
    }
}
