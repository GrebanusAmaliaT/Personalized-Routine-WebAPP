using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AplicatieRutina.Models
{
    public class JournalEntry
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;

        [Required] public string Content { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public IdentityUser? User { get; set; }
    }

}
