using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Controllers
{
    public class ConfigController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
