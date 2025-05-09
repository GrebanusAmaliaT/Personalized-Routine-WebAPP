using Microsoft.AspNetCore.Mvc;

namespace AplicatieRutina.Controllers
{
    public class VisualCareController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
