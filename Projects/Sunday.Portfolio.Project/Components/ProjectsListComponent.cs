using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Sunday.ContentDelivery.Core.Models;
using Sunday.ContentDelivery.Core.Services;
using Sunday.ContentManagement;
using Sunday.ContentManagement.Extensions;
using Sunday.Core.Media.Application;

namespace Sunday.Portfolio.Project.Components
{
    [ViewComponent(Name = "ProjectsList")]
    public class ProjectsListComponent : ViewComponent
    {
        private readonly IContentReader _contentReader;
        private readonly IBlobLinkManager _blobLinkManager;

        public ProjectsListComponent(IContentReader contentReader, IBlobLinkManager blobLinkManager)
        {
            _contentReader = contentReader;
            _blobLinkManager = blobLinkManager;
        }
        public async Task<IViewComponentResult> InvokeAsync(RenderingParameters parameters)
        {
            if (parameters.Datasource == null) return new ContentViewComponentResult(string.Empty);
            var datasource = parameters.Datasource!;
            var projectContents = await _contentReader.GetChilds(datasource.Id, ContentType.Content);
            var projects = await Task.WhenAll(projectContents.Select(async content =>
            {
                var link = content.LinKValue("Link")!;
                var imageId = content.IdValue("Image");
                var imageLink = string.Empty;
                if (imageId.HasValue)
                {
                    var image = await _contentReader.GetContent(imageId.Value);
                    image.IfSome(img => imageLink = _blobLinkManager.GetPreviewLink(img.BlobUriValue("Blob")!));
                }
                return new ProjectListBlock.Project(content.TextValue("Title")!, content.TextValue("Content")!,
                    imageLink, link.Url, link.LinkText);
            }));
            var model = new ProjectListBlock(datasource.TextValue("Heading")!, datasource.TextValue("Subheading")!
                , projects.ToList());
            return View(model);
        }

        public class ProjectListBlock
        {
            public ProjectListBlock(string heading, string subheading, List<Project> projects)
            {
                Heading = heading;
                Subheading = subheading;
                Projects = projects;
            }

            public string Heading { get; }
            public string Subheading { get; }
            public List<Project> Projects { get; }

            public class Project
            {
                public string Title { get; }
                public string Content { get; }
                public string ImageLink { get; }
                public string Link { get; }
                public string LinkText { get; }

                public Project(string title, string content, string imageLink, string link, string linkText)
                {
                    Title = title;
                    Content = content;
                    ImageLink = imageLink;
                    Link = link;
                    LinkText = linkText;
                }
            }
        }
    }
}
