using Microsoft.AspNetCore.Mvc;

namespace ITSamadhan.Controllers
{
    public class SettingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
