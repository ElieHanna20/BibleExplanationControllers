using Microsoft.AspNetCore.Mvc;

namespace BibleExplanationControllers.Controllers.BibleControllers
{
    public class ExplanationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
