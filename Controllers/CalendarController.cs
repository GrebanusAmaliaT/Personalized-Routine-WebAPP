using Microsoft.AspNetCore.Mvc;
using AplicatieRutina.Models;
using System.Linq;

namespace AplicatieRutina.Controllers
{
    public class CalendarController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CalendarController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEvent([FromBody] CalendarEvent ev)
        {
            if (string.IsNullOrWhiteSpace(ev.Description)) return BadRequest();
            _context.CalendarEvents.Add(ev);
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [HttpGet]
        public IActionResult GetEvents()
        {
            var today = DateTime.Today;
            var events = _context.CalendarEvents
                .Where(e => e.Date >= today)
                .OrderBy(e => e.Date)
                .ToList();

            return Json(events);
        }

    }
}
