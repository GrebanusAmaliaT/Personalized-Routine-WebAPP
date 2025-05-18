using Microsoft.AspNetCore.Mvc;

namespace AplicatieRutina.Controllers
{
    public class ChallengesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
