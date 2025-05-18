using Microsoft.AspNetCore.Identity;

namespace AplicatieRutina.Models
{
    public class Reaction
    {
        public int Id { get; set; }
        public string Emoji { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }
    }

}
