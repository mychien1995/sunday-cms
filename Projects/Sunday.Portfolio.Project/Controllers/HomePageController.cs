using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sunday.ContentDelivery.Core.Services;
using Sunday.ContentDelivery.Framework.Extensions;
using Sunday.Core.Media.Application;
using Sunday.Portfolio.Project.Models;

namespace Sunday.Portfolio.Project.Controllers
{
    public class HomePageController : Controller
    {
        private readonly IContentReader _contentReader;
        private readonly IBlobLinkManager _blobLinkManager;

        public HomePageController(IContentReader contentReader, IBlobLinkManager blobLinkManager)
        {
            _contentReader = contentReader;
            _blobLinkManager = blobLinkManager;
        }

        public async Task<ActionResult> Index()
        {
            var page = HttpContext.CurrentPage()!;
            var model = new HomePageModel(page);
            await model.Initialize(_contentReader, _blobLinkManager);
            return View(model);
        }
    }
}
