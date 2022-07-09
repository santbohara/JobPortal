using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Areas.Config.Controllers
{
    [Area("Config")]
    public class JobConfigController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
