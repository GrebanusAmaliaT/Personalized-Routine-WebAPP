using AplicatieRutina.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AplicatieRutina.Controllers
{
    public class RoutinesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public RoutinesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Configure(string type)
        {
            ViewBag.Type = type;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Configure(string type, string products, TimeSpan scheduledTime)
        {
            var userId = _userManager.GetUserId(User);
            var routine = new Routine
            {
                UserId = userId,
                RoutineType = type,
                Products = products,
                ScheduledTime = scheduledTime
            };
            _context.Routines.Add(routine);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}