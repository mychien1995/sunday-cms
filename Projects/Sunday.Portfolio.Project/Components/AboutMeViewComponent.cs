using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Sunday.ContentDelivery.Core.Models;
using Sunday.ContentManagement.Extensions;

namespace Sunday.Portfolio.Project.Components
{
    [ViewComponent(Name = "AboutMe")]
    public class AboutMeViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(RenderingParameters parameters)
        {
            if (parameters.Datasource == null) return new ContentViewComponentResult(string.Empty);
            var heading = parameters.Datasource.TextValue("Heading")!;
            var subheading = parameters.Datasource.TextValue("Subheading")!;
            return View(new AboutMeBlock(heading, subheading));
        }

        public class AboutMeBlock
        {
            public AboutMeBlock(string heading, string subheading)
            {
                Heading = heading;
                Subheading = subheading;
            }

            public string Heading { get; } 
            public string Subheading { get;}
        }
    }
}
