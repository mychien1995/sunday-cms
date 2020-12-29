using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Sunday.ContentDelivery.Core.Models;
using Sunday.ContentManagement.Extensions;

namespace Sunday.Portfolio.Project.Components
{
    [ViewComponent(Name = "Contact")]
    public class ContactComponent : ViewComponent
    {

        public async Task<IViewComponentResult> InvokeAsync(RenderingParameters parameters)
        {
            if (parameters.Datasource == null) return new ContentViewComponentResult(string.Empty);
            var datasource = parameters.Datasource!;
            var model = new ContactBlock(datasource.TextValue("Heading")!, datasource.TextValue("Subheading")!);
            return View(model);
        }

        public class ContactBlock
        {
            public ContactBlock(string heading, string subheading)
            {
                Heading = heading;
                Subheading = subheading;
            }

            public string Heading { get;  }
            public string Subheading { get;  }
        }
    }
}
