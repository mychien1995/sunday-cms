using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Sunday.ContentDelivery.Core.Models;
using Sunday.ContentDelivery.Core.Services;
using Sunday.ContentManagement.Extensions;
using Sunday.Core.Media.Application;

namespace Sunday.Portfolio.Project.Components
{
    [ViewComponent(Name = "MySkills")]
    public class MySkillsComponent : ViewComponent
    {
        private readonly IContentReader _contentReader;
        private readonly IBlobLinkManager _blobLinkManager;

        public MySkillsComponent(IContentReader contentReader, IBlobLinkManager blobLinkManager)
        {
            _contentReader = contentReader;
            _blobLinkManager = blobLinkManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(RenderingParameters parameters)
        {
            if (parameters.Datasource == null) return new ContentViewComponentResult(string.Empty);
            var datasource = parameters.Datasource;
            var images = datasource.IdListValue("Skills");
            var heading = datasource.TextValue("Heading");
            var description = datasource.TextValue("Description");
            var imageContents = new List<MySkillsBlock.Image>();
            foreach (var image in images)
            {
                var imageContent = await _contentReader.GetContent(image);
                imageContent.IfSome(img => imageContents.Add(new MySkillsBlock.Image(img.Name,
                    _blobLinkManager.GetPreviewLink(img.BlobUriValue("Blob")!))));
            }
            var model = new MySkillsBlock(heading!, description!, imageContents);
            return View(model);
        }

        public class MySkillsBlock
        {
            public MySkillsBlock(string heading, string description, List<Image> images)
            {
                Heading = heading;
                Description = description;
                Images = images;
            }

            public string Heading { get; }
            public string Description { get; }
            public List<Image> Images { get; }
            public class Image
            {
                public string Alt { get; }
                public string Source { get; }

                public Image(string alt, string source)
                {
                    Alt = alt;
                    Source = source;
                }
            }
        }
    }
}
