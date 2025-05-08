using AplicatieRutina.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AplicatieRutina.Controllers
{
    public class StepwiseProfileController : Controller
    {
        // GET: StepwiseProfile/CreateName
        public IActionResult CreateName()
        {
            return View();
        }

        // POST: StepwiseProfile/CreateName
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                ModelState.AddModelError("name", "Numele este obligatoriu.");
                return View();
            }

            // Salvăm numele în TempData (temporar, până finalizăm tot profilul)
            TempData["Name"] = name;

            // Redirecționăm la pasul următor
            return RedirectToAction("CreateGender");
        }

        // GET: StepwiseProfile/CreateGender
        public IActionResult CreateGender()
        {
            return View();
        }

        // POST: StepwiseProfile/CreateGender
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateGender(string gender)
        {
            if (string.IsNullOrEmpty(gender))
            {
                ModelState.AddModelError("gender", "Select your gender.");
                return View();
            }

            TempData["Gender"] = gender;
            return RedirectToAction("CreateAge");
        }

        // GET: StepwiseProfile/CreateAge
        public IActionResult CreateAge()
        {
            return View();
        }

        // POST: StepwiseProfile/CreateAge
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAge(string ageCategory)
        {
            if (string.IsNullOrEmpty(ageCategory))
            {
                ModelState.AddModelError("ageCategory", "Please select an age category.");
                return View();
            }

            TempData["AgeCategory"] = ageCategory;
            return RedirectToAction("CreateSkinType");
        }

        // GET: StepwiseProfile/CreateSkinType
        public IActionResult CreateSkinType()
        {
            return View();
        }

        // POST: StepwiseProfile/CreateSkinType
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateSkinType(string skinType)
        {
            if (string.IsNullOrEmpty(skinType))
            {
                ModelState.AddModelError("skinType", "Please select your skin type.");
                return View();
            }

            TempData["SkinType"] = skinType;
            return RedirectToAction("CreateHairType");
        }

        // GET: StepwiseProfile/CreateHairType
        public IActionResult CreateHairType()
        {
            return View();
        }

        // POST: StepwiseProfile/CreateHairType
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateHairType(string hairType)
        {
            if (string.IsNullOrEmpty(hairType))
            {
                ModelState.AddModelError("hairType", "Please select your hair type.");
                return View();
            }

            TempData["HairType"] = hairType;
            return RedirectToAction("ReviewProfile"); // următorul pas: confirmare/salvare
        }


        public IActionResult ReviewProfile()
        {
            var model = new
            {
                Name = TempData["Name"] as string,
                Gender = TempData["Gender"] as string,
                Age = TempData["AgeCategory"] as string,
                SkinType = TempData["SkinType"] as string,
                HairType = TempData["HairType"] as string
            };

            // Re-stocăm în TempData (altfel se pierd după read)
            TempData.Keep();

            return View(model);
        }

        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public StepwiseProfileController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmProfile()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Redirect("/Identity/Account/Login");
            }

            // Preluăm valorile din TempData și le păstrăm
            var name = TempData["Name"] as string ?? "";
            var gender = TempData["Gender"] as string ?? "";
            var ageStr = TempData["AgeCategory"] as string ?? "";
            var skinType = TempData["SkinType"] as string ?? "";
            var hairType = TempData["HairType"] as string ?? "";

            // Convertim categoria de vârstă într-un text prietenos
            var age = ageStr switch
            {
                "Below 18" => "young person",
                "18-24" => "young adult",
                "25-34" => "adult",
                "35-50" => "adult",
                "51+" => "grown adult",
                _ => "unknown"
            };

            // Creăm profilul
            var profile = new UserProfile
            {
                UserId = userId,
                Name = name,
                Gender = gender,
                Age = age,
                SkinType = skinType,
                HairType = hairType,
                Streak = 0,
                DaysOnApp = 0
            };


            _context.UserProfiles.Add(profile);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }


    }
}
