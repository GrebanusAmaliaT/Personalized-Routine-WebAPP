using AplicatieRutina.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class JournalController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public JournalController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> SaveEntry(string entryText)
    {
        var userId = _userManager.GetUserId(User);
        var entry = new JournalEntry
        {
            UserId = userId,
            Content = entryText,
            Date = DateTime.Today
        };
        _context.JournalEntries.Add(entry);
        await _context.SaveChangesAsync();
        return RedirectToAction("Summary");
    }

    public IActionResult Summary()
    {
        var userId = _userManager.GetUserId(User);
        var entriesByDate = _context.JournalEntries
            .Where(e => e.UserId == userId)
            .AsEnumerable()
            .GroupBy(e => e.Date)
            .OrderByDescending(g => g.Key)
            .ToList();

        return View(entriesByDate);
    }

    // GET: Edit
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var entry = _context.JournalEntries.FirstOrDefault(e => e.Id == id);
        if (entry == null || entry.UserId != _userManager.GetUserId(User))
            return NotFound();

        return View(entry);
    }

    // POST: Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(JournalEntry entry)
    {
        var existing = _context.JournalEntries.FirstOrDefault(e => e.Id == entry.Id);
        if (existing == null || existing.UserId != _userManager.GetUserId(User))
            return NotFound();

        existing.Content = entry.Content;
        _context.SaveChanges();
        return RedirectToAction("Summary");
    }

    // GET: Delete confirmation
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var entry = _context.JournalEntries.FirstOrDefault(e => e.Id == id);
        if (entry == null || entry.UserId != _userManager.GetUserId(User))
            return NotFound();

        return View(entry); // doar arată pagina de confirmare
    }

    // POST: Delete
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var entry = _context.JournalEntries.FirstOrDefault(e => e.Id == id);
        if (entry == null || entry.UserId != _userManager.GetUserId(User))
            return NotFound();

        _context.JournalEntries.Remove(entry);
        _context.SaveChanges();
        return RedirectToAction("Summary");
    }
}
