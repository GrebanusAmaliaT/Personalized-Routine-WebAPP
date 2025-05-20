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
        public async Task<IActionResult> AddEvent([FromBody] CalendarEvent ev)
        {
            if (string.IsNullOrWhiteSpace(ev.Description) || ev.Date == default)
                return BadRequest();

            _context.CalendarEvents.Add(ev);
            await _context.SaveChangesAsync();
            return Ok();
        }

        public IActionResult Index()
        {
            var events = _context.CalendarEvents
                .Where(e => e.Date > DateTime.Now)
                .OrderBy(e => e.Date)
                .ToList();

            return View(events);
        }


    }
}
