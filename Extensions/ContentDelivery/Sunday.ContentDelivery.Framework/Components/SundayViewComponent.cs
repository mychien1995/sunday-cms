using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sunday.ContentDelivery.Core.Models;

namespace Sunday.ContentDelivery.Framework.Components
{
    public abstract class SundayViewComponent : ViewComponent
    {
        public abstract Task<IViewComponentResult> InvokeAsync(RenderingParameters parameters);
    }
}
