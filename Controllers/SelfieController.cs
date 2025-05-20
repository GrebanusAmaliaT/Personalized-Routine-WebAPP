using AplicatieRutina.Models;
using AplicatieRutina.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AplicatieRutina.Controllers
{
    [Authorize]
    public class SelfieController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _env;
        private readonly ImageAnalysisService _analyzer;

        public SelfieController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IWebHostEnvironment env, ImageAnalysisService analyzer)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
            _analyzer = analyzer;
        }
        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            var selfies = _context.Selfies
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.UploadDate)
                .ToList();

            return View(selfies);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile Image)
        {
            if (Image == null || Image.Length == 0)
            {
                TempData["Error"] = "Please upload a valid image.";
                return RedirectToAction("Index");
            }

            var userId = _userManager.GetUserId(User);
            var uploadPath = Path.Combine(_env.WebRootPath, "selfies");
            Directory.CreateDirectory(uploadPath);

            var fileName = Guid.NewGuid() + Path.GetExtension(Image.FileName);
            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await Image.CopyToAsync(stream);
            }

            var analysis = _analyzer.Analyze(filePath);
            ViewBag.AnalysisResult = $"Redness: {analysis.redness}, Brightness: {analysis.brightness}, Dark Spots: {analysis.darkness}";

            var selfie = new Selfie
            {
                UserId = userId,
                ImagePath = "/selfies/" + fileName,
                UploadDate = DateTime.Now
            };

            _context.Selfies.Add(selfie);
            await _context.SaveChangesAsync();

            var selfies = _context.Selfies
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.UploadDate)
                .ToList();

            return View("Index", selfies);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var selfie = await _context.Selfies.FindAsync(id);
            if (selfie == null)
                return NotFound();

            // Dacă vrei, verifici și dacă aparține userului curent

            _context.Selfies.Remove(selfie);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
