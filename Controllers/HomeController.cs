using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using AplicatieRutina.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AplicatieRutina.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _userManager = userManager;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Identity/Account/Login");
            }

            var userId = _userManager.GetUserId(User);
            var profile = _context.UserProfiles.FirstOrDefault(p => p.UserId == userId);

            if (profile == null)
            {
                return RedirectToAction("CreateName", "StepwiseProfile");
            }

            var quote = await GetMotivationalQuote();
            ViewBag.Quote = quote;
            ViewBag.Today = DateTime.Today.ToString("dddd, dd MMMM yyyy");

            return View();
        }


        private async Task<string> GetMotivationalQuote()
        {
            var client = _httpClientFactory.CreateClient();

            try
            {
                var response = await client.GetAsync("https://zenquotes.io/api/random");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var quotes = JsonDocument.Parse(content).RootElement;

                    var quote = quotes[0].GetProperty("q").GetString();
                    var author = quotes[0].GetProperty("a").GetString();

                    return $"\"{quote}\" — {author}";
                }
                else
                {
                    return "Stay beautiful on both the outside and the inside!";
                }
            }
            catch
            {
                return "Every day is a new chance to grow.";
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEvent([FromBody] CalendarEvent ev)
        {
            // Salveaz? în baza de date
            _context.CalendarEvents.Add(ev);
            _context.SaveChanges();
            return Ok();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
