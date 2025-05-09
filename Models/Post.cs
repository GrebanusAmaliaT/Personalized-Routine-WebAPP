using Microsoft.AspNetCore.Identity;

namespace AplicatieRutina.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        public string? ImagePath { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public IdentityUser User { get; set; }
    }

}
