using AplicatieRutina.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var routines = _context.Routines
                .Where(r => r.UserId == userId)
                .Select(r => new { r.RoutineType, Hour = r.ScheduledTime.Hours, Minute = r.ScheduledTime.Minutes })
                .ToList();

            ViewBag.RoutinesJson = JsonSerializer.Serialize(routines);
            return View();
        }
    }
}
