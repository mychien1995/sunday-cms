using System;
using System.Collections.Concurrent;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using LanguageExt;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.DependencyInjection;
using Sunday.ContentDelivery.Core.Models;
using Sunday.ContentDelivery.Core.Services;
using Sunday.ContentManagement;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Models.Fields;
using Sunday.Core.Extensions;

namespace Sunday.ContentDelivery.Framework.Extensions
{
    public static class HtmlHelperExtensions
    {
        private static readonly ConcurrentDictionary<string, Type> ComponentTypeCache =
            new ConcurrentDictionary<string, Type>();
        public static async Task RenderingAreaFor<TModel>(this IHtmlHelper<TModel> htmlHelper, Content content,
            string fieldName)
        {
            var field = content[fieldName];
            if (field?.FieldValue == null) return;
            var renderingValues = (field.FieldValue as RenderingAreaValue)!;
            var renderingService = htmlHelper.ViewContext.HttpContext.RequestServices.GetService<IRenderingReader>()!;
            var builder = new HtmlContentBuilder();
            var componentHelper = htmlHelper.ViewContext.HttpContext.RequestServices.GetService<IViewComponentHelper>() as DefaultViewComponentHelper;
            componentHelper!.Contextualize(htmlHelper.ViewContext);
            foreach (var renderingValue in renderingValues.Renderings)
            {
                var renderingOpt = renderingService.GetRendering(renderingValue.RenderingId).Result;
                if (renderingOpt.IsNone) continue;
                var rendering = renderingOpt.Get();
                if (rendering.RenderingType != RenderingTypes.ViewComponent.Code) continue;
                var component = rendering.Properties["Component"];
                if (component == null) continue;
                ComponentTypeCache.TryGetValue(component, out var componentType);
                if (componentType == null)
                {
                    componentType = Type.GetType(component)!;
                    ComponentTypeCache.TryAdd(component, componentType);
                }
                var datasourceId = renderingValue.Datasource;
                Option<Content> datasourceOpt = Option<Content>.None;
                if (datasourceId != null)
                {
                    var contentReader = htmlHelper.ViewContext.HttpContext.RequestServices.GetService<IContentReader>()!;
                    datasourceOpt = contentReader.GetContent(datasourceId.Value).Result;
                }
                var htmlContent = await componentHelper.InvokeAsync(componentType, new RenderingParameters(rendering, renderingValue.Parameters,
                    datasourceOpt.IsSome ? datasourceOpt.Get() : null));
                _ = builder.AppendHtml(htmlContent);
            }
            builder.WriteTo(htmlHelper.ViewContext.Writer, HtmlEncoder.Default);
        }
    }
}
