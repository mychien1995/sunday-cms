using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Sunday.ContentDelivery.Core.Models;
using Sunday.ContentDelivery.Core.Services;
using Sunday.ContentManagement;
using Sunday.ContentManagement.Extensions;

namespace Sunday.Portfolio.Project.Components
{
    [ViewComponent(Name = "OfferList")]
    public class OfferListComponent : ViewComponent
    {
        private readonly IContentReader _contentReader;

        public OfferListComponent(IContentReader contentReader)
        {
            _contentReader = contentReader;
        }

        public async Task<IViewComponentResult> InvokeAsync(RenderingParameters parameters)
        {
            if (parameters.Datasource == null) return new ContentViewComponentResult(string.Empty);
            var datasource = parameters.Datasource!;
            var offerContents = await _contentReader.GetChilds(datasource.Id, ContentType.Content);
            var offers = offerContents.Select(content => new OfferListBlock.Offer(content.TextValue("Icon")!,
                content.TextValue("Heading")!, content.TextValue("Content")!)).ToList();
            var model = new OfferListBlock(datasource.TextValue("Heading")!, offers);
            return View(model);
        }

        public class OfferListBlock
        {
            public OfferListBlock(string heading, List<Offer> offers)
            {
                Heading = heading;
                Offers = offers;
            }

            public string Heading { get; }
            public List<Offer> Offers { get;  }

            public class Offer
            {
                public Offer(string icon, string title, string content)
                {
                    Icon = icon;
                    Title = title;
                    Content = content;
                }

                public string Icon { get;  }
                public string Title { get;  }
                public string Content { get;  }
            }
        }
    }
}
