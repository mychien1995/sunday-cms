using System.Collections.Generic;
using System.Threading.Tasks;
using Sunday.ContentDelivery.Core.Services;
using Sunday.ContentManagement.Extensions;
using Sunday.ContentManagement.Models;
using Sunday.Core.Extensions;
using Sunday.Core.Media.Application;

namespace Sunday.Portfolio.Project.Models
{
    public class HomePageModel
    {
        public Content CurrentPage { get; }
        public string? Heading { get; }
        public string? Subheading { get; }
        public string? ImageUrl { get; set; } = string.Empty;
        public List<LinkModel> NavigationLinks { get; } = new List<LinkModel>();
        public List<SocialLinkModel> SocialLinks { get; } = new List<SocialLinkModel>();

        public HomePageModel(Content currentPage)
        {
            CurrentPage = currentPage;
            Heading = currentPage.TextValue("Heading");
            Subheading = currentPage.TextValue("Subheading");
        }

        public async Task Initialize(IContentReader contentReader, IBlobLinkManager blobLinkManager)
        {
            var navigationLinks = CurrentPage.IdListValue("NavigationLinks");
            foreach (var linkItemId in navigationLinks)
            {
                await contentReader.GetContent(linkItemId).ThenDo(linkOpt => linkOpt.IfSome(linkItem =>
                {
                    var link = linkItem.LinKValue("Link");
                    if (link == null) return;
                    NavigationLinks.Add(new LinkModel(link.LinkText, link.Url));
                }));
            }
            var socialLinks = CurrentPage.IdListValue("SocialLinks");
            foreach (var linkItemId in socialLinks)
            {
                await contentReader.GetContent(linkItemId).ThenDo(linkOpt => linkOpt.IfSome(linkItem =>
                {
                    var link = linkItem.TextValue("Link")!;
                    var icon = linkItem.TextValue("Icon")!;
                    SocialLinks.Add(new SocialLinkModel(icon, link));
                }));
            }
            var image = CurrentPage.IdValue("Avatar");
            if (image == null) return;
            await contentReader.GetContent(image.Value).ThenDo(imageOpt => imageOpt.IfSome(imageItem =>
            {
                var blobUri = imageItem.BlobUriValue("Blob");
                if (blobUri != null)
                    ImageUrl = blobLinkManager.GetPreviewLink(blobUri!);
            }));
        }
    }
    public class SocialLinkModel
    {
        public SocialLinkModel(string icon, string url)
        {
            Icon = icon;
            Url = url;
        }

        public string Icon { get; }
        public string Url { get; }
    }
    public class LinkModel
    {
        public string Text { get; }
        public string Url { get; }

        public LinkModel(string text, string url)
        {
            Text = text;
            Url = url;
        }
    }
}
