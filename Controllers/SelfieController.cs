using AplicatieRutina.Models;
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

        public SelfieController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
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

            var selfie = new Selfie
            {
                UserId = userId,
                ImagePath = "/selfies/" + fileName,
                UploadDate = DateTime.Now
            };

            _context.Selfies.Add(selfie);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
