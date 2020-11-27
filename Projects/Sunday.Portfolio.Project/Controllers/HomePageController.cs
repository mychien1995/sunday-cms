using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Sunday.Portfolio.Project.Controllers
{
    public class HomePageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
