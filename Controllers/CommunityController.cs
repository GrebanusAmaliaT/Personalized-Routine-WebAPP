using AplicatieRutina.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

namespace AplicatieRutina.Controllers
{
    public class CommunityController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public CommunityController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;

        }

        public IActionResult Index()
        {
            var posts = _context.Posts.Include(p => p.User).OrderByDescending(p => p.CreatedAt).ToList();
            return View(posts);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string content, IFormFile? image)
        {
            var userId = _userManager.GetUserId(User);
            string? path = null;

            if (image != null && image.Length > 0)
            {
                var uploads = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploads);
                var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                var filePath = Path.Combine(uploads, fileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await image.CopyToAsync(stream);
                path = "/uploads/" + fileName;
            }

            var post = new Post
            {
                UserId = userId,
                Content = content,
                ImagePath = path
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }

}
