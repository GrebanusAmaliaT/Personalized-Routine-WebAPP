using AplicatieRutina.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ReactionController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public ReactionController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> AddReaction([FromBody] ReactionDto data)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null) return Unauthorized();

        // Verifică dacă utilizatorul a reacționat deja la postarea asta cu același emoji
        var alreadyReacted = await _context.Reactions.AnyAsync(r => r.UserId == user.Id && r.PostId == data.PostId && r.Emoji == data.Emoji);

        if (alreadyReacted)
            return BadRequest("You already reacted with this emoji.");

        var reaction = new Reaction
        {
            PostId = data.PostId,
            Emoji = data.Emoji,
            UserId = user.Id
        };

        _context.Reactions.Add(reaction);
        await _context.SaveChangesAsync();

        // Returnează numărul total actualizat
        var total = await _context.Reactions.CountAsync(r => r.PostId == data.PostId && r.Emoji == data.Emoji);

        return Ok(new { count = total });
    }
}

public class ReactionDto
{
    public int PostId { get; set; }
    public string Emoji { get; set; }
}
